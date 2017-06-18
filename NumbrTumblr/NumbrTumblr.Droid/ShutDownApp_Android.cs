using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NumbrTumblr.Interfaces;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly:Xamarin.Forms.Dependency(typeof(NumbrTumblr.Droid.ShutDownApp_Android))]
namespace NumbrTumblr.Droid
{
    public class ShutDownApp_Android : IShutdownApp
    {
        public void Close()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}