using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RestSharp;

namespace WeatherCheck.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private string? _cityName;
        private string? _weatherCondition;
        private double _temperature;
        private string? _forecast;

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged();
            }
        }

        public string WeatherCondition
        {
            get => _weatherCondition;
            set
            {
                _weatherCondition = value;
                OnPropertyChanged();
            }
        }

        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged();
            }
        }

        public string Forecast
        {
            get => _forecast;
            set
            {
                _forecast = value;
                OnPropertyChanged();
            }
        }

        public void GetWeather(string city)
        {
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid=4d6834e34439b678245f54d3a6efa6be&units=metric");
            var request = new RestRequest(Method.Get.ToString());
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);

                CityName = city;
                WeatherCondition = data.weather[0].description;
                Temperature = data.main.temp;
            }
        }

        public void GetForecast(string city)
        {
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid=4d6834e34439b678245f54d3a6efa6be&units=metric");
            var request = new RestRequest(Method.Get.ToString());
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);

                Forecast = string.Empty;

                for (int i = 0; i < data.list.Count && i < 8; i++)
                {
                    var forecast = data.list[i];
                    Forecast += $"{forecast.dt_txt}: {forecast.weather[0].description} - {forecast.main.temp}°C\n";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}