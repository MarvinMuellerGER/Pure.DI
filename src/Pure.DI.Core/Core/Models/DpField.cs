﻿namespace Pure.DI.Core.Models;

internal readonly record struct DpField(
    IFieldSymbol Field,
    int? Ordinal,
    Injection Injection)
{
    public override string ToString() => $"{Field}<--{Injection.ToString()}";
}