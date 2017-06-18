using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbrTumblr.HelpersAndExtensionMethods
{
    public static class ShuffleExtensions
    {
        /*usage of FisherYatesShuffle: 
                     Random rg = new Random();
                     List<int> list = Enumerable.Range(1, 4).ToList();
                     list.Shuffle(rg);
              */
        public static void FisherYatesShuffle<T>(this IList<T> list, Random rg)
        {
            for (int i = list.Count; i > 1; i--)
            {
                int k = rg.Next(i);
                T temp = list[k];
                list[k] = list[i - 1];
                list[i - 1] = temp;
            }
        }

        //we assume the list is shuffled already - so just keep the order which they are passed in and picked
        public static List<int> GenerateRandomDontShuffle(List<int> candidates, int count)
        {
            Random random = new Random(candidates.Min());
            // generate count random values.
            HashSet<int> randomNumsList = new HashSet<int>();
            while (randomNumsList.Count < count)
            {
                //make sure it is not already in list and make sure it is a candidate!
                var nextRand = random.Next();
                if ((!randomNumsList.Contains(nextRand)) && (candidates.Contains(nextRand)))
                {
                    randomNumsList.Add(random.Next());
                }
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(randomNumsList);

            return result;
        }

        public static List<int> GenerateRandomDoShuffle(List<int> candidates, int count)
        {
            Random random = new Random(candidates.Min());
            // generate count random values.
            HashSet<int> randomNumsList = new HashSet<int>();
            while (randomNumsList.Count < count)
            {
                //make sure it is not already in list and make sure it is a candidate!
                var nextRand = random.Next();
                if ((!randomNumsList.Contains(nextRand)) && (candidates.Contains(nextRand)))
                {
                    randomNumsList.Add(random.Next());
                }
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(randomNumsList);

            // shuffle the results:
            result.FisherYatesShuffle<int>(random);
            return result;
        }
    }
}
