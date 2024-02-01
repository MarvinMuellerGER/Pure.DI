﻿// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBeMadeStatic.Global
namespace Build;

using System.Text.RegularExpressions;
using HostApi;
using NuGet.Versioning;

internal partial class NuGetVersions
{
    private static readonly Regex ReleaseRegex = CreateReleaseRegex();
    
    public NuGetVersion GetNext(NuGetRestoreSettings settings, VersionRange versionRange, int patchIncrement = 1) =>
        GetService<INuGet>()
            .Restore(settings.WithHideWarningsAndErrors(true).WithVersionRange(versionRange).WithNoCache(true))
            .Where(i => i.Name == settings.PackageId)
            .Select(i => i.NuGetVersion)
            .Select(i => i.Release != string.Empty 
                ? GetNextRelease(versionRange, i)
                : new NuGetVersion(i.Major, i.Minor, i.Patch + patchIncrement))
            .Max() ?? new NuGetVersion(2, 0, 0);

    private static NuGetVersion GetNextRelease(VersionRangeBase versionRange, NuGetVersion version)
    {
        if (versionRange.MinVersion.Release != "0" && versionRange.MinVersion.Release != version.Release)
        {
            return versionRange.MinVersion;
        }
        
        var match = ReleaseRegex.Match(version.Release);
        if (!match.Success)
        {
            return version;
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!int.TryParse(match.Groups[2].Value, out var index))
        {
            return version;
        }
        
        return new NuGetVersion(version.Major, version.Minor, version.Patch, match.Groups[1].Value + (index + 1));
    }
 
    [GeneratedRegex("""([^\d]+)([\d]*)""")]
    private static partial Regex CreateReleaseRegex();
}