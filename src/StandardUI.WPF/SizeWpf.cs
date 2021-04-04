using System.ComponentModel;
using Microsoft.StandardUI.Wpf.Converters;

namespace Microsoft.StandardUI.Wpf
{
    [TypeConverter(typeof(SizeTypeConverter))]
    public struct SizeWpf
    {
        public static readonly SizeWpf Default = new SizeWpf(Size.Default);


        public Size Size { get; }

        public SizeWpf(Size size)
        {
            Size = size;
        }
    }
}
