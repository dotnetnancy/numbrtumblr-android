using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using Xamarin.Forms;

namespace NumbrTumblr
{
    public partial class NumberSetsPage : ContentPage,INavigateCommon,IPageViewModelCommon
    {
        private NumberSetsViewModel _numberSetsViewModel;
        private NumberSetNumbersViewModel _numberSetNumbersViewModel;
        private int _numberSetCounter = 0;
        private int _numberSetNumberCounter = 0;
        public Business.NumbrTumblrBusiness _numberTumblerBusiness = new Business.NumbrTumblrBusiness();
        public RangeObservableCollection<NumberSet> NumberSets { get; set; }
        private NumberSetNumbersPage  _numberSetNumbersPage = new NumberSetNumbersPage();
        private NumberSetPage _numberSetPage;
        private ShufflePickPage _shufflePickPage;

        public NumberSetsViewModel NumberSetsViewModel
        {
            get
            {
                return _numberSetsViewModel;
            }
        }

        public NumberSet CurrentNumberSet { get; set; }
        public NumberSetsPage()
        {
            InitializeComponent();
            _numberSetsViewModel = App.Locator.NumberSetsPage;
            _numberSetNumbersViewModel = App.Locator.NumberSetNumbersPage;


            NumberSets = _numberSetsViewModel.NumberSets;
            GenerateContent();
        }

        private void GenerateContent()
        {
            StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Vertical ,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Padding=new Thickness(30,30,30,30)};
            ScrollView scrollView = new ScrollView();
            scrollView.Orientation = ScrollOrientation.Both;

            ListView numberSetsListView = GenerateNumberSetsListView();

            stackLayout.Children.Add(numberSetsListView);           
            scrollView.Content = stackLayout;
            Content = scrollView;
        }

        private async void ManageNumberSetNumberButton_Clicked(object sender, EventArgs e)
        {
            _numberSetNumbersPage = new NumberSetNumbersPage();
            await NavigationManager.PushAsyncPage(Navigation, _numberSetNumbersPage);
        }

        private ListView GenerateNumberSetsListView()
        {
            var numberSetsListView = new ListView();
            this.Title = "NumberSets";
            numberSetsListView.ItemsSource = _numberSetsViewModel.NumberSets;
            numberSetsListView.ItemSelected += NumberSetsListView_ItemSelected;

            numberSetsListView.ItemTemplate = new DataTemplate(typeof(CustomNumberSetCell));
            numberSetsListView.Header = GetNumberSetListViewHeader();
            return numberSetsListView;
        }

        public async void NumberSetsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedNumberSet = ((NumberSet) e.SelectedItem);
            SelectNumberSet(selectedNumberSet);
        }

        private void SelectNumberSet(NumberSet selectedNumberSet)
        {
            CurrentNumberSet = selectedNumberSet;
            _numberSetNumberCounter = 0;
            _numberSetNumbersViewModel.LoadNumberSetNumbersData(selectedNumberSet);
        }

        public class CustomNumberSetCell : ViewCell
        {
            public CustomNumberSetCell()
            {               
                var numberSetNameLabel = new Label();
                numberSetNameLabel.SetBinding(Label.TextProperty, new Binding("NumberSetName"));

                var view = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {                      
                        numberSetNameLabel
                    }
                };
              
                AddTapGesture(view);

                View = view;
            }

            private void AddTapGesture(View view)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += HandleTap;
                view.GestureRecognizers.Add(tapGestureRecognizer);
            }

            public async void HandleTap(object sender, EventArgs e)
            {
                var action = 
                    await Application.Current.MainPage.DisplayActionSheet("Would you like to?",
                    "Cancel",
                    null,
                    "Shuffle and Pick from NumberSet",
                    "View and Manage Saved Shuffles and Picks",
                    "Delete NumberSet", 
                    "Modify NumberSet");

                ViewCell theViewCell = ((StackLayout)sender).Parent as ViewCell;
                var numberSet = theViewCell.BindingContext as NumberSet;                              

                switch (action)
                {
                    case "Delete NumberSet":
                        {
                            DeleteNumberSet(numberSet);
                            break;
                        }
                    case "Modify NumberSet":
                        {
                            EditNumberSet(numberSet);
                            break;
                        }
                    case "Shuffle and Pick from NumberSet":
                        {
                            ShuffleAndPickFromNumberSet(numberSet);
                            break;
                        }
                    case "View and Manage Saved Shuffles and Picks":
                        {
                            ViewAndManageShuffles(numberSet);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            private void ViewAndManageShuffles(NumberSet numberSet)
            {
                var numberSetsPage = this.Parent.Parent.Parent.Parent as NumberSetsPage;
                numberSetsPage.ViewAndManageShuffles(numberSet);
            }

            private void DeleteNumberSet(NumberSet numberSet)
            {
                var numberSetsPage = this.Parent.Parent.Parent.Parent as NumberSetsPage;
                bool success = numberSetsPage.DeleteNumberSet(numberSet);
                if (success)
                {
                    numberSetsPage.NumberSetsViewModel.RefreshData();
                }
            }

            private void EditNumberSet(NumberSet numberSet)
            {
                var numberSetsPage = this.Parent.Parent.Parent.Parent as NumberSetsPage;
                numberSetsPage.EditNumberSet(numberSet);              
            }

            private void ShuffleAndPickFromNumberSet(NumberSet numberSet)
            {
                var numberSetsPage = this.Parent.Parent.Parent.Parent as NumberSetsPage;
                numberSetsPage.SelectNumberSet(numberSet);
                numberSetsPage.ShuffleAndPickFromNumberSet(numberSet);
            }

        }

        private async void ViewAndManageShuffles(NumberSet numberSet)
        {
            _shufflePickPage = new ShufflePickPage(numberSet);
            await NavigationManager.PushAsyncPage(Navigation, _shufflePickPage);
        }

        public class CustomNumberSetNumberCell : ViewCell
        {
            public CustomNumberSetNumberCell()
            {
                var numberSetNumberIDLabel = new Label();
                numberSetNumberIDLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNumberID"));

                var numberSetNumberLabel = new Label();
                numberSetNumberLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNumber"));

                var view = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        numberSetNumberIDLabel,
                        numberSetNumberLabel
                    }
                };            

                View = view;
            }

            private void AddTapGesture(View view)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += HandleTap;
                view.GestureRecognizers.Add(tapGestureRecognizer);
            }

            public async void HandleTap(object sender, EventArgs e)
            {
                Debug.WriteLine("Sender: " + sender);
            }

        }

        
        public StackLayout GetNumberSetListViewHeader()
        {           
            var numberSetNameHeadingLabel = new Label();
           numberSetNameHeadingLabel.BindingContext = _numberSetsViewModel;
             numberSetNameHeadingLabel.SetBinding(Label.TextProperty, new Binding("NumberSetNameLabel"));

            var view = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                    {                       
                        numberSetNameHeadingLabel
                    }
            };

            return view;
        }

          private async void ShuffleAndPickFromNumberSet(NumberSet numberSet)
        {
            _numberSetNumbersPage = new NumberSetNumbersPage();
            await NavigationManager.PushAsyncPage(Navigation, _numberSetNumbersPage);
        }


        //protected override bool OnBackButtonPressed()
        //{
        //    // If you want to continue going back
        //    bool result = base.OnBackButtonPressed();
        //    NavigateAwayFromPage();
        //    DisplayAlert("Back button was clicked and i have coded it not to go back for the moment", "Clicked", "Cancel");

        //    // return result;

        //    // If you want to stop the back button
        //    return true;

        //}

        public bool DeleteNumberSet(NumberSet numberSet)
        {
            bool success = false;
            int numberOfRowsDeleted = _numberTumblerBusiness.DeleteNumberSet(numberSet);
            success = numberOfRowsDeleted > 0 ? true : false;
            return success;
        }

        private async void EditNumberSet(NumberSet numberSet)
        {             
            CurrentNumberSet = numberSet;
            _numberSetNumberCounter = 0;
            _numberSetNumbersViewModel.LoadNumberSetNumbersData(numberSet);
            _numberSetNumbersPage = new NumberSetNumbersPage();
            _numberSetNumbersPage.BindingContext = _numberSetNumbersViewModel;
            _numberSetPage = new NumberSetPage(numberSet,editMode:true);            
            _numberSetPage.Title = "Edit Number Set";
            //so that the duplicate name is not an issue when we load it.
            _numberSetPage.NumberSetViewModel.NumberSetNameError = false;
            NumberSetNumberTabbedPage numberSetNumbersTabbedPage = new NumberSetNumberTabbedPage(_numberSetPage, _numberSetNumbersPage);

            await NavigationManager.PushAsyncPage(Navigation, numberSetNumbersTabbedPage);
        }


        //this would be called when the user clicks the back button on the page where toolbaritem back arrow was clicked on from
        //this is an android specific call to this method from mainactivity.cs onoptionsitemselected override method
        public void NavigateAwayFromPage()
        {
            //_lotteriesPage = new LotteriesPage();
            //_numberSetsPage = new NumberSetsPage();
            //_lotteryPage = new LotteryPage();
            //DisplayAlert("NumberSets Page Navigated away from", "", "ok", "cancel");
        }

        public void ReloadViewModel()
        {
            _numberSetsViewModel.RefreshData();
        }
    }
}
