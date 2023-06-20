﻿/*
$v=true
$p=3
$d=OnCannotResolve Hint
$h=Hints are used to fine-tune code generation. The _OnCannotResolve_ hint determines whether to generate a partial `OnCannotResolve<T>(...)` method to handle a scenario where an instance which cannot be resolved.
$h=In addition, setup hints can be comments before the _Setup_ method in the form ```hint = value```, for example: `// OnCannotResolveContractTypeNameRegularExpression = string`.
*/

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameterInPartialMethod
// ReSharper disable UnusedVariable
namespace Pure.DI.UsageTests.Hints.OnCannotResolveHintScenario;

using System.Diagnostics.CodeAnalysis;
using Shouldly;
using Xunit;

// {
using static Hint;

public interface IDependency
{
}

public class Dependency : IDependency
{
    private readonly string _name;

    public Dependency(string name)
    {
        _name = name;
    }

    public override string ToString() => _name;
}

public interface IService
{
    IDependency Dependency { get; }
}

public class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }
    
}

internal partial class Composition
{
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private partial T OnCannotResolve<T>(object? tag, Lifetime lifetime)
    {
        if (typeof(T) == typeof(string))
        {
            return (T)(object)"Dependency with name";
        }

        throw new InvalidOperationException("Cannot resolve.");
    }
}
// }

public class Scenario
{
    [Fact]
    public void Run()
    {
        // ToString = On
        // FormatCode = On
// {            
        // OnCannotResolveContractTypeNameRegularExpression = string
        DI.Setup("Composition")
            .Hint(OnCannotResolve, "On")
            .Bind<IDependency>().To<Dependency>()
            .Bind<IService>().Tags().To<Service>().Root<IService>("Root");

        var composition = new Composition();
        var service = composition.Root;
        service.Dependency.ToString().ShouldBe("Dependency with name");
        
// }
        TestTools.SaveClassDiagram(composition, nameof(OnCannotResolveHintScenario));
    }
}