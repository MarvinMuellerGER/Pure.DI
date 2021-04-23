﻿namespace Pure.DI.Core
{
    using Microsoft.CodeAnalysis.CSharp;

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class StaticResolveMethodBuilder : IResolveMethodBuilder
    {
        public ResolveMethod Build()
        {
            var resolve = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ParseName(SyntaxRepo.FactoriesTableName),
                            SyntaxFactory.Token(SyntaxKind.DotToken),
                            SyntaxFactory.IdentifierName(nameof(ResolversTable.Resolve))))
                    .AddArgumentListArguments(
                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("type")));

            return new ResolveMethod(
                SyntaxRepo.StaticResolveMethodSyntax.AddBodyStatements(
                    SyntaxFactory.ReturnStatement(resolve)));
        }
    }
}