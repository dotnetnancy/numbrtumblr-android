using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.Business;

namespace NumbrTumblr.ViewModel
{
    public class ShufflePickViewModel : ViewModelBase
    {
        private RangeObservableCollection<Shuffle> _shuffles = new RangeObservableCollection<Shuffle>();
        private NumberSet _numberSet;
        private string _shuffleNumbersName;
        private string _shuffleNumbersShuffleNumbers;
        private string _shuffleNumbersPickedShuffleNumbers;

        public NumberSet NumberSet
        {
            get
            {
                return _numberSet;
            }
            set
            {
                _numberSet = value;
                RaisePropertyChanged(() => NumberSet);
            }
        }

         public string ShuffleNameLabel
        {
            get
            {
                SetShuffleName();
                return _shuffleNumbersName;
            }
            set
            {
                SetShuffleName();
                 RaisePropertyChanged(() => ShuffleNameLabel);
            }
        }

         public string ShuffleAllNumbersNameLabel
        {
            get
            {
                SetShuffleNumbers();
                return _shuffleNumbersShuffleNumbers;
            }
            set
            {
                SetShuffleNumbers();
                 RaisePropertyChanged(() => ShuffleAllNumbersNameLabel);
            }
        }

         public string ShufflePickNameLabel
        {
            get
            {
                SetShufflePicks();
                return _shuffleNumbersPickedShuffleNumbers;
            }
            set
            {
                SetShufflePicks();
                 RaisePropertyChanged(() => ShufflePickNameLabel);
            }
        }


        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();


        public ShufflePickViewModel()
        {
        }

        public RangeObservableCollection<Shuffle> Shuffles
        {
            get { return _shuffles; }
            set
            {
                _shuffles.AddRange(value);
            }
        }

        public void RefreshData()
        {
            List<Shuffle> data = _numbrTumblrBusiness.GetShufflesByNumberSet(NumberSet);

            if (data != null)
            {
                _shuffles.Clear();
                _shuffles.AddRange(data);
                ShuffleNameLabel = string.Empty;
                ShufflePickNameLabel = string.Empty;
                ShuffleAllNumbersNameLabel = string.Empty;
            }
        }

        private void SetShuffleName()
        {
            if (_shuffles == null || _shuffles.Count == 0)
            {
                _shuffleNumbersName = "No Shuffles Saved.";
            }
            else
            {
                _shuffleNumbersName = "Shuffle Date Time";
            }
        }

        private void SetShuffleNumbers()
        {
            if (_shuffles == null || _shuffles.Count == 0)
            {
                _shuffleNumbersShuffleNumbers = string.Empty;
            }
            else
            {               
                _shuffleNumbersShuffleNumbers = "All Numbers in Shuffle.";
            }
        }

        private void SetShufflePicks()
        {
            if (_shuffles == null || _shuffles.Count == 0)
            {
                _shuffleNumbersPickedShuffleNumbers = string.Empty;
            }
            else
            {
                _shuffleNumbersPickedShuffleNumbers = "Numbers Picked in Shuffle.";               
            }
        }
    }

}
