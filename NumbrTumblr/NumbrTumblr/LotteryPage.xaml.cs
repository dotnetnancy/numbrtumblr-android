using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Behaviors;
using NumbrTumblr.Business;
using NumbrTumblr.Controls;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using Xamarin.Forms;

namespace NumbrTumblr
{
    public partial class LotteryPage : ContentPage,INavigateCommon,IPageViewModelCommon
    {
        private LotteryViewModel _lotteryViewModel;
        private NumbrTumblrBusiness _business = new NumbrTumblrBusiness();
        private BehaviorValidatedInput<Entry> LotteryNameStringField = new BehaviorValidatedInput<Entry>();
        private BehaviorValidatedInput<Entry> LotteryNumberMinField = new BehaviorValidatedInput<Entry>();
        private BehaviorValidatedInput<Entry> LotteryNumberMaxField = new BehaviorValidatedInput<Entry>();
        private BehaviorValidatedInput<Entry> LotteryNameDuplicateField = new BehaviorValidatedInput<Entry>();

        private StateProvincesViewModel _stateProvinceViewModel;

        private NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();

        public LotteryViewModel LotteryViewModel
        {
            get
            {
                return _lotteryViewModel;
            }
        }       

        public LotteryPage(Lottery lottery, bool editMode)
        {
            InitializeComponent();
            _lotteryViewModel = App.Locator.LotteryPage;
            BindingContext = _lotteryViewModel;
            _lotteryViewModel.Lottery = lottery;
            _lotteryViewModel.EditMode = editMode;
            GenerateContent(editMode);
        }

        private void GenerateContent(bool editMode = false)
        {
            Title = "New Lottery";

            StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Vertical,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Padding=new Thickness(30,30,30,30)};

            //this might be a drop down and then user can add items that don't already exist.
            // Form fields
            var lotteryNameEntry = new Entry
            {
                Placeholder = "Lottery Name"
            };

            lotteryNameEntry.SetBinding(Entry.TextProperty, new Binding("LotteryName", BindingMode.TwoWay));
            lotteryNameEntry.Behaviors.Add(new StringLengthValidatorBehavior());
            lotteryNameEntry.Behaviors.Add(new DuplicateLotteryNameValidatorBehavior() { LotteryViewModel = _lotteryViewModel });
            LotteryNameStringField.Element = lotteryNameEntry;
            lotteryNameEntry.TextChanged += LotteryNameEntry_TextChanged;

            var lotteryNameErrorLabel = new Label();
            lotteryNameErrorLabel.SetBinding(Label.TextProperty, new Binding("LotteryNameErrorMessage", BindingMode.TwoWay));
            lotteryNameErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("LotteryNameError", BindingMode.TwoWay));
            lotteryNameErrorLabel.TextColor = Color.Red;

            var lotteryNameDuplicateErrorLabel = new Label();
            lotteryNameDuplicateErrorLabel.SetBinding(Label.TextProperty, new Binding("LotteryNameDuplicateErrorMessage", BindingMode.TwoWay));
            lotteryNameDuplicateErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("LotteryNameDuplicateError", BindingMode.TwoWay));
            lotteryNameDuplicateErrorLabel.TextColor = Color.Red;
            LotteryNameDuplicateField.Element = lotteryNameEntry;

            var lotteryDescriptionEntry = new Entry
            {
                Placeholder = "Lottery Description"
            };
            lotteryDescriptionEntry.SetBinding(Entry.TextProperty, new Binding("LotteryDescription", BindingMode.TwoWay));


            Label header = new Label
            {
                Text = "State/Province",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };

            var lotteryNumberMinEntry = new Entry
            {
                Placeholder = "1",
                Keyboard = Keyboard.Numeric
            };
            lotteryNumberMinEntry.SetBinding(Entry.TextProperty,
                new Binding("LotteryNumberMin", BindingMode.TwoWay, new NullableIntegerConverter(), null));
            lotteryNumberMinEntry.Behaviors.Add(new MaxLengthValidator() { MaxLength = 3 });
            LotteryNumberMinField.Element = lotteryNumberMinEntry;
            lotteryNumberMinEntry.TextChanged += LotteryNumberMinEntry_TextChanged;

            var lotteryNumberMinErrorLabel = new Label();
            lotteryNumberMinErrorLabel.SetBinding(Label.TextProperty, new Binding("LotteryNumberMinErrorMessage", BindingMode.TwoWay));
            lotteryNumberMinErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("LotteryNumberMinError", BindingMode.TwoWay));
            lotteryNumberMinErrorLabel.TextColor = Color.Red;

            var lotteryNumberMaxEntry = new Entry
            {
                Placeholder = "500",
                Keyboard = Keyboard.Numeric
            };
            lotteryNumberMaxEntry.SetBinding(Entry.TextProperty,
                new Binding("LotteryNumberMax", BindingMode.TwoWay, new NullableIntegerConverter(), null));
            lotteryNumberMaxEntry.Behaviors.Add(new MaxLengthValidator() { MaxLength = 3 });
            LotteryNumberMaxField.Element = lotteryNumberMaxEntry;
            lotteryNumberMaxEntry.TextChanged += LotteryNumberMaxEntry_TextChanged;

            var lotteryNumberMaxErrorLabel = new Label();
            lotteryNumberMaxErrorLabel.SetBinding(Label.TextProperty, new Binding("LotteryNumberMaxErrorMessage", BindingMode.TwoWay));
            lotteryNumberMaxErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("LotteryNumberMaxError", BindingMode.TwoWay));
            lotteryNumberMaxErrorLabel.TextColor = Color.Red;

            //need a drop down for stateProvince

            stackLayout.Children.Add(lotteryNameEntry);
            stackLayout.Children.Add(lotteryNameDuplicateErrorLabel);
            stackLayout.Children.Add(lotteryNameErrorLabel);
            stackLayout.Children.Add(lotteryDescriptionEntry);
            stackLayout.Children.Add(lotteryNumberMinEntry);
            stackLayout.Children.Add(lotteryNumberMinErrorLabel);
            stackLayout.Children.Add(lotteryNumberMaxEntry);
            stackLayout.Children.Add(lotteryNumberMaxErrorLabel);

            Button saveLotteryButton = new Button()
            {
                Text = "Save Lottery"
            };

            saveLotteryButton.Clicked += SaveLotteryButton_Clicked;

            stackLayout.Children.Add(saveLotteryButton);

            Content = stackLayout;
        }

        private void LotteryNumberMinEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(LotteryNumberMinField.IsValid)
            {
                _lotteryViewModel.LotteryNumberMinError = false;
            }
        }

        private void LotteryNumberMaxEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LotteryNumberMaxField.IsValid)
            {
                _lotteryViewModel.LotteryNumberMaxError = false;
            }
        }

        private void LotteryNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(LotteryNameStringField.IsValid)
            {
                _lotteryViewModel.LotteryNameError = false;
            }
        }


        //this would be called when the user clicks the back button on the page where toolbaritem back arrow was clicked on from
        //this is an android specific call to this method from mainactivity.cs onoptionsitemselected override method
        public void NavigateAwayFromPage()
        {
            //_lotteriesPage = new LotteriesPage();
            //_numberSetsPage = new NumberSetsPage();
            //_lotteryPage = new LotteryPage();
            //DisplayAlert("LotteryPage Navigated away from","","ok","cancel");

        }

        private void LotteryNumberMinEntryOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (LotteryNumberMinField.IsValid)
            {
                _lotteryViewModel.LotteryNumberMinError = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // If you want to continue going back
            bool result = base.OnBackButtonPressed();
            NavigateAwayFromPage();
            //DisplayAlert("Back button was clicked and i have coded it not to go back for the moment", "Clicked", "Cancel");

            // return result;

            // If you want to stop the back button
            return true;
        }


        private async void SaveLotteryButton_Clicked(object sender, EventArgs e)
        {
            if(Validate())
            {               
                var result = await SaveLottery();
                //when you save a lottery i would want the data for the lotteries list refreshed with the new item so 
                //here i am calling RefreshDataForPage right at main which will reload all view that it manages i may
                //want to change this to only reload the lotteries list
                await NavigationManager.RefreshDataNavigationStack(App.Current.MainPage.Navigation);
                await NavigationManager.PopAsyncPage(App.Current.MainPage.Navigation, this);
            }
        }       

        private bool Validate()
        {
            bool valid = true;
            if (!LotteryNameStringField.IsValid)
            {
                _lotteryViewModel.LotteryNameError = true;
                valid = false;
            }
            else
            {
                _lotteryViewModel.LotteryNameError = false;
            }

            if (_lotteryViewModel.EditMode)
            {
                if (_numbrTumblrBusiness.IsLotteryDuplicateNameTakeIntoAccountLotteryId(_lotteryViewModel.LotteryName, _lotteryViewModel.Lottery.LotteryID))
                {
                    _lotteryViewModel.LotteryNameDuplicateError = true;
                    valid = false;
                }
                else
                {
                    _lotteryViewModel.LotteryNameDuplicateError = false;
                }
            }

            if (!LotteryNumberMaxField.IsValid)
            {
                _lotteryViewModel.LotteryNumberMaxError = true;
                valid = false;
            }
            else
            {
                _lotteryViewModel.LotteryNumberMaxError = false;
            }

            if (!LotteryNumberMinField.IsValid)
            {
                _lotteryViewModel.LotteryNumberMinError = true;
                valid = false;
            }
            else
            {
                _lotteryViewModel.LotteryNumberMinError = false;
            }

            return valid;
        }

        private async Task<Lottery> SaveLottery()
        {
            Lottery result = await _business.SaveLottery(_lotteryViewModel.Lottery);
            if (result != null)
            {
                //_lotteryViewModel.SetValues(result);
            }
            else
            {
                //problem nothing was saved
                await DisplayAlert("Null was returned from the save, not good must be an error", "", "Cancel");
            }
            return result;
        }

        public void ReloadViewModel()
        {
            _lotteryViewModel.Lottery = new Lottery();
        }
    }
}
