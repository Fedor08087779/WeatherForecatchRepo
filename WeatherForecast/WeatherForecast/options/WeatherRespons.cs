using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WeatherForecast
{
    class WeatherRespons
    {
        
        public Main Main { get; set; }
        public string Name { get; set; }
        public Clouds Clouds { get; set;}
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
    }
}
