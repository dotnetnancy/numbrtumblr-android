using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using NumbrTumblr.Business;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Models;
using Xamarin.Forms;

namespace NumbrTumblr.ViewModel
{
    public class LotteriesViewModel : ViewModelBase
    {
        private RangeObservableCollection<Lottery> _lotteries = new RangeObservableCollection<Lottery>();
        public Lottery SelectedLottery { get; set; }
        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();
        string _lotteryNameLabel = null;

        public string LotteryNameLabel
        {
            get
            {
                SetNameLabel();
                return _lotteryNameLabel;
            }
            set
            {
                SetNameLabel();
                 RaisePropertyChanged(() => LotteryNameLabel);
            }
        }

        private void SetNameLabel()
        {
            if (_lotteries == null || _lotteries.Count == 0)
                {
                    _lotteryNameLabel = "No Lotteries Created.";
                }
                else
                {
                    _lotteryNameLabel = string.Empty;
                }
        }

        public LotteriesViewModel()
        {
            List<Lottery> data = _numbrTumblrBusiness.GetLotteries();
            if (data != null)
            {
                if (data != null)
                {
                    _lotteries.AddRange(data);
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

        public void RefreshData()
        {
            //if we were doing paging we would of course do that there
            List<Lottery> data = _numbrTumblrBusiness.GetLotteries();

            if (data != null)
            {
                _lotteries.Clear();
                _lotteries.AddRange(data);
                //just to initiate the processing for a dynamic name
                LotteryNameLabel = string.Empty;             

            }
        }
    }
}
