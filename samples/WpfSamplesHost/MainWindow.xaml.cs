using Microsoft.StandardUI.Wpf;
using System.Windows;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WpfStandardUIEnvironment.Init();

            InitializeComponent();
        }
    }
}
