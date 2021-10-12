using Microsoft.StandardUI.Shapes;
using System;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI
{
    /// <summary>
    /// A IDrawingContext turns cross platform vector graphics into an IVisual, a platform specific representation those graphics.
    /// The graphics in the IVisual aren't actually rendered; they are just remembered, as is appropriate for a retained mode
    /// graphics system, for later rendering. For instance, for the Skia visual environment, an IVisual maps to an SkPicture.
    /// For WPF native, IVisual maps to a DrawingVisual. The retained graphics in the IVisual may be cached on the GPU.
    /// </summary>
    public interface IDrawingContext : IDisposable
    {
        void DrawEllipse(IEllipse ellipse);
        void DrawLine(ILine line);
        void DrawPath(IPath path);
        void DrawPolygon(IPolygon polygon);
        void DrawPolyline(IPolyline polyline);
        void DrawRectangle(IRectangle rectangle);
        void DrawTextBlock(ITextBlock textBlock);
        IVisual End();
    }
}
