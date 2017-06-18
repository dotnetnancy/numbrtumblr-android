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
    public class NumberSetViewModel : ViewModelBase
    {
        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();
        NumberSet _numberSet = new NumberSet();
        private Lottery _selectedLottery = null;
        bool _editMode = false;
        
        private bool _numberSetNameError = false;
        private bool _numberSetNameDuplicateError = false;
        private bool _numberSetNumberMaxError = false;
        private bool _numberSetNumberMinError = false;
        private string _numberSetNameErrorMessage = "NumberSet Name must be filled in";
        private string _numberSetNameDuplicateErrorMessage = "NumberSet Name is a Duplicate, please fill in another name";
        private string _numberSetNumberMaxErrorMessage = "Number Max must be filled in";
        private string _numberSetNumberMinErrorMessage = "Number Min must be filled in";

        public bool EditMode
        {
            get { return _editMode; }
            set { _editMode = value; }
        }
        public NumberSet NumberSet
        {
            get { return _numberSet; }
            set { _numberSet = value; }
        }

        public Lottery Lottery
        {
            get { return _selectedLottery; }
            set { _selectedLottery = value; }
        }

        public string NumberSetNameDuplicateErrorMessage
        {
            get { return _numberSetNameDuplicateErrorMessage; }
            set
            {
                _numberSetNameDuplicateErrorMessage = value;
                RaisePropertyChanged(() => NumberSetNameDuplicateErrorMessage);
            }

        }

        public string NumberSetNameErrorMessage
        {
            get { return _numberSetNameErrorMessage; }
            set
            {
                _numberSetNameErrorMessage = value;
                RaisePropertyChanged(() => NumberSetNameErrorMessage);
            }
        }

        public string NumberSetNumberMaxErrorMessage
        {
            get { return _numberSetNumberMaxErrorMessage; }
            set
            {
                _numberSetNumberMaxErrorMessage = value;
                RaisePropertyChanged(() => NumberSetNumberMaxErrorMessage);
            }
        }

        public string NumberSetNumberMinErrorMessage
        {
            get { return _numberSetNumberMinErrorMessage; }
            set
            {
                _numberSetNumberMinErrorMessage = value;
                RaisePropertyChanged(() => NumberSetNumberMinErrorMessage);
            }
        }


        public bool NumberSetNameError
        {
            get { return _numberSetNameError; }
            set
            {
                _numberSetNameError = value;
                RaisePropertyChanged(() => NumberSetNameError);
            }
        }

        public bool NumberSetNameDuplicateError
        {
            get { return _numberSetNameDuplicateError; }
            set
            {
                _numberSetNameDuplicateError = value;
                RaisePropertyChanged(() => NumberSetNameDuplicateError);
            }
        }

        public bool NumberSetNumberMaxError
        {
            get { return _numberSetNumberMaxError; }
            set
            {
                _numberSetNumberMaxError = value;
                RaisePropertyChanged(() => NumberSetNumberMaxError);
            }
        }

        public bool NumberSetNumberMinError
        {
            get { return _numberSetNumberMinError; }
            set
            {
                _numberSetNumberMinError = value;
                RaisePropertyChanged(() => NumberSetNumberMinError);
            }
        }


        public string NumberSetName
        {
            get { return _numberSet.NumberSetName; }
            set
            {
                _numberSet.NumberSetName = value;
                RaisePropertyChanged(() => NumberSetName);
            }
        }

        public string NumberSetDescription
        {
            get { return _numberSet.NumberSetDescription; }
            set
            {
                _numberSet.NumberSetDescription = value;
                RaisePropertyChanged(() => NumberSetDescription);
            }
        }

        public int? NumberSetNumberMax
        {
            get { return _numberSet.NumberSetNumberMax; }
            set
            {
                _numberSet.NumberSetNumberMax = value;
                RaisePropertyChanged(() => NumberSetNumberMax);
            }
        }

        public int? NumberSetNumberMin
        {
            get { return _numberSet.NumberSetNumberMin; }
            set
            {
                _numberSet.NumberSetNumberMin = value;
                RaisePropertyChanged(() => NumberSetNumberMin);
            }
        }


        public void SetValues(NumberSet result)
        {
            NumberSetName = result.NumberSetName;
            NumberSetDescription = result.NumberSetDescription;
            NumberSetNumberMax = result.NumberSetNumberMax;
            NumberSetNumberMin = result.NumberSetNumberMin;
        }

        public NumberSet GetNumberSet()
        {
            return new NumberSet()
            {
                NumberSetName = NumberSetName,
                NumberSetDescription = NumberSetDescription,
                NumberSetNumberMax = NumberSetNumberMax,
            NumberSetNumberMin = NumberSetNumberMin
        };
        }
    }
}