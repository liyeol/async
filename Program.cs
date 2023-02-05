using System;
using System.Net.Http;
using System.Text.Json;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Створюємо HttpClient object
            var client = new HttpClient();

            var response = await client.GetAsync("https://api.example.com/data");

           
            if (response.IsSuccessStatusCode)
            {
               
                var responseContent = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<Data>(responseContent);

                Console.WriteLine($"Name: {data.Name}");
                Console.WriteLine($"Age: {data.Age}");
                Console.WriteLine($"Location: {data.Location}");
            }
            else
            {
                Console.WriteLine("Request failed. Status code: {0}", response.StatusCode);
            }


            Console.WriteLine("Enter a city:");
            var city = Console.ReadLine();

            var weather = await GetWeather(city);
            Console.WriteLine($"Temperature in {city}: {weather.main.temp}°F");
        }

        static async Task<Weather> GetWeather(string city)
        {
            var location = await GetLocation(city);

            var client = new HttpClient();
            var response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?lat={location.lat}&lon={location.lon}&appid=<your_api_key>&units=imperial");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<Weather>(responseContent);
                return weather;
            }
            else
            {
                Console.WriteLine("Request failed. Status code: {0}", response.StatusCode);
                return null;
            }
        }

        static async Task<Location> GetLocation(string city)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"https://api.opencagedata.com/geocode/v1/json?q={city}&key=<your_api_key>");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<OpenGeoCodeResponse>(responseContent);
                var location = data.results[0].geometry;
                return location;
            }
            else
            {
                Console.WriteLine("Request failed. Status code: {0}", response.StatusCode);
                return null;
            }
        }

    }

    class Data
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
    }

    
    class Weather
    {
        public Main main { get; set; }
    }

    class Main
    {
        public float temp { get; set; }
    }

    class Location
    {
        public float lat { get; set; }
        public float lon { get; set; }
    }

    class OpenGeoCodeResponse
    {
        public Location[] results { get; set; }
    }



}


