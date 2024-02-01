// ReSharper disable StringLiteralTypo
// ReSharper disable ClassNeverInstantiated.Global
namespace Build;

using System.CommandLine;
using System.CommandLine.Invocation;
using HostApi;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

internal class TemplateTarget: Command, ITarget<string>
{
    private readonly Settings _settings;
    private readonly NuGetVersions _nuGetVersions;
    private readonly ITeamCityWriter _teamCityWriter;

    public TemplateTarget(
        Settings settings,
        NuGetVersions nuGetVersions,
        ITeamCityWriter teamCityWriter)
        : base("template", "Creates and push templates")
    {
        _settings = settings;
        _nuGetVersions = nuGetVersions;
        _teamCityWriter = teamCityWriter;
        this.SetHandler(RunAsync);
        AddAlias("t");
    }
    
    public Task<string> RunAsync(InvocationContext ctx)
    {
        Info("Creating templates");
        var templatePackageVersion = _settings.VersionOverride ?? _nuGetVersions.GetNext(new NuGetRestoreSettings("Pure.DI.Templates"), _settings.VersionRange);
        var props = new[]
        {
            ("configuration", _settings.Configuration),
            ("version", templatePackageVersion.ToString()!)
        };

        var projectDirectory = Path.Combine("src", "Pure.DI.Templates");
        var pack = new DotNetPack()
            .WithProject(Path.Combine(projectDirectory, "Pure.DI.Templates.csproj"))
            .WithProps(props);

        pack.Build().Succeed();
        
        var targetPackage = Path.Combine(projectDirectory, "bin", $"Pure.DI.Templates.{templatePackageVersion}.nupkg");
        _teamCityWriter.PublishArtifact($"{targetPackage} => .");

        if (!string.IsNullOrWhiteSpace(_settings.NuGetKey))
        {
            var push = new DotNetNuGetPush()
                .WithPackage(targetPackage)
                .WithSources("https://api.nuget.org/v3/index.json")
                .WithApiKey(_settings.NuGetKey);

            push.Build().Succeed();
        }
        else
        {
            Warning($"The NuGet key was not specified, the package {targetPackage} will not be pushed.");
        }

        return Task.FromResult(targetPackage);
    }
}