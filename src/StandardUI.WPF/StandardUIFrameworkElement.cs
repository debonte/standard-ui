using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.StandardUI.Wpf
{
    /// <summary>
    /// This is the base for predefined standard UI controls. 
    /// </summary>
    public class StandardUIFrameworkElement : FrameworkElement, IUIElement
    {
        StandardUIFrameworkElementHelper _helper = new StandardUIFrameworkElementHelper();

        void IUIElement.Measure(Size availableSize)
        {
            Measure(availableSize.ToWpfSize());
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToWpfRect());
        }

        Size IUIElement.DesiredSize => SizeExtensions.FromWpfSize(DesiredSize);

        public double ActualX => throw new NotImplementedException();

        public double ActualY => throw new NotImplementedException();

        protected override void OnRender(DrawingContext drawingContextWpf)
        {
            base.OnRender(drawingContextWpf);

            if (Visibility != Visibility.Visible)
                return;

            IVisualEnvironment visualEnvironment = StandardUIEnvironment.VisualEnvironment;

            Rect cullingRect = new Rect(0, 0, 200, 200);

            IVisual visual;
            using (IDrawingContext drawingContext = visualEnvironment.CreateDrawingContext(cullingRect)) {
                Draw(drawingContext);
                visual = drawingContext.End();
            }

            _helper.OnRender(visual, Width, Height, drawingContextWpf);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        public virtual void Draw(IDrawingContext visualizer)
        {
        }

        public object GetValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            return GetValue(dependencyProperty);
        }

        public object ReadLocalValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            return ReadLocalValue(dependencyProperty);
        }

        public void SetValue(IUIProperty property, object value)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            SetValue(dependencyProperty, value);
        }
    }
}
