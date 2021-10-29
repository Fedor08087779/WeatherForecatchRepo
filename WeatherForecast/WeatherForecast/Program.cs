using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using System.Globalization;


namespace WeatherForecast
{
    [Serializable]
    class Program
    {

        static void Main(string[] args)
        {
            WeatherRequest weatherRequest = new WeatherRequest(null, null, null, null, null, 0, 0, 0);
            bool cycle = true;
            string TypeRequest = "";
            while (cycle)
            {
                Console.Write("Введите номер(1-ввести параметры, 2-вывести полную инфу, 3-вывести короткую инфу, 4-сериализация, 5-десериализация, exit-выход): ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        {
                            Console.Write("Введите название параметра(city,api,temp,mode,cords,id,exit): ");
                            input = Console.ReadLine();
                            OptionsWriting(input);
                            break;
                        }
                    case "2":
                        {
                            weatherRequest.WriteInfo();
                            break;
                        }
                    case "3":
                        {
                            weatherRequest.WriteShortInfo();
                            break;
                        }
                    case "4":
                        {
                            weatherRequest.DataSerialization();
                            break;
                        }
                    case "5":
                        {
                            weatherRequest.DataDeserialization();
                            break;
                        }
                    default:
                        {
                            cycle = false;
                            break;
                        }
                }
            }
            void OptionsWriting(string input)
            {
                switch (input)
                {
                    case "city":
                        {
                            Console.Write("Введите Город:");
                            string city = Console.ReadLine();
                            TypeRequest = "name";
                            weatherRequest.WriteCity(city);
                            break;
                        }
                    case "api":
                        {
                            Console.Write("Введите ApiKey:");
                            string apiKey = Console.ReadLine();
                            weatherRequest.WriteApiKey(apiKey);
                            break;
                        }
                    case "temp":
                        {
                            Console.Write("Введите Вид температуры:");
                            string typeTemp = Console.ReadLine();
                            weatherRequest.WriteTypeTemp(typeTemp);
                            break;
                        }
                    case "mode":
                        {
                            Console.Write("Введите вид возвращаемого ответа:");
                            string mode = Console.ReadLine();
                            weatherRequest.WriteMode(mode);
                            break;
                        }
                    case "cords":
                        {
                            Console.Write("Введите Язык:");
                            string lang = Console.ReadLine();
                            weatherRequest.WriteLang(lang);
                            break;
                        }
                    case "lat":
                        {
                            Console.Write("Введите Координаты по широте:");
                            double.TryParse(Console.ReadLine(), out double x);
                            Console.Write("Введите Координаты по долготе:");
                            double.TryParse(Console.ReadLine(), out double y);
                            TypeRequest = "coords";
                            weatherRequest.WriteCoords(x, y);
                            break;

                        }
                    case "id":
                        {
                            Console.Write("Введите Id города:");
                            int.TryParse(Console.ReadLine(), out int id);
                            weatherRequest.WriteId(id);
                            TypeRequest = "id";
                            break;
                        }
                    default:
                        {
                            cycle = false;
                            break;
                        }
                }               
            }
            weatherRequest.Request(TypeRequest);
        }
    }
}