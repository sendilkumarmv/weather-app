using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Weather.App.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Weather.App.Service
{
    public class WeatherService : IWeatherService
    {
        private IMemoryCache _cache;
        private readonly string apiKey = "34327264272d90968fb6aa0d98d1f21d";
        private string baseUrl = $"http://api.openweathermap.org/data/2.5/weather?";

        public WeatherService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<List<City>> GetCities(string filter)
        {
            List<City> filteredResult = new List<City>();
            try
            {
                List<City> fullList = null;
                if (!_cache.TryGetValue("Full", out fullList))
                {
                    if (fullList == null)
                    {
                        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "city-list.json", SearchOption.AllDirectories);
                        string filePath = files[0];
                        fullList = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));
                        _cache.Set("Full", fullList);

                    }
                }

                List<Country> fullCountryList = null;
                if (!_cache.TryGetValue("FullCountry", out fullCountryList))
                {
                    if (fullCountryList == null)
                    {
                        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "countries.json", SearchOption.AllDirectories);
                        string filePath = files[0];
                        fullCountryList = JsonConvert.DeserializeObject<List<Country>>(File.ReadAllText(filePath));
                        _cache.Set("FullCountry", fullCountryList);

                    }
                }

                if (fullList != null)
                    filteredResult = fullList.FindAll((city) => city.Name.ToLower().StartsWith(filter.ToLower()));

                var finalResult = from ci in filteredResult
                                  join co in fullCountryList on ci.Country equals co.Code
                                  select new City
                                  {
                                      Id = ci.Id,
                                      Coord = ci.Coord,
                                      Name = ci.Name,
                                      CountryName = co.Name,
                                      Country = ci.Country
                                  };

                return await Task.FromResult(finalResult.ToList());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<WeatherData> GetWeatherByGeoLocation(string lon, string lat)
        {
            string geoLocUrl = $"lat={lat}&lon={lon}&appid={apiKey}";
            return await GetWeather(geoLocUrl);
        }

        public async Task<WeatherData> GetWeatherByCityName(string cityName)
        {
            string cityUrl = $"q={cityName}&appid={apiKey}";
            return await GetWeather(cityUrl);
        }

        public async Task<WeatherData> GetWeatherByCityId(string cityId)
        {
            string cityUrl = $"id={cityId}&appid={apiKey}";
            return await GetWeather(cityUrl);
        }

        public async Task<WeatherData> GetWeatherByZip(string zipCode,string countryCode)
        {
            string cityUrl = $"zip={zipCode},{countryCode}&appid={apiKey}";
            return await GetWeather(cityUrl);
        }


        private async Task<WeatherData> GetWeather(string url)
        {
            HttpResponseMessage apiResponse = null;
            using (HttpClient client = new HttpClient())
            {
                apiResponse = await client.GetAsync($"{baseUrl}{url}");
            }
            if (apiResponse.IsSuccessStatusCode)
            {
                var resultJson = await apiResponse.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<WeatherData>(resultJson);
                
                return ConvertAndAddUnits(resultObject);
            }
            return null;
        }

        private WeatherData ConvertAndAddUnits(WeatherData weatherObject)
        {
            weatherObject.sys.sunrise = ConvertToDate(Convert.ToDouble(weatherObject.sys.sunrise), true);
            weatherObject.sys.sunset = ConvertToDate(Convert.ToDouble(weatherObject.sys.sunset), true);
            weatherObject.dt = ConvertToDate(Convert.ToDouble(weatherObject.dt));
            weatherObject.main.temp = ConvertTemprature(weatherObject.main.temp);
            weatherObject.main.feels_like = ConvertTemprature(weatherObject.main.feels_like);
            weatherObject.main.temp_max = ConvertTemprature(weatherObject.main.temp_max);
            weatherObject.main.temp_min = ConvertTemprature(weatherObject.main.temp_min);
            weatherObject.weather[0].icon = GetIconUrl(weatherObject.weather[0].icon);
            return weatherObject;
        }
        private string GetIconUrl(string icon)
        {
            return $"http://openweathermap.org/img/wn/{icon}@2x.png";
        }
        private string ConvertTemprature(string temprature)
        {
            var resultTemp = Convert.ToDecimal(temprature) - 273;
            return $"{resultTemp} &deg; C";
        }
        private string ConvertToDate(double timeStamp, bool onlyTime = false)
        {
            var timeSpan = TimeSpan.FromSeconds(timeStamp);
            var localDateTime = new DateTime(timeSpan.Ticks).ToLocalTime();
            return onlyTime ? localDateTime.ToShortTimeString() : localDateTime.ToString();
        }
    }

   
}
