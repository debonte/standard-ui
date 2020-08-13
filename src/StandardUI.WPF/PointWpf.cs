using System.ComponentModel;
using Microsoft.StandardUI.Wpf.Converters;

namespace Microsoft.StandardUI.Wpf
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct PointWpf

    {
        public static readonly PointWpf Default = new PointWpf(Microsoft.StandardUI.Point.Default);
        public static readonly PointWpf CenterDefault = new PointWpf(Microsoft.StandardUI.Point.CenterDefault);


        public Point Point { get; }

        public PointWpf(Point point)
        {
            Point = point;
        }
    }
}
