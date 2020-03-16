using Weather.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.App.Service
{
    public interface IWeatherService
    {
        Task<List<City>> GetCities(string filter);
    }
}
