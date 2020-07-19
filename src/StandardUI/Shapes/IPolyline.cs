using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPolyline : IShape
    {
        [DefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; }

        Points Points { get; }
    }
}
