using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.HelpersAndExtensionMethods;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using Xamarin.Forms;

namespace NumbrTumblr
{
    public partial class LotteriesPage : ContentPage, INavigateCommon, IPageViewModelCommon
    {
        private LotteriesViewModel _lotteriesViewModel;
        private int _lotteryCounter = 0;
        private LotteryPage _lotteryPage;

        public RangeObservableCollection<Lottery> Lotteries { get; set; }
        public Business.NumbrTumblrBusiness _numberTumblerBusiness = new Business.NumbrTumblrBusiness();

        public LotteriesViewModel LotteriesViewModel
        {
            get { return _lotteriesViewModel; }
            set { _lotteriesViewModel = value; }
        }

        public LotteriesPage()
        {
            InitializeComponent();
            _lotteriesViewModel = App.Locator.LotteriesPage;
            Lotteries = _lotteriesViewModel.Lotteries;

            GenerateContent();
        }

        private void GenerateContent()
        {
             StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Vertical,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Padding=new Thickness(30,30,30,30) };

            ScrollView scrollView = new ScrollView();
            scrollView.Orientation = ScrollOrientation.Both;

            ListView lotteriesListView = GenerateLotteriesListView();

            stackLayout.Children.Add(lotteriesListView);
          
            scrollView.Content = stackLayout;
            Content = scrollView;
        }

        private ListView GenerateLotteriesListView()
        {          
            var lotteriesListView = new ListView();           
            this.Title = "Lotteries";
            lotteriesListView.ItemsSource = _lotteriesViewModel.Lotteries;          
            lotteriesListView.ItemTemplate = new DataTemplate(typeof(CustomLotteryCell));
            lotteriesListView.Header = GetLotteryListViewHeader();

            return lotteriesListView;
        }           
        

        public StackLayout GetLotteryListViewHeader()
        {         
            var lotteryNameHeadingLabel = new Label();
            lotteryNameHeadingLabel.BindingContext = _lotteriesViewModel;
             lotteryNameHeadingLabel.SetBinding(Label.TextProperty, new Binding("LotteryNameLabel"));

            var view = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                    {                        
                        lotteryNameHeadingLabel
                    }
            };
            return view;
        }


        public class CustomLotteryCell : ViewCell
        {
            public CustomLotteryCell()
            {
                var lotteryNameLabel = new Label();
                lotteryNameLabel.SetBinding(Label.TextProperty, new Binding("LotteryName"));

                var view = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {                       
                        lotteryNameLabel
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
                var action = await Application.Current.MainPage.DisplayActionSheet("Would you like to?", "Cancel", null, "Delete", "Modify");

                ViewCell theViewCell = ((StackLayout)sender).Parent as ViewCell;
                var lottery = theViewCell.BindingContext as Lottery;
                
                switch (action)
                {
                    case "Delete":
                        {                            
                            DeleteLottery(lottery);
                            break;
                        }
                    case "Modify":
                        {
                            EditLottery(lottery);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }               
            }

            private void DeleteLottery(Lottery lottery)
            {
                var lotteriesPage = this.Parent.Parent.Parent.Parent as LotteriesPage;                
                bool success = lotteriesPage.DeleteLottery(lottery);
                if(success)
                {
                    lotteriesPage.LotteriesViewModel.RefreshData();                    
                }
            }

            private void EditLottery(Lottery lottery)
            {
                var lotteriesPage = this.Parent.Parent.Parent.Parent as LotteriesPage;
                lotteriesPage.EditLottery(lottery);
            }
        }       

        private void SelectCurrentLotteryItem(Lottery selectedLottery)
        {
            _lotteriesViewModel.SelectedLottery = selectedLottery;
        }

        public bool DeleteLottery(Lottery lottery)
        {
            bool success = false;
            int numberOfRowsDeleted = _numberTumblerBusiness.DeleteLottery(lottery);
            success = numberOfRowsDeleted > 0 ? true : false;
            return success;
        }

        private async void EditLottery(Lottery lottery)
        {
            SelectCurrentLotteryItem(lottery);
           
            _lotteryPage = new LotteryPage(lottery, editMode: true);
            _lotteryPage.Title = "Edit Lottery";
            //so that the duplicate name is not an issue when we load it.
            _lotteryPage.LotteryViewModel.LotteryNameError = false;
           
            await NavigationManager.PushAsyncPage(Navigation, _lotteryPage);
        }      

        //this would be called when the user clicks the back button on the page where toolbaritem back arrow was clicked on from
        //this is an android specific call to this method from mainactivity.cs onoptionsitemselected override method
        public void NavigateAwayFromPage()
        {
            //_lotteriesPage = new LotteriesPage();
            //_numberSetsPage = new NumberSetsPage();
            //_lotteryPage = new LotteryPage();
            //DisplayAlert("Lotteries Page Navigated away from", "", "ok", "cancel");
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

        public void ReloadViewModel()
        {
            _lotteriesViewModel.RefreshData();
        }
    }
}
