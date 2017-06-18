using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace NumbrTumblr.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<LotteriesViewModel>();
            SimpleIoc.Default.Register<NumberSetsViewModel>();
            SimpleIoc.Default.Register<NumberSetNumbersViewModel>();
            SimpleIoc.Default.Register<LotteryViewModel>();
            SimpleIoc.Default.Register<NumberSetViewModel>();
            SimpleIoc.Default.Register<StateProvincesViewModel>();
            SimpleIoc.Default.Register<ShufflePickViewModel>();
            SimpleIoc.Default.Register<ShuffleNumbersViewModel>();
        }

        /// <summary>
        /// Gets the MainPage property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainPageViewModel MainPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPageViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LotteriesViewModel LotteriesPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LotteriesViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public NumberSetsViewModel NumberSetsPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NumberSetsViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public NumberSetNumbersViewModel NumberSetNumbersPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NumberSetNumbersViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public LotteryViewModel LotteryPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LotteryViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public StateProvincesViewModel StateProvincePage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StateProvincesViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public NumberSetViewModel NumberSetPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NumberSetViewModel>();
            }
        }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public ShufflePickViewModel ShufflePickPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShufflePickViewModel>();
            }
        }

          [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public ShuffleNumbersViewModel ShuffleNumbersPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShuffleNumbersViewModel>();
            }
        }
    }
}
