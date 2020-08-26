using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI
{
    public interface IUIElementFactory
    {
        IEllipse CreateEllipse();

        ILine CreateLine();

        IPath CreatePath();

        IPolygon CreatePolygon();

        IPolyline CreatePolyline();

        IRectangle CreateRectangle();

        ICanvas CreateCanvas();

        ICanvasAttached CanvasAttached { get; }

        IStackPanel CreateStackPanel();

        IGrid CreateGrid();
    }
}
