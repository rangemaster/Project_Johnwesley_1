using BillJazzly.Bill;
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
    /// Interaction logic for BillPage.xaml
    /// </summary>
    public partial class BillPage : PageFunction<String>
    {
        private List<JBill> _Bills = null;
        public BillPage(List<JBill> bills)
        {
            _Bills = bills;
            InitializeComponent();
            _Name_tx.Text = _Bills[0]._Name;
            _Price_tx.Text = _Bills[0]._Price.ToString();
            _Date_tx.Text = _Bills[0]._Date.ToString();
            _Description_tx.Text = _Bills[0]._Description;
        }
    }
}
