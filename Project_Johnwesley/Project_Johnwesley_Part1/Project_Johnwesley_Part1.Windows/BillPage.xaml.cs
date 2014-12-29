using Project_Johnwesley_Part1.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Project_Johnwesley_Part1
{
    public sealed partial class BillPage : Page
    {
        private double _TotalMoney = 2000;
        private List<Bill> _Bills;
        private List<TextBox> textBoxes;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Logic _Logic = null;
        private Button _Done_bn, _Add_Bill_bn;
        public BillPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        #region Initialisation
        #region Init
        private void Init()
        {
            InitPanels();
            InitBills();
            InitColors();
            Update();
        }
        #endregion
        #region InitBills
        private void InitBills()
        {
            this._Bills = new List<Bill>();
            Bill testBill = new Bill.BillBuilder().Payer("John").Price(12).Description("This is an test bill").Build();
            Bill testBill2 = new Bill.BillBuilder().Payer("Ashley").Price(15).Description("This is an test bill").Build();
            this._Bills.Add(testBill);
            this._Bills.Add(testBill2);
            foreach (Bill bill in _Bills)
            { Bill_List.Items.Add(bill); }
            if (Bill_List.Items.Count > 0)
            { Bill_List.SelectedIndex = 0; }
        }
        #endregion
        #region InitColors
        private void InitColors() { }
        #endregion
        #region InitPanels
        private void InitPanels()
        {
            this._Total_Money_tx.TextChanged += _Total_Money_tx_TextChanged;
            this._Payed_Money_tx.IsEnabled = false;
            this._InPossession_Money_tx.IsEnabled = false;
            this.textBoxes = new List<TextBox>();
            textBoxes.Add(CreateTextBox("Payer:"));
            textBoxes.Add(CreateTextBox("Price"));
            textBoxes.Add(CreateTextBox("Time:"));
            textBoxes.Add(CreateTextBox("Description:"));
            this._Information_Panel_1.Children.Add(textBoxes[0]);
            this._Information_Panel_1.Children.Add(textBoxes[1]);
            this._Information_Panel_2.Children.Add(textBoxes[2]);
            this._Information_Panel_3.Children.Add(textBoxes[3]);
            this._Add_Bill_bn = new Button();
            this._Add_Bill_bn.Content = "Add";
            this._Add_Bill_bn.Click += _Add_Bill_bn_Click;
            this._Done_bn = new Button();
            this._Done_bn.Content = "Done";
            this._Done_bn.Click += _Done_bn_Click;
            this._Information_Panel_3.Children.Add(_Done_bn);
            InformationFieldEditable(false);
            TopInformationFIeldVisible(true);
        }
        private void _Total_Money_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Total_Money_tx.Text.Length > 0)
            {
                try { this._TotalMoney = (double)(Double.Parse(_Total_Money_tx.Text)); }
                catch (FormatException) { this._TotalMoney = 0; }
            }
            else { this._TotalMoney = 0; }
            Update();
        }
        private TextBox CreateTextBox(string header)
        {
            TextBox tb = new TextBox();
            tb.FontSize = 24;
            tb.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            tb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            tb.Header = header;
            tb.Text = "";
            tb.TextWrapping = TextWrapping.Wrap;
            return tb;
        }
        #endregion
        #region Update
        private void Update()
        {
            this._Total_Money_tx.Text = _TotalMoney.ToString();
            double payedMoney = 0;
            foreach (Bill bill in _Bills)
            { payedMoney += bill.price; }
            this._Payed_Money_tx.Text = payedMoney.ToString();
            this._InPossession_Money_tx.Text = (_TotalMoney - payedMoney).ToString();
        }
        private void UpdateList()
        {
            this.Bill_List.Items.Clear();
            foreach (Bill bill in _Bills)
            { this.Bill_List.Items.Add(bill); }
            if (_Bills.Count == 0)
            { ClearInformationField(); }
        }
        private void ClearInformationField()
        {
            foreach (TextBox tb in textBoxes)
            { tb.Text = ""; }
        }
        private void TopInformationFIeldVisible(bool result)
        {
            Windows.UI.Xaml.Visibility Vis = Windows.UI.Xaml.Visibility.Collapsed;
            if (result)
                Vis = Windows.UI.Xaml.Visibility.Visible;
            _Total_Money_tx.Visibility = Vis;
            _Total_Money_tx.IsEnabled = result;
            _Payed_Money_tx.Visibility = Vis;
            _InPossession_Money_tx.Visibility = Vis;
            Bill_List.Visibility = Vis;
            Bill_List.IsEnabled = result;
            Add_bn.Visibility = Vis;
            Add_bn.IsEnabled = result;
            Edit_bn.Visibility = Vis;
            Edit_bn.IsEnabled = result;
            Remove_bn.Visibility = Vis;
            Remove_bn.IsEnabled = result;
            if (result) _Done_bn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else _Done_bn.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _Done_bn.IsEnabled = !result;

        }
        private void InformationFieldEditable(bool result)
        {
            foreach (TextBox tb in textBoxes)
            { tb.IsEnabled = result; }
        }
        #endregion
        #endregion

        #region NavigationHelper registration
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        { Init(); }
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        { }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        { navigationHelper.OnNavigatedTo(e); }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        { navigationHelper.OnNavigatedFrom(e); }
        public ObservableDictionary DefaultViewModel
        { get { return this.defaultViewModel; } }
        public NavigationHelper NavigationHelper
        { get { return this.navigationHelper; } }

        #endregion

        #region Logic
        private void Method1() { Logic.handle(Logic.Method1); }
        private void Method2() { Logic.handle(Logic.Method2); }

        private Logic Logic
        {
            get
            {
                if (_Logic == null)
                { _Logic = new Logic(); }
                return _Logic;
            }
        }
        #endregion

        #region Buttons
        private void Add_bn_Click(object sender, RoutedEventArgs e)
        { }
        private void Edit_bn_Click(object sender, RoutedEventArgs e)
        { ShowEditFlyout(); }
        private void Remove_bn_Click(object sender, RoutedEventArgs e)
        {
            this._Bills.RemoveAt(Bill_List.SelectedIndex);
            UpdateList();
            ClearInformationField();
        }
        private void Edit_Flyout_Continue_bn(object sender, RoutedEventArgs e)
        {
            if (CheckAdminPassword(_Edit_Field_tx.Text))
            {
                InformationFieldEditable(true);
                HideEditFlyout();
                TopInformationFIeldVisible(false);
            }
            _Edit_Field_tx.Text = "";
        }
        private void Close_Flyout_Continue_bn(object sender, RoutedEventArgs e)
        { HideEditFlyout(); }
        private void _Add_Bill_bn_Click(object sender, RoutedEventArgs e)
        { }
        private void _Done_bn_Click(object sender, RoutedEventArgs e)
        { ReplaceEditedBill(); }
        #endregion
        #region Button Functions
        private void ShowEditFlyout()
        {
            if (this._Edit_Flyout.Visibility == Visibility.Visible)
            { this._Edit_Flyout.Visibility = Visibility.Collapsed; }
            else
            { this._Edit_Flyout.Visibility = Visibility.Visible; }
        }
        private void HideEditFlyout()
        { this._Edit_Flyout.Visibility = Visibility.Collapsed; }
        private bool CheckAdminPassword(string pass)
        {
            if (pass == "1234")
                return true;
            return false;
        }
        private void ReplaceEditedBill()
        {
            int index = this.Bill_List.SelectedIndex;
            if (index != -1)
            {
                string[] array = textBoxes[2].Text.Split('-');
                DateTime time = new DateTime(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
                Bill bill = new Bill.BillBuilder()
                    .Payer(textBoxes[0].Text)
                    .Price(Double.Parse(textBoxes[1].Text))
                    .Time(time)
                    .Description(textBoxes[3].Text).Build();
                this._Bills.Insert(index, bill);
                this._Bills.RemoveAt(index + 1);
                TopInformationFIeldVisible(true);
                InformationFieldEditable(false);
                Update();
                UpdateList();
            }
        }
        #endregion
        private void Bill_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Bill_List.SelectedIndex != -1)
            {
                Bill bill = (Bill)(_Bills[Bill_List.SelectedIndex]);
                if (bill != null)
                {
                    ((TextBox)this._Information_Panel_1.Children[0]).Text = bill.payer;
                    ((TextBox)this._Information_Panel_1.Children[1]).Text = bill.price.ToString();
                    ((TextBox)this._Information_Panel_2.Children[0]).Text = bill.time.Year + " - " + bill.time.Month + " - " + bill.time.Day;
                    ((TextBox)this._Information_Panel_3.Children[0]).Text = bill.description;
                    Update();
                }
            }
            Debug.WriteLine("Selection Changed");
        }

        private void _Edit_Field_tx_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && _Edit_Flyout.Visibility == Windows.UI.Xaml.Visibility.Visible)
            { Edit_Flyout_Continue_bn(sender, e); }
        }
    }
}
