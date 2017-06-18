using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NumbrTumblr.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace NumbrTumblr.Data.Entities
{
    [Table("Lottery")]
    public class Lottery : ViewModelBase
    {
        [PrimaryKey,AutoIncrement]
        public int LotteryID { get; set; }
        public string LotteryName { get; set; }
        public string LotteryDescription { get; set; }
        public int? LotteryNumberMax { get; set; }
        public int? LotteryNumberMin { get; set; }
        [ForeignKey(typeof(int))]     // Specify the foreign key
        public int StateProvinceID { get; set; }

        [Ignore]
        public StateProvince StateProvince { get; set; }
        
    }
}
