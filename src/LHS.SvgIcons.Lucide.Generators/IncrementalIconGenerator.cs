using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace LHS.SvgIcons.Lucide.Generators;

[Generator]
public class IncrementalIconGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        #if DEBUG
                // uncomment if you want to debug the generator
                //if (!Debugger.IsAttached) Debugger.Launch();
        #endif

        var files = context.AdditionalTextsProvider
            .Where(file => file.Path?.EndsWith(".svg") == true);

        var iconsProvider = files.Select((file, ct)
            => (
                FileName: Path.GetFileNameWithoutExtension(file.Path),
                SvgContents: file.GetText(ct)?.ToString()
            )).Collect();

        context.RegisterSourceOutput(iconsProvider, (spc, ia) =>
        {
            var sb = new StringBuilder();

            foreach (var (name, svg) in ia)
            {
                var iconName = PascalCaseFromHyphenatedString(name);
                if (svg != null)
                {
                    sb.AppendLine(SvgParser.GenerateStaticClassMethod(svg, iconName));
                }
            }

            var sourceCode = $@"
using LHS.SvgIcons.Core;

namespace LHS.SvgIcons.Lucide
{{
         public static partial class LucideIconSet
         {{
                {sb}
         }}
}}";

            spc.AddSource("LucideIconSet.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
        });


    }

    private static string PascalCaseFromHyphenatedString(string input)
    {
        var sb = new StringBuilder();

        var upperNext = true;
        foreach (var ch in input.ToCharArray())
        {
            if (ch == '-')
            {
                upperNext = true;
            }
            else
            {
                sb.Append(upperNext ? char.ToUpper(ch): ch);
                upperNext = false;
            }
        }

        return sb.ToString();
    }
}