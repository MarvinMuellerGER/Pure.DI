﻿/*
$v=true
$p=99
$d=Func with arguments
*/

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable VariableHidesOuterVariable
namespace Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario;

using System.Collections.Immutable;
using Shouldly;
using Xunit;

// {
interface IClock
{
    DateTimeOffset Now { get; }
}

class Clock : IClock
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}

interface IDependency
{
    int Id { get; }
}

class Dependency : IDependency
{
    public Dependency(IClock clock, int id) => 
        Id = id;

    public int Id { get; }
}

interface IService
{
    ImmutableArray<IDependency> Dependencies { get; }
}

class Service : IService
{
    public Service(Func<int, IDependency> dependencyFactory) =>
        Dependencies = Enumerable
            .Range(0, 10)
            .Select((_, index) => dependencyFactory(index))
            .ToImmutableArray();

    public ImmutableArray<IDependency> Dependencies { get; }
}
// }

public class Scenario
{
    [Fact]
    public void Run()
    {
// {    
        DI.Setup("Composition")
            .Bind<IClock>().As(Lifetime.Singleton).To<Clock>()
            // Binds a dependency of type int
            // to the source code statement "dependencyId"
            .Bind<int>().To<int>("dependencyId")
            .Bind<Func<int, IDependency>>()
                .To<Func<int, IDependency>>(ctx =>
                    dependencyId =>
                    {
                        // Builds up an instance of type Dependency
                        // referring the source code statement "dependencyId"
                        ctx.Inject<Dependency>(out var dependency);
                        return dependency;
                    })
            .Bind<IService>().To<Service>().Root<IService>("Root");

        var composition = new Composition();
        var service = composition.Root;
        service.Dependencies.Length.ShouldBe(10);
        service.Dependencies[3].Id.ShouldBe(3);
// }            
        composition.SaveClassDiagram();
    }
}