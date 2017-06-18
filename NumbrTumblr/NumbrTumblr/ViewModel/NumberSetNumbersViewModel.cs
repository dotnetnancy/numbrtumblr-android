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
    public class NumberSetNumbersViewModel : ViewModelBase
    {
        private RangeObservableCollection<NumberSetNumber> _numberSetNumbers = new RangeObservableCollection<NumberSetNumber>();
        private NumberSet _numberSet;

        public NumberSet NumberSet
        {
            get { return _numberSet; }
            set { _numberSet = value; }
        }

        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();

        public NumberSetNumbersViewModel()
        {
            List<NumberSetNumber> data = new List<NumberSetNumber>();
            if (_numberSet != null)
            {
                _numbrTumblrBusiness.GetNumberSetNumbersByNumberSetId(_numberSet.NumberSetID);
            }
            _numberSetNumbers.Clear();
            _numberSetNumbers.AddRange(data);

        }

        public void LoadNumberSetNumbersData(NumberSet numberSet)
        {
            List<NumberSetNumber> list = new List<NumberSetNumber>();

            NumberSet = numberSet;

            int min = numberSet.NumberSetNumberMin ?? 1;
            int max = numberSet.NumberSetNumberMax ?? 500;

            for (int i = min; i <= max; i++)
            {
                var numberSetNumber = new NumberSetNumber()
                {
                    Number = i,
                    NumberSet = NumberSet,
                    NumberSetID = NumberSet.NumberSetID,
                    NumberSetNumberID = i
                };
                if (_numbrTumblrBusiness.IsNumberSetNumber(numberSet.NumberSetID, numberSetNumber.Number))
                {
                    numberSetNumber.SelectedNumber = true;
                }
                list.Add(numberSetNumber);

            }
            _numberSetNumbers.Clear();
            _numberSetNumbers.AddRange(list);

        }


        //public void SetNumberSet(NumberSet numberSet)
        //{
        //    _numberSet = numberSet;
        //    SetNumberSetNumbers(_numberSet);
        //}

        //private void SetNumberSetNumbers(NumberSet numberSet)
        //{
        //    _numberSet = numberSet;

        //    List<NumberSetNumber> data = new List<NumberSetNumber>();

        //    if (_numberSet != null)
        //    {
        //        data = _numbrTumblrBusiness.GetNumberSetNumbersByNumberSetId(_numberSet.NumberSetID);
        //    }

        //    _numberSetNumbers.Clear();
        //    _numberSetNumbers.AddRange(data);
        //}

        public RangeObservableCollection<NumberSetNumber> NumberSetNumbers
        {
            get
            {
                //MockData(_numberSet);
                return _numberSetNumbers;
            }
        }

        public void RefreshData(NumberSet numberSet)
        {
            LoadNumberSetNumbersData(numberSet);
        }

        public void AddNumberToNumberSet(int number)
        {
            var foundNumber = NumberSetNumbers.FirstOrDefault(x => x.NumberSetID == NumberSet.NumberSetID
                                                                   && x.Number == number);
            if (foundNumber != null)
            {
                _numbrTumblrBusiness.AddNumberSetNumber(foundNumber);
                foundNumber.SelectedNumber = true;
            }

        }

        public void RemoveNumberFromNumberSet(int number)
        {
            var foundNumber = NumberSetNumbers.FirstOrDefault(x => x.NumberSetID == NumberSet.NumberSetID
                                                                  && x.Number == number);
            if (foundNumber != null)
            {
                _numbrTumblrBusiness.RemoveNumberSetNumber(foundNumber);
                foundNumber.SelectedNumber = false;
            }
        }
    }
}
