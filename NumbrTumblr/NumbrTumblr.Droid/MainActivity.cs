using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using NumbrTumblr.Interfaces;
using NumbrTumblr.ViewModel;
using Xamarin.Forms;

namespace NumbrTumblr.Droid
{
    [Activity(Label = "NumbrTumblr", Icon = "@drawable/icon", MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {

        //To handle the navigationbar back click i did this on android(in my MainActivity in android project):
        //this will call the OnBackButtonPressed code in the Page were the navigationbar back button was clicked.
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (Android.Resource.Id.Home != item.ItemId) return false;

            var currentApp = App.Current;
            var navPage = currentApp.MainPage;


            if (currentApp != null && navPage.Navigation.NavigationStack.Count > 0)
            {
                int index = navPage.Navigation.NavigationStack.Count - 1;

                var currentPage = navPage.Navigation.NavigationStack[index];

                //var vm = currentPage.BindingContext as MainPageViewModel;

                //if (vm != null)
                //{
                //    var answer = vm.OnBackButtonPressed();
                //    if (answer)
                //        return true;
                //}
                if (currentPage is INavigateCommon)
                {
                    var page = currentPage as INavigateCommon;
                    page.NavigateAwayFromPage();
                }
            }

            return base.OnOptionsItemSelected(item);


            //OnBackPressed();

            //return true;

        }



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

