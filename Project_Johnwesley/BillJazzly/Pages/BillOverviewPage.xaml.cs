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
    /// Interaction logic for BillOverviewPage.xaml
    /// </summary>
    public partial class BillOverviewPage : PageFunction<String>
    {
        private static string[] months_NL = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
        private static string[] months_ENG = new string[] { "january", "february", "march", "april", "may", "jun", "july", "august", "september", "october", "november", "december" };
        public BillOverviewPage()
        {
            InitializeComponent();
            _Main_stackpanel.Width = 900;
            _Year_stackpanel.Width = _Main_stackpanel.Width / 2;
            _Month_stackpanel.Width = _Main_stackpanel.Width / 2;
            UpdateYearButtons();
            _Feedback_tx.Text = "";
            SelectCurrentYear();
            SelectCurrentMonth();
        }
        private void SelectCurrentYear()
        {
            try
            {
                int year = int.Parse(DateTime.Now.ToString("yyyy"));
                for (int i = 0; i < _Year_stackpanel.Children.Count; i++)
                {
                    if ((_Year_stackpanel.Children[i] as RadioButton).Content.ToString().Contains(year.ToString()))
                    {
                        Debug.WriteLine("Current Year Found");
                        (_Year_stackpanel.Children[i] as RadioButton).IsChecked = true;
                        UpdateMonthButtons(int.Parse(StripCounter((_Year_stackpanel.Children[i] as RadioButton).Content.ToString())));
                    }
                }
            }
            catch (FormatException) { return; }

        }
        private void SelectCurrentMonth()
        {
            try
            {
                int month = int.Parse(DateTime.Now.ToString("MM"));
                string month_NL = months_NL[month - 1];
                string month_ENG = months_ENG[month - 1];
                for (int i = 0; i < _Month_stackpanel.Children.Count; i++)
                {
                    string m = (_Month_stackpanel.Children[i] as RadioButton).Content.ToString().ToLower();
                    if (m.Contains(month_NL) || m.Contains(month_ENG))
                    { (_Month_stackpanel.Children[i] as RadioButton).IsChecked = true; }
                }
            }
            catch (FormatException) { return; }
        }
        #region Update
        void UpdateYearButtons()
        {
            DataHolder.Get.SortBillsOnYears();
            ClearYear_stackpanel();
            foreach (int year in DataHolder.Get.Bills.Keys)
            { AddYearRadioButton(year); }
        }
        void UpdateMonthButtons(int year)
        {
            Debug.WriteLine("Update Month: " + year);
            if (year != -1)
            {
                ClearMonth_stackpanel();
                List<string> months = new List<string>();
                foreach (string month in DataHolder.Get.GetMonthsFromYear(year).Keys)
                { months.Add(month); }
                int amount = 1;
                while (amount > 0)
                {
                    amount = 0;
                    for (int i = 1; i < months.Count; i++)
                    {
                        int compare_value = CompareMonths(months_NL, months_ENG, months[i - 1], months[i]);
                        if (compare_value == 1)
                        {
                            string x = months[i - 1];
                            months[i - 1] = months[i];
                            months[i] = x;
                            amount++;
                        }
                    }
                }
                foreach (string month in months)
                { AddMonthRadioButton(year, month); }
            }
        }
        private string StripCounter(string year_or_month)
        {
            string content = year_or_month;
            string[] array = content.Split('(', ')');
            return array[0].Trim();
        }
        private int CompareMonths(string[] months_NL, string[] months_ENG, string month1, string month2)
        {
            int index_month1 = -1;
            int index_month2 = -1;
            month1 = month1.ToLower();
            month2 = month2.ToLower();
            for (int idx = 0; idx < months_NL.Length && idx < months_ENG.Length; idx++)
            {
                if (index_month1 == -1)
                    if (month1.Equals(months_NL[idx]) || month1.Equals(months_ENG[idx]))
                    { index_month1 = idx; }
                if (index_month2 == -1)
                    if (month2.Equals(months_NL[idx]) || month2.Equals(months_ENG[idx]))
                    { index_month2 = idx; }
                if (index_month1 < index_month2)
                    return 1;
                if (index_month1 > index_month2)
                    return -1;
            }
            return 0;
        }
        #endregion
        void ClearYear_stackpanel()
        {
            _Year_stackpanel.Children.Clear();
        }
        void ClearMonth_stackpanel()
        {
            _Month_stackpanel.Children.Clear();
        }
        void AddYearRadioButton(int year)
        {
            RadioButton button = CreateDefaultRadioButton((year + " (" + DataHolder.Get.Bills[year].Count + ")").ToString());
            button.Click += _Year_rbn_Click;
            _Year_stackpanel.Children.Add(button);
        }
        void AddMonthRadioButton(int year, string month)
        {
            RadioButton button = CreateDefaultRadioButton(month + " (" + DataHolder.Get.GetBillsFromMonth(year, month).Item2.Count + ")");
            _Month_stackpanel.Children.Add(button);
        }
        RadioButton CreateDefaultRadioButton(string name)
        {
            RadioButton button = new RadioButton();
            button.Content = name;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            button.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            button.FontSize = DataHolder.Get.GetSettingsValue(Short_Keys.Settings.OverView_FontSize);
            return button;
        }
        void WrongInput(string location, string feedback) { SetFeedback(Short_Keys.Execute.WrongInput(feedback)); }
        private void _Year_Input_bn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int year = int.Parse(_Year_Input_tx.Text);
                if (year < DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Min_Bill_Year))
                { SetFeedback(Short_Keys.Feedback.ToLow); return; }
                else if (year > DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Max_Bill_Year))
                { SetFeedback(Short_Keys.Feedback.ToHigh); return; }
                DataHolder.Get.Bills.Add(year, new Dictionary<string, Tuple<double, List<RMSBill>>>());
            }
            catch (FormatException) { SetFeedback(Short_Keys.Feedback.WrongInputFormat); return; }
            catch (ArgumentException) { SetFeedback(Short_Keys.Feedback.WrongInputDoesExists); return; }
            _Year_Input_tx.Text = "";
            UpdateYearButtons();
            HideYearInput();
        }
        private void _Month_Input_bn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string month = _Month_Input_tx.Text;
                try { int.Parse(month); SetFeedback(Short_Keys.Feedback.LettersNotNumbers); return; }
                catch (FormatException) { }
                DataHolder.Get.Bills[SelectedYear()].Add(month, new Tuple<double, List<RMSBill>>(DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Salary), new List<RMSBill>()));
            }
            catch (FormatException) { SetFeedback(Short_Keys.Feedback.WrongInputFormat); return; }
            catch (ArgumentException) { SetFeedback(Short_Keys.Feedback.WrongInputDoesExists); return; }
            _Month_Input_tx.Text = "";
            UpdateMonthButtons(SelectedYear());
            HideMonthInput();
        }
        private void _Reload_bn_Click(object sender, RoutedEventArgs e)
        { DataHolder.Get.LoadBills(); UpdateYearButtons(); }
        private void _Save_bn_Click(object sender, RoutedEventArgs e)
        { DataHolder.Get.SaveBills(); }
        private void _Year_rbn_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as RadioButton).Content.ToString();
            int year = int.Parse(StripCounter(content));
            UpdateMonthButtons(year);
        }
        private void _Back_bn_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            { this.NavigationService.GoBack(); }
        }
        private void _Open_bn_Click(object sender, RoutedEventArgs e)
        {
            int state = 0;
            for (int i = 0; i < _Year_stackpanel.Children.Count; i++)
            {
                if ((_Year_stackpanel.Children[i] as RadioButton).IsChecked == true)
                    state = 1;
            }
            if (state != 1)
            { SetFeedback(Short_Keys.Feedback.SelectYear); return; }
            for (int i = 0; i < _Month_stackpanel.Children.Count; i++)
            {
                if ((_Month_stackpanel.Children[i] as RadioButton).IsChecked == true)
                    state = 2;
            }
            if (state != 2)
            { SetFeedback(Short_Keys.Feedback.SelectMonth); return; }
            GoToBillPage();
        }
        private void GoToBillPage()
        {
            int year = SelectedYear();
            string month = SelectedMonth();
            Tuple<double, List<RMSBill>> bills = null;
            bills = DataHolder.Get.GetBillsFromMonth(year, month);
            BillPage page = new BillPage(year, month, bills);
            this.NavigationService.Navigate(page);
        }
        private void SetFeedback(string feedback)
        { _Feedback_tx.Text = feedback; }
        private int SelectedYear()
        {
            for (int i = 0; i < _Year_stackpanel.Children.Count; i++)
            {
                if ((_Year_stackpanel.Children[i] as RadioButton).IsChecked == true)
                {
                    string content = ((_Year_stackpanel.Children[i] as RadioButton).Content.ToString());
                    string[] array = content.Split('(', ')');
                    return int.Parse(array[0].Trim());
                }
            }
            return -1;
        }
        private int HeighestYear()
        {
            int heighest = 0;
            foreach (int year in DataHolder.Get.Bills.Keys)
            { if (heighest < year) { heighest = year; } }
            return heighest;
        }
        private string SelectedMonth()
        {
            for (int i = 0; i < _Month_stackpanel.Children.Count; i++)
            {
                if ((_Month_stackpanel.Children[i] as RadioButton).IsChecked == true)
                {
                    return StripCounter((_Month_stackpanel.Children[i] as RadioButton).Content.ToString());
                }
            }
            return null;
        }
        private void _Year_plus_bn_Click(object sender, RoutedEventArgs e)
        {
            ShowYearInput();
            _Year_Input_tx.Focus();
        }
        private void _Year_min_bn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedYear() < 0)
            { SetFeedback(Short_Keys.Feedback.SelectYear); return; }
            DataHolder.Get.Bills.Remove(SelectedYear());
            UpdateYearButtons();
        }
        private void _Month_plus_bn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedYear() < 0)
            { SetFeedback(Short_Keys.Feedback.SelectYear); return; }
            ShowMonthInput();
            _Month_Input_tx.Focus();
        }
        private void _Month_min_bn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedYear() < 0)
            { SetFeedback(Short_Keys.Feedback.SelectYear); return; }
            if (SelectedMonth() == null)
            { SetFeedback(Short_Keys.Feedback.SelectMonth); return; }
            DataHolder.Get.Bills[SelectedYear()].Remove(SelectedMonth());
            UpdateMonthButtons(SelectedYear());
        }
        private void ShowYearInput() { _Input_Year_stackpanel.Visibility = System.Windows.Visibility.Visible; }
        private void ShowMonthInput() { _Input_Month_stackpanel.Visibility = System.Windows.Visibility.Visible; }
        private void HideYearInput() { _Input_Year_stackpanel.Visibility = System.Windows.Visibility.Collapsed; }
        private void HideMonthInput() { _Input_Month_stackpanel.Visibility = System.Windows.Visibility.Collapsed; }
    }
}
