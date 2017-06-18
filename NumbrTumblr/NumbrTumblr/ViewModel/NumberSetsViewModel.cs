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
    public class NumberSetsViewModel : ViewModelBase
    {
        private RangeObservableCollection<NumberSet> _numberSets = new RangeObservableCollection<NumberSet>();
        private RangeObservableCollection<NumberSetNumber> _numberSetNumbers = new RangeObservableCollection<NumberSetNumber>();

        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();
        private string _numberSetNameLabel;

        public NumberSetsViewModel()
        {
            List<NumberSet> data = _numbrTumblrBusiness.GetNumberSets();
            if (data != null)
            {
                if (data != null)
                {
                    _numberSets.AddRange(data);
                }
            }
        }

         public string NumberSetNameLabel
        {
            get
            {
                SetNameLabel();
                return _numberSetNameLabel;
            }
            set
            {
                SetNameLabel();
                 RaisePropertyChanged(() => NumberSetNameLabel);
            }
        }

        private void SetNameLabel()
        {
            if (_numberSets == null || _numberSets.Count == 0)
                {
                    _numberSetNameLabel = "No NumberSets Created.";
                }
                else
                {
                    _numberSetNameLabel = string.Empty;
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

        public void RefreshData()
        {
            List<NumberSet> data = _numbrTumblrBusiness.GetNumberSets();

            if (data != null)
            {
                _numberSets.Clear();
                _numberSets.AddRange(data);
                NumberSetNameLabel = string.Empty;
            }
        }
    }
}