using System.StandardUI.Controls;
using System.Windows.Media;

namespace System.StandardUI.Wpf
{
    public class StandardUIUserControlWpf : StandardUIControlWpf
    {
        private StandardUIUserControl _userControl;

        public StandardUIUserControlWpf(StandardUIUserControl userControl) : base(userControl)
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
