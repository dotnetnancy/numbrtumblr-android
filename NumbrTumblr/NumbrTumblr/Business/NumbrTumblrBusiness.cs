using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NumbrTumblr.Data;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.Data.Files;
using NumbrTumblr.Data.Files.FileEntities.City;
using NumbrTumblr.Models;
using NumbrTumblr.ViewModel;

namespace NumbrTumblr.Business
{
    
    public class NumbrTumblrBusiness
    {
        SQLiteLotteryDatabase _lotteryDatabase = new SQLiteLotteryDatabase();

        public LotteryDataModel GetLotteryData()
        {
            LotteryDataModel lotteryData = new LotteryDataModel();

           lotteryData.MyLotteries = _lotteryDatabase.GetLottery().ToList();
            lotteryData.MyNumberSets = _lotteryDatabase.GetNumberSet().ToList();
            lotteryData.MyNumberPool = _lotteryDatabase.GetNumberPool();
            return lotteryData;

        }

        public List<Lottery> GetLotteries()
        {
            return _lotteryDatabase.GetLottery().ToList();
        }

        internal List<Shuffle> GetShufflesByNumberSet(NumberSet numberSet)
        {
            return _lotteryDatabase.GetShufflesByNumberSet(numberSet.NumberSetID);
        }

        public List<NumberSet> GetNumberSets()
        {
            return _lotteryDatabase.GetNumberSet().ToList();
        }

        public List<NumberSetNumber> GetNumberSetNumbersByNumberSetId(int numberSetId)
        {
            return _lotteryDatabase.GetNumberSetNumbersByNumberSetId(numberSetId);
        }

        internal Shuffle GetShuffleByShuffleAndLoadNumbersByShuffleId(int shuffleID)
        {
            return _lotteryDatabase.GetShuffle(shuffleID);
        }

        public List<ShuffleNumber> GetShuffleNumbersByShuffleId(int shuffleId)
        {
            return _lotteryDatabase.GetShuffleNumbersByShuffleId(shuffleId);
        }

       
         public async Task<Lottery> SaveLottery(Lottery lottery)
        {
            return _lotteryDatabase.AddOrUpdateLottery(lottery);
        }

        public bool IsLotteryDuplicateName(string newTextValue)
        {
            bool isDuplicate = false;
            var lottery = _lotteryDatabase.GetLotteryByName(newTextValue);
            if (lottery != null)
            {
                isDuplicate = true;
            }
            return isDuplicate;
        }

        internal bool IsLotteryDuplicateNameTakeIntoAccountLotteryId(string newTextValue, int lotteryID)
        {
            bool isDuplicate = false;
            var lottery = _lotteryDatabase.GetLottery(lotteryID);
            if (lottery != null)
            {
                var lotteryByName = _lotteryDatabase.GetLotteryByName(newTextValue);
                if (lotteryByName != null)
                {
                    if (lotteryByName.LotteryID != lottery.LotteryID)
                    {
                        isDuplicate = true;
                    }
                }

            }
            return isDuplicate;
        }
       
        internal bool IsNumberSetDuplicateNameTakeIntoAccountNumberSetId(string newTextValue, int numberSetID)
        {
            bool isDuplicate = false;
            var numberSet = _lotteryDatabase.GetNumberSet(numberSetID);
            if (numberSet != null)
            {
                var numberSetByName = _lotteryDatabase.GetNumberSetByName(newTextValue);
                if(numberSetByName != null)
                {
                    if (numberSetByName.NumberSetID != numberSet.NumberSetID)
                    {
                        isDuplicate = true;
                    }
                }
                
            }
            return isDuplicate;
        }       

        public bool IsNumberSetDuplicateName(string newTextValue)
        {
            bool isDuplicate = false;
            var numberSet = _lotteryDatabase.GetNumberSetByName(newTextValue);
            if (numberSet != null)
            {
                isDuplicate = true;
            }
            return isDuplicate;
        }

        public Task<NumberSet> SaveNumberSet(NumberSetViewModel numberSetViewModel)
        {
            return _lotteryDatabase.AddOrUpdateNumberSet(numberSetViewModel);
        }

         public Shuffle SaveShuffle(Shuffle shuffle)
        {
            return _lotteryDatabase.SaveShuffle(shuffle);
        }


        public bool IsNumberSetNumber(int numberSetId, int number)
        {
            bool isSelected = false;
            var numberSet = _lotteryDatabase.GetNumberSetNumberByNumberSetIdAndNumberSetNumber(numberSetId,number);
            if (numberSet != null)
            {
                isSelected = true;
            }
            return isSelected;
        }

        public void RemoveNumberSetNumber(NumberSetNumber numberSetNumber)
        {
            _lotteryDatabase.DeleteNumberSetNumber(numberSetNumber);
        }

         internal void RemoveShuffleNumber(ShuffleNumber shuffleNumber)
        {
            _lotteryDatabase.DeleteShuffleNumber(shuffleNumber.ShuffleNumberID);
        }

        public void AddNumberSetNumber(NumberSetNumber numberSetNumber)
        {
            _lotteryDatabase.AddNumberSetNumber(numberSetNumber);
        }

        internal void AddShuffleNumber(ShuffleNumber shuffleNumber)
        {
            _lotteryDatabase.AddShuffleNumber(shuffleNumber);
        }

        public NumberSetNumber GetNumberNumberByNumberSetIdAndNumber(int numberSetId, int number)
        {
            return _lotteryDatabase.GetNumberSetNumberByNumberSetIdAndNumberSetNumber(numberSetId, number);
        }

        public List<StateProvince> GetStateProvinces()
        {
            return _lotteryDatabase.GetStateProvinces();
        }

        public List<City> GetCitiesFromJsonFile()
        {
            #region How to load an Json file embedded resource
            var assembly = typeof(LoadResourceText).GetTypeInfo().Assembly;
            //to get the resourceid it is the namespace prepending the filename
            Stream stream = assembly.GetManifestResourceStream("NumbrTumblr.Data.Files.cities.json");

            //Country[] countries;
            List< NumbrTumblr.Data.Files.FileEntities.City.City> cities;
            //Dictionary<string, string> countries = new Dictionary<string, string>();

            using (var reader = new System.IO.StreamReader(stream))
            {

                var json = reader.ReadToEnd();
                var rootobject = JsonConvert.DeserializeObject<NumbrTumblr.Data.Files.FileEntities.City.RootObject>(json);
                //countries = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                //cities = rootobject.Cities.Where(x=> x.countrycode == );
                cities = rootobject.Cities;
            }
            #endregion

            return cities;

            //var listView = new ListView();
            //listView.ItemsSource = countries.Select(x => x.name);
            ////listView.ItemsSource = countries;

            //Content = new StackLayout
            //{
            //    Padding = new Thickness(0, 20, 0, 0),
            //    VerticalOptions = LayoutOptions.StartAndExpand,
            //    Children = {
            //        new Label { Text = "Embedded Resource JSON File (PCL)",
            //            FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
            //            FontAttributes = FontAttributes.Bold
            //        }, listView
            //    }
            //};

            // NOTE: use for debugging, not in released app code!
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);
        }

        internal int DeleteLottery(Lottery lottery)
        {
            return _lotteryDatabase.DeleteLottery(lottery.LotteryID);
        }

        internal int DeleteNumberSet(NumberSet numberSet)
        {
            return _lotteryDatabase.DeleteNumberSet(numberSet);
        }

        internal int DeleteShuffle(Shuffle shuffle)
        {
            return _lotteryDatabase.DeleteShuffle(shuffle);
        }
    }
}
