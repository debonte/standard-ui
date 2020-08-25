using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using static Microsoft.StandardUI.FactoryStatics;

namespace SimpleControls
{
    public class RadialGauge : StandardUIUserControl
    {
        public RadialGauge()
        {
            var redBrush = SolidColorBrush().Color(Colors.Red);
            var blueBrush = SolidColorBrush().Color(Colors.Blue);

            Content = Rectangle() .Width(50) .Height(50) .Stroke(blueBrush) .Fill(redBrush);
        }
    }
}
