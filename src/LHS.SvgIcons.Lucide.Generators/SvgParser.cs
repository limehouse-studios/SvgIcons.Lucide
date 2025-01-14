using System.Linq;
using System.Xml.Linq;

namespace LHS.SvgIcons.Lucide.Generators;

public class SvgParser
{
    public static string GenerateStaticClassMethod(string svgString, string iconName)
    {
        var xDoc = XDocument.Parse(svgString);
        var svgElement = xDoc.Root;

        var width = svgElement.Attribute("width")?.Value;
        var height = svgElement.Attribute("height")?.Value;
        var fill = svgElement.Attribute("fill")?.Value;
        var stroke = svgElement.Attribute("stroke")?.Value;
        var strokeLineCap = svgElement.Attribute("stroke-linecap")?.Value;
        var strokeLineJoin = svgElement.Attribute("stroke-linejoin")?.Value;
        var viewBox = svgElement.Attribute("viewBox")?.Value;

        var elements = svgElement.Descendants()
            .Select(element =>
            {
                element.Name = element.Name.LocalName;
                return element.ToString(SaveOptions.DisableFormatting);
            });

        var paths = string.Concat(elements);

        var methodTemplate = $@"
            public static ISvgIcon {iconName} => new SvgIcon(
                SvgContent: """"""{paths}"""""",
                ViewBox: ""{viewBox}"",
                Width: ""{width}"",
                Height: ""{height}"",
                Fill: ""{fill}"",
                Stroke: ""{stroke}"",
                StrokeWidth: GetStrokeWidth,
                StrokeLineCap: ""{strokeLineCap}"",
                StrokeLineJoin: ""{strokeLineJoin}""
            );";

            return methodTemplate;

    }
}