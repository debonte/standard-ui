using System.Collections.Generic;

namespace System.StandardUI.Media
{
    [UIModelObject]
    public interface IPathGeometry : IGeometry
    {
        IEnumerable<IPathFigure> Figures { get; set; }

        [DefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; set; }
    }
}
