using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherData
    {
        public string CityName { get; set; }
        public double TemperatureC { get; set; }
        public double TemperatureF { get; set; }
        public double WindSpeedMph { get; set; }
    }

}
