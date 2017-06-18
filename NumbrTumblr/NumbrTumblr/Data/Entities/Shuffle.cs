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
    [Table("Shuffle")]
    public class Shuffle
    {
         [PrimaryKey, AutoIncrement]
        public int ShuffleID { get; set; }
        public DateTime ShuffleDateTime { get; set; }
        [ForeignKey(typeof(int))]     // Specify the foreign key
        public int NumberSetID { get; set; }     
        [Ignore]
        public NumberSet NumberSet { get; set; }
        [Ignore]
        public List<ShuffleNumber> ResultOfShuffle { get; set; }
        [Ignore]
        public string ResultOfShuffleConcatenated
        {
            get
            {
                if(ResultOfShuffle != null && ResultOfShuffle.Count > 0)
                {
                    return string.Join(",", ResultOfShuffle.Select(x => x.Number.ToString()));
                }
                return string.Empty;
            }
        }

         [Ignore]
        public string ResultOfShufflePicksConcatenated
        {
            get
            {
                if(ResultOfShuffle != null && ResultOfShuffle.Count > 0)
                {
                    return string.Join(",", ResultOfShuffle.Where(y=> y.Picked).Select(x => x.Number.ToString()));
                }
                return string.Empty;
            }
        }
    }
}
