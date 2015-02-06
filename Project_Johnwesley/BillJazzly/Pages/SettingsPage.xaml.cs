using BillJazzly.SingleTon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BillJazzly.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : PageFunction<String>
    {
        private TextBox _Input_Min_Year_tx, _Input_Max_Year_tx, _Input_Salary_tx, _Input_FontSize_Overview_tx;
        private Button _Audio_on_bn, _Audio_off_bn, _Video_on_bn, _Video_off_bn;
        private Button _Audio_1_bn, _Audio_2_bn, _Audio_3_bn, _Audio_4_bn, _Audio_5_bn;
        private int _Volume;
        private DispatcherTimer _Timer = null;
        public SettingsPage()
        {
            InitializeComponent();
            Init();
            LoadSettings();
        }

        #region Init
        private void Init()
        {
            InitFields();
            AddSetting(Short_Keys.Settings.OverView_FontSize, _Input_FontSize_Overview_tx); 
            AddSetting(Short_Keys.Settings.Salary, _Input_Salary_tx);
            AddSetting(Short_Keys.Settings.Min_Bill_Year, _Input_Min_Year_tx);
            AddSetting(Short_Keys.Settings.Max_Bill_Year, _Input_Max_Year_tx);
            AddSetting(Short_Keys.Settings.Volume, _Audio_1_bn, _Audio_2_bn, _Audio_3_bn, _Audio_4_bn, _Audio_5_bn);
            AddSetting(Short_Keys.Settings.Audio, _Audio_on_bn, _Audio_off_bn);
            AddSetting(Short_Keys.Settings.Video, _Video_on_bn, _Video_off_bn);
        }
        private void InitFields()
        {
            _Input_Min_Year_tx = DefaultTextBoxSettings();
            _Input_Max_Year_tx = DefaultTextBoxSettings();
            _Input_Salary_tx = DefaultTextBoxSettings();
            _Input_FontSize_Overview_tx = DefaultTextBoxSettings();
            _Input_FontSize_Overview_tx.Text = DataHolder.Get.GetSettingsValue(Short_Keys.Settings.OverView_FontSize).ToString();
            _Audio_on_bn = DefaultButtonSettings(Short_Keys.Settings.On, AudioOn_Click);
            _Audio_off_bn = DefaultButtonSettings(Short_Keys.Settings.Off, AudioOff_Click);
            _Video_on_bn = DefaultButtonSettings(Short_Keys.Settings.On, VideoOn_Click);
            _Video_off_bn = DefaultButtonSettings(Short_Keys.Settings.Off, VideoOff_Click);
            _Audio_1_bn = DefaultButtonSettings("1", Audio_1_Click);
            _Audio_2_bn = DefaultButtonSettings("2", Audio_2_Click);
            _Audio_3_bn = DefaultButtonSettings("3", Audio_3_Click);
            _Audio_4_bn = DefaultButtonSettings("4", Audio_4_Click);
            _Audio_5_bn = DefaultButtonSettings("5", Audio_5_Click);
        }
        #endregion
        #region Add function
        private void AddOnOffSetting(string description, Action<object, RoutedEventArgs> On_Action, Action<object, RoutedEventArgs> Off_Action)
        { AddOnOffSetting(description, On_Action, Off_Action, false); }
        private void AddOnOffSetting(string description, Action<object, RoutedEventArgs> On_Action, Action<object, RoutedEventArgs> Off_Action, bool on)
        {
            Button On_bn = DefaultButtonSettings(Short_Keys.Settings.On);
            On_bn.Click += new RoutedEventHandler(On_Action);
            Button Off_bn = DefaultButtonSettings(Short_Keys.Settings.Off);
            Off_bn.Click += new RoutedEventHandler(Off_Action);
            AddOnOffSetting(description, On_bn, Off_bn, on);
        }
        private void AddOnOffSetting(string description, Button On_bn, Button Off_bn, bool on)
        {
            if (on) { On_bn.IsEnabled = false; Off_bn.IsEnabled = true; }
            else { On_bn.IsEnabled = true; Off_bn.IsEnabled = false; }
            AddSetting(description, On_bn, Off_bn);
        }
        private Button DefaultButtonSettings(string name)
        { return DefaultButtonSettings(name, null); }
        private Button DefaultButtonSettings(string name, Action<object, RoutedEventArgs> Method)
        {
            Button button = new Button();
            button.Content = name;
            button.Width = 50;
            button.Height = 30;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            button.Foreground = new SolidColorBrush(Colors.Green);
            button.Background = new SolidColorBrush(Colors.LightGray);
            button.BorderThickness = new Thickness(0);
            if (Method != null)
                button.Click += new RoutedEventHandler(Method);
            return button;
        }
        private TextBlock DefaultTextBlockSettings(string description)
        {
            TextBlock tb = new TextBlock();
            tb.Text = description;
            tb.TextAlignment = TextAlignment.Center;
            tb.FontSize = 20;
            tb.Width = 300;
            tb.Height = 50;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            return tb;
        }
        private TextBox DefaultTextBoxSettings()
        { return DefaultTextBoxSettings(""); }
        private TextBox DefaultTextBoxSettings(string text)
        {
            TextBox tbx = new TextBox();
            tbx.Width = 100;
            tbx.Height = 30;
            tbx.TextWrapping = TextWrapping.Wrap;
            tbx.FontSize = 20;
            tbx.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tbx.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            tbx.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tbx.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            return tbx;
        }
        private void AddSetting(string description, params UIElement[] Components)
        {
            StackPanel sp = new StackPanel();
            sp.Width = 600;
            sp.Orientation = Orientation.Horizontal;
            sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            sp.Margin = new Thickness(0, 10, 0, 10);
            sp.Background = new SolidColorBrush(Colors.Salmon);
            StackPanel left = new StackPanel();
            left.Width = sp.Width / 2;
            left.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            left.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            StackPanel right = new StackPanel();
            right.Width = sp.Width / 2;
            right.Orientation = Orientation.Horizontal;
            right.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            right.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            TextBlock tb = DefaultTextBlockSettings(description);
            left.Children.Add(tb);
            foreach (UIElement bn in Components)
            { right.Children.Add(bn); }
            sp.Children.Add(left);
            sp.Children.Add(right);
            _Settings_stackpanel.Children.Add(sp);
        }
        #endregion
        #region Functions
        private void Audio_1_Click(object sender, RoutedEventArgs e) { Volume_SetValue(1); }
        private void Audio_2_Click(object sender, RoutedEventArgs e) { Volume_SetValue(2); }
        private void Audio_3_Click(object sender, RoutedEventArgs e) { Volume_SetValue(3); }
        private void Audio_4_Click(object sender, RoutedEventArgs e) { Volume_SetValue(4); }
        private void Audio_5_Click(object sender, RoutedEventArgs e) { Volume_SetValue(5); }
        private void AudioOn_Click(object sender, RoutedEventArgs e)
        { AudioButton_SetEnabled(false); Debug.WriteLine("Audio On Pressed"); }
        private void AudioOff_Click(object sender, RoutedEventArgs e)
        { AudioButton_SetEnabled(false); } // TODO: To implement audio player (change '1' to '2')
        private void VideoOn_Click(object sender, RoutedEventArgs e)
        { VideoButton_SetEnabled(false); Debug.WriteLine("Video On Pressed"); }
        private void VideoOff_Click(object sender, RoutedEventArgs e)
        { VideoButton_SetEnabled(false); } // TODO: To implement video player (change '1' to '2')
        void _Input_FontSize_Overview_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            int size = 12;
            try
            { size = int.Parse(_Input_FontSize_Overview_tx.Text); }
            catch (FormatException) { SetFeedback(Short_Keys.Exception.FormatFontSize); return; }
            DataHolder.Get.SetSetting(Short_Keys.Settings.OverView_FontSize, size);
            _Settings_stackpanel.Children.Clear();
            Init();
        }
        #endregion

        private void _Back_bn_Click(object sender, RoutedEventArgs e)
        { if (this.NavigationService.CanGoBack) { this.NavigationService.GoBack(); } }
        private void _Reload_bn_Click(object sender, RoutedEventArgs e)
        { LoadSettings(); }
        private void _Save_bn_Click(object sender, RoutedEventArgs e)
        { SaveSettings(); }
        private void LoadSettings()
        {
            DataHolder.Get.LoadSettings();
            _Input_FontSize_Overview_tx.Text = DataHolder.Get.GetSettingsValue(Short_Keys.Settings.OverView_FontSize).ToString();
            _Input_Salary_tx.Text = DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Salary).ToString();
            _Input_Min_Year_tx.Text = DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Min_Bill_Year).ToString();
            _Input_Max_Year_tx.Text = DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Max_Bill_Year).ToString();
            Volume_SetValue(DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Volume));
            AudioButton_SetEnabled((DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Audio) == 1 ? true : false));
            VideoButton_SetEnabled((DataHolder.Get.GetSettingsValue(Short_Keys.Settings.Video) == 1 ? true : false));
            SetFeedback(Short_Keys.Settings.Loaded);
        }
        private void SaveSettings()
        {
            int fontsize = 10, salary = 0, year_min = 0, year_max = 0;
            try
            { fontsize = int.Parse(_Input_FontSize_Overview_tx.Text); }
            catch (FormatException) { SetFeedback(Short_Keys.Invalid.FontSze); return; }
            try
            { salary = int.Parse(_Input_Salary_tx.Text); }
            catch (FormatException) { SetFeedback(Short_Keys.Invalid.Salary); return; }
            try
            { year_min = int.Parse(_Input_Min_Year_tx.Text); }
            catch (FormatException) { SetFeedback(Short_Keys.Invalid.Year); return; }
            try
            { year_max = int.Parse(_Input_Max_Year_tx.Text); }
            catch (FormatException) { SetFeedback(Short_Keys.Invalid.Year); return; }
            DataHolder.Get.SetSetting(Short_Keys.Settings.OverView_FontSize, fontsize);
            DataHolder.Get.SetSetting(Short_Keys.Settings.Salary, salary);
            DataHolder.Get.SetSetting(Short_Keys.Settings.Min_Bill_Year, year_min);
            DataHolder.Get.SetSetting(Short_Keys.Settings.Max_Bill_Year, year_max);
            DataHolder.Get.SetSetting(Short_Keys.Settings.Volume, Volume_GetValue());
            DataHolder.Get.SetSetting(Short_Keys.Settings.Audio, (AudioButton_IsEnabled() ? 1 : 0));
            DataHolder.Get.SetSetting(Short_Keys.Settings.Video, (VideoButton_IsEnabled() ? 1 : 0));
            DataHolder.Get.SaveSettings();
            SetFeedback(Short_Keys.Settings.Saved);
        }
        private bool AudioButton_IsEnabled() { return _Audio_on_bn.IsEnabled; }
        private void AudioButton_SetEnabled(bool result) { _Audio_on_bn.IsEnabled = result; _Audio_off_bn.IsEnabled = !result; }
        private bool VideoButton_IsEnabled() { return _Video_on_bn.IsEnabled; }
        private void VideoButton_SetEnabled(bool result) { _Video_on_bn.IsEnabled = result; _Video_off_bn.IsEnabled = !result; }
        private void Volume_SetValue(int value)
        {
            _Volume = value;
            Button[] array = new Button[] { _Audio_1_bn, _Audio_2_bn, _Audio_3_bn, _Audio_4_bn, _Audio_5_bn };
            foreach (Button button in array)
            {
                if (value == int.Parse(button.Content.ToString()))
                { button.Foreground = new SolidColorBrush(Colors.Black); button.Background = new SolidColorBrush(Colors.Green); }
                else
                { button.Foreground = new SolidColorBrush(Colors.Black); button.Background = new SolidColorBrush(Colors.Red); }
            }
            Debug.WriteLine("Set Volume (" + _Volume + ")");
        }
        private int Volume_GetValue() { return _Volume; }
        private void SetFeedback(string feedback) { Timer(feedback); }
        private void Timer(string feedback)
        {
            _Feedback_tb.Text = feedback;
            if (_Timer == null)
            {
                _Timer = new DispatcherTimer();
                _Timer.Tick += Timer_Tick;
                _Timer.Interval = new TimeSpan(0, 0, 3);
            }
            else { _Timer.Stop(); }
            _Timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _Feedback_tb.Text = "";
            DispatcherTimer timer = (sender as DispatcherTimer);
            timer.Stop();
            timer = null;
        }
    }
}
