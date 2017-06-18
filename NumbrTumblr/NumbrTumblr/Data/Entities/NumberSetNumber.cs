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
    [Table("NumberSetNumber")]
    public class NumberSetNumber
    {
        [PrimaryKey, AutoIncrement]
        public int NumberSetNumberID { get; set; } 
        public int Number { get; set; }
        [ForeignKey(typeof(int))]     // Specify the foreign key
        public int NumberSetID { get; set; }
        [Ignore]
        public NumberSet NumberSet { get; set; }
        [Ignore]
        public bool SelectedNumber { get; set; }

    }
}
