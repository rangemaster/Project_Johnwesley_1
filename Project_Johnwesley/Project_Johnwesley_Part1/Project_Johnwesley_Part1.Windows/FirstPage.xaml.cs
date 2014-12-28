using Project_Johnwesley_Part1.Common;
using System;
using System.Collections.Generic;
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
    public sealed partial class FirstPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public FirstPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        #region Initialisation
        #region Init
        private void Init() { }
        #endregion
        #region InitColors
        private void InitColors() { }
        #endregion
        #region InitPanels
        private void InitPanels() { }
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

        #region Logic

        #endregion
    }
}
