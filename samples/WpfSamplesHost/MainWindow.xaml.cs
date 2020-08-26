using Microcharts;
using System.StandardUI;
using System.StandardUI.SkiaVisualizer;
using System.StandardUI.VisualEnvironment.WpfNative;
using System.StandardUI.Wpf;
using SimpleControls;
using System.Windows;
using System.Windows.Controls;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WpfStandardUIEnvironment.Init(new SkiaVisualEnvironment());
            InitializeComponent();

            /*
            var radialGauge = new RadialGauge();
            radialGauge.Width = 100;
            radialGauge.Height = 100;

            var radialGaugeWpf = new StandardUIUserControlWpf(radialGauge);
            controlStack.Children.Add(radialGaugeWpf);
            */

            var barChart = new BarChart()
            {
                Entries = CreateChartEntries(),
                LabelTextSize = 60,
                LabelOrientation = Microcharts.Orientation.Horizontal,
                Width = 200,
                Height = 200
            };
            barChart.Build();

            var barChartWpf = new StandardUIUserControlWpf(barChart);
            controlStack.Children.Add(barChartWpf);
        }

        public static ChartEntry[] CreateChartEntries()
        {
            return new[]
            {
                new ChartEntry(200)
                {
                        Label = "January",
                        ValueLabel = "200",
                        Color = Color.FromHex("#266489")
                },
                new ChartEntry(400)
                {
                        Label = "February",
                        ValueLabel = "400",
                        Color = Color.FromHex("#68B9C0"),
                },
                new ChartEntry(100)
                {
                        Label = "March",
                        ValueLabel = "100",
                        Color = Color.FromHex("#90D585"),
                },
            };
        }
    }
}
