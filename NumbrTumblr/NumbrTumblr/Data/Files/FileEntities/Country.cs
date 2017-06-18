﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbrTumblr.Data.Files.FileEntities.Country
{
    public class Country
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class RootObject
    {
        public List<Country> Countries { get; set; }
    }
}
