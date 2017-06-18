using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace LotteryBuddy.Droid
{
    [Activity(Label = "SplashActivity",
     MainLauncher = true,//Set it as boot activity, set this to true and main activity to false to have this splash screen
     Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
     NoHistory = true,//Doesn't place it in back stack
     ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
     WindowSoftInputMode = SoftInput.AdjustPan)] 
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(500); //Let's wait awhile...
            this.StartActivity(typeof(MainActivity));

            // Create your application here
        }
    }
}