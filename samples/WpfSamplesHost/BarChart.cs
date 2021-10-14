// This code will eventually be generated.

using Microcharts;

namespace SimpleControls.Wpf
{
    public class BarChart : Chart, IBarChart
    {
        public BarChart()
        {
            InitImplementation(new BarChartImplementation<IBarChart>(this));
        }
    }
}
