﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ActionAsyncFilter("Controller")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        [ActionAsyncFilter("GET")]
        public async Task<string> Get(string cityName)
        {
            return await _weatherService.Get(cityName);
        }
    }
}