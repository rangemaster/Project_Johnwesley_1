using BillJazzly.Bill;
using BillJazzly.SingleTon;
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
            UpdateYearButtons();
            _Feedback_tx.Text = "";
        }
        private void InitTextBlocks()
        {
            TextBlock ytb = new TextBlock();
            ytb.Text = "Year";

        }
        void UpdateYearButtons()
        {
            // TODO: Sort on ABC
            DataHolder.Get.SortBillsOnYears();
            ClearYear_stackpanel();
            foreach (int year in DataHolder.Get.Bills.Keys)
            { AddYearRadioButton(year); }
        }
        void UpdateMonthButtons(int year)
        {
            // TODO: Sort on ABC
            if (year != -1)
            {
                DataHolder.Get.SortBillsOnMonths();
                ClearMonth_stackpanel();
                foreach (string month in DataHolder.Get.GetMonthsFromYear(year).Keys)
                { AddMonthRadioButton(month); }
            }
        }
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
            RadioButton button = new RadioButton();
            button.Content = year;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            button.Click += _Year_rbn_Click;
            _Year_stackpanel.Children.Add(button);
        }
        void AddMonthRadioButton(string month)
        {
            RadioButton button = new RadioButton();
            button.Content = month;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _Month_stackpanel.Children.Add(button);
        }
        void WrongInputY() { WrongInputY("0"); }
        void WrongInputM() { WrongInputM("0"); }
        void WrongInputY(string feedback) { WrongInput("Year", feedback); }
        void WrongInputM(string feedback) { WrongInput("Month", feedback); }
        void WrongInput(string location, string feedback) { SetFeedback("Wrong Input (" + feedback + ")"); } // TODO: Magic cookie
        private void _Year_Input_bn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int year = int.Parse(_Year_Input_tx.Text);
                if (year < DataHolder.Get.GetSettingsValue("Min_Year"))
                { WrongInputY("To Low"); return; }
                else if (year > DataHolder.Get.GetSettingsValue("Max_Year"))
                { WrongInputY("To High"); return; }
                DataHolder.Get.Bills.Add(year, new Dictionary<string, List<RMSBill>>());
            }
            catch (FormatException) { WrongInputY("Format"); return; } // TODO: Magic cookie
            catch (ArgumentException) { WrongInputY("Allready exists"); return; } // TODO: Magic cookie
            _Year_Input_tx.Text = "";
            UpdateYearButtons();
            HideYearInput();
        }
        private void _Month_Input_bn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string month = _Month_Input_tx.Text;
                try { int.Parse(month); WrongInputM("Use Letters, not numbers"); return; }
                catch (FormatException) { }
                DataHolder.Get.Bills[SelectedYear()].Add(month, new List<RMSBill>());
            }
            catch (FormatException) { WrongInputM("Format"); return; } // TODO: Magic cookie
            catch (ArgumentException) { WrongInputM("Allready exists"); return; } // TODO: Magic cookie
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
            int year = int.Parse(content);
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
            if (SelectedYear() == -1)
                QuickFix();
            for (int i = 0; i < _Year_stackpanel.Children.Count; i++)
            {
                if ((_Year_stackpanel.Children[i] as RadioButton).IsChecked == true)
                    state = 1;
            }
            if (state != 1)
            { SetFeedback("Select a year to continue"); return; } // TODO: Magic cookie
            for (int i = 0; i < _Month_stackpanel.Children.Count; i++)
            {
                if ((_Month_stackpanel.Children[i] as RadioButton).IsChecked == true)
                    state = 2;
            }
            if (state != 2)
            { SetFeedback("Select a month to continue"); return; } // TODO: Magic cookie
            SetFeedback("Continue");
            GoToBillPage();
        }
        private void GoToBillPage()
        {
            int year = SelectedYear();
            string month = SelectedMonth();
            List<RMSBill> bills = null;
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
                { return int.Parse((_Year_stackpanel.Children[i] as RadioButton).Content.ToString()); }
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
                { return (_Month_stackpanel.Children[i] as RadioButton).Content.ToString(); }
            }
            return null;
        }
        private void QuickFix()
        {
            (_Year_stackpanel.Children[0] as RadioButton).IsChecked = true;
            UpdateMonthButtons(int.Parse((_Year_stackpanel.Children[0] as RadioButton).Content.ToString()));
            (_Month_stackpanel.Children[0] as RadioButton).IsChecked = true;
        }
        private void _Year_plus_bn_Click(object sender, RoutedEventArgs e)
        {
            ShowYearInput();
        }
        private void _Year_min_bn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedYear() < 0)
            { SetFeedback("First Select a Year"); return; } // TODO: Magic cookie
            DataHolder.Get.Bills.Remove(SelectedYear());
            UpdateYearButtons();
        }
        private void _Month_plus_bn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedYear() < 0)
            { SetFeedback("First Select a Year"); return; } // TODO: Magic cookie
            ShowMonthInput();
        }
        private void _Month_min_bn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedYear() < 0)
            { SetFeedback("First Select a Year"); return; } // TODO: Magic cookie
            if (SelectedMonth() == null)
            { SetFeedback("First Select a Month"); return; } // TODO: Magic cookie
            DataHolder.Get.Bills[SelectedYear()].Remove(SelectedMonth());
            UpdateMonthButtons(SelectedYear());
        }
        private void ShowYearInput() { _Input_Year_stackpanel.Visibility = System.Windows.Visibility.Visible; }
        private void ShowMonthInput() { _Input_Month_stackpanel.Visibility = System.Windows.Visibility.Visible; }
        private void HideYearInput() { _Input_Year_stackpanel.Visibility = System.Windows.Visibility.Collapsed; }
        private void HideMonthInput() { _Input_Month_stackpanel.Visibility = System.Windows.Visibility.Collapsed; }
    }
}
