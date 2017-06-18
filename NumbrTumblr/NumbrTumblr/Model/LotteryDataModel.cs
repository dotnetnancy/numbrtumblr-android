using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Data.Entities;

namespace NumbrTumblr.Models
{
    public class LotteryDataModel
    {
        public List<Lottery> MyLotteries { get; set; } 

        public List<NumberSet> MyNumberSets { get; set; }

        public NumberSet MyNumberPool { get; set; } 
    }
}
