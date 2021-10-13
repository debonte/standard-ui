using Microsoft.StandardUI.Controls;
using System;
using System.Windows.Media;

namespace Microsoft.StandardUI.Wpf
{
    public class StandardControl<T> : System.Windows.Controls.Control, IControl, IStandardControlEnvironmentPeer where T : IControl
    {
        private StandardControlImplementation<T> _implementation;
        private StandardUIFrameworkElement? _buildContent;
        private bool _invalid = true;

        protected void InitImplementation(StandardControlImplementation<T> implementation)
        {
            _implementation = implementation;
        }

        void IUIElement.Measure(Size availableSize)
        {
            Measure(availableSize.ToWpfSize());
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToWpfRect());
        }

        Size IUIElement.DesiredSize => SizeExtensions.FromWpfSize(DesiredSize);

        IUIPropertyObject? IControl.GetTemplateChild(string childName)
        {
            throw new NotImplementedException();
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            if (_invalid)
            {
                Rebuild();
                _invalid = false;
            }

            _implementation.Measure(new Size(constraint.Width, constraint.Height));
            return _implementation.DesiredSize.ToWpfSize();
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize)
        {
            _implementation.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
            return arrangeSize;
        }

        double IUIElement.ActualX => throw new System.NotImplementedException();

        double IUIElement.ActualY => throw new System.NotImplementedException();

        public object GetValue(IUIProperty dp)
        {
            System.Windows.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            return GetValue(wpfDependencyProperty);
        }

        public object ReadLocalValue(IUIProperty dp)
        {
            System.Windows.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            return ReadLocalValue(wpfDependencyProperty);
        }

        public void SetValue(IUIProperty dp, object value)
        {
            System.Windows.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            SetValue(wpfDependencyProperty, value);
        }

        protected override int VisualChildrenCount => _buildContent != null ? 1 : 0;

        IUIElement? IStandardControlEnvironmentPeer.BuildContent => _buildContent;

        protected override Visual GetVisualChild(int index)
        {
            if (_buildContent == null)
                throw new ArgumentOutOfRangeException("index", index, "Control returned null from build");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index", index, "Index out of range; control only has a single visual child.");

            return _buildContent;
        }

        private void Rebuild()
        {
            if (_buildContent != null)
            {
                RemoveVisualChild(_buildContent);
                RemoveLogicalChild(_buildContent);
                _buildContent = null;
            }

            _buildContent = (StandardUIFrameworkElement?)_implementation.Build();

            if (_buildContent != null)
            {
                AddVisualChild(_buildContent);
                AddLogicalChild(_buildContent);
            }
        }

#if false
        IControlTemplate? IControl.Template
        {
            get
            {
                System.Windows.Controls.ControlTemplate controlTemplate = Template;
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
#endif

#if false
        IUIPropertyObject? IStandardUIControlEnvironmentPeer.GetTemplateChild(string childName)
        {
            System.Windows.DependencyObject? child = this.GetTemplateChild(childName);
            if (child == null)
                return null;

            // TODO:Finish this
            return null;
        }
#endif
    }
}
