using BillJazzly.Pages;
using BillJazzly.SingleTon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BillJazzly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
        }

        private void _Bills_bn_Click(object sender, RoutedEventArgs e)
        {
            BillOverviewPage page = new BillOverviewPage();
            this.NavigationService.Navigate(page);
        }

        private void _Settings_bn_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage page = new SettingsPage();
            this.NavigationService.Navigate(page);
        }
        private void _Exit_bn_Click(object sender, RoutedEventArgs e)
        { Application.Current.Shutdown(); }
    }
}
