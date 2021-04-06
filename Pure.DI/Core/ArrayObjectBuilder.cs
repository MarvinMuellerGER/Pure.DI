﻿namespace Pure.DI.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ArrayObjectBuilder: IObjectBuilder
    {
        public ExpressionSyntax TryBuild(ITypeResolver typeResolver, IBindingExpressionStrategy bindingExpressionStrategy, TypeResolveDescription typeDescription)
        {
            var objectCreationExpressions = new List<ExpressionSyntax>();
            if (typeDescription.Type is IArrayTypeSymbol arrayTypeSymbol)
            {
                objectCreationExpressions.AddRange(
                    from elementTypeDescriptor in typeResolver.Resolve(arrayTypeSymbol.ElementType)
                    let objectCreationExpression = bindingExpressionStrategy.TryBuild(elementTypeDescriptor)
                    select objectCreationExpression);

                return SyntaxFactory.ArrayCreationExpression(
                        SyntaxFactory.ArrayType(arrayTypeSymbol.ElementType.ToTypeSyntax(typeDescription.SemanticModel)))
                    .AddTypeRankSpecifiers(SyntaxFactory.ArrayRankSpecifier().AddSizes(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(objectCreationExpressions.Count))))
                    .WithInitializer(
                        SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression)
                            .AddExpressions(objectCreationExpressions.ToArray()));
            }
            
            return SyntaxFactory.ImplicitArrayCreationExpression(SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression)
                .AddExpressions(objectCreationExpressions.ToArray()));
        }
    }
}
