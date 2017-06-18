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
    public class MainPageViewModel : ViewModelBase
    {
       
        private RangeObservableCollection<Lottery> _lotteries  = new RangeObservableCollection<Lottery>();
        private RangeObservableCollection<NumberSet> _numberSets = new RangeObservableCollection<NumberSet>();
        private NumberSet _numberPool = null;
        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();

        public MainPageViewModel()
        {
            LotteryDataModel data = _numbrTumblrBusiness.GetLotteryData();
            if (data != null)
            {
                if (data.MyLotteries != null)
                {
                    _lotteries.AddRange(data.MyLotteries);
                }

                if (data.MyNumberSets != null)
                {
                    _numberSets.AddRange(data.MyNumberSets);
                }
                _numberPool = data.MyNumberPool;
            }
        }

        public NumberSet NumberPool
        {
            get { return _numberPool; }
            set
            {
                if (Set(() => NumberPool, ref _numberPool, value))
                {
                    RaisePropertyChanged();
                }
            }
        }

        public RangeObservableCollection<Lottery> Lotteries
        {
            get { return _lotteries; }
            set
            {
                _lotteries.AddRange(value);
            }
        }


        public RangeObservableCollection<NumberSet> NumberSets
        {
            get { return _numberSets; }
            set
            {
                _numberSets.AddRange(value);
            }
        }
    }
}
