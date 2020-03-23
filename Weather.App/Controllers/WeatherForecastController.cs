using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Weather.App.Models;
using Weather.App.Service;

/// <summary>
/// http://weatherapi.ap-south-1.elasticbeanstalk.com/
/// https://flagpedia.net/download
/// </summary>

namespace Weather.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IWeatherService weatherService, 
            IMemoryCache cache,ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [Route("getcities/{filter}")]
        public async Task<IActionResult> GetCities(string filter)
        {
            var result = await _weatherService.GetCities(filter);
            if (result.Count > 0)
                return Ok(result);
            else
                return NotFound(filter);
        }

        [HttpGet]
        [Route("getweather/loc/{lon}/{lat}")]
        public async Task<IActionResult> GetWeatherByCoord(string lon, string lat)
        {
            return Ok(await _weatherService.GetWeatherByGeoLocation(lon, lat));
        }

        [HttpGet]
        [Route("getweather/cityname/{cityName}")]
        public async Task<IActionResult> GetWeatherByCityName(string cityName)
        {
            return Ok(await _weatherService.GetWeatherByCityName(cityName));
        }


        [HttpGet]
        [Route("getweather/cityid/{cityId}")]
        public async Task<IActionResult> GetWeatherByCityId(string cityId)
        {
            return Ok(await _weatherService.GetWeatherByCityId(cityId));
        }

        [HttpGet]
        [Route("getweather/zip/{zipCode}/{countryCode}")]
        public async Task<IActionResult> GetWeatherByZipCode(string zipCode, string countryCode)
        {
            return Ok(await _weatherService.GetWeatherByZip(zipCode, countryCode));
        }

    }
}
