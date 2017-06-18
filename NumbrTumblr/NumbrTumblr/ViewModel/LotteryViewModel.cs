using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NumbrTumblr.Business;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Interfaces;
using NumbrTumblr.Models;

namespace NumbrTumblr.ViewModel
{
    public class LotteryViewModel : ViewModelBase
    {
        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();
        Lottery _lottery = new Lottery();
        bool _editMode = false;

        private bool _lotteryNameError = false;
        private bool _lotteryNumberMaxError = false;
        private bool _lotteryNumberMinError = false;
        private bool _lotteryNameDuplicateError = false;

        private string _lotteryNameErrorMessage = "Lottery Name must be filled in";
        private string _lotteryNumberMaxErrorMessage = "Number Max must be filled in";
        private string _lotteryNumberMinErrorMessage = "Number Min must be filled in";
        private string _lotteryNameDuplicateErrorMessage = "Lottery Name is a Duplicate, please fill in another name";

        public Lottery Lottery
        {
            get { return _lottery; }
            set { _lottery = value; }
        }

        public string LotteryNameDuplicateErrorMessage
        {
            get { return _lotteryNameDuplicateErrorMessage; }
            set
            {
                _lotteryNameDuplicateErrorMessage = value;
                RaisePropertyChanged(() => LotteryNameDuplicateErrorMessage);
            }

        }

        public string LotteryNameErrorMessage
        {
            get { return _lotteryNameErrorMessage; }
            set
            {
                _lotteryNameErrorMessage = value;
                RaisePropertyChanged(() => LotteryNameErrorMessage);
            }
        }
        public string LotteryNumberMaxErrorMessage
        {
            get { return _lotteryNumberMaxErrorMessage; }
            set
            {
                _lotteryNumberMaxErrorMessage = value;
                RaisePropertyChanged(() => LotteryNumberMaxErrorMessage);
            }
        }

        public string LotteryNumberMinErrorMessage
        {
            get { return _lotteryNumberMinErrorMessage; }
            set
            {
                _lotteryNumberMinErrorMessage = value;
                RaisePropertyChanged(() => LotteryNumberMinErrorMessage);
            }
        }


        //public int LotteryID
        //{
        //    get { return _lottery.LotteryID; }
        //    set
        //    {
        //        _lottery.LotteryID = value;
        //        RaisePropertyChanged(() => LotteryID);
        //    }
        //}

        //public int StateProvinceID
        //{
        //    get { return _lottery.StateProvinceID; }
        //    set
        //    {
        //        _lottery.StateProvinceID = value;
        //        RaisePropertyChanged(() => StateProvinceID);
        //    }
        //}

        public bool LotteryNameError
        {
            get { return _lotteryNameError; }
            set
            {
                _lotteryNameError = value;
                RaisePropertyChanged(() => LotteryNameError);
            }
        }

        public bool LotteryNameDuplicateError
        {
            get { return _lotteryNameDuplicateError; }
            set
            {
                _lotteryNameDuplicateError = value;
                RaisePropertyChanged(() => LotteryNameDuplicateError);
            }
        }

        public bool LotteryNumberMaxError
        {
            get { return _lotteryNumberMaxError; }
            set
            {
                _lotteryNumberMaxError = value;
                RaisePropertyChanged(() => LotteryNumberMaxError);
            }
        }

        public bool LotteryNumberMinError
        {
            get { return _lotteryNumberMinError; }
            set
            {
                _lotteryNumberMinError = value;
                RaisePropertyChanged(() => LotteryNumberMinError);
            }
        }

        public string LotteryName
        {
            get { return _lottery.LotteryName; }
            set
            {
                _lottery.LotteryName = value;
                RaisePropertyChanged(() => LotteryName);
            }
        }

        public string LotteryDescription
        {
            get { return _lottery.LotteryDescription; }
            set
            {
                _lottery.LotteryDescription = value;
                RaisePropertyChanged(() => LotteryDescription);
            }
        }

        public int? LotteryNumberMax
        {
            get { return _lottery.LotteryNumberMax; }
            set
            {
                _lottery.LotteryNumberMax = value;
                RaisePropertyChanged(() => LotteryNumberMax);
            }
        }

        public int? LotteryNumberMin
        {
            get { return _lottery.LotteryNumberMin; }
            set
            {
                _lottery.LotteryNumberMin = value;
                RaisePropertyChanged(() => LotteryNumberMin);
            }
        }

        public bool EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
            }
        }

        public void SetValues(Lottery result)
        {
            LotteryName = result.LotteryName;
            LotteryDescription = result.LotteryDescription;
            LotteryNumberMax = result.LotteryNumberMax;
            LotteryNumberMin = result.LotteryNumberMin;
        }

        public Lottery GetLottery()
        {
            return new Lottery()
            {
                LotteryName = LotteryName,
                LotteryNumberMax = LotteryNumberMax,
                LotteryDescription = LotteryDescription,
                LotteryNumberMin = LotteryNumberMin

            };
        }


    }
}
