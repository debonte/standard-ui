using System.StandardUI.Controls;
using System.StandardUI.Shapes;

namespace System.StandardUI
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
