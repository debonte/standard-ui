using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPolygon : IShape
    {
        [DefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; }

        Points Points { get; }
    }
}
