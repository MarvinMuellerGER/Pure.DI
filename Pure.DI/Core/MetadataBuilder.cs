// ReSharper disable ClassNeverInstantiated.Global
namespace Pure.DI.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal class MetadataBuilder : IMetadataBuilder
    {
        private readonly ILog<MetadataBuilder> _log;
        private readonly IDiagnostic _diagnostic;
        private readonly Func<SemanticModel, IMetadataWalker> _metadataWalkerFactory;
        private readonly ICache<LanguageVersion, SourceBuilderState> _stateCache;

        public MetadataBuilder(
            ILog<MetadataBuilder> log,
            IDiagnostic diagnostic,
            Func<SemanticModel, IMetadataWalker> metadataWalkerFactory,
            [Tag(Tags.GlobalScope)] ICache<LanguageVersion, SourceBuilderState> stateCache)
        {
            _log = log;
            _diagnostic = diagnostic;
            _metadataWalkerFactory = metadataWalkerFactory;
            _stateCache = stateCache;
        }

        public MetadataContext Build(Compilation compilation, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var components = GetComponents(compilation).ToList().AsReadOnly();

            var sourceSet = GetSourceSet(compilation);
            var syntaxTreesCount = compilation.SyntaxTrees.Count();
            compilation = compilation.AddSyntaxTrees(sourceSet.ComponentsTrees.Concat(sourceSet.FeaturesTrees));
            var featuresTrees = compilation.SyntaxTrees.Skip(syntaxTreesCount);
            var featuresMetadata = GetMetadata(compilation, featuresTrees, cancellationToken).ToList();
            foreach (var metadata in featuresMetadata)
            {
                metadata.DependsOn.Clear();
            }

            var currentTrees = compilation.SyntaxTrees.Take(syntaxTreesCount);
            var currentMetadata = GetMetadata(compilation, currentTrees, cancellationToken).ToList();
            stopwatch.Stop();
            return new MetadataContext(compilation, cancellationToken, components, featuresMetadata.AsReadOnly(), currentMetadata.AsReadOnly(), stopwatch.ElapsedMilliseconds);
        }
        
        private IEnumerable<Source> GetComponents(Compilation compilation) =>
            AreComponentsAvailable(compilation) ? Enumerable.Empty<Source>() : GetSourceSet(compilation).ComponentSources;
        
        private static bool AreComponentsAvailable(Compilation compilation)
        {
            var diType = compilation.GetTypesByMetadataName(typeof(DI).FullName).FirstOrDefault();
            if (diType == null)
            {
                return false;
            }

            var type = ( 
                from tree in compilation.SyntaxTrees
                let semanticModel = compilation.GetSemanticModel(tree)
                from typeDeclaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>()
                let symbol = ModelExtensions.GetDeclaredSymbol(semanticModel, typeDeclaration)
                select symbol).FirstOrDefault();
            
            return type != null && compilation.IsSymbolAccessibleWithin(diType, type);
        }
        
        private SourceSet GetSourceSet(Compilation compilation)
        {
            if (compilation is CSharpCompilation csharpCompilation)
            {
                return _stateCache.GetOrAdd(
                        csharpCompilation.LanguageVersion,
                        _ => new SourceBuilderState(new SourceSet(new CSharpParseOptions(csharpCompilation.LanguageVersion))))
                    .SourceSet;
            }

            var error = $"{compilation.Language} is not supported.";
            _diagnostic.Error(Diagnostics.Error.Unsupported, error);
            throw new HandledException(error);
        }

        private IEnumerable<ResolverMetadata> GetMetadata(Compilation compilation, IEnumerable<SyntaxTree> trees, CancellationToken cancellationToken)
        {
            foreach (var tree in trees)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _log.Trace(() => new[] { "Canceled" });
                    yield break;
                }
                
                var walker = _metadataWalkerFactory(compilation.GetSemanticModel(tree));
                walker.Visit(tree.GetRoot());
                foreach (var metadata in walker.Metadata)
                {
                    yield return metadata;
                }
            }
        }
        
        internal class SourceBuilderState
        {
            public readonly SourceSet SourceSet;
            
            public SourceBuilderState(SourceSet sourceSet)
            {
                SourceSet = sourceSet;
            }
        }
    }
}