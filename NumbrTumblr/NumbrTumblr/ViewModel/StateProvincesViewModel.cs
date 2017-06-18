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
using NumbrTumblr.Data.Files.FileEntities;
using NumbrTumblr.Data.Files.FileEntities.City;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Models;
using Xamarin.Forms;

namespace NumbrTumblr.ViewModel
{
    public class StateProvincesViewModel : ViewModelBase
    {
        private RangeObservableCollection<StateProvince> _stateProvinces = new RangeObservableCollection<StateProvince>();

        NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();

        public StateProvincesViewModel()
        {
            List <StateProvince> data = _numbrTumblrBusiness.GetStateProvinces();
            if (data != null)
            {
                if (data != null)
                {
                    _stateProvinces.AddRange(data);
                }
            }
        }

        public RangeObservableCollection<StateProvince> StateProvinces
        {
            get { return _stateProvinces; }
            set
            {
                _stateProvinces.AddRange(value);
            }
        }

        public void RefreshData()
        {
            //if we were doing paging we would of course do that there
            List<StateProvince> data = _numbrTumblrBusiness.GetStateProvinces();

            if (data != null)
            {
                _stateProvinces.Clear();
                _stateProvinces.AddRange(data);
            }
        }
    }
}
