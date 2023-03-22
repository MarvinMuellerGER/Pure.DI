namespace Pure.DI.Core.CSharp;

internal class DefaultConstructorBuilder: IBuilder<CompositionCode, CompositionCode>
{
    public CompositionCode Build(CompositionCode composition, CancellationToken cancellationToken)
    {
        if (composition.Args.Any())
        {
            return composition;
        }

        var code = composition.Code;
        var membersCounter = composition.MembersCount;
        if (membersCounter > 0)
        {
            code.AppendLine();
        }

        code.AppendLine($"public {composition.ClassName}()");
        code.AppendLine("{");
        if (composition.Singletons.Any())
        {
            using (code.Indent())
            {
                code.AppendLine($"{Variable.DisposablesFieldName} = new {CodeConstants.DisposableTypeName}[{composition.DisposableSingletonsCount}];");
            }
        }

        code.AppendLine("}");
        membersCounter++;
        return composition with { MembersCount = membersCounter };
    }
}