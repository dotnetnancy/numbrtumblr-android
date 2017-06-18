using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Interfaces;
using Xamarin.Forms;

namespace NumbrTumblr.HelpersAndExtensionMethods
{
    public class BehaviorValidatedInput<T> where T : InputView
    {
        public T Element { get; set; }
        public bool EditMode { get; set; }

    public bool IsValid
    {
        get
        {
            foreach (var b in Element.Behaviors)
            {
                if (b is IValidatorBehavior && !((IValidatorBehavior)b).IsValid)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
}
