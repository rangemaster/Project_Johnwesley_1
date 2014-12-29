using Project_Johnwesley_Part1.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
            this.backButton.Click += backButton_Click;
        }
        #region Initialisation
        #region Init
        private void Init()
        {
            InitPanels();
            InitBills();
            InitColors();
            LoadList();
            UpdateCalculation();
            UpdateList();
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
            this._Information_Panel_3.Children.Add(_Add_Bill_bn);
            InformationFieldEditable(false);
            TopInformationFIeldVisible(true);
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
        private void UpdateCalculation()
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
        #region Unused
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
        #region Changed Events
        private void _Total_Money_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Total_Money_tx.Text.Length > 0)
            {
                try { this._TotalMoney = (double)(Double.Parse(_Total_Money_tx.Text)); }
                catch (FormatException) { this._TotalMoney = 0; }
            }
            else { this._TotalMoney = 0; }
            UpdateCalculation();
        }
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
                    UpdateCalculation();
                }
            }
            Debug.WriteLine("Selection Changed");
        }
        #endregion
        #endregion

        #region Buttons
        private void backButton_Click(object sender, RoutedEventArgs e)
        { BackButtonPressed(); }
        private void _Sort_ABC_bn_Click(object sender, RoutedEventArgs e)
        { SortOnAlphabeticalOrder(); }
        private void _Sort_Euro_bn_Click(object sender, RoutedEventArgs e)
        { SortOnPriceOrder(); }
        private void _Sort_Date_bn_Click(object sender, RoutedEventArgs e)
        { SortOnDateOrder(); }
        private void _Sort_Description_bn_Click(object sender, RoutedEventArgs e)
        { SortOnDescriptionOrder(); }
        private void Add_bn_Click(object sender, RoutedEventArgs e)
        { AddNewBillVisible(); }
        private void Edit_bn_Click(object sender, RoutedEventArgs e)
        { ShowEditFlyout(); }
        private void Remove_bn_Click(object sender, RoutedEventArgs e)
        { RemoveSelectedBill(); }
        private void Edit_Flyout_Continue_bn(object sender, RoutedEventArgs e)
        { TryToEditCheck(); }
        private void Close_Flyout_Continue_bn(object sender, RoutedEventArgs e)
        { HideEditFlyout(); }
        private void _Add_Bill_bn_Click(object sender, RoutedEventArgs e)
        { AddNewBill(); }
        private void _Done_bn_Click(object sender, RoutedEventArgs e)
        { ReplaceEditedBill(); }
        #endregion

        #region Button Functions
        #region Back button
        private void BackButtonPressed()
        {
            this.backButton.Click -= backButton_Click;
            SaveList();
        }
        #endregion
        #region Add/Edit/Remove
        private void AddNewBill()
        {
            string payer = textBoxes[0].Text;
            string price = textBoxes[1].Text;
            DateTime time = new DateTime(1000, 1, 1);
            if (textBoxes[2].Text != "" && textBoxes[2].Text != "YYYY-MM-DD")
            {
                string[] array = textBoxes[2].Text.Split('-');
                int year = int.Parse(array[0].Trim());
                int month = int.Parse(array[1].Trim());
                int day = int.Parse(array[2].Trim());
                time = new DateTime(year, month, day);
            }
            string description = textBoxes[3].Text;

            Bill bill = CreateBill(payer, price, time.Year, time.Month, time.Day, description);
            if (bill != null)
            {
                this._Bills.Add(bill);

                TopInformationFIeldVisible(true);
                InformationFieldEditable(false);
                UpdateCalculation();
                UpdateList();
            }
        }
        private void AddNewBillVisible()
        {
            TopInformationFIeldVisible(false);
            InformationFieldEditable(true);
            ClearInformationField();
            textBoxes[2].Text = DateTime.Now.ToString("yyyy-MM-dd");
            _Done_bn.Visibility = Visibility.Collapsed;
            _Done_bn.IsEnabled = false;
            _Add_Bill_bn.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _Add_Bill_bn.IsEnabled = true;
        }
        private void RemoveSelectedBill()
        {
            this._Bills.RemoveAt(Bill_List.SelectedIndex);
            UpdateList();
            ClearInformationField();
        }
        private Bill CreateBill(string payer, string price, int year, int month, int day, string description)
        {
            Bill.BillBuilder billBuilder = new Bill.BillBuilder();
            try
            {
                if (payer != "" && payer != null)
                    billBuilder.Payer(payer);
                if (price != "" && price != null)
                    billBuilder.Price(Double.Parse(price));
                if (year != 1 && month != 1 && day != 1)
                    billBuilder.Time(new DateTime(year, month, day));
                if (description != "" && description != null)
                    billBuilder.Description(description);

            }
            catch (FormatException) { Debug.WriteLine("Format Exception Null"); return null; }
            Bill bill = billBuilder.Build();
            return bill;
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
                UpdateCalculation();
                UpdateList();
            }
        }
        private void _Edit_Field_tx_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && _Edit_Flyout.Visibility == Windows.UI.Xaml.Visibility.Visible)
            { Edit_Flyout_Continue_bn(sender, e); }
        }
        #endregion
        #region Checks
        private void TryToEditCheck()
        {
            if (CheckAdminPassword(_Edit_Field_tx.Text))
            {
                InformationFieldEditable(true);
                HideEditFlyout();
                TopInformationFIeldVisible(false);
            }
            _Edit_Field_tx.Text = "";
        }
        private bool CheckAdminPassword(string pass)
        {
            if (pass == "1234")
                return true;
            return false;
        }
        #endregion
        # region Visibility
        private void TopInformationFIeldVisible(bool result)
        {
            Windows.UI.Xaml.Visibility Vis = Windows.UI.Xaml.Visibility.Collapsed;
            if (result)
                Vis = Windows.UI.Xaml.Visibility.Visible;
            #region Components
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
            _Sort_ABC_bn.Visibility = Vis;
            _Sort_ABC_bn.IsEnabled = result;
            _Sort_Euro_bn.Visibility = Vis;
            _Sort_Euro_bn.IsEnabled = result;
            _Sort_Date_bn.Visibility = Vis;
            _Sort_Date_bn.IsEnabled = result;
            _Sort_Description_bn.Visibility = Vis;
            _Sort_Description_bn.IsEnabled = result;
            #endregion
            if (result)
            {
                _Done_bn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                _Add_Bill_bn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else _Done_bn.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _Done_bn.IsEnabled = !result;
            _Add_Bill_bn.IsEnabled = !result;

        }
        private void InformationFieldEditable(bool result)
        {
            foreach (TextBox tb in textBoxes)
            { tb.IsEnabled = result; }
        }
        private void ShowEditFlyout()
        {
            if (this._Edit_Flyout.Visibility == Visibility.Visible)
            { this._Edit_Flyout.Visibility = Visibility.Collapsed; }
            else
            { this._Edit_Flyout.Visibility = Visibility.Visible; }
        }
        private void HideEditFlyout()
        { this._Edit_Flyout.Visibility = Visibility.Collapsed; }
        #endregion
        #region Sort
        #region Help Method
        private void SwitchMethod(int index1, int index2)
        {
            Bill otherBill = _Bills[index1];
            _Bills[index1] = _Bills[index2];
            _Bills[index2] = otherBill;
        }
        private bool WordLowerInAlphabetThen(string word1, string word2)
        {
            char[] char1 = word1.ToCharArray();
            char[] char2 = word2.ToCharArray();
            for (int i = 0; i < char1.Length && i < char2.Length; i++)
            {
                int value = char1[i].CompareTo(char2[i]);
                if (value > 0)
                    return true;
                else if (value < 0)
                    break;
            }
            return false;
        }
        private bool IsHeigherDateTimeThen(DateTime time1, DateTime time2)
        {
            if (time1.CompareTo(time2) > 0)
                return true;
            return false;
        }
        #endregion
        #region ABC
        private void SortOnAlphabeticalOrder()
        {
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int i = 1; i < _Bills.Count; i++)
                {
                    if (WordLowerInAlphabetThen(_Bills[i - 1].payer, _Bills[i].payer))
                    {
                        SwitchMethod(i - 1, i);
                        amount++;
                    }
                }
            }
            UpdateCalculation();
            UpdateList();
        }
        #endregion
        #region Price
        private void SortOnPriceOrder()
        {

            int amount = 1;
            while (amount > 0)
            {
                Debug.WriteLine("Sort on price");
                amount = 0;
                for (int i = 1; i < _Bills.Count; i++)
                {
                    if (_Bills[i - 1].price > _Bills[i].price)
                    {
                        SwitchMethod(i - 1, i);
                        amount++;
                    }
                }
            }
            UpdateCalculation();
            UpdateList();
        }
        #endregion
        #region Date
        private void SortOnDateOrder()
        {
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int i = 1; i < _Bills.Count; i++)
                {
                    if (IsHeigherDateTimeThen(_Bills[i - 1].time, _Bills[i].time))
                    {
                        SwitchMethod(i - 1, i);
                        amount++;
                    }
                }
            }
            UpdateCalculation();
            UpdateList();
        }
        #endregion
        #region Description
        private void SortOnDescriptionOrder()
        {
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int i = 1; i < _Bills.Count; i++)
                {
                    if (_Bills[i - 1].description.Length > _Bills[i].description.Length)
                    {
                        SwitchMethod(i - 1, i);
                        amount++;
                    }
                }
            }
            UpdateCalculation();
            UpdateList();
        }

        #endregion
        #endregion
        #endregion
        #region Data Handler
        private void SaveList()
        {
            string key = "Bill_Value_";
            int amountOfBills = 0;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Amount_Of_Bills"))
            {
                amountOfBills = (int)ApplicationData.Current.LocalSettings.Values["Amount_Of_Bills"];
                for (int i = 0; i < amountOfBills; i++)
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key + i))
                        ApplicationData.Current.LocalSettings.Values.Remove(key + i);
            }
            amountOfBills = _Bills.Count;
            for (int i = 0; i < amountOfBills; i++)
            {
                string[] bill_information = new string[4];
                bill_information[0] = _Bills[i].payer;
                bill_information[1] = _Bills[i].price.ToString();
                bill_information[2] = _Bills[i].time.ToString();
                bill_information[3] = _Bills[i].description;
                Debug.WriteLine("Saving: " + key + i);
                ApplicationData.Current.LocalSettings.Values[key + i] = bill_information;
            }
            ApplicationData.Current.LocalSettings.Values["Amount_Of_Bills"] = amountOfBills;
        }
        private void LoadList()
        {
            string key = "Bill_Value_";
            List<string[]> data = new List<string[]>();
            int amountOfBills = 0;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Amount_Of_Bills"))
                amountOfBills = (int)ApplicationData.Current.LocalSettings.Values["Amount_Of_Bills"];

            for (int i = 0; i < amountOfBills; i++)
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key + i))
                {
                    Debug.WriteLine("Loading: " + key + i);
                    data.Add((string[])(ApplicationData.Current.LocalSettings.Values[key + i]));
                }

            if (_Bills.Count > 0)
                _Bills.Clear();
            foreach (string[] info in data)
            {
                string[] array = info[2].Split('/', ' ');
                int year = int.Parse(array[2].Trim());
                int month = int.Parse(array[0].Trim());
                int day = int.Parse(array[1].Trim());

                Bill bill = CreateBill(info[0].Trim(), info[1].Trim(), year, month, day, info[3].Trim());
                if (bill != null)
                    _Bills.Add(bill);
            }
        }
        #endregion

    }
}
