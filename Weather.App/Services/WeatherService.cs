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

namespace Weather.App.Service
{
    public class WeatherService : IWeatherService
    {
        private IMemoryCache _cache;
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
    }
}
