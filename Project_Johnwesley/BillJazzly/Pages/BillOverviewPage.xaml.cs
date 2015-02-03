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
            CheckYearButtons();
        }
        private void InitTextBlocks()
        {
            TextBlock ytb = new TextBlock();
            ytb.Text = "Year";

        }
        void CheckYearButtons()
        {
            foreach (int year in DataHolder.Get.Bills.Keys)
            { AddYearRadioButton(year); }
        }
        void CheckMonthButtons(int year)
        {
            foreach (string month in DataHolder.Get.GetMonthsFromYear(year).Keys)
            { AddMonthRadioButton(month); }
        }
        void AddYearRadioButton(int year)
        {
            // TODO: Add year buttons
            RadioButton button = new RadioButton();
            button.Content = year;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            button.Click += _Year_bn_Click;
            _Year_stackpanel.Children.Add(button);
        }
        void AddMonthRadioButton(string month)
        {
            // TODO: Add month buttons
            RadioButton button = new RadioButton();
            button.Content = month;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _Month_stackpanel.Children.Add(button);
        }
        private void _Year_bn_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as RadioButton).Content.ToString();
            int year = int.Parse(content);
            CheckMonthButtons(year);
        }
        private void _Back_bn_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            { this.NavigationService.GoBack(); }
        }
    }
}
