using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json.Linq;
using System.Globalization;
namespace WeatherForecast
{
    [Serializable]
    class WeatherRequest
    {
        HttpWebRequest request;
        HttpWebResponse response;
        WeatherRequest weatherRequest;
        [NonSerialized]
        BinaryFormatter formatter;
        [NonSerialized]
        WeatherRespons weatherRespons;
        public WeatherRequest(string apiKey, string city, string typeTemp = "metric", string mode = "", string lang = "en",
            double coordinatsX = 51.5085, double coordinatsY = -0.12574, int idCity = 2643743)
        {
            ApiKey = apiKey;
            City = city;
            TypeTemp = typeTemp;
            Mode = mode;
            Lang = lang;
            CoordinatsX = coordinatsX;
            CoordinatsY = coordinatsY;
            IdCity = idCity;
            weatherRespons = new WeatherRespons();
            formatter = new BinaryFormatter();
        }
        public string ApiKey
        {
            get { return _apiKey; }
            private set
            {
                if (value != null && value != " ")
                {
                    _apiKey = value;
                }
                else
                {
                    _apiKey = "19524b03f1a7d94394e16c461293bc9c";
                }
            }
        }
        private string _apiKey;
        public string City
        {
            get { return _city; }
            private set
            {
                if (value == null)
                {
                    _city = "London";
                }
                else if (value == " ")
                {
                    _city = "London";
                }
                else
                {
                    _city = value;
                }
            }
        }
        private string _city;
        private string TypeTemp
        {
            get { return _typeTemp; }
            set
            {
                if (value != "standard" && value != "metric" && value != "imperial")
                {
                    _typeTemp = "&units=metric";
                }
                else
                {
                    _typeTemp = "&units=" + value;
                }
            }
        }
        private string _typeTemp;
        private string Mode
        {
            get { return _mode; }
            set
            {
                if (value != "xml" && value != "html")
                {
                    _mode = "";
                }
                else
                {
                    _mode = "&mode=" + value;
                }
            }
        }
        private string _mode;
        private string Lang
        {
            get { return _lang; }
            set
            {
                if (value != "en")
                {
                    _lang = "&lang=en";
                }
                else
                {
                    _lang = "&lang=" + value;
                }
            }
        }
        private string _lang;
        private double CoordinatsX
        {
            get { return _coordsX; }
            set
            {
                if (value > 90)
                {
                    _coordsX = 90;
                }
                else if (value < -90)
                {
                    _coordsX = -90;
                }
                else
                {
                    _coordsX = value;
                }
            }
        }
        private double _coordsX;
        private double CoordinatsY
        {
            get { return _coordsY; }
            set
            {
                if (value > 90)
                {
                    _coordsY = 90;
                }
                else if (value < -90)
                {
                    _coordsY = -90;
                }
                else
                {
                    _coordsY = value;
                }
            }
        }
        private double _coordsY;
        private int IdCity
        {
            get { return _idCity; }
            set
            {
                if (value != 0)
                {
                    _idCity = value;
                }
                else
                {
                    _idCity = 2643743;
                }
            }
        }
        private int _idCity;
        public void CreateRequest(string typeRequest)
        {
            if (typeRequest == "name")// по названию
            {
                request = (HttpWebRequest)WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?q={City}{Mode}{TypeTemp}&appid={ApiKey}{Lang}");
            }
            else if (typeRequest == "coords")//по кордам
            {
                request = (HttpWebRequest)WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?lat={CoordinatsX}&lon={CoordinatsY}{Mode}{TypeTemp}&appid={ApiKey}{Lang}");
            }
            else//по id
            {
                request = (HttpWebRequest)WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?id={IdCity}{Mode}{TypeTemp}&appid={ApiKey}{Lang}");
            }
        }
        string tempSystem;
        public void TemperatureType()
        {
            if (TypeTemp.Contains("metric"))
            {
                tempSystem = "C`";
            }
            else if (TypeTemp.Contains("imperial"))
            {
                tempSystem = "K";
            }
            else
            {
                tempSystem = "F";
            }
        }
        public void GettingResponse()
        {
            response = (HttpWebResponse)request.GetResponse();
        }
        string line;
        public void ReadResponse()
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                line = reader.ReadToEnd();
            }
        }
        public void Request(string typeCity)
        {
            CreateRequest(typeCity);
            GettingResponse();
            ReadResponse();
            TemperatureType();
            weatherRespons = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherRespons>(line);
            Console.WriteLine($"Temperature in {weatherRespons.Name}: {weatherRespons.Main.Temp}{tempSystem}, скорость ветра: {weatherRespons.Wind.Speed}м/с," +
                $" направление ветра(градусы): {weatherRespons.Wind.Deg}, порыв ветра: {weatherRespons.Wind.Gust}м/с, облачность: {weatherRespons.Clouds.All}%," +
                $" влажность: {weatherRespons.Main.Humidity}%, восход: {weatherRespons.Sys.Sunrise}с, закат: {weatherRespons.Sys.Sunset}с," +
                $" давление: {weatherRespons.Main.Pressure}гПА, мин температура: {weatherRespons.Main.Temp_min}{tempSystem}," +
                $" макс температура: {weatherRespons.Main.Temp_max}{tempSystem}");
            // дождь: {weatherRespons.Rain}мм, снег: {weatherRespons.Snow}мм, дождь: {weatherRespons.Rain.H1}мм
            FeelingTemp();
        }
        public void WriteInfo()
        {
            Console.WriteLine($"ApiKey-{ApiKey}");
            Console.WriteLine($"City-{City}");
            Console.WriteLine($"Тип температуры-{TypeTemp}");
            Console.WriteLine($"Вид возвращаемого ответа-{Mode}");
            Console.WriteLine($"Язык-{Lang}");
            Console.WriteLine($"Координаты по широте-{CoordinatsX}");
            Console.WriteLine($"Координаты по долготе-{CoordinatsY}");
            Console.WriteLine($"Id города-{IdCity}");
        }
        public void WriteShortInfo()
        {
            Console.WriteLine($"ApiKey-{ApiKey}");
            Console.WriteLine($"City-{City}");
        }
        public void DataSerialization()
        {
            using (FileStream fs = new FileStream("weatheRequest.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, weatherRequest);
                Console.WriteLine("Объект сериализован");
            }
        }
        public void DataDeserialization()
        {
            using (FileStream fs = new FileStream("assistt.dat", FileMode.OpenOrCreate))
            {
                WeatherRequest newClass = (WeatherRequest)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован");
            }
        }
        public void FeelingTemp()
        {
            if (weatherRespons.Main.Temp >= 15 && weatherRespons.Main.Temp < 30 && TypeTemp == "metric")
            {
                Console.WriteLine("Сейчас тепло");
            }
            else if (weatherRespons.Main.Temp >= 30 && TypeTemp == "metric")
            {
                Console.WriteLine("Сейчас жарко");
            }
            else if (weatherRespons.Main.Temp < 15 && weatherRespons.Main.Temp >= 0 && TypeTemp == "metric")
            {
                Console.WriteLine("Сейчас прохладно");
            }
            else
            {
                Console.WriteLine("Сейчас холодно");
            }
        }
        public void WriteCity(string city)
        {
            City = city;
        }
        public void WriteApiKey(string apiKey)
        {
            ApiKey = apiKey;
        }
        public void WriteTypeTemp(string typeTemp)
        {
            TypeTemp = typeTemp;
        }
        public void WriteMode(string mode)
        {
            Mode = mode;
        }
        public void WriteLang(string lang)
        {
            Lang = lang;
        }
        public void WriteCoords(double coordx, double coordy)
        {
            CoordinatsX = coordx;
            CoordinatsY = coordy;
        }
        public void WriteId(int idCity)
        {
            IdCity = idCity;
        }
    }
}