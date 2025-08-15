using System.Drawing;

namespace Squares.Domain.Helpers;

public class ColorGenerator : IColorGenerator
{
    private static readonly Random rand = new Random();
    private const int rgbMaxValue = 255;

    public string GetRandomColorHexString()
    {
        var color = Color.FromArgb(rand.Next(rgbMaxValue), rand.Next(rgbMaxValue), rand.Next(rgbMaxValue));
        return ColorTranslator.ToHtml(color);
    }
}
