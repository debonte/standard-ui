using System.StandardUI.Controls;
using System.Windows.Media;

namespace System.StandardUI.Wpf
{
    public class StandardUIControlWpf : Windows.Controls.Control, IStandardUIControlEnvironmentPeer
    {
        private StandardUIControl _standardUIControl;
        private ControlTemplateWpf? _controlTemplateWpf;

        public StandardUIControlWpf(StandardUIControl standardUIControl)
        {
            _standardUIControl = standardUIControl;

            Width = standardUIControl.Width;
            MinWidth = standardUIControl.MinWidth;
            MaxWidth = standardUIControl.MaxWidth;
            Height = standardUIControl.Height;
            MinHeight = standardUIControl.MinHeight;
            MaxHeight = standardUIControl.MaxHeight;
        }

        protected override Windows.Size MeasureOverride(Windows.Size constraint)
        {
            _standardUIControl.Measure(new Size(constraint.Width, constraint.Height));
            return _standardUIControl.DesiredSize.ToWpfSize();
        }

        protected override Windows.Size ArrangeOverride(Windows.Size arrangeSize)
        {
            _standardUIControl.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
            return arrangeSize;
        }

        /*
        protected override int VisualChildrenCount => _standardUIControl.Content != null ? 1 : 0;

        protected override Visual GetVisualChild(int index)
        {
            IUIElement? content = _standardUIControl.Content;

            if (content == null)
                throw new ArgumentOutOfRangeException("index", index, "Control has no content");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index", index, "Index out of range; control only has a single visual child.");

            return (Visual)content;
        }
        */

        public object GetValue(IUIProperty dp)
        {
            Windows.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            return GetValue(wpfDependencyProperty);
        }

        public object ReadLocalValue(IUIProperty dp)
        {
            Windows.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            return ReadLocalValue(wpfDependencyProperty);
        }

        public void SetValue(IUIProperty dp, object value)
        {
            Windows.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            SetValue(wpfDependencyProperty, value);
        }

        IControlTemplate? IStandardUIControlEnvironmentPeer.Template
        {
            get
            {
                Windows.Controls.ControlTemplate controlTemplate = Template;
                if (controlTemplate == null)
                    _controlTemplateWpf = null;
                else
                {
                    // Cache our ControlTemplateWpf wrapper, so the reference stays the same except
                    // when the underlying Template changes
                    if (!(ReferenceEquals(_controlTemplateWpf?.ControlTemplate, controlTemplate)))
                        _controlTemplateWpf = new ControlTemplateWpf(controlTemplate);
                }
                return _controlTemplateWpf;
            }

            set
            {
                var controlTemplateWpf = (ControlTemplateWpf?)value;
                Template = controlTemplateWpf?.ControlTemplate;
                _controlTemplateWpf = controlTemplateWpf;
            }
        }

        IUIPropertyObject? IStandardUIControlEnvironmentPeer.GetTemplateChild(string childName)
        {
            Windows.DependencyObject? child = this.GetTemplateChild(childName);
            if (child == null)
                return null;

            // TODO:Finish this
            return null;
        }
    }
}
