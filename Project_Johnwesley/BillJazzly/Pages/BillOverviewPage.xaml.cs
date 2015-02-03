using System;
using System.Collections.Generic;
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

namespace BillJazzly.Pages
{
    /// <summary>
    /// Interaction logic for BillOverviewPage.xaml
    /// </summary>
    public partial class BillOverviewPage : PageFunction<String>
    {
        public BillOverviewPage()
        {
            InitializeComponent();
            _Main_stackpanel.Width = 900;
            _Year_stackpanel.Width = _Main_stackpanel.Width / 2;
            _Month_stackpanel.Width = _Main_stackpanel.Width / 2;
        }

        private void _Back_bn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
