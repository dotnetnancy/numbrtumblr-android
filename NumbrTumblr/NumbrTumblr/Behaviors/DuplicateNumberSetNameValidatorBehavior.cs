using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Business;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using Xamarin.Forms;

namespace NumbrTumblr.Behaviors
{
    public class DuplicateLotteryNameValidatorBehavior : Behavior<Entry>, IValidatorBehavior
    {

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(NumberValidatorBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;
        public LotteryViewModel LotteryViewModel { get; set; }

        private Business.NumbrTumblrBusiness numbrTumblrBusiness = new NumbrTumblrBusiness();

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

            if (LotteryViewModel.EditMode)
            {

                if (numbrTumblrBusiness.IsLotteryDuplicateNameTakeIntoAccountLotteryId(e.NewTextValue, LotteryViewModel.Lottery.LotteryID))
                {
                    duplicateNameError = true;
                }


            }
            else
            {
                duplicateNameError = numbrTumblrBusiness.IsLotteryDuplicateName(e.NewTextValue);
            }
            ((Entry)sender).TextColor = duplicateNameError ? Color.Red : Color.Default;
            LotteryViewModel.LotteryNameDuplicateError = duplicateNameError;
            IsValid = true;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }



    }

}
