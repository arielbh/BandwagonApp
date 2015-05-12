using System.Linq;
using System.Threading.Tasks;
using Microsoft.Band;
using SuiteValue.UI.MVVM;

namespace BandwagonApp.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        public async Task GetBands()
        {
            Bands = await BandClientManager.Instance.GetBandsAsync();
            SelectedBand = Bands.FirstOrDefault();
        }


        private IBandInfo[] _bands;
        public IBandInfo[] Bands
        {
            get { return _bands; }
            set
            {
                _bands = value;
                OnPropertyChanged();
            }
        }
        private IBandInfo _selectedBand;

        public IBandInfo SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                _selectedBand = value;
                OnPropertyChanged();
                ConnectCommand.Refresh();
            }
        }

        private DelegateCommand _refreshCommand;

        public DelegateCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new DelegateCommand(async () =>
                {
                    await GetBands();
                }));
            }
        }

        private DelegateCommand _connectCommand;

        public DelegateCommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new DelegateCommand(async () =>
                {
                    try
                    {
                        BandClient = await
                            BandClientManager.Instance.ConnectAsync(SelectedBand);
                        // do work after successful connect
                        try
                        {
                            FirmwareVersion = await BandClient.GetFirmwareVersionAsync();
                            HardwareVersion = await BandClient.GetHardwareVersionAsync();
                        }
                        catch (BandException ex)
                        {
                            // handle any BandExceptions
                        }
                    }
                    catch (BandException ex)
                    {

                    }
                }
                    ,
                    () => SelectedBand != null));
            }
        }

        private IBandClient _bandClient;

        public IBandClient BandClient
        {
            get { return _bandClient; }
            set
            {
                if (value != _bandClient)
                {
                    _bandClient = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _firmwareVersion;

        public string FirmwareVersion
        {
            get { return _firmwareVersion; }
            set
            {
                if (value != _firmwareVersion)
                {
                    _firmwareVersion = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _hardwareVersion;

        public string HardwareVersion
        {
            get { return _hardwareVersion; }
            set
            {
                if (value != _hardwareVersion)
                {
                    _hardwareVersion = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}