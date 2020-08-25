using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf
{
    public class StandardUIUserControlWpf : UIElement
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
    }

    public override void Draw(IVisualizer visualizer)
        {
            IUIElement? content = _userControl.Content;

            if (content is UIElement uiElement)
                uiElement.Draw(visualizer);

            OnDraw(visualizer);
        }

        /// <summary>
        /// This method can be overridden to add further graphical elements (not previously defined in a logical tree) to a drawn element.
        /// </summary>
        /// <param name="visualizer">visualizer that should draw to</param>
        public virtual void OnDraw(IVisualizer visualizer)
        {
        }
    }
}
