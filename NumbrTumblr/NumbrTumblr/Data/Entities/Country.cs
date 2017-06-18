using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace NumbrTumblr.Data.Entities
{
    [Table("Country")]
    public class Country
    {
        [PrimaryKey, AutoIncrement]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}
