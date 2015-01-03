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
    public sealed partial class BillOverviewPage : Page
    {
        private int _flyout_selected = 0; // 1 = year, 2 = month
        private List<RadioButton> yearButtons = null;
        private List<RadioButton> monthButtons = null;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public BillOverviewPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Init();
        }

        #region Initialisation
        private void Init()
        {
            this.yearButtons = new List<RadioButton>();
            this.monthButtons = new List<RadioButton>();
            InitYearButtons();
            AddYearButtons();
            AddMonthButtons();
        }
        private void InitYearButtons()
        { LoadYears(); }
        private void AddYearButtons()
        {
            this.Overview_Year_Panel.Children.Clear();
            foreach (RadioButton button in yearButtons)
            { this.Overview_Year_Panel.Children.Add(button); }
        }
        private void AddMonthButtons()
        {
            this.Overview_Month_Panel.Children.Clear();
            foreach (RadioButton button in monthButtons)
            { this.Overview_Month_Panel.Children.Add(button); }
        }

        #region Loading Bills
        private List<Bill> LoadBills(string billYear, string billMonth)
        {
            string key = Settings.CreateKey(billYear, billMonth);
            List<Bill> bills = new List<Bill>();
            List<string[]> data = new List<string[]>();
            int amountOfBills = 0;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Settings.KeyToAmountOfBillsKey(key)))
                amountOfBills = (int)ApplicationData.Current.LocalSettings.Values[Settings.KeyToAmountOfBillsKey(key)];

            for (int i = 0; i < amountOfBills; i++)
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key + i))
                {
                    Debug.WriteLine("Loading: " + key + i);
                    data.Add((string[])(ApplicationData.Current.LocalSettings.Values[key + i]));
                }

            if (bills.Count > 0)
                bills.Clear();
            foreach (string[] info in data)
            {
                string[] array = info[2].Split('/', ' ');
                int year = int.Parse(array[2].Trim());
                int month = int.Parse(array[0].Trim());
                int day = int.Parse(array[1].Trim());

                Bill bill = CreateBill(info[0].Trim(), info[1].Trim(), year, month, day, info[3].Trim());
                if (bill != null)
                    bills.Add(bill);
            }
            //if (bills.Count > 0)
            return bills;
            //return null;
        }
        private Bill CreateBill(string payer, string price, int year, int month, int day, string description)
        {
            return new Bill.BillBuilder()
            .Payer(payer)
            .Price(Double.Parse(price))
            .Time(new DateTime(year, month, day))
            .Description(description)
            .Build();
        }
        #endregion

        #endregion

        #region NavigationHelper registration
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        { }
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

        #region Buttons
        private void Add_Year_bn_Click(object sender, RoutedEventArgs e)
        { AddYear(); }
        private void Add_Month_bn_Click(object sender, RoutedEventArgs e)
        { AddMonth(); }
        private void Remove_Year_bn_Click(object sender, RoutedEventArgs e)
        { RemoveSelectedYear(); }
        private void Remove_Month_bn_Click(object sender, RoutedEventArgs e)
        { RemoveSelectedMonth(); }
        private void Save_Year_bn_Click(object sender, RoutedEventArgs e)
        { SaveYears(); }
        private void Save_Month_bn_Click(object sender, RoutedEventArgs e)
        { SaveMonthsOfYear(GetSelectedYear()); }
        private void _Go_Bn_Click(object sender, RoutedEventArgs e)
        { Go(); }
        private void Flyout_Save_bn_Click(object sender, RoutedEventArgs e)
        { SaveFlyout(); }
        private void Flyout_Close_bn_Click(object sender, RoutedEventArgs e)
        { HideFlyout(); }
        #endregion
        #region Button Functions
        #region Add Year
        private void AddYear()
        {
            this._flyout_selected = Flyout_years;
            this.Flyout_Input_tx.PlaceholderText = "[Year]";
            this.Flyout_Input_tx.Focus(FocusState.Keyboard);
            ShowFlyout();
            Feedback("Unsuccesfull added next year");
        }
        #endregion
        #region Add Month
        private void AddMonth()
        {
            this._flyout_selected = Flyout_months;
            this.Flyout_Input_tx.PlaceholderText = "[Month]";
            this.Flyout_Input_tx.Focus(FocusState.Keyboard);
            ShowFlyout();
            Feedback("Unsuccesfull added next month");
        }
        #endregion
        #region Remove Year
        private void RemoveSelectedYear()
        {
            for (int i = 0; i < yearButtons.Count; i++)
            {
                if (yearButtons[i].IsChecked == true)
                {
                    for (int j = monthButtons.Count - 1; j >= 0; j--)
                    {
                        string name = monthButtons[j].Content.ToString();
                        string key = Settings.CreateMonthKey(yearButtons[i].Content.ToString());
                        if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                        {
                            ApplicationData.Current.LocalSettings.Values.Remove(key);
                        }
                        monthButtons.RemoveAt(j);
                        Feedback("Succesfully Removed Month '" + name + "' in year " + yearButtons[i].Content.ToString());
                    }
                    monthButtons.Clear();
                    Feedback("Succesfully Removed '" + yearButtons[i].Content.ToString() + "'.");
                    yearButtons.RemoveAt(i);
                    AddMonthButtons();
                    AddYearButtons();
                    return;
                }
            }
            Feedback("Select a year to remove it");
        }
        #endregion
        #region Remove Month
        private void RemoveSelectedMonth()
        {
            for (int i = 0; i < monthButtons.Count; i++)
            {
                if (monthButtons[i].IsChecked == true)
                {
                    string year = GetSelectedYear();
                    string key = Settings.CreateMonthKey(year);
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                    {
                        ApplicationData.Current.LocalSettings.Values.Remove(key);
                        Feedback("Succesfully Removed '" + monthButtons[i].ToString() + "'.");
                    }
                    monthButtons.RemoveAt(i);
                    AddMonthButtons();
                    return;
                }
            }
            Feedback("Select a month to remove it");
        }
        #endregion
        #region Save Years
        private void SaveYears()
        {
            string yearKey = Settings.CreateYearKey();
            string[] yearArray = GetAvailableYears();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(yearKey))
            { ApplicationData.Current.LocalSettings.Values.Remove(yearKey); }
            if (yearArray.Length > 0)
            {
                ApplicationData.Current.LocalSettings.Values[yearKey] = yearArray;
                Feedback("Succesfully Saved years (" + yearButtons.Count + ")");
            }
        }
        private string[] GetAvailableYears()
        {
            if (yearButtons.Count > 0)
            {
                string[] array = new string[yearButtons.Count];
                for (int i = 0; i < yearButtons.Count; i++)
                { array[i] = yearButtons[i].Content.ToString().Trim(); }
                return array;
            }
            return new string[] { };
        }
        #endregion
        #region Save Months
        private void SaveMonthsOfYear(string year)
        {
            if (year != null)
            {
                string monthKey = Settings.CreateMonthKey(year);
                string[] monthArray = GetAvailableMonths();

                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(monthKey))
                { ApplicationData.Current.LocalSettings.Values.Remove(monthKey); }
                ApplicationData.Current.LocalSettings.Values[monthKey] = monthArray;
                Feedback("Succesfully Saved year(" + year + ") (" + monthArray.Length + " months)");
            }
            else { Feedback("Save Months of year (" + year + ")" + " UnSuccesfull!"); }
        }
        private string[] GetAvailableMonths()
        {
            string[] array = new string[monthButtons.Count];
            for (int i = 0; i < monthButtons.Count; i++)
            { array[i] = monthButtons[i].Content.ToString().Trim(); }
            return array;
        }
        #endregion
        #region LoadYears
        private void LoadYears()
        {
            string yearKey = Settings.CreateYearKey();
            string[] yearArray = null;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(yearKey))
            {
                yearArray = (string[])ApplicationData.Current.LocalSettings.Values[yearKey];
                Feedback("Succesfully loaded years");
            }
            else
            {
                yearArray = new string[] { DateTime.Now.ToString("yyyy") };
                Feedback("Create New Year Radio Button (" + yearArray[0].ToString() + ")");
            }
            yearButtons.Clear();
            for (int i = 0; i < yearArray.Length; i++)
            {
                RadioButton button = new RadioButton();
                button.Content = yearArray[i].ToString();
                button.Click += YearSelected_Click;
                this.yearButtons.Add(button);
            }
            SaveYears();
        }
        #endregion
        #region LoadMonths
        private void LoadMonths()
        {
            string year = GetSelectedYear();
            string[] months = null;
            string key = Settings.CreateMonthKey(year);
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                months = (string[])ApplicationData.Current.LocalSettings.Values[key];
                Feedback("Succesfully loaded Months of year(" + year + ")");
            }
            else
            {
                string[] monthNames = new string[] { "Januari", "Februari", "March", "April", "May", "June", "July", "August", "September", "Oktober", "November", "December" };
                int index = int.Parse(DateTime.Now.ToString("MM")) - 1;
                string month = monthNames[index];
                months = new string[] { month };
                Feedback("Month Januari Created");
            }
            monthButtons.Clear();
            foreach (string month in months)
            {
                RadioButton button = new RadioButton();
                button.Content = month;
                monthButtons.Add(button);
            }
            SaveMonthsOfYear(year);
            AddMonthButtons();
        }
        #endregion
        #region Go Button
        private void Go()
        {
            string year = GetSelectedYear();
            string month = GetSelectedMonth();
            string total = null;
            string bill_key = Settings.CreateKey(year, month);
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Settings.KeyToTotalMoneyKey(bill_key)))
            { total = ApplicationData.Current.LocalSettings.Values[Settings.KeyToTotalMoneyKey(bill_key)].ToString(); }

            List<string> info = new List<string>();
            info.Add(year);
            info.Add(month);
            info.Add(total);
            foreach (string text in info) { Debug.WriteLine("Info: " + (text == null ? "Null" : text)); }

            List<Bill> bills = LoadBills(year, month);

            if (info[0] == null) { Feedback("Select a year!"); return; }
            if (info[1] == null) { Feedback("Select a month!"); return; }
            if (info[2] == null) { Feedback("New Table is being created"); }
            if (bills == null) { Feedback("Bills could not be found (Error" + year + ", " + month + ")"); }
            Tuple<List<Bill>, List<string>> data = new Tuple<List<Bill>, List<string>>(bills, info);
            this.Frame.Navigate(typeof(BillPage), data);
        }
        private void Feedback(string feedback)
        { this._Feedback_tx.Text = "Feedback: " + feedback; Debug.WriteLine("Debug: Feedback: " + feedback); }
        private string GetSelectedYear()
        {
            string year = null;
            foreach (RadioButton button in yearButtons)
            { if (button.IsChecked == true) { year = button.Content.ToString(); break; } }
            return year;
        }
        private string GetSelectedMonth()
        {
            string month = null;
            foreach (RadioButton button in monthButtons)
            { if (button.IsChecked == true) { month = button.Content.ToString(); break; } }
            return month;
        }
        #endregion
        #region YearRadioButton Checked
        private void YearSelected_Click(object sender, RoutedEventArgs e)
        { LoadMonths(); }
        #endregion
        #region Show Flyout
        private void ShowFlyout() { this.Add_Flyout.Visibility = Windows.UI.Xaml.Visibility.Visible; }
        #endregion
        #region Hide Flyout
        private void HideFlyout() { this.Add_Flyout.Visibility = Windows.UI.Xaml.Visibility.Collapsed; }
        #endregion
        #region Save Flyout
        private void SaveFlyout()
        {
            string text = Flyout_Input_tx.Text;
            RadioButton button = new RadioButton();
            button.Content = text;

            if (_flyout_selected == Flyout_years)
            {
                button.Click += YearSelected_Click;
                this.yearButtons.Add(button);
                AddYearButtons();
                Feedback("Succesfully Added year: " + text);
            }// years
            else if (_flyout_selected == Flyout_months)
            {
                this.monthButtons.Add(button);
                AddMonthButtons();
                Feedback("Succesfully Added month: " + text);
            }// months
            _flyout_selected = Flyout_none;
            Flyout_Input_tx.Text = "";
            HideFlyout();
        }
        private int Flyout_none = 0;
        private int Flyout_years = 1;
        private int Flyout_months = 2;
        #endregion
        #endregion

    }
}
