using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IPathFigure
    {
        IEnumerable<IPathSegment> Segments { get; }

        Point StartPoint { get; }

        [DefaultValue(false)]
        bool IsClosed { get; }

        [DefaultValue(true)]
        bool IsFilled { get; }
    }
}
