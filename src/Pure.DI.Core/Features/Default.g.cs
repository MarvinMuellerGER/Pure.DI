// <auto-generated/>
#if !PUREDI_API_SUPPRESSION || PUREDI_API_V2
#pragma warning disable

namespace Pure.DI
{
    internal static class Default
    {
        [global::System.Diagnostics.Conditional("A2768DE22DE3E430C9653990D516CC9B")]
        private static void Setup()
        {
            DI.Setup("", CompositionKind.Global)
                .TypeAttribute<TypeAttribute>()
                .TagAttribute<TagAttribute>()
                .OrdinalAttribute<OrdinalAttribute>()
                .Accumulate<global::System.IDisposable, global::Pure.DI.Owned>(
                    Lifetime.Transient,
                    Lifetime.PerResolve,
                    Lifetime.PerBlock)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                .Accumulate<global::System.IAsyncDisposable, global::Pure.DI.Owned>(
                    Lifetime.Transient,
                    Lifetime.PerResolve,
                    Lifetime.PerBlock)
#endif
                .Bind<global::Pure.DI.IOwned>().To(ctx =>
                {
                    ctx.Inject<Owned>(out var owned);
                    return owned;
                })
                .Bind<global::Pure.DI.Owned<TT>>()
                    .As(Lifetime.PerBlock)
                    .To(ctx => {
                        ctx.Inject<Owned>(out var owned);
                        ctx.Inject<TT>(ctx.Tag, out var value);
                        return new Owned<TT>(value, owned);
                    })
                .Bind<global::System.Func<TT>>()
                    .As(Lifetime.PerResolve)
                    .To(ctx => new global::System.Func<TT>(() =>
                    {
                        ctx.Inject<TT>(ctx.Tag, out var value);
                        return value;
                    }))
                .Bind<global::System.Collections.Generic.IComparer<TT>>()
                .Bind<global::System.Collections.Generic.Comparer<TT>>()
                    .To(_ => global::System.Collections.Generic.Comparer<TT>.Default)
                .Bind<global::System.Collections.Generic.IEqualityComparer<TT>>()
                .Bind<global::System.Collections.Generic.EqualityComparer<TT>>()
                    .To(_ => global::System.Collections.Generic.EqualityComparer<TT>.Default)
#if NETSTANDARD || NET || NETCOREAPP || NET40_OR_GREATER
                .Bind<global::System.Lazy<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<global::System.Func<TT>>(ctx.Tag, out var factory);
                        return new global::System.Lazy<TT>(factory, true);
                    })
                .Bind<global::System.Threading.CancellationToken>().To(_ => global::System.Threading.CancellationToken.None)
                .Bind<global::System.Threading.Tasks.TaskScheduler>()
                    .To(_ => global::System.Threading.Tasks.TaskScheduler.Default)
                .Bind<global::System.Threading.Tasks.TaskCreationOptions>()
                    .To(_ => global::System.Threading.Tasks.TaskCreationOptions.None)
                .Bind<global::System.Threading.Tasks.TaskContinuationOptions>()
                    .To(_ => global::System.Threading.Tasks.TaskContinuationOptions.None)
                .Bind<global::System.Threading.Tasks.TaskFactory>().As(Lifetime.PerBlock)
                    .To(ctx =>
                    {
                        ctx.Inject(out global::System.Threading.CancellationToken cancellationToken);
                        ctx.Inject(out global::System.Threading.Tasks.TaskCreationOptions taskCreationOptions);
                        ctx.Inject(out global::System.Threading.Tasks.TaskContinuationOptions taskContinuationOptions);
                        ctx.Inject(out global::System.Threading.Tasks.TaskScheduler taskScheduler);
                        return new global::System.Threading.Tasks.TaskFactory(cancellationToken, taskCreationOptions, taskContinuationOptions, taskScheduler);
                    })
                .Bind<global::System.Threading.Tasks.TaskFactory<TT>>().As(Lifetime.PerBlock)
                    .To(ctx =>
                    {
                        ctx.Inject(out global::System.Threading.CancellationToken cancellationToken);
                        ctx.Inject(out global::System.Threading.Tasks.TaskCreationOptions taskCreationOptions);
                        ctx.Inject(out global::System.Threading.Tasks.TaskContinuationOptions taskContinuationOptions);
                        ctx.Inject(out global::System.Threading.Tasks.TaskScheduler taskScheduler);
                        return new global::System.Threading.Tasks.TaskFactory<TT>(cancellationToken, taskCreationOptions, taskContinuationOptions, taskScheduler);
                    })
                .Bind<global::System.Threading.Tasks.Task<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject(ctx.Tag, out global::System.Func<TT> factory);
                        ctx.Inject(out global::System.Threading.Tasks.TaskFactory<TT> taskFactory);
                        return taskFactory.StartNew(factory);
                    })
#endif                
#if NETSTANDARD2_1_OR_GREATER || NET || NETCOREAPP                
                .Bind<global::System.Threading.Tasks.ValueTask<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject(ctx.Tag, out TT value);
                        return new global::System.Threading.Tasks.ValueTask<TT>(value);
                    })
#endif                
#if NETSTANDARD || NET || NETCOREAPP                
                .Bind<global::System.Lazy<TT, TT1>>()
                    .To(ctx =>
                    {
                        ctx.Inject<global::System.Func<TT>>(ctx.Tag, out var factory);
                        ctx.Inject<TT1>(ctx.Tag, out var metadata);
                        return new global::System.Lazy<TT, TT1>(factory, metadata, true);
                    })
#endif
                // Collections
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
                .Bind<global::System.Memory<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Memory<TT>(arr);
                    })
                .Bind<global::System.ReadOnlyMemory<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.ReadOnlyMemory<TT>(arr);
                    })
                .Bind<global::System.Buffers.MemoryPool<TT>>()
                    .To(_ => global::System.Buffers.MemoryPool<TT>.Shared)
                .Bind<global::System.Buffers.ArrayPool<TT>>()
                    .To(_ => global::System.Buffers.ArrayPool<TT>.Shared)
#endif                
                .Bind<global::System.Collections.Generic.ICollection<TT>>()
                .Bind<global::System.Collections.Generic.IList<TT>>()
                .Bind<global::System.Collections.Generic.List<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Generic.List<TT>(arr);
                    })
#if NETSTANDARD || NET || NETCOREAPP || NET45_OR_GREATER
                .Bind<global::System.Collections.Generic.IReadOnlyCollection<TT>>()
                .Bind<global::System.Collections.Generic.IReadOnlyList<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return arr;
                    })
#endif
#if NETSTANDARD1_1_OR_GREATER || NET || NETCOREAPP || NET40_OR_GREATER
                .Bind<global::System.Collections.Concurrent.IProducerConsumerCollection<TT>>()
                .Bind<global::System.Collections.Concurrent.ConcurrentBag<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Concurrent.ConcurrentBag<TT>(arr);
                    })
                .Bind<global::System.Collections.Concurrent.ConcurrentQueue<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Concurrent.ConcurrentQueue<TT>(arr);
                    })
                .Bind<global::System.Collections.Concurrent.ConcurrentStack<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Concurrent.ConcurrentStack<TT>(arr);
                    })
                .Bind<global::System.Collections.Concurrent.BlockingCollection<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<global::System.Collections.Concurrent.ConcurrentBag<TT>>(out var concurrentBag);
                        return new global::System.Collections.Concurrent.BlockingCollection<TT>(concurrentBag);
                    })
#endif
#if NETSTANDARD || NET || NETCOREAPP || NET40_OR_GREATER                
                .Bind<global::System.Collections.Generic.ISet<TT>>()
#endif
#if NETSTANDARD || NET || NETCOREAPP || NET35_OR_GREATER
                .Bind<global::System.Collections.Generic.HashSet<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Generic.HashSet<TT>(arr);
                    })
#endif
#if NETSTANDARD || NET || NETCOREAPP || NET45_OR_GREATER
                .Bind<global::System.Collections.Generic.SortedSet<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Generic.SortedSet<TT>(arr);
                    })
#endif                
                .Bind<global::System.Collections.Generic.Queue<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Generic.Queue<TT>(arr);
                    })
                .Bind<global::System.Collections.Generic.Stack<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return new global::System.Collections.Generic.Stack<TT>(arr);
                    })
#if NETCOREAPP || NET                
#if NETCOREAPP3_0_OR_GREATER
                .Bind<global::System.Collections.Immutable.ImmutableArray<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableArray<TT>>(ref arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableList<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableList<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableList<TT>>(ref arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableSet<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableHashSet<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableHashSet<TT>>(ref arr);
                    })
                .Bind<global::System.Collections.Immutable.ImmutableSortedSet<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableSortedSet<TT>>(ref arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableQueue<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableQueue<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableQueue<TT>>(ref arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableStack<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableStack<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableStack<TT>>(ref arr);
                    })
#else                
                .Bind<global::System.Collections.Immutable.ImmutableArray<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Collections.Immutable.ImmutableArray.Create<TT>(arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableList<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableList<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Collections.Immutable.ImmutableList.Create<TT>(arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableSet<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableHashSet<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Collections.Immutable.ImmutableHashSet.Create<TT>(arr);
                    })
                .Bind<global::System.Collections.Immutable.ImmutableSortedSet<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Collections.Immutable.ImmutableSortedSet.Create<TT>(arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableQueue<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableQueue<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Collections.Immutable.ImmutableQueue.Create<TT>(arr);
                    })
                .Bind<global::System.Collections.Immutable.IImmutableStack<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableStack<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject<TT[]>(out var arr);
                        return global::System.Collections.Immutable.ImmutableStack.Create<TT>(arr);
                    })
#endif
#endif
#if NET6_0_OR_GREATER
                .Bind<global::System.Random>().To(_ => global::System.Random.Shared)
#endif
                    ;
        }
    }
}
#pragma warning restore
#endif
