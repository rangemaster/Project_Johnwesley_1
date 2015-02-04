using BillJazzly.Bill;
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

namespace BillJazzly.Pages
{
    /// <summary>
    /// Interaction logic for BillPage.xaml
    /// </summary>
    public partial class BillPage : PageFunction<String>
    {
        private List<JBill> _Bills = null;
        private bool _PricePressedState = false;
        private bool _NamePressedState = false;
        private bool _DatePressedState = false;
        private bool _DescriptionPressedState = false;
        public BillPage(List<JBill> bills)
        {
            _Bills = bills;
            InitializeComponent();
            UpdateBills();
        }
        #region Buttons
        #region Sort Buttons
        private void _ABC_Sort_Click(object sender, RoutedEventArgs e)
        { SortABC(); UpdateBills(); }
        private void _Price_Sort_Click(object sender, RoutedEventArgs e)
        { SortPrice(); UpdateBills(); }
        private void _Date_Sort_Click(object sender, RoutedEventArgs e)
        { SortDate(); UpdateBills(); }
        private void _Description_Sort_Click(object sender, RoutedEventArgs e)
        { SortDescription(); UpdateBills(); }
        #endregion
        private void UpdateBills()
        {
            _Bill_List.Children.Clear();
            foreach (JBill bill in _Bills)
            {
                Button button = new Button();
                button.Content = bill.ButtonName();
                _Bill_List.Children.Add(button);
            }
            ShowFirstBill();
        }
        private void ShowFirstBill()
        {
            if (_Bills.Count > 0)
            {
                _Name_tx.Text = _Bills[0]._Name;
                _Price_tx.Text = _Bills[0]._Price.ToString();
                _Date_tx.Text = _Bills[0].DateTimeToString();
                _Description_tx.Text = _Bills[0]._Description;
            }
        }
        #region Sort Functions
        private void SortABC()
        {
            _NamePressedState = !_NamePressedState;
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int lower = 1; lower < _Bills.Count; lower++)
                {
                    if ((_NamePressedState ? _Bills[lower - 1]._Name.CompareTo(_Bills[lower]._Name) > 0 : _Bills[lower - 1]._Name.CompareTo(_Bills[lower]._Name) < 0))
                    { Switch(lower - 1, lower); amount++; }
                }
            }
        }
        private void SortPrice()
        {
            _PricePressedState = !_PricePressedState;
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int lower = 1; lower < _Bills.Count; lower++)
                {
                    if (_PricePressedState ? _Bills[lower - 1]._Price < _Bills[lower]._Price : _Bills[lower - 1]._Price > _Bills[lower]._Price)
                    { Switch(lower - 1, lower); }
                }
            }
        }
        private void SortDate()
        {
            _DatePressedState = !_DatePressedState;
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int lower = 1; lower < _Bills.Count; lower++)
                {
                    if (_DatePressedState ? _Bills[lower - 1]._Date.CompareTo(_Bills[lower]._Date) > 0 : _Bills[lower - 1]._Date.CompareTo(_Bills[lower]._Date) < 0)
                    { Switch(lower - 1, lower); }
                }
            }
        }
        private void SortDescription()
        {
            _DescriptionPressedState = !_DescriptionPressedState;

            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int lower = 1; lower < _Bills.Count; lower++)
                {
                    if (_DatePressedState ? _Bills[lower - 1]._Description.CompareTo(_Bills[lower]._Description) > 0 : _Bills[lower - 1]._Description.CompareTo(_Bills[lower]._Description) < 0)
                    { Switch(lower - 1, lower); }
                }
            }
        }
        private void Switch(int billIndex1, int billIndex2)
        {
            JBill higherBill = _Bills[billIndex1];
            _Bills[billIndex1] = _Bills[billIndex2];
            _Bills[billIndex2] = higherBill;
        }
        #endregion

        private void _TotalMoney_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateChartValues();
        }
        private void UpdateChartValues()
        {
            try
            {

                int totalprice = 0;
                try
                { totalprice = int.Parse(_TotalMoney_tx.Text); }
                catch (FormatException) { return; }
                double totalCosts = TotalCosts();
                _TotalCosts_tx.Text = totalCosts.ToString();
                double totalLeft = totalprice - totalCosts;
                _TotalLeft_tx.Text = totalLeft.ToString();
            }
            catch (NullReferenceException) { Debug.WriteLine("Null reference Update Chart Values"); return; }
        }
        private double TotalCosts()
        {
            double costs = 0;
            foreach (JBill bill in _Bills)
            { costs += bill._Price; }
            return costs;
        }
        #endregion
    }
}
