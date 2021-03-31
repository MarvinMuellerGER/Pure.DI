﻿namespace Pure.DI.Core
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal interface IObjectBuilder
    {
        ExpressionSyntax TryBuild(
            BindingMetadata binding,
            INamedTypeSymbol contractType,
            ExpressionSyntax tag,
            SemanticModel semanticModel,
            ITypeResolver typeResolver,
            int level = 0);
    }
}