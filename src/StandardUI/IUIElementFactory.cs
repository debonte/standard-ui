using System.StandardUI.Controls;
using System.StandardUI.Shapes;

namespace System.StandardUI
{
    public interface IUIElementFactory
    {
        ICanvas CreateCanvas();
        ICanvasAttached CanvasAttached { get; }
        IStackPanel CreateStackPanel();
        IGrid CreateGrid();

        IEllipse CreateEllipse();
        ILine CreateLine();
        IPath CreatePath();
        IPolygon CreatePolygon();
        IPolyline CreatePolyline();
        IRectangle CreateRectangle();

        ITextBlock CreateTextBlock();
    }
}
