// This code will eventually be generated.

using Microcharts;

namespace SimpleControls.Wpf
{
    public class PointChart : Chart, IPointChart
    {
        public PointChart()
        {
            InitImplementation(new PointChartImplementation(this));
        }
    }
}
