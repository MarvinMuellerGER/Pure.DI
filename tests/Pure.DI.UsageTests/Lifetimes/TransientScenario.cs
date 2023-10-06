﻿/*
$v=true
$p=2
$d=Transient
$h=The _Transient _ lifetime is the default lifetime and can be omitted.
*/

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeTypeModifiers
namespace Pure.DI.UsageTests.Lifetimes.TransientScenario;

using Xunit;

// {
interface IDependency { }

class Dependency : IDependency { }

interface IService
{
    public IDependency Dependency1 { get; }
            
    public IDependency Dependency2 { get; }
}

class Service : IService
{
    public Service(
        IDependency dependency1,
        IDependency dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency Dependency1 { get; }
            
    public IDependency Dependency2 { get; }
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
        DI.Setup("Composition")
            .Bind<IDependency>().As(Lifetime.Transient).To<Dependency>()
            .Bind<IService>().To<Service>().Root<IService>("Root");

        var composition = new Composition();
        var service1 = composition.Root;
        var service2 = composition.Root;
        service1.Dependency1.ShouldNotBe(service1.Dependency2);
        service2.Dependency1.ShouldNotBe(service1.Dependency1);
// }
        composition.SaveClassDiagram();
    }
}