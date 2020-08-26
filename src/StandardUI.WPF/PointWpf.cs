using System.ComponentModel;
using System.StandardUI.Wpf.Converters;

namespace System.StandardUI.Wpf
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct PointWpf

    {
        public static readonly PointWpf Default = new PointWpf(Point.Default);
        public static readonly PointWpf CenterDefault = new PointWpf(Point.CenterDefault);


        public Point Point { get; }

        public PointWpf(Point point)
        {
            Point = point;
        }
    }
}
