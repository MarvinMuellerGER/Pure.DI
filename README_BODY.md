# Pure DI for .NET

[![NuGet](https://buildstats.info/nuget/Pure.DI)](https://www.nuget.org/packages/Pure.DI)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[<img src="http://tcavs2015.cloudapp.net/app/rest/builds/buildType:(id:DevTeam_PureDI_Build)/statusIcon"/>](http://tcavs2015.cloudapp.net/viewType.html?buildTypeId=DevTeam_PureDI_Build&guest=1)

<img src="Docs/Images/demo.gif"/>

## Key features:

- [X] DI without any IoC/DI containers, frameworks, dependencies, and thus without any performance impacts and side-effects
- [X] A predictable and validated dependencies graph which is building and validating on the fly while you are writing your code
- [X] Does not add dependencies to any other assemblies
- [X] High performance with all .NET compiler/JIT optimizations
- [X] Easy to use
- [X] Ultra-fine tuning of generic types
- [X] Supports major .NET BCL types from the box

## [Schrödinger's cat](Samples/ShroedingersCat) shows how it works

### The reality is that

![Cat](Docs/Images/cat.png)

### Let's create an abstraction

```csharp
interface IBox<out T> { T Content { get; } }

interface ICat { State State { get; } }

enum State { Alive, Dead }
```

### Here is our implementation

```csharp
class CardboardBox<T> : IBox<T>
{
    public CardboardBox(T content) => Content = content;

    public T Content { get; }
}

class ShroedingersCat : ICat
{
  // Represents the superposition of the states
  private readonly Lazy<State> _superposition;

  public ShroedingersCat(Lazy<State> superposition) => _superposition = superposition;

  // Decoherence of the superposition at the time of observation via an irreversible process
  public State State => _superposition.Value;

  public override string ToString() => $"{State} cat";
}
```

It is important to note that our abstraction and our implementation do not know anything about any IoC/DI containers or any frameworks at all.

### Let's glue all together

#### Add a package reference to:

[![NuGet](https://buildstats.info/nuget/Pure.DI)](https://www.nuget.org/packages/Pure.DI)

- Package Manager

  ```
  Install-Package Pure.DI
  ```
  
- .NET CLI
  
  ```
  dotnet add package Pure.DI
  ```

#### Declare required dependencies in a class like:

```csharp
static partial class Composer
{
  // Models a random subatomic event that may or may not occur
  private static readonly Random Indeterminacy = new();

  static Composer() => DI.Setup()
      // Represents a quantum superposition of 2 states: Alive or Dead
      .Bind<State>().To(_ => (State)Indeterminacy.Next(2))
      // Represents schrodinger's cat
      .Bind<ICat>().To<ShroedingersCat>()
      // Represents a cardboard box with any content
      .Bind<IBox<TT>>().To<CardboardBox<TT>>()
      // Composition Root
      .Bind<Program>().As(Singleton).To<Program>();
  }
}
```

This code creates *__Composer__* to create a composition root *__Program__* below.

> Defining generic type arguments using special marker types like *__TT__* in the sample above is one of the distinguishing features of this library. So there is an easy way to bind complex generic types with nested generic types and with any type constraints.

### Time to open boxes!

```csharp
class Program
{
  // Composition Root, a single place in an application
  // where the composition of the object graphs for an application take place
  public static void Main() => Composer.Resolve<Program>().Run();

  private readonly IBox<ICat> _box;

  internal Program(IBox<ICat> box) => _box = box;

  private void Run() => Console.WriteLine(_box);
}
```

*__Program__* is a [*__Composition Root__*](https://blog.ploeh.dk/2011/07/28/CompositionRoot/) here, a single place in an application where the composition of the object graphs for an application take place. To have an ability create multiple instances or to do it on demand you could use *__Func<>__* with required type specified. Each instance is resolved by a strongly-typed block of statements like the operator [*__new__*](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator) which are compiled with all optimizations with minimal impact on performance or memory consumption. For instance, the creating of a composition root *__Program__* looks like this:
```csharp
Random Indeterminacy = new();

new Program(
  new CardboardBox<ICat>(
    new ShroedingersCat(
      new Lazy<State>(
        new Func<State>(
          () => (State)Indeterminacy.Next(2)))));
```

Take full advantage of Dependency Injection everywhere and every time without any compromise!

## Simple and powerful API

```csharp
// Starts DI configuration chain.
// This method contains a single optional argument to specify a custom DI type name to generate.
// By default, it is a name of an owner class.
DI.Setup("MyComposer")
  
  // This is a basic binding format
  .Bind<IMyInterface>().To<MyImplementation>()
   
  // Determines a binding lifetime.
  .Bind<IMyInterface>().As(Lifetime.Singleton).To<MyImplementation>()
  
  // Determines a binding tag
  .Bind<IMyInterface>().Tag("MyImpl").Tag(123).To<MyImplementation>()

  // Or a binding suitable for any tag
  .Bind<IMyInterface>().AnyTag().To<MyImplementation>()
  
  // Determines a binding implementation using a factory method.
  // It allows to create instance manually and invoke required methods,
  // initialize properties and etc. 
  .Bind<IMyInterface>().To(
    ctx => new MyImplementation(
      ctx.Resolve<ISomeDependency1>(),
      "Some value",
      ctx.Resolve<ISomeDependency2>()))

  // Overrides a default lifetime. Transient by default.
  .Default(Lifetime.Singleton)

  // Determines a custom attribute overriding an injection type.
  .TypeAttribure<MyTypeAttribute>()
  
  // Determines a tag attribute overriding an injection tag.
  .TagAttribure<MyTagAttribute>()
  
  // Determines a custom attribute overriding an injection order.
  .OrderAttribure<MyOrderAttribute>()
  
  // Use a DI configuration as a base.
  .DependsOn(nameof(BasicComposer)) 
```

Predefined lifetimes:

- *__Transient__* - Creates a new object of the requested type every time.
- *__Singleton__* - Creates a singleton object first time you and then returns the same object.
- *__PerResolve__* - Similar to the Transient, but it reuses the same object in the recursive object graph. 
- *__ContainerSingleton__* - This lifetime is applicable for ASP.NET, specifies that a single instance of the service will be created
- *__Scoped__* - This lifetime is applicable for ASP.NET, specifies that a new instance of the service will be created for each scope

You can [add a lifetime](#custom-singleton-lifetime) yourself.

## Development environment requirements

- [.NET 5.0.102+](https://dotnet.microsoft.com/download/dotnet/5.0)
- [Visual Studio 16.8+](https://visualstudio.microsoft.com/vs)
- [C# 4.0+](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history#c-version-40)

## Supported frameworks

- .NET 5.0+
- [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/) 1.0+
- [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) 1.0+
- .NET Framework 3.5+
- [UWP](https://docs.microsoft.com/en-us/windows/uwp/index) 10+

## Samples

### [ASP.NET Core Blazor](Samples/BlazorServerApp)

![blazor](Docs/Images/blazor.png?raw=true)

When a targeting project is an ASP.NET project, a special extension method is generated automatically. This extension method could be used to integrate DI into a web application infrastructure. Pay attention to [this single statement](https://github.com/DevTeam/Pure.DI/blob/d1c4cdf3d6d7015f809cf7f9153d091a1d42dc34/Samples/BlazorServerApp/Startup.cs#L24)  that makes all magic.

### [WPF](Samples/WpfAppNetCore)

![wpf](Docs/Images/wpf.png?raw=true)

This sample demonstrates how to apply DI for a WPF application. The crucial class is [DataProvider](Samples/WpfAppNetCore/DataProvider.cs), which connects view and view models. Besides that, it provides two sets of models for [design-time](Samples/WpfAppNetCore/ClockDomainDesignTime.cs) and [running](Samples/WpfAppNetCore/ClockDomain.cs) modes.

### Others

* [Schrödinger's cat](Samples/ShroedingersCat) - a simple example
* [C# script tool](https://github.com/JetBrains/teamcity-csharp-interactive/blob/master/Teamcity.CSharpInteractive/Composer.cs) - JetBrain TeamCity interactive tool for running C# scripts
* [MSBuild logger](https://github.com/JetBrains/teamcity-msbuild-logger/blob/master/TeamCity.MSBuild.Logger/Composer.cs) - Provides the JetBrain TeamCity integration with Microsoft MSBuild.
