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
            Sensors = new SensorViewModel[]
            {
                new SensorViewModel<IBandHeartRateReading>
                {
                    BandSensor = _client.SensorManager.HeartRate,
                    Name = "Heart Rate",
                    FormatResultAction = r => "Heart Rate: " + r.HeartRate + " Quality: " + r.Quality,
                    HasConnection = true,
                },
                new SensorViewModel<IBandCaloriesReading>()
                {
                    BandSensor = _client.SensorManager.Calories,
                    Name = "Calories",
                    FormatResultAction = r => "Calories: " + r.Calories,
                    HasConnection = true,
                },
                new SensorViewModel<IBandSkinTemperatureReading>()
                {
                    BandSensor = _client.SensorManager.SkinTemperature,
                    Name = "Skin Temperture",
                    FormatResultAction = r => "Temperture: " + r.Temperature,
                    HasConnection = true,
                },
                new SensorViewModel<IBandContactReading>()
                {
                    BandSensor = _client.SensorManager.Contact,
                    Name = "Contact",
                    FormatResultAction = r => "Contact: " + r.State,
                    HasConnection = true,
                },
                new SensorViewModel<IBandDistanceReading>()
                {
                    BandSensor = _client.SensorManager.Distance,
                    Name = "Distance",
                    FormatResultAction = r => "Distance: " + r.TotalDistance + 
                                              " Motion: " + r.CurrentMotion + 
                                              " Pace:" + r.Pace +
                                              " Speed:" + r.Speed,

                    HasConnection = true,
                },
                new SensorViewModel<IBandUVReading>()
                {
                    BandSensor = _client.SensorManager.UV,
                    Name = "UV",
                    FormatResultAction = r => "Level: " + r.IndexLevel,
                    HasConnection = true,
                },
                new SensorViewModel<IBandPedometerReading>()
                {
                    BandSensor = _client.SensorManager.Pedometer,
                    Name = "Pedometer",
                    FormatResultAction = r => "Steps: " + r.TotalSteps,
                    HasConnection = true,
                },
                new SensorViewModel<IBandGyroscopeReading>()
                {
                    BandSensor = _client.SensorManager.Gyroscope,
                    Name = "Gyroscope",
                    FormatResultAction = r => "X: " + r.AngularVelocityX +
                                              "Y: " + r.AngularVelocityY +
                                              "Z: " + r.AngularVelocityZ,
                    HasConnection = true,
                },
                new SensorViewModel<IBandAccelerometerReading>()
                {
                    BandSensor = _client.SensorManager.Accelerometer,
                    Name = "Accelerometer",
                    FormatResultAction = r => "X: " + r.AccelerationX +
                                              "Y: " + r.AccelerationY +
                                              "Z: " + r.AccelerationZ,
                    HasConnection = true,
                }
            };
        }

        private SensorViewModel[] _sensors;

        public SensorViewModel[] Sensors
        {
            get { return _sensors; }
            set
            {
                if (value != _sensors)
                {
                    _sensors = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}