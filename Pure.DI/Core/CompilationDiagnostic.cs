﻿namespace Pure.DI.Core
{
    using Microsoft.CodeAnalysis;

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class CompilationDiagnostic : IDiagnostic
    {
        private readonly GeneratorExecutionContext _context;

        public CompilationDiagnostic(GeneratorExecutionContext context) => _context = context;

        public void Error(string id, string message, Location? location)
        {
            _context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    id,
                    "Error",
                    message + GetLine(location),
                    "Error",
                    DiagnosticSeverity.Error,
                    true),
                location));

            throw new HandledException(message);
        }

        public void Warning(string id, string message, Location? location) =>
            _context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    id,
                    "Warning",
                    message + GetLine(location),
                    "Warning",
                    DiagnosticSeverity.Warning,
                    true),
                location));

        private static string GetLine(Location? location)
        {
            if (location == null || !location.IsInSource)
            {
                return string.Empty;
            }

            var line = location.SourceTree.ToString().Substring(location.SourceSpan.Start, location.SourceSpan.Length);
            return $" at line {location.GetMappedLineSpan().StartLinePosition.Line + 1}: {line}";
        }
    }
}
