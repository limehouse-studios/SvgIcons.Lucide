using System.Globalization;

namespace LHS.SvgIcons.Lucide;

public static partial class LucideIconSet
{
    private static string GetStrokeWidth => LucideIconSettings.StrokeWidth.ToString(CultureInfo.InvariantCulture);

}