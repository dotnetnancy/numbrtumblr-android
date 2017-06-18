using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System;
using Xamarin.Forms;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using NumbrTumblr.Data.Files;
using Newtonsoft.Json;


namespace NumbrTumblr.HelpersAndExtensionMethods
{
    public class EmbeddedResourceManager
    {

        //GetManifestResourceStream is used to access the embedded file using its Resource ID.
        //By default the resource ID is the filename prefixed with the default namespace for the project 
        //it is embedded in - in this case the assembly is WorkingWithFiles and the filename is 
        //PCLTextResource.txt, so the resource ID is WorkingWithFiles.PCLTextResource.txt.
        public string ReadEmbeddedResourceFile()
        {
            var assembly = typeof (LoadResourceText).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("WorkingWithFiles.PCLTextResource.txt");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
}

