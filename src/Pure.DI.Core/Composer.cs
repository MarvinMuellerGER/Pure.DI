// ReSharper disable UnusedMember.Local
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation
namespace Pure.DI;

using System.Text.RegularExpressions;
using Core;
using Core.Models;

// ReSharper disable once PartialTypeWithSinglePart
internal static partial class Composer
{
    private static void Setup() => IoC.DI.Setup()
        // Transients
        .Bind<IMetadataSyntaxWalker>().To<MetadataSyntaxWalker>()
        .Bind<IBuilder<SyntaxUpdate, IEnumerable<MdSetup>>>().To<SetupsBuilder>()
        .Bind<IBuilder<IEnumerable<SyntaxUpdate>, IEnumerable<MdSetup>>>().To<MetadataBuilder>()
        .Bind<IGenerator>().To<Generator>()
        .Bind<IBuilder<Unit, IEnumerable<Source>>>().To<ApiBuilder>()
        .Bind<IBuilder<MdSetup, DependencyGraph>>().To<DependencyGraphBuilder>()
        .Bind<IBuilder<MdSetup, IEnumerable<DependencyNode>>>(typeof(RootDependencyNodeBuilder)).To<RootDependencyNodeBuilder>()
        .Bind<IBuilder<MdSetup, IEnumerable<DependencyNode>>>(typeof(ImplementationDependencyNodeBuilder)).To<ImplementationDependencyNodeBuilder>()
        .Bind<IBuilder<MdSetup, IEnumerable<DependencyNode>>>(typeof(FactoryDependencyNodeBuilder)).To<FactoryDependencyNodeBuilder>()
        .Bind<IBuilder<MdSetup, IEnumerable<DependencyNode>>>(typeof(ArgDependencyNodeBuilder)).To<ArgDependencyNodeBuilder>()
        .Bind<IBuilder<DependencyGraph, IReadOnlyDictionary<Injection, Root>>>().To<RootsBuilder>()
        .Bind<IVarIdGenerator>().To<VarIdGenerator>()
        .Bind<IBuilder<DependencyGraph, string>>().To<ComposerClassBuilder>()
        .Bind<IBuilder<MdBinding, ISet<Injection>>>().To<InjectionsBuilder>()
        .Bind<IBuilder<DependencyGraph, ComposerInfo>>().To<ComposerBuilder>()
        .Bind<ITypeConstructor>().To<TypeConstructor>()
        .Bind<IBuilder<RewriterContext<MdFactory>, MdFactory>>().To<FactoryTypeRewriter>()
        .Bind<IValidator<MdSetup>>().To<MetadataValidator>()
        .Bind<ILogger<TT>>().To<Logger<TT>>()

        // Singletons
        .Default(IoC.Lifetime.Singleton)
        .Bind<IResourceManager>().To<ResourceManager>()
        .Bind<IObserversRegistry>().Bind<IObserversProvider>().To<ObserversRegistry>()
        .Bind<IClock>().To<Clock>()
        .Bind<IFormatting>().To<Formatting>()
        .Bind<ICache<IoC.TT1, IoC.TT2>>().To<Cache<IoC.TT1, IoC.TT2>>()
        .Bind<IContextInitializer>().Bind<IContextOptions>().Bind<IContextProducer>().Bind<IContextDiagnostic>().To<GeneratorContext>()
        .Bind<IResources>().To<Resources>()
        .Bind<IMarker>().To<Marker>()
        .Bind<IUnboundTypeConstructor>().To<UnboundTypeConstructor>()
        .Bind<Func<string, Regex>>().To(_ => new Func<string, Regex>(value => new Regex(value, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase)));
}