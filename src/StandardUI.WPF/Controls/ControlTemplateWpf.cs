using System.StandardUI.Controls;

namespace System.StandardUI.Wpf
{
    public class ControlTemplateWpf : IControlTemplate
    {
        private Windows.Controls.ControlTemplate _controlTemplate;

        public ControlTemplateWpf(Windows.Controls.ControlTemplate controlTemplate)
        {
            _controlTemplate = controlTemplate;
        }

        public Windows.Controls.ControlTemplate ControlTemplate => _controlTemplate;
    }
}
