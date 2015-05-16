using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Microsoft.Band;
using Microsoft.Band.Sensors;
using SuiteValue.UI.MVVM;

namespace BandwagonApp.ViewModels
{
    public class SensorViewModel : ViewModelBase
    {}
    public class SensorViewModel<T> : SensorViewModel where T : IBandSensorReading
    {
        protected const string InitialMessage = "Not Collecting";
        protected const string StartCollecting = "Starting to Collect";

        protected bool IsCollecting { get; set; }

        private bool _hasConnection;

        public bool HasConnection
        {
            get { return _hasConnection; }
            set
            {
                if (value != _hasConnection)
                {
                    _hasConnection = value;
                    OnPropertyChanged();
                    ToggleSensorCommand.Refresh();
                }
            }
        }

        

        private DelegateCommand _toggleSensorCommand;

        public DelegateCommand ToggleSensorCommand
        {
            get
            {
                return _toggleSensorCommand ?? (_toggleSensorCommand = new DelegateCommand(
                    () =>
                    {

                        try
                        {
                            if (IsCollecting)
                            {
                                StopCollection();
                                return;
                            }
                            StartCollection();
                        }
                        catch (Exception e)
                        {
                            ErrorMessage = "Error with Sensor: " + Name + ": " + e.Message;
                        }
                    },
                    () => HasConnection));
            }
        }

        protected async virtual void StartCollection()
        {
            if (CheckIsSupported()) return;
            var hasPermission = await CheckForPermission();
            if (hasPermission)
            {
                await StartReading();
            }
            else
            {
                ErrorMessage = "Sorry. User has denied permissions for sensor: " + Name;
            }
        }

        protected async virtual Task StartReading()
        {
            Result = StartCollecting;
            BandSensor.ReadingChanged += SensorManager_ReadingChanged;
            IsCollecting = await BandSensor.StartReadingsAsync();
        }

        private bool CheckIsSupported()
        {
            if (!BandSensor.IsSupported)
            {
                ErrorMessage = "Sorry." + Name + " Sensor is not supported on this Band.";
                return true;
            }
            return false;
        }

        protected async virtual Task<bool> CheckForPermission()
        {
            if (BandSensor.GetCurrentUserConsent() != UserConsent.Granted)
            {
                if (await BandSensor.RequestUserConsentAsync())
                {
                    return BandSensor.GetCurrentUserConsent() == UserConsent.Granted;
                }       
                return false;
            }
            return true;
        }

        protected async virtual void StopCollection()
        {
            BandSensor.ReadingChanged -= SensorManager_ReadingChanged;
            IsCollecting = false;
            await BandSensor.StopReadingsAsync();
            Result = InitialMessage;
        }

        protected async void SensorManager_ReadingChanged(object sender, BandSensorReadingEventArgs<T> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (FormatResultAction != null)
                {
                    Result = FormatResultAction(e.SensorReading) + ". T: " + e.SensorReading.Timestamp;
                }
            });
        }

        private IBandSensor<T> _bandSensor;

        public IBandSensor<T> BandSensor
        {
            get { return _bandSensor; }
            set
            {
                if (value != _bandSensor)
                {
                    _bandSensor = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _result = InitialMessage;

        public string Result
        {
            get { return _result; }
            set
            {
                if (value != _result)
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private Func<T, string> _formatResultAction;

        public Func<T, string> FormatResultAction
        {
            get { return _formatResultAction; }
            set
            {
                if (value != _formatResultAction)
                {
                    _formatResultAction = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
