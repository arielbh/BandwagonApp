using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using SuiteValue.UI.MVVM;

namespace BandwagonApp.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            InfoViewModel = new InfoViewModel();
            InfoViewModel.PropertyChanged += InfoViewModel_PropertyChanged;
            AreWeLockedOnInfo = true;
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

        public Task GetBands()
        {
            return InfoViewModel.GetBands();
        }
    }
}
