using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NumbrTumblr
{

    public partial class ShufflePickPage : ContentPage, INavigateCommon, IPageViewModelCommon
    {
        private ShufflePickViewModel _shufflePickViewModel = null;
        private NumberSet _numberSet = new NumberSet();
        private Shuffle _currentShuffle;
        private ShuffleNumbersPage _shuffleNumbersPage;
        private ShuffleNumbersViewModel _shuffleNumbersViewModel = new ShuffleNumbersViewModel();
        private ShufflePickPage _shufflePickPage;
        private NumbrTumblr.Business.NumbrTumblrBusiness _numbrTumblrBusiness = new Business.NumbrTumblrBusiness();

        public Shuffle CurrentShuffle
        {
            get
            {
                return _currentShuffle;
            }
            set
            {
                _currentShuffle = value;
            }
        }

        public ShufflePickViewModel ShufflePickViewModel
        {
            get
            {
                return _shufflePickViewModel;
            }

            set
            {
                _shufflePickViewModel = value;
            }
        }

        public ShufflePickPage(NumberSet numberSet)
        {
            InitializeComponent();
            _numberSet = numberSet;
            ShufflePickViewModel = App.Locator.ShufflePickPage;
            ShufflePickViewModel = new ShufflePickViewModel();
            ShufflePickViewModel.NumberSet = numberSet;
            ShufflePickViewModel.RefreshData();
            GenerateContent();
        }

        public ShufflePickPage()
        {
            InitializeComponent();
            ShufflePickViewModel = App.Locator.ShufflePickPage;
            ShufflePickViewModel = new ShufflePickViewModel();
            GenerateContent();
        }

        private void GenerateContent()
        {
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(30, 30, 30, 30)
            };

            ListView shufflesListView = GenerateShufflesListView();

            stackLayout.Children.Add(shufflesListView);
            ScrollView scrollView = new ScrollView();
            scrollView.Orientation = ScrollOrientation.Both;
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private ListView GenerateShufflesListView()
        {
            var shufflesListView = new ListView();
            this.Title = "Shuffles";
            shufflesListView.ItemsSource = ShufflePickViewModel.Shuffles;
            shufflesListView.ItemSelected += ShufflesListView_ItemSelected;

            shufflesListView.ItemTemplate = new DataTemplate(typeof(CustomShuffleCell));
            shufflesListView.Header = GetShuffleListViewHeader();
            //this line here was so important!! or else the rows are never big enough for the content
            shufflesListView.HasUnevenRows = true;
            return shufflesListView;
        }

        public async void ShufflesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //await DisplayAlert("Selected an item in Shuffles listview" + ((Shuffle)e.SelectedItem).ShuffleName, "Item selected event", "Cancel");
            //Debug.WriteLine("Item Selected: " + ((Shuffle)e.SelectedItem).ShuffleName);
            var selectedShuffle = ((Shuffle)e.SelectedItem);
            SelectShuffle(selectedShuffle);
        }

        private void SelectShuffle(Shuffle selectedShuffle)
        {
            CurrentShuffle = selectedShuffle;

            _shuffleNumbersViewModel.LoadShuffleNumbersData(selectedShuffle, _numberSet);
        }

        public class CustomShuffleCell : ViewCell
        {
            /// <summary>
            /// 
            /// </summary>
            public CustomShuffleCell()
            {

                Grid grid = new Grid
                {
                    Padding = new Thickness(30, 30, 30, 30),

                    RowDefinitions = new RowDefinitionCollection
                    {
                    new RowDefinition
                        {

                        },
                    },
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition() {Width=new GridLength(1,GridUnitType.Star) },
                        new ColumnDefinition() {Width=new GridLength(1,GridUnitType.Star) },
                        new ColumnDefinition() {Width=new GridLength(1,GridUnitType.Star) },
                    },
                };
                WrapLayout nameWrapLayout = new WrapLayout();

                var shuffleNameLabel = new Label();
                shuffleNameLabel.SetBinding(Label.TextProperty, new Binding("ShuffleDateTime"));
                 shuffleNameLabel.VerticalOptions = LayoutOptions.StartAndExpand;
                 shuffleNameLabel.HorizontalOptions = LayoutOptions.StartAndExpand;
                nameWrapLayout.Children.Add(shuffleNameLabel);

                WrapLayout shuffleWrapLayout = new WrapLayout();
                var shuffleConcat = new Label();
                shuffleConcat.SetBinding(Label.TextProperty, new Binding("ResultOfShuffleConcatenated"));
                shuffleConcat.VerticalOptions = LayoutOptions.StartAndExpand;
                 shuffleConcat.HorizontalOptions = LayoutOptions.StartAndExpand;
                shuffleConcat.WidthRequest = 200;
                shuffleWrapLayout.WidthRequest = 200;
                shuffleWrapLayout.Children.Add(shuffleConcat);
                

                WrapLayout picksWrapLayout = new WrapLayout();
                var shufflePicks = new Label();
                shufflePicks.SetBinding(Label.TextProperty, new Binding("ResultOfShufflePicksConcatenated"));
                 shufflePicks.VerticalOptions = LayoutOptions.StartAndExpand;
                 shufflePicks.HorizontalOptions = LayoutOptions.StartAndExpand;
                shufflePicks.WidthRequest = 200;
                picksWrapLayout.WidthRequest = 200;
                picksWrapLayout.Children.Add(shufflePicks);

                grid = new Grid();

                grid.Children.Add(nameWrapLayout, 0, 0);
                grid.Children.Add(shuffleWrapLayout, 1, 0);
                grid.Children.Add(picksWrapLayout, 2, 0);

                var view = grid;

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
                var action = await Application.Current.MainPage.DisplayActionSheet("Would you like to?", "Cancel", null, "Delete", "Modify Picks");

                ViewCell theViewCell = ((Grid)sender).Parent as ViewCell;
                var shuffle = theViewCell.BindingContext as Shuffle;

                LoadShuffleNumbers(shuffle);
                switch (action)
                {
                    case "Delete":
                        {
                            DeleteShuffle(shuffle);
                            break;
                        }
                    //case "Modify":
                    //    {
                    //        EditShuffle(shuffle);
                    //        break;
                    //    }
                    case "Modify Picks":
                        {
                            ManageShuffleNumbers(shuffle);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }

            private void LoadShuffleNumbers(Shuffle shuffle)
            {
                var shufflesPage = this.Parent.Parent.Parent.Parent as ShufflePickPage;

                shufflesPage.LoadShuffleWithNumbers(shuffle);


            }

            private void DeleteShuffle(Shuffle shuffle)
            {
                var shufflesPage = this.Parent.Parent.Parent.Parent as ShufflePickPage;
                bool success = shufflesPage.DeleteShuffle(shuffle);
                if (success)
                {
                    shufflesPage.ShufflePickViewModel.RefreshData();
                }
            }

            private void EditShuffle(Shuffle shuffle)
            {
                var shufflesPage = this.Parent.Parent.Parent.Parent as ShufflePickPage;
                shufflesPage.EditShuffle(shuffle);
            }

            private void ManageShuffleNumbers(Shuffle shuffle)
            {
                var shufflesPage = this.Parent.Parent.Parent.Parent as ShufflePickPage;
                shufflesPage.SelectShuffle(shuffle);
                shufflesPage.ManageShuffleNumbers(shuffle);
            }

        }

        private void LoadShuffleWithNumbers(Shuffle shuffle)
        {
            Shuffle data = _numbrTumblrBusiness.GetShuffleByShuffleAndLoadNumbersByShuffleId(shuffle.ShuffleID);
            CurrentShuffle = data;
            _shuffleNumbersViewModel.Shuffle = data;
            _shuffleNumbersViewModel.LoadShuffleNumbersData(data, _shuffleNumbersViewModel.NumberSet);
        }

        public class CustomShuffleNumberCell : ViewCell
        {
            public CustomShuffleNumberCell()
            {
                var shuffleNumberIDLabel = new Label();
                shuffleNumberIDLabel.SetBinding(Label.TextProperty, new Binding("ShuffleNumberID"));

                var shuffleNumberLabel = new Label();
                shuffleNumberLabel.SetBinding(Label.TextProperty, new Binding("ShuffleNumber"));

                var view = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        shuffleNumberIDLabel,
                        shuffleNumberLabel
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

            }
        }

        public Grid GetShuffleListViewHeader()
        {

            Grid grid = new Grid();

            grid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition()
                };

            grid.ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition() { Width= new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition() { Width= new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition() { Width= new GridLength(1,GridUnitType.Star) }
                };

            var shuffleNameHeadingLabel = new Label();
            shuffleNameHeadingLabel.BindingContext = _shufflePickViewModel;
            shuffleNameHeadingLabel.SetBinding(Label.TextProperty, new Binding("ShuffleNameLabel"));
            shuffleNameHeadingLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            shuffleNameHeadingLabel.HorizontalTextAlignment = TextAlignment.Center;


            var shuffleNumbersHeadingLabel = new Label();
            shuffleNumbersHeadingLabel.BindingContext = _shufflePickViewModel;
            shuffleNumbersHeadingLabel.SetBinding(Label.TextProperty, new Binding("ShuffleAllNumbersNameLabel"));
            shuffleNumbersHeadingLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            shuffleNumbersHeadingLabel.HorizontalTextAlignment = TextAlignment.Center;


            var shufflePickedNumbersHeadingLabel = new Label();
            shufflePickedNumbersHeadingLabel.BindingContext = _shufflePickViewModel;
            shufflePickedNumbersHeadingLabel.SetBinding(Label.TextProperty, new Binding("ShufflePickNameLabel"));
            shufflePickedNumbersHeadingLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            shufflePickedNumbersHeadingLabel.HorizontalTextAlignment = TextAlignment.Center;

            grid = new Grid();

            grid.Children.Add(shuffleNameHeadingLabel, 0, 0);
            grid.Children.Add(shuffleNumbersHeadingLabel, 1, 0);
            grid.Children.Add(shufflePickedNumbersHeadingLabel, 2, 0);

            var view = grid;
            return view;
        }

        private async void ManageShuffleNumbers(Shuffle shuffle)
        {
            _shuffleNumbersPage = new ShuffleNumbersPage(shuffle, _numberSet);
            await NavigationManager.PushAsyncPage(Navigation, _shuffleNumbersPage);
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

        public bool DeleteShuffle(Shuffle shuffle)
        {
            bool success = false;
            int numberOfRowsDeleted = _numbrTumblrBusiness.DeleteShuffle(shuffle);
            success = numberOfRowsDeleted > 0 ? true : false;
            return success;
        }

        private async void EditShuffle(Shuffle shuffle)
        {
            //CurrentShuffle = shuffle;
            ////_shuffleNumbersViewModel.SetShuffle(CurrentShuffle);

            ////_shuffleNumbersViewModel.SetShuffle(selectedShuffle);
            //_shuffleNumbersViewModel.LoadShuffleNumbersData(shuffle);
            //_shuffleNumbersPage = new ShuffleNumbersPage();
            //_shuffleNumbersPage.BindingContext = _shuffleNumbersViewModel;
            ////_shufflePickPage = new ShufflePickPage(shuffle,editMode:true);            
            ////_shufflePickPage.Title = "Edit Shuffle";
            //////so that the duplicate name is not an issue when we load it.
            ////_shufflePickPage.ShufflePickViewModel.ShuffleNameError = false;
            ////ShuffleNumberTabbedPage shuffleNumbersTabbedPage = new ShuffleNumberTabbedPage(_shufflePage, _shuffleNumbersPage);

            //await NavigationManager.PushAsyncPage(Navigation, _shuffleNumbersPage);
        }


        //this would be called when the user clicks the back button on the page where toolbaritem back arrow was clicked on from
        //this is an android specific call to this method from mainactivity.cs onoptionsitemselected override method
        public void NavigateAwayFromPage()
        {
            //_lotteriesPage = new LotteriesPage();
            //_shufflesPage = new ShufflesPage();
            //_lotteryPage = new LotteryPage();
            //DisplayAlert("Shuffles Page Navigated away from", "", "ok", "cancel");
        }

        public void ReloadViewModel()
        {
            ShufflePickViewModel.RefreshData();
        }
    }
}
