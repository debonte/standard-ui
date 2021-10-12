// This code will eventually be generated.

using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Wpf;
using Microsoft.StandardUI.Media;

namespace SimpleControls.Wpf
{
    public class RadialGauge : StandardControl<IRadialGauge>, IRadialGauge
    {
        public static readonly System.Windows.DependencyProperty FillProperty = PropertyUtils.Register(nameof(Fill), typeof(Brush), typeof(RadialGauge), null);

        public RadialGauge()
        {
            InitImplementation(new SimpleControls.RadialGaugeImplementation(this));
        }

        public Brush? Fill
        {
            get => (Brush?)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        IBrush? IRadialGauge.Fill
        {
            get => Fill;
            set => Fill = (Brush?)value;
        }
    }
}
