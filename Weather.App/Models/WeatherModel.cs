﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.App.Models
{
    public class Coordinates
    {
        public string Lon { get; set; }
        public string Lat { get; set; }
    }

    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public string CountryName { get; set; }
        public Coordinates Coord { get; set; }
    }

    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class Main
    {
        public string temp { get; set; }
        public string feels_like { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }
        public string pressure { get; set; }
        public string humidity { get; set; }

    }
    public class Wind
    {
        public string Speed { get; set; }
    }
    public class Clouds
    {
        public string All { get; set; }
    }

    public class Sys
    {
        public string type { get; set; }
        public string id { get; set; }
        public string country { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
       
    }

    public class Weather
    {
        public string id { get; set; }
        public string main { set; get; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class WeatherData
    {
        public Coordinates Coord { get; set; }
        public Weather[] weather { get; set; }
        public string Base { get; set; }
        public Main main { get; set; }
        public string visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public string dt { get; set; }
        public Sys sys { get; set; }
        public string timezone { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string cod { get; set; }
    }
}
