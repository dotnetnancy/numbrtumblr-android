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
    public partial class NumberSetPage : ContentPage, INavigateCommon, IPageViewModelCommon
    {
        private NumberSetViewModel _numberSetViewModel;
        private NumberSetNumbersViewModel _numberSetNumbersViewModel;
        private NumbrTumblrBusiness _business = new NumbrTumblrBusiness();
        private LotteriesViewModel _lotteriesViewModel;
        bool _editMode = false;
        string _editingName;

        private BehaviorValidatedInput<Entry> NumberSetNameStringField = new BehaviorValidatedInput<Entry>();
        public BehaviorValidatedInput<Entry> NumberSetNameDuplicateField = new BehaviorValidatedInput<Entry>();
        private BehaviorValidatedInput<Entry> NumberSetNumberMinField = new BehaviorValidatedInput<Entry>();
        private BehaviorValidatedInput<Entry> NumberSetNumberMaxField = new BehaviorValidatedInput<Entry>();

        public NumberSetViewModel NumberSetViewModel
        {
            get
            {
                return _numberSetViewModel;
            }
        }
        private NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();
        public NumberSetPage(NumberSet numberSet, bool editMode)
        {
            InitializeComponent();
            _numberSetViewModel = App.Locator.NumberSetPage;
            _lotteriesViewModel = App.Locator.LotteriesPage;
            BindingContext = _numberSetViewModel;
            _numberSetViewModel.NumberSet = numberSet;
            _numberSetViewModel.EditMode = editMode;
            GenerateContent(editMode);

        }     

        private void GenerateContent(bool editMode = false)
        {   
            Title = "New NumberSet";
            _numberSetViewModel.EditMode = editMode;

            StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(30, 30, 30, 30) };

            var numberSetNameEntry = new Entry
            {
                Placeholder = "NumberSet Name"
            };

            numberSetNameEntry.SetBinding(Entry.TextProperty, new Binding("NumberSetName", BindingMode.TwoWay));
            numberSetNameEntry.Behaviors.Add(new StringLengthValidatorBehavior());
            numberSetNameEntry.Behaviors.Add(new DuplicateNumberSetNameValidatorBehavior() { NumberSetViewModel = _numberSetViewModel });
            NumberSetNameStringField.Element = numberSetNameEntry;
            numberSetNameEntry.TextChanged += NumberSetNameEntryOnTextChanged;

            var numberSetNameErrorLabel = new Label();
            numberSetNameErrorLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNameErrorMessage", BindingMode.TwoWay));
            numberSetNameErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("NumberSetNameError", BindingMode.TwoWay));
            numberSetNameErrorLabel.TextColor = Color.Red;

            var numberSetNameDuplicateErrorLabel = new Label();
            numberSetNameDuplicateErrorLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNameDuplicateErrorMessage", BindingMode.TwoWay));
            numberSetNameDuplicateErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("NumberSetNameDuplicateError", BindingMode.TwoWay));
            numberSetNameDuplicateErrorLabel.TextColor = Color.Red;
            NumberSetNameDuplicateField.Element = numberSetNameEntry;

            var numberSetDescriptionEntry = new Entry
            {
                Placeholder = "NumberSet Description"
            };
            numberSetDescriptionEntry.SetBinding(Entry.TextProperty, new Binding("NumberSetDescription", BindingMode.TwoWay));
            var numberSetNumberMinEntry = new Entry
            {
                Placeholder = "1",
                Keyboard = Keyboard.Numeric
            };
            numberSetNumberMinEntry.SetBinding(Entry.TextProperty,
                new Binding("NumberSetNumberMin", BindingMode.TwoWay, new NullableIntegerConverter(), null));
            numberSetNumberMinEntry.Behaviors.Add(new MaxLengthValidator() { MaxLength = 3 });
            NumberSetNumberMinField.Element = numberSetNumberMinEntry;
            numberSetNumberMinEntry.TextChanged += NumberSetNumberMinEntryOnTextChanged;

            var numberSetNumberMinErrorLabel = new Label();
            numberSetNumberMinErrorLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNumberMinErrorMessage", BindingMode.TwoWay));
            numberSetNumberMinErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("NumberSetNumberMinError", BindingMode.TwoWay));
            numberSetNumberMinErrorLabel.TextColor = Color.Red;


            var numberSetNumberMaxEntry = new Entry
            {
                Placeholder = "500",
                Keyboard = Keyboard.Numeric
            };
            numberSetNumberMaxEntry.SetBinding(Entry.TextProperty,
                new Binding("NumberSetNumberMax", BindingMode.TwoWay, new NullableIntegerConverter(), null));
            numberSetNumberMaxEntry.Behaviors.Add(new MaxLengthValidator() { MaxLength = 3 });
            NumberSetNumberMaxField.Element = numberSetNumberMaxEntry;

            numberSetNumberMaxEntry.TextChanged += NumberSetNumberMaxEntryOnTextChanged;

            var numberSetNumberMaxErrorLabel = new Label();
            numberSetNumberMaxErrorLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNumberMaxErrorMessage", BindingMode.TwoWay));
            numberSetNumberMaxErrorLabel.SetBinding(Label.IsVisibleProperty, new Binding("NumberSetNumberMaxError", BindingMode.TwoWay));
            numberSetNumberMaxErrorLabel.TextColor = Color.Red;

            //BindablePicker picker = new BindablePicker()
            //{
            //    Title = "Associate Lottery",
            //    VerticalOptions = LayoutOptions.CenterAndExpand,                
            //    Opacity = 100,
            //    ItemsSource = _lotteriesViewModel.Lotteries.Select(x => x.LotteryName).ToList(),

            //};

            //picker.SelectedIndexChanged += PickerOnSelectedIndexChanged;

            stackLayout.Children.Add(numberSetNameEntry);
            stackLayout.Children.Add(numberSetNameDuplicateErrorLabel);
            stackLayout.Children.Add(numberSetNameErrorLabel);
            stackLayout.Children.Add(numberSetDescriptionEntry);
            stackLayout.Children.Add(numberSetNumberMinEntry);
            stackLayout.Children.Add(numberSetNumberMinErrorLabel);
            stackLayout.Children.Add(numberSetNumberMaxEntry);
            stackLayout.Children.Add(numberSetNumberMaxErrorLabel);
            //stackLayout.Children.Add(picker);

            Button saveNumberSetButton = new Button()
            {
                Text = "Save NumberSet"
            };

            saveNumberSetButton.Clicked += SaveNumberSetButton_Clicked;

            stackLayout.Children.Add(saveNumberSetButton);

            Content = stackLayout;
        }

        private void NumberSetNumberMaxEntryOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (NumberSetNumberMaxField.IsValid)
            {
                _numberSetViewModel.NumberSetNumberMaxError = false;
            }
        }

        private void NumberSetNumberMinEntryOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (NumberSetNumberMinField.IsValid)
            {
                _numberSetViewModel.NumberSetNumberMinError = false;
            }
        }

        private void NumberSetNameEntryOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (NumberSetNameStringField.IsValid)
            {
                _numberSetViewModel.NumberSetNameError = false;
            }
        }

        private void PickerOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var picker = ((NumbrTumblr.Controls.BindablePicker)sender);
            var index = picker.SelectedIndex;
            if (picker.ItemsSource != null)
                picker.SelectedItem = (picker.ItemsSource as List<string>)[index];
            var lotteryName = picker.SelectedItem.ToString();
            var selectedLottery = _lotteriesViewModel.Lotteries.FirstOrDefault(x => x.LotteryName == lotteryName);
            _numberSetViewModel.Lottery = selectedLottery;
        }

        //this would be called when the user clicks the back button on the page where toolbaritem back arrow was clicked on from
        //this is an android specific call to this method from mainactivity.cs onoptionsitemselected override method
        public void NavigateAwayFromPage()
        {
            //_lotteriesPage = new LotteriesPage();
            //_numberSetsPage = new NumberSetsPage();
            //_numberSetPage = new NumberSetPage();
            //DisplayAlert("NumberSetPage Navigated away from", "", "ok", "cancel");
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


        private async void SaveNumberSetButton_Clicked(object sender, EventArgs e)
        {
            if (Validate())
            {

                var result = await SaveNumberSet();
                //when you save a numberSet i would want the data for the lotteries list refreshed with the new item so 
                //here i am calling RefreshDataForPage right at main which will reload all view that it manages i may
                //want to change this to only reload the lotteries list
                await NavigationManager.RefreshDataNavigationStack(App.Current.MainPage.Navigation);
                await NavigationManager.PopAsyncPage(App.Current.MainPage.Navigation, this);
            }
        }


        private bool Validate()
        {
            bool valid = true;
            if (!NumberSetNameStringField.IsValid)
            {
                _numberSetViewModel.NumberSetNameError = true;
                valid = false;
            }
            else
            {
                _numberSetViewModel.NumberSetNameError = false;
            }

            if (_numberSetViewModel.EditMode)
            {
                if (_numbrTumblrBusiness.IsNumberSetDuplicateNameTakeIntoAccountNumberSetId(_numberSetViewModel.NumberSetName,_numberSetViewModel.NumberSet.NumberSetID))
                {
                    _numberSetViewModel.NumberSetNameDuplicateError = true;
                    valid = false;
                }
                else
                {
                    _numberSetViewModel.NumberSetNameDuplicateError = false;
                }
            }

            if (!NumberSetNumberMaxField.IsValid)
            {
                _numberSetViewModel.NumberSetNumberMaxError = true;
                valid = false;
            }
            else
            {
                _numberSetViewModel.NumberSetNumberMaxError = false;
            }

            if (!NumberSetNumberMinField.IsValid)
            {
                _numberSetViewModel.NumberSetNumberMinError = true;
                valid = false;
            }
            else
            {
                _numberSetViewModel.NumberSetNumberMinError = false;
            }

            return valid;
        }

        private async Task<NumberSet> SaveNumberSet()
        {
            NumberSet result = await _business.SaveNumberSet(_numberSetViewModel);
            if (result != null)
            {
                //_numberSetViewModel.SetValues(result);
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
            _numberSetViewModel.NumberSet = new NumberSet();
        }
    }
}
