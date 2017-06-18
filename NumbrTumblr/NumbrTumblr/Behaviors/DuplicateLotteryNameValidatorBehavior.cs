using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Business;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using Xamarin.Forms;

namespace NumbrTumblr.Behaviors
{
    public class DuplicateNumberSetNameValidatorBehavior : Behavior<Entry>, IValidatorBehavior
    {       
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(NumberValidatorBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        private Business.NumbrTumblrBusiness numbrTumblrBusiness = new NumbrTumblrBusiness();
        public NumberSetViewModel NumberSetViewModel { get; set; }

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool duplicateNameError = false;

            if (NumberSetViewModel.EditMode)
            {
               
                    if (numbrTumblrBusiness.IsNumberSetDuplicateNameTakeIntoAccountNumberSetId(e.NewTextValue,NumberSetViewModel.NumberSet.NumberSetID))
                    {
                        duplicateNameError = true;
                    }
                

            }
            else
            {
                duplicateNameError = numbrTumblrBusiness.IsNumberSetDuplicateName(e.NewTextValue);
            }
            ((Entry)sender).TextColor = duplicateNameError ? Color.Red : Color.Default;
            NumberSetViewModel.NumberSetNameDuplicateError = duplicateNameError;
            IsValid = true;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }



    }

}
