using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace NumbrTumblr.Data.Entities
{
    [Table("StateProvince")]
    public class StateProvince
    {
        [PrimaryKey, AutoIncrement]
        public int StateProvinceID { get; set; }

        public string StateProvinceName { get; set; }
        public string StateProvinceCode { get; set; }

        [ForeignKey(typeof(int))] // Specify the foreign key
        public string CountryID { get; set; }

        [Ignore]
        public Country Country { get; set; }

}
}
