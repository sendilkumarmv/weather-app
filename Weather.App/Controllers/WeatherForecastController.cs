using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        [Route("{getcities}/{filter}")]
        public async Task<IActionResult> GetCities(string filter)
        {
            var result = await _weatherService.GetCities(filter);
            if (result.Count > 0)
                return Ok(result);
            else
                return NotFound(filter);
        }
       
    }
}
