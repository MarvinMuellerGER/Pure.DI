﻿namespace Pure.DI.IntegrationTests;

using System.Collections.Immutable;

[Collection(nameof(NonParallelTestsCollectionDefinition))]
public class ArgsTests
{
    [Fact]
    public async Task ShouldSupportArg()
    {
        // Given

        // When
        var result = await """
using System;
using Pure.DI;

namespace Sample
{
    interface IDependency {}

    class Dependency: IDependency {}

    interface IService
    {
        IDependency Dep { get; }

        string Name { get; }
    }

    class Service: IService 
    {
        public Service(IDependency dep, string name)
        { 
            Dep = dep;
            Name = name;
        }

        public IDependency Dep { get; }

        public string Name { get; private set; }
    }

    static class Setup
    {
        private static void SetupComposer()
        {
            DI.Setup("Composer")
                .Bind<IDependency>().As(Lifetime.Singleton).To<Dependency>()
                .Bind<IService>().To<Service>()    
                .Arg<string>("serviceName")           
                .Root<IService>("Service");
        }
    }

    public class Program
    {
        public static void Main()
        {
            var composer = new Composer("Some Name");
            Console.WriteLine(composer.Service.Name);                               
        }
    }                
}
""".RunAsync();

        // Then
        result.StdOut.ShouldBe(ImmutableArray.Create("Some Name"), result.GeneratedCode);
    }

    [Fact]
    public async Task ShouldSupportSeveralArgs()
    {
        // Given

        // When
        var result = await """
using System;
using Pure.DI;

namespace Sample
{
    interface IDependency {}

    class Dependency: IDependency
    {
        int _id;

        public Dependency(int id)
        { 
            _id = id;            
        }

        public override string ToString() => _id.ToString();
    }

    interface IService
    {
        IDependency Dep { get; }

        string Name { get; }
    }

    class Service: IService 
    {
        int _id;
        string _name;

        public Service(IDependency dep, [Tag(99)] int id, string name)
        { 
            Dep = dep;
            _id = id;
            _name = name;
        }

        public IDependency Dep { get; }

        public string Name => $"{_name} {_id} {Dep}";
    }

    static class Setup
    {
        private static void SetupComposer()
        {
            DI.Setup("Composer")
                .Bind<IDependency>().As(Lifetime.Singleton).To<Dependency>()
                .Bind<IService>().To<Service>()    
                .Arg<string>("serviceName")           
                .Arg<int>("id", 99)
                .Arg<int>("depId")
                .Root<IService>("Service");
        }
    }

    public class Program
    {
        public static void Main()
        {
            var composer = new Composer("Some Name", 37, 56);
            Console.WriteLine(composer.Service.Name);                               
        }
    }                
}
""".RunAsync();

        // Then
        result.StdOut.ShouldBe(ImmutableArray.Create("Some Name 37 56"), result.GeneratedCode);
    }
}