#### Tag Unique

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](../tests/Pure.DI.UsageTests/Basics/TagUniqueScenario.cs)

`Tag.Unique` is useful to register a binding with a unique tag. It will not be available through the composition root or `Resolve` methods directly, but can be embedded in compositions as some kind of enumeration.

```c#
interface IDependency<T>;

class AbcDependency<T> : IDependency<T>;

class XyzDependency<T> : IDependency<T>;

interface IService<T>
{
    ImmutableArray<IDependency<T>> Dependencies { get; }
}

class Service<T>(IEnumerable<IDependency<T>> dependencies) : IService<T>
{
    public ImmutableArray<IDependency<T>> Dependencies { get; }
        = dependencies.ToImmutableArray();
}

DI.Setup(nameof(Composition))
    .Bind<IDependency<TT>>(Tag.Unique).To<AbcDependency<TT>>()
    .Bind<IDependency<TT>>(Tag.Unique).To<XyzDependency<TT>>()
    .Bind<IService<TT>>().To<Service<TT>>()
        .Root<IService<string>>("Root");

var composition = new Composition();
var stringService = composition.Root;
stringService.Dependencies.Length.ShouldBe(2);
```

<details open>
<summary>Class Diagram</summary>

```mermaid
classDiagram
  class Composition {
    +IServiceᐸStringᐳ Root
    + T ResolveᐸTᐳ()
    + T ResolveᐸTᐳ(object? tag)
    + object Resolve(Type type)
    + object Resolve(Type type, object? tag)
  }
  class IEnumerableᐸIDependencyᐸStringᐳᐳ
  AbcDependencyᐸStringᐳ --|> IDependencyᐸStringᐳ : 7d2ddd11-ab70-439c-9a3e-37e44b47b2f4 
  class AbcDependencyᐸStringᐳ {
    +AbcDependency()
  }
  XyzDependencyᐸStringᐳ --|> IDependencyᐸStringᐳ : 8d646619-d717-4e36-8f7b-2cda82e59a61 
  class XyzDependencyᐸStringᐳ {
    +XyzDependency()
  }
  ServiceᐸStringᐳ --|> IServiceᐸStringᐳ : 
  class ServiceᐸStringᐳ {
    +Service(IEnumerableᐸIDependencyᐸStringᐳᐳ dependencies)
  }
  class IDependencyᐸStringᐳ {
    <<abstract>>
  }
  class IServiceᐸStringᐳ {
    <<abstract>>
  }
  IEnumerableᐸIDependencyᐸStringᐳᐳ *--  AbcDependencyᐸStringᐳ : 7d2ddd11-ab70-439c-9a3e-37e44b47b2f4  IDependencyᐸStringᐳ
  IEnumerableᐸIDependencyᐸStringᐳᐳ *--  XyzDependencyᐸStringᐳ : 8d646619-d717-4e36-8f7b-2cda82e59a61  IDependencyᐸStringᐳ
  Composition ..> ServiceᐸStringᐳ : IServiceᐸStringᐳ Root
  ServiceᐸStringᐳ o--  "PerBlock" IEnumerableᐸIDependencyᐸStringᐳᐳ : IEnumerableᐸIDependencyᐸStringᐳᐳ
```

</details>

<details>
<summary>Pure.DI-generated partial class Composition</summary><blockquote>

```c#
/// <para>
/// Composition roots:<br/>
/// <list type="table">
/// <listheader>
/// <term>Root</term>
/// <description>Description</description>
/// </listheader>
/// <item>
/// <term>
/// <see cref="Pure.DI.UsageTests.Basics.TagUniqueScenario.Service&lt;string&gt;"/> Root
/// </term>
/// <description>
/// </description>
/// </item>
/// </list>
/// </para>
/// <example>
/// This shows how to get an instance of type <see cref="Pure.DI.UsageTests.Basics.TagUniqueScenario.Service<string>"/> using the composition root <see cref="Root"/>:
/// <code>
/// var composition = new Composition();
/// var instance = composition.Root;
/// </code>
/// </example>
/// <a href="https://mermaid.live/view#pako:eNqtVc1ygjAQfpVMzj3ITwl4U7Ez3jrqoQcuIVktLRAH0Bnr-A6-Sy99Hd-kQLRgC5HWXnYW9tvs9202yQ4zwQH3MQtpmroBXSY08hIvLr_RSEQrkQZZIGLkrXs9MixihacPJzNINgGD4-FjliVBvDwe3tFUiKyCoDmaQirCTQGa5_Hyt11a5ypM-C_AssI3HlBGlz_SJOCcK5Pm2xWgLDd_QI-QuiRxq85MxvE6goT6YUF64sIKYg4x29bbUUjJMwY-a46XPdXdypJRWXuMJooEY4AI1znnmiYzqU960jMNh0nPoYYU5RoETNM3ia8vTFQJUJOqb_QFsmkHZV-etm__rNLmlmlZmiMzOdHISSUYlvTsBfGlpzNObR3uHWppNZVqUnWVF8h2lU1D366vDZ2Lqzi2gersThjJq-vsIX6OBJAqRrlThwwpakT9NEvo6ZCMT7Zp1euyui_ZWXGpclDfEHRl0m85TW2tu5myemxvORoqyt9ve73cC2nzaVYMs_ItaIqJS70efoRkGAr26uHOV-tX8V9cxfgO59CIBjx_83Yezp4hAg_3Pcxpkhff4_0nC9aoeQ">Class diagram</a><br/>
/// This class was created by <a href="https://github.com/DevTeam/Pure.DI">Pure.DI</a> source code generator.
/// </summary>
/// <seealso cref="Pure.DI.DI.Setup"/>
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
partial class Composition
{
  private readonly Composition _rootM03D08di;
  
  /// <summary>
  /// This constructor creates a new instance of <see cref="Composition"/>.
  /// </summary>
  public Composition()
  {
    _rootM03D08di = this;
  }
  
  /// <summary>
  /// This constructor creates a new instance of <see cref="Composition"/> scope based on <paramref name="baseComposition"/>. This allows the <see cref="Lifetime.Scoped"/> life time to be applied.
  /// </summary>
  /// <param name="baseComposition">Base composition.</param>
  internal Composition(Composition baseComposition)
  {
    _rootM03D08di = baseComposition._rootM03D08di;
  }
  
  #region Composition Roots
  public Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string> Root
  {
    #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER || NET
    [global::System.Diagnostics.Contracts.Pure]
    #endif
    get
    {
      [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x200)]
      System.Collections.Generic.IEnumerable<Pure.DI.UsageTests.Basics.TagUniqueScenario.IDependency<string>> LocalperBlockM03D08di1_IEnumerable()
      {
          yield return new Pure.DI.UsageTests.Basics.TagUniqueScenario.AbcDependency<string>();
          yield return new Pure.DI.UsageTests.Basics.TagUniqueScenario.XyzDependency<string>();
      }
      var perBlockM03D08di1_IEnumerable = LocalperBlockM03D08di1_IEnumerable();
      return new Pure.DI.UsageTests.Basics.TagUniqueScenario.Service<string>(perBlockM03D08di1_IEnumerable);
    }
  }
  #endregion
  
  #region API
  /// <summary>
  /// Resolves the composition root.
  /// </summary>
  /// <typeparam name="T">The type of the composition root.</typeparam>
  /// <returns>A composition root.</returns>
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER || NET
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  public T Resolve<T>()
  {
    return ResolverM03D08di<T>.Value.Resolve(this);
  }
  
  /// <summary>
  /// Resolves the composition root by tag.
  /// </summary>
  /// <typeparam name="T">The type of the composition root.</typeparam>
  /// <param name="tag">The tag of a composition root.</param>
  /// <returns>A composition root.</returns>
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER || NET
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  public T Resolve<T>(object? tag)
  {
    return ResolverM03D08di<T>.Value.ResolveByTag(this, tag);
  }
  
  /// <summary>
  /// Resolves the composition root.
  /// </summary>
  /// <param name="type">The type of the composition root.</param>
  /// <returns>A composition root.</returns>
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER || NET
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  public object Resolve(global::System.Type type)
  {
    var index = (int)(_bucketSizeM03D08di * ((uint)global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(type) % 1));
    var finish = index + _bucketSizeM03D08di;
    do {
      ref var pair = ref _bucketsM03D08di[index];
      if (ReferenceEquals(pair.Key, type))
      {
        return pair.Value.Resolve(this);
      }
    } while (++index < finish);
    
    throw new global::System.InvalidOperationException($"Cannot resolve composition root of type {type}.");
  }
  
  /// <summary>
  /// Resolves the composition root by tag.
  /// </summary>
  /// <param name="type">The type of the composition root.</param>
  /// <param name="tag">The tag of a composition root.</param>
  /// <returns>A composition root.</returns>
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER || NET
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  public object Resolve(global::System.Type type, object? tag)
  {
    var index = (int)(_bucketSizeM03D08di * ((uint)global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(type) % 1));
    var finish = index + _bucketSizeM03D08di;
    do {
      ref var pair = ref _bucketsM03D08di[index];
      if (ReferenceEquals(pair.Key, type))
      {
        return pair.Value.ResolveByTag(this, tag);
      }
    } while (++index < finish);
    
    throw new global::System.InvalidOperationException($"Cannot resolve composition root \"{tag}\" of type {type}.");
  }
  #endregion
  
  /// <summary>
  /// This method provides a class diagram in mermaid format. To see this diagram, simply call the method and copy the text to this site https://mermaid.live/.
  /// </summary>
  public override string ToString()
  {
    return
      "classDiagram\n" +
        "  class Composition {\n" +
          "    +IServiceᐸStringᐳ Root\n" +
          "    + T ResolveᐸTᐳ()\n" +
          "    + T ResolveᐸTᐳ(object? tag)\n" +
          "    + object Resolve(Type type)\n" +
          "    + object Resolve(Type type, object? tag)\n" +
        "  }\n" +
        "  class IEnumerableᐸIDependencyᐸStringᐳᐳ\n" +
        "  AbcDependencyᐸStringᐳ --|> IDependencyᐸStringᐳ : 7d2ddd11-ab70-439c-9a3e-37e44b47b2f4 \n" +
        "  class AbcDependencyᐸStringᐳ {\n" +
          "    +AbcDependency()\n" +
        "  }\n" +
        "  XyzDependencyᐸStringᐳ --|> IDependencyᐸStringᐳ : 8d646619-d717-4e36-8f7b-2cda82e59a61 \n" +
        "  class XyzDependencyᐸStringᐳ {\n" +
          "    +XyzDependency()\n" +
        "  }\n" +
        "  ServiceᐸStringᐳ --|> IServiceᐸStringᐳ : \n" +
        "  class ServiceᐸStringᐳ {\n" +
          "    +Service(IEnumerableᐸIDependencyᐸStringᐳᐳ dependencies)\n" +
        "  }\n" +
        "  class IDependencyᐸStringᐳ {\n" +
          "    <<abstract>>\n" +
        "  }\n" +
        "  class IServiceᐸStringᐳ {\n" +
          "    <<abstract>>\n" +
        "  }\n" +
        "  IEnumerableᐸIDependencyᐸStringᐳᐳ *--  AbcDependencyᐸStringᐳ : 7d2ddd11-ab70-439c-9a3e-37e44b47b2f4  IDependencyᐸStringᐳ\n" +
        "  IEnumerableᐸIDependencyᐸStringᐳᐳ *--  XyzDependencyᐸStringᐳ : 8d646619-d717-4e36-8f7b-2cda82e59a61  IDependencyᐸStringᐳ\n" +
        "  Composition ..> ServiceᐸStringᐳ : IServiceᐸStringᐳ Root\n" +
        "  ServiceᐸStringᐳ o--  \"PerBlock\" IEnumerableᐸIDependencyᐸStringᐳᐳ : IEnumerableᐸIDependencyᐸStringᐳᐳ";
  }
  
  private readonly static int _bucketSizeM03D08di;
  private readonly static global::Pure.DI.Pair<global::System.Type, global::Pure.DI.IResolver<Composition, object>>[] _bucketsM03D08di;
  
  static Composition()
  {
    var valResolverM03D08di_0000 = new ResolverM03D08di_0000();
    ResolverM03D08di<Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string>>.Value = valResolverM03D08di_0000;
    _bucketsM03D08di = global::Pure.DI.Buckets<global::System.Type, global::Pure.DI.IResolver<Composition, object>>.Create(
      1,
      out _bucketSizeM03D08di,
      new global::Pure.DI.Pair<global::System.Type, global::Pure.DI.IResolver<Composition, object>>[1]
      {
         new global::Pure.DI.Pair<global::System.Type, global::Pure.DI.IResolver<Composition, object>>(typeof(Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string>), valResolverM03D08di_0000)
      });
  }
  
  #region Resolvers
  private sealed class ResolverM03D08di<T>: global::Pure.DI.IResolver<Composition, T>
  {
    public static global::Pure.DI.IResolver<Composition, T> Value = new ResolverM03D08di<T>();
    
    public T Resolve(Composition composite)
    {
      throw new global::System.InvalidOperationException($"Cannot resolve composition root of type {typeof(T)}.");
    }
    
    public T ResolveByTag(Composition composite, object tag)
    {
      throw new global::System.InvalidOperationException($"Cannot resolve composition root \"{tag}\" of type {typeof(T)}.");
    }
  }
  
  private sealed class ResolverM03D08di_0000: global::Pure.DI.IResolver<Composition, Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string>>
  {
    public Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string> Resolve(Composition composition)
    {
      return composition.Root;
    }
    
    public Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string> ResolveByTag(Composition composition, object tag)
    {
      switch (tag)
      {
        case null:
          return composition.Root;
      }
      throw new global::System.InvalidOperationException($"Cannot resolve composition root \"{tag}\" of type Pure.DI.UsageTests.Basics.TagUniqueScenario.IService<string>.");
    }
  }
  #endregion
}
```

</blockquote></details>
