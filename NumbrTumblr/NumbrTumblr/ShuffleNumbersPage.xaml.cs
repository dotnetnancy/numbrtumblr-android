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
using NumbrTumblr.Business;

namespace NumbrTumblr
{
    public partial class ShuffleNumbersPage : ContentPage, IPageViewModelCommon, INavigateCommon
    {
        private static ShuffleNumbersViewModel _shuffleNumbersViewModel;
        private NumbrTumblrBusiness _numbrTumblrBusiness = new NumbrTumblrBusiness();

        public void LoadShuffleNumbersData(NumberSet numberSet, bool shuffle = true)
        {
            _shuffleNumbersViewModel.LoadNumberSetNumbersData(numberSet, shuffle);
        }

        public ShuffleNumbersPage(NumberSet numberSet, bool shuffle = true)
        {
            InitializeComponent();
            _shuffleNumbersViewModel = App.Locator.ShuffleNumbersPage;
            _shuffleNumbersViewModel.LoadNumberSetNumbersData(numberSet, shuffle);
            GenerateContent();
        }
        public ShuffleNumbersPage(Shuffle shuffle, NumberSet numberSet)
        {
            InitializeComponent();
            _shuffleNumbersViewModel = App.Locator.ShuffleNumbersPage;
            _shuffleNumbersViewModel.LoadShuffleNumbersData(shuffle, numberSet);
            GenerateContent();
        }

        private void GenerateContent()
        {
            ScrollView scrollView = new ScrollView();
            StackLayout stackLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                 Padding=new Thickness(30,30,30,30) 
            };

            var wrapLayout = GenerateShuffleNumberWrapLayout();
            stackLayout.Children.Add(wrapLayout);

            Button saveShuffleAndPicks = new Button()
            {
                Text = "Save Shuffle and Picks"
            };

            saveShuffleAndPicks.Clicked += SaveShuffleAndPicks_Clicked;

            stackLayout.Children.Add(saveShuffleAndPicks);
            scrollView.Content = stackLayout;
            Content = scrollView;
        }

        private async void SaveShuffleAndPicks_Clicked(object sender, EventArgs e)
        {
            SaveShuffleAndPicks();
            //await NavigationManager.RefreshDataNavigationStack(App.Current.MainPage.Navigation);

            await NavigationManager.PopAsyncToRoot(App.Current.MainPage.Navigation);
            await NavigationManager.PushAsyncPage(App.Current.MainPage.Navigation, new ShufflePickPage(_shuffleNumbersViewModel.Shuffle.NumberSet));
        }

        private void SaveShuffleAndPicks()
        {
            Shuffle shuffle = _shuffleNumbersViewModel.Shuffle;
            if (shuffle == null || shuffle.ShuffleID <= 0)
            {
                shuffle = new Shuffle();
                shuffle.NumberSet = _shuffleNumbersViewModel.NumberSet;
                shuffle.ResultOfShuffle = _shuffleNumbersViewModel.ShuffleNumbers.ToList();

            }
            _shuffleNumbersViewModel.Shuffle = _numbrTumblrBusiness.SaveShuffle(shuffle);
        }

        private async void ShuffleAllNumbersInNumberSet_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Shuffle All Numbers in NumberSet Button Clicked", "", "OK", "Cancel");
        }

        private async void ShuffleSelectedNumbersButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Shuffle Selected Numbers Button Clicked", "", "OK", "Cancel");
        }

        private async void AddShuffleNumberButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Selected Add Number Set Number Button", "", "OK", "Cancel");
        }

        private WrapLayout GenerateShuffleNumberWrapLayout()
        {
            var numberSetNumbersWrapLayout = new WrapLayout()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Padding = new Thickness(0, 0, 0, 0)
            };
            this.Title = "Shuffle Numbers";
            numberSetNumbersWrapLayout.ItemTemplate = new DataTemplate(typeof(CustomShuffleNumberCell));
            numberSetNumbersWrapLayout.ItemsSource = _shuffleNumbersViewModel.ShuffleNumbers.ToList();

            return numberSetNumbersWrapLayout;
        }

        private void ShuffleNumbersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedShuffleNumber = ((ShuffleNumber)e.SelectedItem);
            Debug.WriteLine("Item Selected: " + ((ShuffleNumber)e.SelectedItem).Number.ToString());

        }

        public void ReloadViewModel()
        {
            _shuffleNumbersViewModel.RefreshData(_shuffleNumbersViewModel.Shuffle, _shuffleNumbersViewModel.NumberSet);
        }

        public void NavigateAwayFromPage()
        {
            //DisplayAlert("NumberSet Numbers Page Navigated away from", "", "ok", "cancel");
        }

        public class CustomShuffleNumberCell : ViewCell
        {

            public CustomShuffleNumberCell()
            {

                var numberSetNumberLabel = new Label()
                {
                };
                numberSetNumberLabel.SetBinding(Label.TextProperty, new Binding("Number", BindingMode.TwoWay, new IntegerConverter(), null));

                Switch switcher = new Switch { };
                switcher.SetBinding(Switch.IsToggledProperty, new Binding("Picked", BindingMode.TwoWay, new BooleanConverter(), null));
                switcher.SetBinding(Switch.ClassIdProperty, new Binding("Number", BindingMode.TwoWay, new IntegerConverter(), null));

                switcher.Toggled += switcher_Toggled;


                var view = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    WidthRequest = 80,
                    VerticalOptions = LayoutOptions.End,

                    Children =
                    {
                        numberSetNumberLabel,
                        switcher
                    }
                };
              
                View = view;
            }

            public async void switcher_Toggled(object sender, ToggledEventArgs e)
            {
                if (e.Value)
                {
                    _shuffleNumbersViewModel.AddShuffleNumberToShufflePick(Convert.ToInt32(((Switch)sender).ClassId));
                }
                else
                {
                    _shuffleNumbersViewModel.RemoveShuffleNumberFromShufflePick(Convert.ToInt32(((Switch)sender).ClassId));
                }
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
    }
}
