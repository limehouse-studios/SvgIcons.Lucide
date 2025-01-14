namespace LHS.SvgIcons.Lucide;

public class LucideIconSettings
{
    public static decimal StrokeWidth { get; set; } = 2;


    public static void Customise(decimal strokeWidth)
    {
        StrokeWidth = strokeWidth;
    }
}