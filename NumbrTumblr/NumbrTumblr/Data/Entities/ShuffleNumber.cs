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
    [Table("ShuffleNumber")]
    public class ShuffleNumber
    {
        [PrimaryKey, AutoIncrement]
        public int ShuffleNumberID { get; set; }
        public int Order { get; set; }
        public int Number { get; set; }
        public bool Picked { get; set; }

        [ForeignKey(typeof(int))]     // Specify the foreign key
        public int ShuffleID { get; set; }      
    }
}
