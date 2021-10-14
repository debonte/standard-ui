// This code will eventually be generated.

using Microsoft.StandardUI.Wpf;
using Microcharts;
using System.Collections.Generic;
using Microsoft.StandardUI;

namespace SimpleControls.Wpf
{
    public class BarChart : StandardControl, IBarChart
    {
        public static readonly System.Windows.DependencyProperty EntriesProperty = PropertyUtils.Register(nameof(Entries), typeof(IEnumerable<ChartEntry>), typeof(BarChart), null);
        public static readonly System.Windows.DependencyProperty BackgroundColorProperty = PropertyUtils.Register(nameof(BackgroundColor), typeof(ColorWpf), typeof(BarChart), ColorWpf.Default);
        public static readonly System.Windows.DependencyProperty LabelColorProperty = PropertyUtils.Register(nameof(LabelColor), typeof(ColorWpf), typeof(BarChart), ColorWpf.Default);

        public BarChart()
        {
            InitImplementation(new BarChartImplementation(this));
        }

        public IEnumerable<ChartEntry> Entries
        {
            get => (IEnumerable<ChartEntry>)GetValue(EntriesProperty);
            set => SetValue(EntriesProperty, value);
        }

        public ColorWpf BackgroundColor
        {
            get => (ColorWpf)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        Color IChart.BackgroundColor
        {
            get => BackgroundColor.Color;
            set => BackgroundColor = new ColorWpf(value);
        }

        public ColorWpf LabelColor
        {
            get => (ColorWpf)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }
        Color IChart.LabelColor
        {
            get => LabelColor.Color;
            set => LabelColor = new ColorWpf(value);
        }
    }
}
