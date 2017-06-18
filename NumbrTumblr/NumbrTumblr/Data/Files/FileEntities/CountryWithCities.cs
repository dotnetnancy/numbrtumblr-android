using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbrTumblr.Data.Files.FileEntities
{
    public class CountryWithCities
    {
        public string Country { get; set; }
        //public List<string> Cities { get; set; }
    }

    public class RootObject
    {
        public List<CountryWithCities> CountriesWithCities { get; set; }
    }
}
