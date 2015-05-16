using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Microsoft.Band;
using Microsoft.Band.Sensors;
using SuiteValue.UI.MVVM;

namespace BandwagonApp.ViewModels
{
    public class SensorsViewModel : ViewModelBase
    {
        private readonly IBandClient _client;

        public SensorsViewModel(IBandClient client)
        {
            _client = client;                      
        }

        private DelegateCommand _temperturesCommand;

        public DelegateCommand TemperturesCommand
        {
            get
            {
                return _temperturesCommand ?? (_temperturesCommand = new DelegateCommand(async () =>
                    {
                        if (_tempertureStarted)
                        {
                            _client.SensorManager.SkinTemperature.ReadingChanged -= SkinTemperature_ReadingChanged;
                            Temperture = 0.0;
                            _tempertureStarted = false;
                            await _client.SensorManager.SkinTemperature.StopReadingsAsync();
                            return;
                        }
                        var hasPermission = _client.SensorManager.SkinTemperature.GetCurrentUserConsent() ==
                                            UserConsent.Granted;
                        if (!hasPermission)
                        {
                            hasPermission = await _client.SensorManager.SkinTemperature.RequestUserConsentAsync();
                        }
                        if (hasPermission)
                        {
                            _client.SensorManager.SkinTemperature.ReadingChanged += SkinTemperature_ReadingChanged;
                            try
                            {
                                _tempertureStarted = await _client.SensorManager.SkinTemperature.StartReadingsAsync();
                            }
                            catch (BandException)
                            {
                            }
                        }
                    }));
            }
        }

        private async void SkinTemperature_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandSkinTemperatureReading> e)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Temperture = e.SensorReading.Temperature;
            });
        }

        private double _temperture;

        public double Temperture
        {
            get { return _temperture; }
            set
            {
                if (value != _temperture)
                {
                    _temperture = value;
                    OnPropertyChanged();
                }
            }
        }

        private DelegateCommand _caloriesCommand;

        public DelegateCommand CaloriesCommand
        {
            get
            {
                return _caloriesCommand ?? (_caloriesCommand = new DelegateCommand(async () =>
                    {
                        if (_caloriesStarted)
                        {
                            _client.SensorManager.Calories.ReadingChanged -= Calories_ReadingChanged;
                            Calories = 0;
                            await _client.SensorManager.Calories.StopReadingsAsync();
                            _caloriesStarted = false;
                            return;
                        }
                        var hasPermission = _client.SensorManager.Calories.GetCurrentUserConsent() ==
                                            UserConsent.Granted;
                        if (!hasPermission)
                        {
                            hasPermission = await _client.SensorManager.Calories.RequestUserConsentAsync();
                        }
                        if (hasPermission)
                        {
                            _client.SensorManager.Calories.ReadingChanged += Calories_ReadingChanged;

                            try
                            {
                                _caloriesStarted = await _client.SensorManager.Calories.StartReadingsAsync();
                            }
                            catch (BandException e)
                            {
                            }
                        }

                    }));
            }
        }

        private async void Calories_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandCaloriesReading> e)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Calories = e.SensorReading.Calories;
            });
        }

        private long _calories;

        public long Calories
        {
            get { return _calories; }
            set
            {
                if (value != _calories)
                {
                    _calories = value;
                    OnPropertyChanged();
                }
            }
        }

        private DelegateCommand _heartRateCommand;

        public DelegateCommand HeartRateCommand
        {
            get
            {
                return _heartRateCommand ?? (_heartRateCommand = new DelegateCommand(
                    async () =>
                    {
                        if (_heartRateStarted)
                        {
                            _client.SensorManager.HeartRate.ReadingChanged -= OnHeartRateOnReadingChanged;
                            HeartRate = 0;
                            HeartRateQuality = null;
                            _heartRateStarted = false;
                            await _client.SensorManager.HeartRate.StopReadingsAsync();
                            return;
                        }
                        var hasPermission = _client.SensorManager.HeartRate.GetCurrentUserConsent() ==
                                            UserConsent.Granted;
                        IEnumerable<TimeSpan> supportedHeartBeatReportingIntervals =
                            _client.SensorManager.HeartRate.SupportedReportingIntervals;
               
                        // check current user heart rate consent
                        if (!hasPermission)
                        {

                           // user has not consented, request it
                           hasPermission = await
                                _client.SensorManager.HeartRate.RequestUserConsentAsync();
                            
                        }
                       if (hasPermission)
                       {
                           _client.SensorManager.HeartRate.ReadingChanged += OnHeartRateOnReadingChanged;
                           try
                           {
                                _heartRateStarted = await _client.SensorManager.HeartRate.StartReadingsAsync();
                           }
                           catch (BandException e)
                           {
                           }

                       }
                   }));
            }
        }

        private async void OnHeartRateOnReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> args)
        {
            // do work when the reading changes (i.e. update a UI element)
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                HeartRate = args.SensorReading.HeartRate;
                HeartRateQuality = args.SensorReading.Quality;
            });
        }

        private HeartRateQuality? _heartRateQuality;

        public HeartRateQuality? HeartRateQuality
        {
            get { return _heartRateQuality; }
            set
            {
                if (value != _heartRateQuality)
                {
                    _heartRateQuality = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _heartRate;
        private bool _heartRateStarted;
        private bool _caloriesStarted;
        private bool _tempertureStarted;

        public int HeartRate
        {
            get { return _heartRate; }
            set
            {
                if (value != _heartRate)
                {
                    _heartRate = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}