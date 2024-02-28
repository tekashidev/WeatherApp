using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherCheck.ViewModels;
using WeatherApp.Models;

namespace WeatherApp.View
{
    public partial class WeatherView : Window
    {
        public WeatherView()
        {
            InitializeComponent();

            var viewModel = new WeatherViewModel();
            DataContext = viewModel;

            CityInput.TextChanged += (sender, args) => viewModel.CityName = ((TextBox)sender).Text;

            GetWeatherButton.Click += (sender, args) => viewModel.GetWeather(viewModel.CityName);
            GetForecastButton.Click += (sender, args) => viewModel.GetForecast(viewModel.CityName);

        }
        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            Temperature temp = await viewModel.GetWeatherAsync(viewModel.CityName);
            if (temp != null)
            {
                TemperatureLabel.Content = $"{temp.TemperatureValue}°C";
                DescriptionLabel.Content = temp.Description;
            }
        }

        private async void GetForecastButton_Click(object sender, RoutedEventArgs e)
        {
            List<ForecastData> forecasts = await viewModel.GetForecastAsync(viewModel.CityName);
            if (forecasts != null)
            {
                ForecastsListBox.ItemsSource = forecasts;
            }
        }

        private async Task<WeatherData> FetchWeatherDataAsync(string cityName)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid=4d6834e34439b678245f54d3a6efa6be");
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WeatherData>(json);
            }
        }

        private async Task<ForecastData> FetchForecastDataAsync(string cityName)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid=4d6834e34439b678245f54d3a6efa6be");
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ForecastData>(json);
            }
        }
    }
}
