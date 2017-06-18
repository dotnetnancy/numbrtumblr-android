using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NumbrTumblr.Business;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Models;

namespace NumbrTumblr.ViewModel
{
    public class ShuffleNumbersViewModel : ViewModelBase
    {
        private RangeObservableCollection<ShuffleNumber> _shuffleNumbers = new RangeObservableCollection<ShuffleNumber>();
        private Shuffle _shuffle;
        private NumberSet _numberSet;

        public Shuffle Shuffle
        {
            get { return _shuffle; }
            set { _shuffle = value; }
        }
        public NumberSet NumberSet
        {
            get { return _numberSet; }
            set { _numberSet = value; }
        }

        public RangeObservableCollection<ShuffleNumber> ShuffleNumbers
        {
            get
            {
                return _shuffleNumbers;
            }

            set
            {
                _shuffleNumbers = value;
            }
        }


        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();

        public ShuffleNumbersViewModel()
        {
            List<ShuffleNumber> data = new List<ShuffleNumber>();
            if (_shuffle != null)
            {
                _numbrTumblrBusiness.GetShuffleNumbersByShuffleId(_shuffle.ShuffleID);
            }
            _shuffleNumbers.Clear();
            _shuffleNumbers.AddRange(data);

        }

        internal void LoadNumberSetNumbersData(NumberSet numberSet, bool shuffle)
        {
            NumberSet = numberSet;
            _shuffleNumbers.Clear();
            _shuffle = new Shuffle();

            List<ShuffleNumber> shuffleNumbers = new List<ShuffleNumber>();
            foreach (NumberSetNumber numberSetNumber in numberSet.NumberSetNumbers)
            {
                ShuffleNumber shuffleNumber = new ShuffleNumber
                {
                    Number = numberSetNumber.Number,
                    Order = -1
                };
                shuffleNumbers.Add(shuffleNumber);
            }

           if(shuffle)
            {
                IList<int> numbersToShuffle = shuffleNumbers.Select(x => x.Number).ToList();
                numbersToShuffle.FisherYatesShuffle(new Random());
                
                foreach(ShuffleNumber shuffleNumber in shuffleNumbers)
                {
                    shuffleNumber.Order = numbersToShuffle.IndexOf(shuffleNumber.Number);
                }
                
            }
            _shuffleNumbers.AddRange(shuffleNumbers.OrderBy(x=> x.Order));
        }

        public void LoadShuffleNumbersData(Shuffle shuffle,NumberSet numberSet)
        {
            Shuffle = shuffle;
            Shuffle.NumberSet = numberSet;            
           
            var shuffleNumbers = _numbrTumblrBusiness.GetShuffleNumbersByShuffleId(shuffle.ShuffleID);
            

            _shuffleNumbers.Clear();
            _shuffleNumbers.AddRange(shuffleNumbers.ToList());
            shuffle.ResultOfShuffle = _shuffleNumbers.ToList();

        }


        //public void SetShuffle(Shuffle shuffle)
        //{
        //    _shuffle = shuffle;
        //    SetShuffleNumbers(_shuffle);
        //}

        //private void SetShuffleNumbers(Shuffle shuffle)
        //{
        //    _shuffle = shuffle;

        //    List<ShuffleNumber> data = new List<ShuffleNumber>();

        //    if (_shuffle != null)
        //    {
        //        data = _numbrTumblrBusiness.GetShuffleNumbersByShuffleId(_shuffle.ShuffleID);
        //    }

        //    _shuffleNumbers.Clear();
        //    _shuffleNumbers.AddRange(data);
        //}     


        public void RefreshData(Shuffle shuffle,NumberSet numberSet)
        {
            LoadShuffleNumbersData(shuffle,numberSet);
        }

        public void AddNumberToShuffle(int number)
        {
            var foundNumber = _shuffleNumbers.FirstOrDefault(x => x.ShuffleID == Shuffle.ShuffleID
                                                                   && x.Number == number);
            if (foundNumber != null)
            {
                _numbrTumblrBusiness.AddShuffleNumber(foundNumber);
                //foundNumber.SelectedNumber = true;
            }

        }

        public void RemoveNumberFromShuffle(int number)
        {
            var foundNumber = _shuffleNumbers.FirstOrDefault(x => x.ShuffleID == Shuffle.ShuffleID
                                                                  && x.Number == number);
            if (foundNumber != null)
            {
                _numbrTumblrBusiness.RemoveShuffleNumber(foundNumber);
                //foundNumber.SelectedNumber = false;
            }
        }

        internal void AddShuffleNumberToShufflePick(int number)
        {
            var foundNumber = ShuffleNumbers.FirstOrDefault(x=> x.Number == number);
            if (foundNumber != null)
            {               
                foundNumber.Picked = true;
            }
        }

        internal void RemoveShuffleNumberFromShufflePick(int number)
        {
            var foundNumber = ShuffleNumbers.FirstOrDefault(x=> x.Number == number);
            if (foundNumber != null)
            {               
                foundNumber.Picked = false;
            }
        }
    }
}
