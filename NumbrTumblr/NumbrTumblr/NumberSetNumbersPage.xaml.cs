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
    public partial class NumberSetNumbersPage : ContentPage, IPageViewModelCommon, INavigateCommon
    {
        private static NumberSetNumbersViewModel _numberSetNumbersViewModel;
        private static ShuffleNumbersPage _shuffleNumbersPage;

        public NumberSetNumbersPage()
        {

            InitializeComponent();
            _numberSetNumbersViewModel = App.Locator.NumberSetNumbersPage;
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

            var wrapLayout = GenerateNumberSetNumberWrapLayout();
            stackLayout.Children.Add(wrapLayout);
           
            Button shuffleSelectedNumbersButton = new Button()
            {
                Text = "Shuffle Selected Numbers In This NumberSet"
            };

            shuffleSelectedNumbersButton.Clicked += ShuffleSelectedNumbersButton_Clicked;

            Button shuffleAllNumbersInNumberSetButton = new Button()
            {
                Text = "Shuffle All Numbers In This NumberSet"
            };

            shuffleAllNumbersInNumberSetButton.Clicked += ShuffleAllNumbersInNumberSet_Clicked;

            stackLayout.Children.Add(shuffleSelectedNumbersButton);
            stackLayout.Children.Add(shuffleAllNumbersInNumberSetButton);
            scrollView.Content = stackLayout;
            Content = scrollView;
        }

        private async void ShuffleAllNumbersInNumberSet_Clicked(object sender, EventArgs e)
        {
            NumberSet numberSet = _numberSetNumbersViewModel.NumberSet;
            numberSet.NumberSetNumbers = _numberSetNumbersViewModel.NumberSetNumbers.ToList();
            _shuffleNumbersPage = new ShuffleNumbersPage(numberSet);            
            await NavigationManager.PushAsyncPage(Navigation, _shuffleNumbersPage);
        }

        private async void ShuffleSelectedNumbersButton_Clicked(object sender, EventArgs e)
        {
             NumberSet numberSet = _numberSetNumbersViewModel.NumberSet;
            numberSet.NumberSetNumbers = _numberSetNumbersViewModel.NumberSetNumbers.Where(x => x.SelectedNumber).ToList();
             _shuffleNumbersPage = new ShuffleNumbersPage(numberSet);
            await NavigationManager.PushAsyncPage(Navigation, _shuffleNumbersPage);
        }

        private ListView GenerateNumberSetNumberListView()
        {
            var numberSetNumbersListView = new ListView();
            this.Title = "NumberSet Numbers";
            numberSetNumbersListView.ItemsSource = _numberSetNumbersViewModel.NumberSetNumbers;
            numberSetNumbersListView.ItemSelected += NumberSetNumbersListView_ItemSelected;
            numberSetNumbersListView.ItemTemplate = new DataTemplate(typeof(CustomNumberSetNumberCell));
            numberSetNumbersListView.Header = GetNumberSetNumberListViewHeader();
            return numberSetNumbersListView;
        }

        private WrapLayout GenerateNumberSetNumberWrapLayout()
        {
            var numberSetNumbersWrapLayout = new WrapLayout()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Padding = new Thickness(0, 0, 0, 0)
            };
            this.Title = "NumberSet Numbers";
            numberSetNumbersWrapLayout.ItemTemplate = new DataTemplate(typeof(CustomNumberSetNumberCell));
            numberSetNumbersWrapLayout.ItemsSource = _numberSetNumbersViewModel.NumberSetNumbers.ToList();           
            return numberSetNumbersWrapLayout;
        }

        public StackLayout GetNumberSetNumberListViewHeader()
        {
            var numberSetNumberIDHeadingLabel = new Label();
            numberSetNumberIDHeadingLabel.Text = "NumberSet Number ID";

            var numberSetNumberHeadingLabel = new Label();
            numberSetNumberHeadingLabel.Text = "NumberSet Number";

            var view = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Children =
                    {
                        numberSetNumberIDHeadingLabel,
                        numberSetNumberHeadingLabel
                    }
            };

            return view;
        }


        private void NumberSetNumbersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedNumberSetNumber = ((NumberSetNumber)e.SelectedItem);
            Debug.WriteLine("Item Selected: " + ((NumberSetNumber)e.SelectedItem).Number.ToString());

        }

        public void ReloadViewModel()
        {
            _numberSetNumbersViewModel.RefreshData(_numberSetNumbersViewModel.NumberSet);
        }

        public void NavigateAwayFromPage()
        {
            //DisplayAlert("NumberSet Numbers Page Navigated away from", "", "ok", "cancel");
        }

        public class CustomNumberSetNumberCell : ViewCell
        {
            public CustomNumberSetNumberCell()
            {
                var numberSetNumberLabel = new Label()
                {
                };
                numberSetNumberLabel.SetBinding(Label.TextProperty, new Binding("Number", BindingMode.TwoWay, new IntegerConverter(), null));

                Switch switcher = new Switch { };
                switcher.SetBinding(Switch.IsToggledProperty, new Binding("SelectedNumber", BindingMode.TwoWay, new BooleanConverter(), null));
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
                    _numberSetNumbersViewModel.AddNumberToNumberSet(Convert.ToInt32(((Switch)sender).ClassId));
                }
                else
                {
                    _numberSetNumbersViewModel.RemoveNumberFromNumberSet(Convert.ToInt32(((Switch)sender).ClassId));
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
