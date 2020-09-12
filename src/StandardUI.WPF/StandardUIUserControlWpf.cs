using System.StandardUI.Controls;
using System.Windows.Media;

namespace System.StandardUI.Wpf
{
    public class StandardUIUserControlWpf : StandardUIElement
    {
        private StandardUIUserControl _userControl;

        public StandardUIUserControlWpf(StandardUIUserControl userControl)
        {
            _userControl = userControl;

            Width = userControl.Width;
            MinWidth = userControl.MinWidth;
            MaxWidth = userControl.MaxWidth;
            Height = userControl.Height;
            MinHeight = userControl.MinHeight;
            MaxHeight = userControl.MaxHeight;

            var content = (Windows.UIElement?) userControl.Content;
            if (content != null)
                AddLogicalChild(content);
        }

        public override void OnDraw(IVisualizer visualizer)
        {
            _userControl.OnDraw(visualizer);
        }

        protected override Windows.Size MeasureOverride(Windows.Size constraint)
        {
            _userControl.Measure(new Size(constraint.Width, constraint.Height));
            return _userControl.DesiredSize.ToWpfSize();
        }

        protected override Windows.Size ArrangeOverride(Windows.Size arrangeSize)
        {
            _userControl.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
            return arrangeSize;
        }

        protected override int VisualChildrenCount => _userControl.Content != null ? 1 : 0;

        protected override Visual GetVisualChild(int index)
        {
            IUIElement? content = _userControl.Content;

            if (content == null)
                throw new ArgumentOutOfRangeException("index", index, "Control has no content");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index", index, "Index out of range; control only has a single visual child.");

            return (Visual)content;
        }
    }
}
