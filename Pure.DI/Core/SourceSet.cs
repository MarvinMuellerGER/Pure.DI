namespace Pure.DI.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;

    internal class SourceSet
    {
        private static readonly List<(string name, string text)> Features;
        private static readonly List<(string name, string text)> Components;
        public readonly IReadOnlyList<Source> ComponentSources;
        public readonly IEnumerable<SyntaxTree> FeaturesTrees;
        public readonly IEnumerable<SyntaxTree> ComponentsTrees;

        static SourceSet()
        {
            Regex featuresRegex = new(@"Pure.DI.Features.[\w]+.cs", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex componentsRegex = new(@"Pure.DI.Components.[\w]+.cs", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Features = GetResources(featuresRegex).Select(i => (i.file, i.code)).ToList();
            Components = GetResources(componentsRegex).Select(i => (i.file, i.code)).ToList();
        }

        public SourceSet(CSharpParseOptions parseOptions)
        {
            ComponentSources = CreateSources(Components).ToList();
            FeaturesTrees = Features.Select(source => CSharpSyntaxTree.ParseText(source.text, parseOptions));
            ComponentsTrees = Components.Select(source => CSharpSyntaxTree.ParseText(source.text, parseOptions));
        }

        private static IEnumerable<Source> CreateSources(IEnumerable<(string name, string text)> sources) =>
            from source in sources 
            select new Source(source.name, SourceText.From(source.text, Encoding.UTF8));
        
        private static IEnumerable<(string file, string code)> GetResources(Regex filter)
        {
            var assembly = typeof(SourceSet).Assembly;
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (!filter.IsMatch(resourceName))
                {
                    continue;
                }

                using var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Cannot read {resourceName}."));
                var code = reader.ReadToEnd();
                yield return (resourceName, code);
            }
        }
    }
}