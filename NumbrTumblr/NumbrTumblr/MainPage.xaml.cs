using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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
    public partial class MainPage : ContentPage, INavigateCommon, IPageViewModelCommon
    {
        private MainPageViewModel _mainPageViewModel;
        public RangeObservableCollection<NumberSetNumber> NumberSetNumbers { get; set; }

        private NumberSetsPage _numberSetsPage = new NumberSetsPage();
        private LotteriesPage _lotteriesPage = new LotteriesPage();
        private LotteryPage _lotteryPage;
        private NumberSetPage _numberSetPage;

        public LotteriesPage LotteriesPage
        {
            get
            {
                return _lotteriesPage;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            Title = "NumbrTumblr Main";
            _mainPageViewModel = App.Locator.MainPage;

            var newLotteryToolBarItem = new ToolbarItem
            {
                Text = "New Lottery",
            };
            newLotteryToolBarItem.Clicked += NewLotteryToolBarItem_Clicked;

            ToolbarItems.Add(newLotteryToolBarItem);

            var newNumberSetToolBarItem = new ToolbarItem
            {
                Text = "New NumberSet",
            };
            newNumberSetToolBarItem.Clicked += NewNumberSetToolBarItem_Clicked;

            ToolbarItems.Add(newNumberSetToolBarItem);

            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Start
            };

          
            Button manageLotteriesButton = new Button
            {
                Text = "Lotteries",
                HorizontalOptions = LayoutOptions.Start                
            };
            manageLotteriesButton.Clicked += ManageLotteriesButton_Clicked;

            Button manageNumerSetsButton = new Button()
            {
                Text = "NumberSets",
                HorizontalOptions = LayoutOptions.Start
            };
            manageNumerSetsButton.Clicked += ManageNumerSetsButton_Clicked;

            Button exitAppButton = new Button()
            {
                Text = "Exit NumbrTumblr",
                HorizontalOptions = LayoutOptions.Start
            };

            exitAppButton.Clicked += ExitAppButton_Clicked;

            stackLayout.Children.Add(manageLotteriesButton);
            stackLayout.Children.Add(manageNumerSetsButton);
            stackLayout.Children.Add(exitAppButton);

            Content = stackLayout;
        }

        private void ExitAppButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IShutdownApp>().Close();
        }

        //this could be called when the user clicks the back button on the page where toolbaritem back arrow was clicked on from
        //this is an android specific call to this method from mainactivity.cs onoptionsitemselected override method
        public void NavigateAwayFromPage()
        {
            _lotteriesPage = new LotteriesPage();
            _numberSetsPage = new NumberSetsPage();            
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

        private async void NewLotteryToolBarItem_Clicked(object sender, EventArgs e)
        {           
            _lotteryPage = new LotteryPage(new Lottery(),false);
            await NavigationManager.PushAsyncPage(Navigation, _lotteryPage);
        }

        private async void NewNumberSetToolBarItem_Clicked(object sender, EventArgs e)
        {           
            _numberSetPage = new NumberSetPage(new NumberSet(),false);
            await NavigationManager.PushAsyncPage(Navigation, _numberSetPage);
        }

        private async void ManageNumerSetsButton_Clicked(object sender, EventArgs e)
        {
            await NavigationManager.PushAsyncPage(Navigation, _numberSetsPage);
        }
        private async void ManageLotteriesButton_Clicked(object sender, EventArgs e)
        {
            await NavigationManager.PushAsyncPage(Navigation, _lotteriesPage);
        }

        public async void ReloadViewModel()
        {
            if (_lotteriesPage != null)
            {
                _lotteriesPage.ReloadViewModel();
            }
            if (_lotteryPage != null)
            {
                _lotteryPage.ReloadViewModel();
            }
            if (_numberSetsPage != null)
            {
                _numberSetsPage.ReloadViewModel();
            }
            if (_numberSetPage != null)
            {
                _numberSetPage.ReloadViewModel();
            }
        }
    }
}
