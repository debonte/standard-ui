using System.StandardUI.Media;

namespace System.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPolygon : IShape
    {
        [DefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; set; }

        Points Points { get; set; }
    }
}
