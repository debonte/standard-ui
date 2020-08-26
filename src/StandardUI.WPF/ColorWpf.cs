using System.ComponentModel;
using System.StandardUI.Wpf.Converters;

namespace System.StandardUI.Wpf
{
    [TypeConverter(typeof(ColorTypeConverter))]
    public struct ColorWpf
    {
        public static readonly ColorWpf Default = new ColorWpf(System.StandardUI.Color.Default);
        public static readonly ColorWpf Transparent = new ColorWpf(Colors.Transparent);

        public static ColorWpf FromColor(Color color) => new ColorWpf(color);

        // Auto properties
        public Color Color { get; }

        public ColorWpf(Color color)
        {
            Color = color;
        }
    }
}
