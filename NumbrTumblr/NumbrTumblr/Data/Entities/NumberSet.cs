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
    [Table("NumberSet")]
    public class NumberSet
    {
        [PrimaryKey, AutoIncrement]
        public int NumberSetID { get; set; }
        public string NumberSetName { get; set; }
        public string NumberSetDescription { get; set; }

        public int? NumberSetNumberMax { get; set; }
        public int? NumberSetNumberMin { get; set; }
        [Ignore]
        public List<NumberSetNumber> NumberSetNumbers { get; set; }
        public bool IsApplicationNumberPool { get; set; }
    }
}
