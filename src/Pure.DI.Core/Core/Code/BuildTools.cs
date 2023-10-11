﻿// ReSharper disable InvertIf
// ReSharper disable ClassNeverInstantiated.Global
namespace Pure.DI.Core.Code;

internal class BuildTools : IBuildTools
{
    private readonly IFilter _filter;

    public BuildTools(IFilter filter) => _filter = filter;

    public string GetDeclaration(Variable variable, bool typeIsRequired = false) =>
        variable.IsDeclared ? "" : typeIsRequired ? $"{variable.InstanceType} " : "var ";
    
    public string OnInjected(BuildContext ctx, Variable variable)
    {
        if (ctx.DependencyGraph.Source.Hints.GetHint(Hint.OnDependencyInjection) != SettingState.On)
        {
            return variable.VariableName;
        }

        if (!_filter.IsMeetRegularExpression(
                ctx.DependencyGraph.Source,
                (Hint.OnDependencyInjectionImplementationTypeNameRegularExpression, variable.InstanceType.ToString()),
                (Hint.OnDependencyInjectionContractTypeNameRegularExpression, variable.ContractType.ToString()),
                (Hint.OnDependencyInjectionTagRegularExpression, variable.Injection.Tag.ValueToString()),
                (Hint.OnDependencyInjectionLifetimeRegularExpression, variable.Node.Lifetime.ValueToString())))
        {
            return variable.VariableName;
        }
        
        var tag = GetTag(ctx, variable);
        return $"{Names.OnDependencyInjectionMethodName}<{variable.ContractType}>({variable.VariableName}, {tag.ValueToString()}, {variable.Node.Lifetime.ValueToString()})";
    }
    
    public IEnumerable<Line> OnCreated(BuildContext ctx, Variable variable)
    {
        if (variable.Node.Arg is not null)
        {
            yield break;
        }

        if (ctx.DependencyGraph.Source.Hints.GetHint(Hint.OnNewInstance) != SettingState.On)
        {
            yield break;
        }

        if (!_filter.IsMeetRegularExpression(
                ctx.DependencyGraph.Source,
                (Hint.OnNewInstanceImplementationTypeNameRegularExpression, variable.Node.Type.ToString()),
                (Hint.OnNewInstanceTagRegularExpression, variable.Injection.Tag.ValueToString()),
                (Hint.OnNewInstanceLifetimeRegularExpression, variable.Node.Lifetime.ValueToString())))
        {
            yield break;
        }

        var tag = GetTag(ctx, variable);
        yield return new Line(0, $"{Names.OnNewInstanceMethodName}<{variable.InstanceType}>(ref {variable.VariableName}, {tag.ValueToString()}, {variable.Node.Lifetime.ValueToString()})" + ";");
    }

    private static object? GetTag(BuildContext ctx, Variable variable)
    {
        var tag = variable.Injection.Tag;
        if (ReferenceEquals(tag, MdTag.ContextTag))
        {
            tag = ctx.ContextTag;
        }

        return tag;
    }
}