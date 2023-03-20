// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation.Possible
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation
namespace Pure.DI.Core;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
internal sealed class MetadataBuilder : IBuilder<IEnumerable<SyntaxUpdate>, IEnumerable<MdSetup>>
{
    private readonly ILogger<MetadataBuilder> _logger;
    private readonly Func<IBuilder<SyntaxUpdate, IEnumerable<MdSetup>>> _setupsBuilderFactory;
    private readonly IValidator<MdSetup> _metadataValidator;

    public MetadataBuilder(
        ILogger<MetadataBuilder> logger,
        Func<IBuilder<SyntaxUpdate, IEnumerable<MdSetup>>> setupsBuilderFactory,
        IValidator<MdSetup> metadataValidator)
    {
        _logger = logger;
        _setupsBuilderFactory = setupsBuilderFactory;
        _metadataValidator = metadataValidator;
    }

    public IEnumerable<MdSetup> Build(IEnumerable<SyntaxUpdate> updates, CancellationToken cancellationToken)
    {
        using var logToken = _logger.TraceProcess("building metadata");
        
        var actualUpdates = 
            updates
                .GroupBy(i => i.Node.SyntaxTree.GetRoot())
                .Select(i => new SyntaxUpdate(i.Key, i.First().SemanticModel))
                .ToList();
        
        var setups = new List<MdSetup>();
        foreach (var update in actualUpdates)
        {
            using var visitLogToken = _logger.TraceProcess("processing update", update.Node.GetLocation());
            var setupsBuilder = _setupsBuilderFactory();
            foreach (var newSetup in setupsBuilder.Build(update, cancellationToken))
            {
                _metadataValidator.Validate(newSetup, cancellationToken);
                setups.Add(newSetup);    
            }
            
            cancellationToken.ThrowIfCancellationRequested();
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (setups.Count == 0)
        {
            yield break;
        }

        using var aggregatingToken = _logger.TraceProcess("aggregating metadata");
        var setupMap = setups
            .GroupBy(i => i.ComposerTypeName)
            .Select(setupGroup =>
            {
                MergeSetups(setupGroup, out var mergedSetup, false, cancellationToken);
                return mergedSetup;
            })
            .ToDictionary(i => i.ComposerTypeName, i => i);

        var globalSetups = setupMap.Values.Where(i => i.Kind == ComposerKind.Global).ToList();
        foreach (var setup in setupMap.Values.Where(i => i.Kind == ComposerKind.Public))
        {
            var setupsChain = globalSetups
                .Concat(ResolveDependencies(setup, setupMap, new HashSet<string>(), cancellationToken))
                .Concat(Enumerable.Repeat(setup, 1));
            
            MergeSetups(setupsChain, out var mergedSetup, true, cancellationToken);
            _logger.Trace(mergedSetup, i => Enumerable.Repeat("Metadata created", 1).Concat(i.ToStrings(1)), setup.Source.GetLocation());
            yield return mergedSetup;
        }
    }

    private IEnumerable<MdSetup> ResolveDependencies(
        MdSetup setup,
        IReadOnlyDictionary<string, MdSetup> map, ISet<string> processed,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        foreach (var dependsOn in setup.DependsOn)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var composerTypeName in dependsOn.ComposerTypeNames)
            {
                if (!processed.Add(composerTypeName))
                {
                    continue;
                }

                if (!map.TryGetValue(composerTypeName, out var dependsOnSetup))
                {
                    _logger.CompileError($"Cannot find setup \"{composerTypeName}\" for {dependsOn}.", dependsOn.Source.GetLocation(), LogId.ErrorCannotFindSetup);
                    throw HandledException.Shared;
                }

                yield return dependsOnSetup;
                foreach (var result in ResolveDependencies(dependsOnSetup, map, processed, cancellationToken))
                {
                    yield return result;
                }   
            }
        }
    }

    private static void MergeSetups(IEnumerable<MdSetup> setups, out MdSetup mergedSetup, bool resolveDependsOn, in CancellationToken cancellationToken)
    {
        SyntaxNode? source = default;
        var composerTypeName = "";
        var kind = ComposerKind.Global;
        var settings = new Settings();
        var bindingsBuilder = ImmutableArray.CreateBuilder<MdBinding>(64);
        var rootsBuilder = ImmutableArray.CreateBuilder<MdRoot>(64);
        var dependsOnBuilder = ImmutableArray.CreateBuilder<MdDependsOn>(2);
        var typeAttributesBuilder = ImmutableArray.CreateBuilder<MdTypeAttribute>(2);
        var tagAttributesBuilder = ImmutableArray.CreateBuilder<MdTagAttribute>(2);
        var ordinalAttributesBuilder = ImmutableArray.CreateBuilder<MdOrdinalAttribute>(2);
        var ns = string.Empty;
        var usingDirectives = new HashSet<string>();
        var bindingId = 0;
        foreach (var setup in setups)
        {
            source = setup.Source;
            composerTypeName = setup.ComposerTypeName;
            kind = setup.Kind;
            foreach (var setting in setup.Settings)
            {
                settings[setting.Key] = setting.Value;
            }

            if (resolveDependsOn)
            {
                bindingsBuilder.AddRange(setup.Bindings.Select(i => i with { Id = bindingId++ }));
            }
            else
            {
                bindingsBuilder.AddRange(setup.Bindings);    
            }
            
            rootsBuilder.AddRange(setup.Roots);
            dependsOnBuilder.AddRange(setup.DependsOn);
            typeAttributesBuilder.AddRange(setup.TypeAttributes);
            tagAttributesBuilder.AddRange(setup.TagAttributes);
            ordinalAttributesBuilder.AddRange(setup.OrdinalAttributes);
            ns = setup.Namespace;
            foreach (var usingDirective in setup.UsingDirectives)
            {
                usingDirectives.Add(usingDirective);   
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        mergedSetup = new MdSetup(
            source!,
            composerTypeName,
            ns,
            usingDirectives.ToImmutableArray(),
            kind,
            settings,
            bindingsBuilder.ToImmutable(),
            rootsBuilder.ToImmutable(),
            resolveDependsOn ? ImmutableArray<MdDependsOn>.Empty : dependsOnBuilder.ToImmutable(),
            typeAttributesBuilder.ToImmutable(),
            tagAttributesBuilder.ToImmutable(),
            ordinalAttributesBuilder.ToImmutable());
    }
}