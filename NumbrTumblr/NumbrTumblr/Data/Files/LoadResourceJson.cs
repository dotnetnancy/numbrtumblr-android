using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Data.Entities;
using NumbrTumblr.Data.Files.FileEntities;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NumbrTumblr.Data.Files
{
    public class LoadResourceJson : BaseClasses.BaseContentPage
    {
        public LoadResourceJson()
        {
            #region How to load an Json file embedded resource
            var assembly = typeof(LoadResourceText).GetTypeInfo().Assembly;
            //to get the resourceid it is the namespace prepending the filename
            Stream stream = assembly.GetManifestResourceStream("NumbrTumblr.Data.Files.countries.json");

            //Country[] countries;
            List< NumbrTumblr.Data.Files.FileEntities.Country.Country> countries;
            //Dictionary<string, string> countries = new Dictionary<string, string>();

            using (var reader = new System.IO.StreamReader(stream))
            {

                var json = reader.ReadToEnd();
                var rootobject = JsonConvert.DeserializeObject<NumbrTumblr.Data.Files.FileEntities.Country.RootObject>(json);
                //countries = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                countries = rootobject.Countries;
            }
            #endregion

            var listView = new ListView();
            listView.ItemsSource = countries.Select(x=> x.name);
            //listView.ItemsSource = countries;

            Content = new StackLayout
            {
                Padding = new Thickness(0, 20, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    new Label { Text = "Embedded Resource JSON File (PCL)",
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold
                    }, listView
                }
            };

            // NOTE: use for debugging, not in released app code!
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);
        }
    }
}
    
