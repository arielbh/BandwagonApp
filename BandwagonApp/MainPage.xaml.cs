using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BandwagonApp.ViewModels;
using Microsoft.Band;
using SuiteValue.UI.MVVM;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace BandwagonApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {


        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            InfoViewModel = new InfoViewModel();
            InfoViewModel.PropertyChanged += InfoViewModel_PropertyChanged;
            AreWeLockedOnInfo = true;
            DataContext = this;
        }

        private void InfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BandClient")
            {
                if (InfoViewModel.BandClient != null)
                {
                    SensorsViewModel = new SensorsViewModel(InfoViewModel.BandClient);
                    StuffViewModel = new StuffViewModel(InfoViewModel.BandClient);
                    AreWeLockedOnInfo = false;
                }

            }
        }

        private bool _areWeLockedOnInfo;

        public bool AreWeLockedOnInfo
        {
            get { return _areWeLockedOnInfo; }
            set
            {
                if (value != _areWeLockedOnInfo)
                {
                    _areWeLockedOnInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await InfoViewModel.GetBands();
        }

   
     

        private StuffViewModel _stuffViewModel;

        public StuffViewModel StuffViewModel
        {
            get { return _stuffViewModel; }
            set
            {
                if (value != _stuffViewModel)
                {
                    _stuffViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private SensorsViewModel _sensorsViewModel;

        public SensorsViewModel SensorsViewModel
        {
            get { return _sensorsViewModel; }
            set
            {
                if (value != _sensorsViewModel)
                {
                    _sensorsViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private InfoViewModel _infoViewModel;

        public InfoViewModel InfoViewModel
        {
            get { return _infoViewModel; }
            set
            {
                if (value != _infoViewModel)
                {
                    _infoViewModel = value;
                    OnPropertyChanged();
                }
            }
        }
     









        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
