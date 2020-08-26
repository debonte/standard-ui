using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.StandardUI.Wpf
{
    struct SizeInPixels
    {
        public static SizeInPixels Empty = new SizeInPixels(-1, -1);

        public int Width { get; }
        public int Height { get; }

        public SizeInPixels(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class UIElement : System.Windows.FrameworkElement, IUIElement
    {
        private WriteableBitmap? _bitmap;
        private bool _ignorePixelScaling;

        public void Measure(Size availableSize)
        {
            Measure(availableSize.ToWpfSize());
        }

        public void Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToWpfRect());
        }

        public bool IgnorePixelScaling
        {
            get => _ignorePixelScaling;
            set
            {
                _ignorePixelScaling = value;
                InvalidateVisual();
            }
        }

        Size IUIElement.DesiredSize => throw new System.NotImplementedException();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (Visibility != Visibility.Visible)
                return;

            IVisualEnvironment visualEnvironment = StandardUIEnvironment.VisualEnvironment;

            Rect cullingRect = new Rect(0, 0, 200, 200);

            IVisual visual;
            using (IVisualizer visualizer = visualEnvironment.CreateVisualizer(cullingRect)) {
                Draw(visualizer);
                visual = visualizer.End();
            }

            if (visual.NativeVisual is DrawingGroup drawingGroup)
            {
                drawingContext.DrawDrawing(drawingGroup);
            }
            else
            {
                SizeInPixels size = ComputeSize(out double scaleX, out double scaleY);
                if (size.Width <= 0 || size.Height <= 0)
                    return;

                // reset the bitmap if the size has changed
                if (_bitmap == null || size.Width != _bitmap.PixelWidth || size.Height != _bitmap.PixelHeight)
                    _bitmap = new WriteableBitmap(size.Width, size.Height, 96 * scaleX, 96 * scaleY, PixelFormats.Pbgra32, null);

                // draw on the bitmap
                _bitmap.Lock();

                visualEnvironment.RenderToBuffer(visual, _bitmap.BackBuffer, size.Width, size.Height, _bitmap.BackBufferStride);

                _bitmap.AddDirtyRect(new Int32Rect(0, 0, size.Width, size.Height));
                _bitmap.Unlock();
                drawingContext.DrawImage(_bitmap, new System.Windows.Rect(0, 0, size.Width, size.Height));


                //var rect = new System.Windows.Rect(0, 0, 175, 50);
                //drawingContext.DrawRectangle(new SolidColorBrush(System.Windows.Media.Colors.Red), null, rect);
            }

            /*
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Colors.LimeGreen;
            Pen myPen = new Pen(Brushes.Blue, 10);
            Rect myRect = new Rect(0, 0, 500, 500);
            dc.DrawRectangle(mySolidColorBrush, myPen, myRect);
            */
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        private SizeInPixels ComputeSize(out double scaleX, out double scaleY)
        {
            scaleX = 1.0;
            scaleY = 1.0;

            double w = Width;
            double h = Height;

            if (!IsPositive(w) || !IsPositive(h))
                return SizeInPixels.Empty;

            if (_ignorePixelScaling)
                return new SizeInPixels((int)w, (int)h);

            Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            scaleX = m.M11;
            scaleY = m.M22;
            return new SizeInPixels((int)(w * scaleX), (int)(h * scaleY));

            bool IsPositive(double value)
            {
                return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
            }
        }

        public virtual void Draw(IVisualizer visualizer)
        {
        }
    }
}
