using BillJazzly.Bill;
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

namespace BillJazzly.Pages
{
    /// <summary>
    /// Interaction logic for BillPage.xaml
    /// </summary>
    public partial class BillPage : PageFunction<String>
    {
        private int _SelectedIndex = -1;
        private int _Year = -1;
        private string _Month = null;
        private List<RMSBill> _Bills = null;
        private bool _ChangesDone = false;
        private bool _PricePressedState = false;
        private bool _NamePressedState = false;
        private bool _DatePressedState = false;
        private bool _DescriptionPressedState = false;
        public BillPage(int year, string month, List<RMSBill> bills)
        {
            _Year = year;
            _Month = month;
            _Bills = bills;
            InitializeComponent();
            UpdateBills();
            UpdateButtons();
            VisabilityAllEditButtons(false);
        }
        #region Sort Buttons
        private void _ABC_Sort_Click(object sender, RoutedEventArgs e)
        { SortABC(); UpdateBills(); UpdateButtons(); }
        private void _Price_Sort_Click(object sender, RoutedEventArgs e)
        { SortPrice(); UpdateBills(); UpdateButtons(); }
        private void _Date_Sort_Click(object sender, RoutedEventArgs e)
        { SortDate(); UpdateBills(); UpdateButtons(); }
        private void _Description_Sort_Click(object sender, RoutedEventArgs e)
        { SortDescription(); UpdateBills(); UpdateButtons(); }
        #endregion

        #region Update
        private void UpdateBills()
        {
            _Bill_List.Children.Clear();
            foreach (RMSBill bill in _Bills)
            {
                Button button = new Button();
                button.Content = bill.ButtonName();
                button.Click += ShowButton;
                _Bill_List.Children.Add(button);
            }
            ShowFirstBill();
            UpdateChartValues();
        }
        private void UpdateButtons()
        {
            if (_ChangesDone)
            { _Save_bn.IsEnabled = true; }
            else
            { _Save_bn.IsEnabled = false; }
        }
        private void UpdateChartValues()
        {
            try
            {

                double totalprice = 0;
                try
                { totalprice = double.Parse(_TotalMoney_tx.Text); }
                catch (FormatException) { return; }
                double totalCosts = TotalCosts();
                _TotalCosts_tx.Text = totalCosts.ToString();
                double totalLeft = totalprice - totalCosts;
                _TotalLeft_tx.Text = totalLeft.ToString();
            }
            catch (NullReferenceException) { Debug.WriteLine("Null reference Update Chart Values"); return; }
        }
        private void ShowButton(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            ShowBillInfo(FindBill_JBill(content));
        }
        #endregion

        #region Find
        private int FindBill_int(string billButtonName)
        {
            int index = -1;
            for (int i = 0; i < _Bills.Count; i++)
            { if (_Bills[i].ButtonName().Equals(billButtonName)) { index = i; } }
            _SelectedIndex = index;
            return index;
        }
        private RMSBill FindBill_JBill(string billButtonName)
        { return _Bills[FindBill_int(billButtonName)]; }
        private void ShowFirstBill()
        {
            if (_Bills.Count > 0)
            {
                ShowBillInfo(_Bills[0]);
                _SelectedIndex = 0;
            }
        }
        #endregion
        private void ShowBillInfo(RMSBill bill)
        {
            if (bill != null)
            {
                _Name_tx.Text = bill._Name;
                _Price_tx.Text = bill._Price.ToString();
                _Date_tx.Text = bill.DateTimeToString();
                Debug.WriteLine("Description To Field: [" + bill._Description + "]");
                _Description_tx.Text = bill._Description;
                Debug.WriteLine("Description Check Field: [" + _Description_tx.Text + "]");
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
                    { Switch(lower - 1, lower); amount++; }
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
                    { Switch(lower - 1, lower); amount++; }
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
                    { Switch(lower - 1, lower); amount++; }
                }
            }
        }
        private void Switch(int billIndex1, int billIndex2)
        {
            RMSBill higherBill = _Bills[billIndex1];
            _Bills[billIndex1] = _Bills[billIndex2];
            _Bills[billIndex2] = higherBill;
        }
        #endregion

        #region Money Functions
        private void _TotalMoney_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateChartValues();
        }
        private double TotalCosts()
        {
            double costs = 0;
            foreach (RMSBill bill in _Bills)
            { costs += bill._Price; }
            return costs;
        }
        #endregion

        #region Buttons Add/Edit
        private void _Add_bn_Click(object sender, RoutedEventArgs e)
        {
            VisabilityAllEditButtons(false);
            MakeEditFieldEditable(true);
            ShowAddButton();
            ShowCancelButton();
            ShowCleanEditField();
        }
        private void _Edit_bn_Click(object sender, RoutedEventArgs e)
        {
            VisabilityAllEditButtons(false);
            MakeEditFieldEditable(true);
            ShowEditButton();
            ShowRemoveButton();
            ShowCancelButton();
        }
        private void _Add_EditField_bn_Click(object sender, RoutedEventArgs e)
        { AddBillInfo(); }
        private void _Edit_EditField_bn_Click(object sender, RoutedEventArgs e)
        { EditBillInfo(); }
        private void _Remove_EditField_bn_Click(object sender, RoutedEventArgs e)
        { RemoveBillInfo(); }
        private void _Cancel_EditField_bn_Click(object sender, RoutedEventArgs e)
        { CancelBillInfo(); }
        #endregion

        #region Button Functions Add/Edit/Remove/Cancel
        private void MakeEditFieldEditable(bool result)
        {
            _Name_tx.IsEnabled = result;
            _Price_tx.IsEnabled = result;
            _Date_tx.IsEnabled = result;
            _Description_tx.IsEnabled = result;
            if (!result)
            { _Toplane_stackpanel.Visibility = System.Windows.Visibility.Visible; HideCancelButton(); HideAddButton(); HideEditButton(); HideRemoveButton(); }
            else
            { _Toplane_stackpanel.Visibility = System.Windows.Visibility.Collapsed; }
        }
        private void AddBillInfo()
        {
            try
            {
                RMSBill bill = ToBillConverter(_Name_tx.Text, _Price_tx.Text, _Date_tx.Text, _Description_tx.Text);
                _Bills.Add(bill);
                _ChangesDone = true;
                MakeEditFieldEditable(false);
            }
            catch (FormatException) { }
            UpdateBills();
            UpdateButtons();
        }
        private void EditBillInfo()
        {
            RMSBill bill = ToBillConverter(_Name_tx.Text, _Price_tx.Text, _Date_tx.Text, _Description_tx.Text);
            _Bills[_SelectedIndex] = bill;
            _ChangesDone = true;
            MakeEditFieldEditable(false);
            UpdateBills();
            UpdateButtons();
        }
        private void RemoveBillInfo()
        {
            try
            {
                _Bills.RemoveAt(_SelectedIndex);
                _ChangesDone = true;
                MakeEditFieldEditable(false);
            }
            catch (ArgumentOutOfRangeException) { MessageBox.Show("Remove Bill Info Argument Unknown"); }
            UpdateBills();
            UpdateButtons();
        }
        private void CancelBillInfo()
        {
            ShowCleanEditField();
            MakeEditFieldEditable(false);
            VisabilityAllEditButtons(false);
            _Toplane_stackpanel.Visibility = System.Windows.Visibility.Visible;
            UpdateButtons();
            UpdateBills();
            ShowFirstBill();
        }

        private RMSBill ToBillConverter(string name_tx, string price_tx, string date_tx, string description_tx)
        {
            try
            {
                double price = double.Parse(_Price_tx.Text);
                DateTime date = DateTimeFieldToDateTime(_Date_tx.Text);
                RMSBill bill = new RMSBill.Builder()
                    .Name(_Name_tx.Text)
                    .Price(price)
                    .Date(date)
                    .Description(_Description_tx.Text)
                    .Build();
                return bill;
            }
            catch (FormatException) { throw new FormatException(); }
        }
        #region DateTimeConverter
        private DateTime DateTimeFieldToDateTime(string date)
        {
            int year, month, day, hour, min, sec;
            string[] array = (date.Trim()).Split('-', ' ', ':');
            year = int.Parse(array[0]);
            month = int.Parse(array[1]);
            day = int.Parse(array[2]);
            if (array.Length > 3)
            {
                hour = int.Parse(array.Length == 7 ? array[4] : array[3]);
                min = int.Parse(array.Length == 7 ? array[5] : array[4]);
                sec = int.Parse(array.Length == 7 ? array[6] : array[5]);
            }
            else
            {
                string[] array2 = (DateTime.Now.ToString("hh:mm:ss")).Split(':');
                hour = int.Parse(array2[0]);
                min = int.Parse(array2[1]);
                sec = int.Parse(array2[2]);
            }
            return new DateTime(year, month, day, hour, min, sec);
        }
        #endregion

        private void _Back_bn_Click(object sender, RoutedEventArgs e)
        { if (this.NavigationService.CanGoBack) { this.NavigationService.GoBack(); } }
        private void _Save_bn_Click(object sender, RoutedEventArgs e)
        {
            DataHolder.Get.Bills[_Year][_Month] = _Bills;
            DataHolder.Get.SaveBills();
            _ChangesDone = false;
            UpdateButtons();
        }

        #region Show Functions
        private void ShowCleanEditField()
        {
            _Name_tx.Text = "";
            _Price_tx.Text = "0,00";
            _Date_tx.Text = DateTime.Now.ToString("yyyy-MM-dd");
            _Description_tx.Text = "";
        }
        private void VisabilityAllEditButtons(bool result)
        {
            System.Windows.Visibility Vis = System.Windows.Visibility.Collapsed;
            if (result)
                Vis = System.Windows.Visibility.Visible;
            _Add_EditField_bn.Visibility = Vis;
            _Add_EditField_bn.IsEnabled = result;
            _Edit_EditField_bn.Visibility = Vis;
            _Edit_EditField_bn.IsEnabled = result;
            _Remove_EditField_bn.Visibility = Vis;
            _Remove_EditField_bn.IsEnabled = result;
            _Cancel_EditField_bn.Visibility = Vis;
            _Cancel_EditField_bn.IsEnabled = result;
            _EditField_stackpanel.Visibility = Vis;
        }
        private void ShowAddButton()
        {
            _Add_EditField_bn.IsEnabled = true;
            _Add_EditField_bn.Visibility = System.Windows.Visibility.Visible;
            _EditField_stackpanel.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideAddButton()
        {
            _Add_EditField_bn.IsEnabled = false;
            _Add_EditField_bn.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void ShowEditButton()
        {
            _Edit_EditField_bn.IsEnabled = true;
            _Edit_EditField_bn.Visibility = System.Windows.Visibility.Visible;
            _EditField_stackpanel.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideEditButton()
        {
            _Edit_EditField_bn.IsEnabled = false;
            _Edit_EditField_bn.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void ShowRemoveButton()
        {
            _Remove_EditField_bn.IsEnabled = true;
            _Remove_EditField_bn.Visibility = System.Windows.Visibility.Visible;
            _EditField_stackpanel.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideRemoveButton()
        {
            _Remove_EditField_bn.IsEnabled = false;
            _Remove_EditField_bn.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void ShowCancelButton()
        {
            _Cancel_EditField_bn.IsEnabled = true;
            _Cancel_EditField_bn.Visibility = System.Windows.Visibility.Visible;
            _EditField_stackpanel.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideCancelButton()
        {
            _Cancel_EditField_bn.IsEnabled = false;
            _Cancel_EditField_bn.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion
        #endregion

    }
}
