using Microcharts;
using Microsoft.StandardUI;
using Microsoft.StandardUI.SkiaVisualizer;
using Microsoft.StandardUI.VisualEnvironment.WpfNative;
using Microsoft.StandardUI.Wpf;
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

            var radialGauge = new RadialGauge();
            radialGauge.Width = 100;
            radialGauge.Height = 100;

            var radialGaugeWpf = new StandardUIUserControlWpf(radialGauge);
            radialGaugeWpf.HorizontalAlignment = HorizontalAlignment.Left;
            controlStack.Children.Add(radialGaugeWpf);

            var barChart = new BarChart()
            {
                Entries = CreateChartEntries(),
                LabelTextSize = 14,
                LabelOrientation = Microcharts.Orientation.Horizontal,
                IsAnimated = false,
                Width = 400,
                Height = 400,
            };
            barChart.Build();

            var barChartWpf = new StandardUIUserControlWpf(barChart);
            barChartWpf.HorizontalAlignment = HorizontalAlignment.Left;
            controlStack.Children.Add(barChartWpf);

            var radarChart = new RadarChart()
            {
                Entries = CreateChartEntries(),
                LabelTextSize = 14,
                IsAnimated = false,
                Width = 400,
                Height = 400,
            };
            radarChart.Build();

            var radarChartWpf = new StandardUIUserControlWpf(radarChart);
            radarChartWpf.HorizontalAlignment = HorizontalAlignment.Left;
            controlStack.Children.Add(radarChartWpf);
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
