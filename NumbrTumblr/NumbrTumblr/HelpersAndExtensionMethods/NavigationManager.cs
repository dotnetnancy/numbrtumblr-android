using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumbrTumblr.Interfaces;
using Xamarin.Forms;

namespace NumbrTumblr.HelpersAndExtensionMethods
{
    public static class NavigationManager
    {
        public static object _lockObject = new object();
        public static async Task PushAsyncPage(INavigation navigation, Page page)
        {
            await navigation.PushAsync(page);

        }

        public static async Task<Page> PopAsyncPage(INavigation navigation, Page page)
        {
            return await navigation.PopAsync();
        }

        public static async Task PopAsyncToRoot(INavigation navigation)
        {
            await navigation.PopToRootAsync();
        }

        public static void Remove(INavigation navigation, Page pageToRemove)
        {
            lock (_lockObject)
            {
                navigation.RemovePage(pageToRemove);
            }
        }

        public static async Task RemoveAndPush(INavigation navigation, Page pageToRemove, Page pageToPush)
        {
            lock (_lockObject)
            {
                navigation.RemovePage(pageToRemove);
            }
            await navigation.PushAsync(pageToPush);
        }

        //public static async Task ReloadNavigationStack(INavigation navigation)
        //{
        //    if (navigation.NavigationStack.Count > 0)
        //    {
        //        for (int i = 0; i < navigation.NavigationStack.Count; i++)
        //        {
        //            var currentPage = navigation.NavigationStack[i];
        //            if (currentPage is IViewModelCommon)
        //            {
        //                var page = currentPage as IViewModelCommon;
        //                page.ReloadViewModel();
        //            }
        //        }
        //    }
        //}

        public static async Task RefreshDataNavigationStack(INavigation navigation)
        {
            if (navigation.NavigationStack.Count > 0)
            {
                for (int i = 0; i < navigation.NavigationStack.Count; i++)
                {
                    var currentPage = navigation.NavigationStack[i];
                    if (currentPage is IPageViewModelCommon)
                    {
                        var pageToRefresh = currentPage as IPageViewModelCommon;
                        pageToRefresh.ReloadViewModel();
                    }
                }
            }
        }

        public static async Task RefreshDataForPage(INavigation navigation, Page page)
        {
            if (page is IPageViewModelCommon)
            {
                var pageToRefresh = page as IPageViewModelCommon;
                pageToRefresh.ReloadViewModel();
            }
        }
    }
}
