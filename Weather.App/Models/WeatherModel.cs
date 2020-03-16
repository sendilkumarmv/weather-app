using System;
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
    

}
