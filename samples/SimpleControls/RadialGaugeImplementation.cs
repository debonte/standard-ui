using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using static Microsoft.StandardUI.FactoryStatics;

namespace SimpleControls
{
    public interface IRadialGauge : IControl
    {
        [DefaultValue(null)]
        IBrush? Fill { get; set; }
    }

    public class RadialGaugeImplementation : StandardControlImplementation<IRadialGauge>
    {
        public RadialGaugeImplementation(IRadialGauge control) : base(control)
        { }

        public override IUIElement? Build()
        {
            var blueBrush = SolidColorBrush().Color(Colors.Blue);

            return Rectangle() .Width(50) .Height(50) .Stroke(blueBrush) .Fill(Control.Fill);
        }
    }
}
