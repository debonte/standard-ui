using System.StandardUI;
using System.StandardUI.Controls;
using System.StandardUI.Media;
using System.StandardUI.Shapes;
using static System.StandardUI.FactoryStatics;

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
