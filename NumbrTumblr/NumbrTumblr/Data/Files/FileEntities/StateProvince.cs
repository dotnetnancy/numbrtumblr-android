using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbrTumblr.Data.Files.FileEntities.City
{
    public class City
    {
        public double lng { get; set; }
        public int geonameId { get; set; }
        public string countrycode { get; set; }
        public string name { get; set; }
        public string fclName { get; set; }
        public string toponymName { get; set; }
        public string fcodeName { get; set; }
        public string wikipedia { get; set; }
        public double lat { get; set; }
        public string fcl { get; set; }
        public int population { get; set; }
        public string fcode { get; set; }
    }

    public class RootObject
    {
        public List<City> Cities { get; set; }
    }
}
