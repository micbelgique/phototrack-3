using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Trasys.PhotoTrack.Mobile.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Trasys.PhotoTrack.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : PageBase
    {
        private bool _mapLoaded = false;


        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.DataContext = ViewModelLocator.Sites;
            ViewModelLocator.Sites.Groups[1].Items.CollectionChanged += Items_CollectionChanged;
        }


        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            try
            {                
                wvMaps.Navigate(new Uri("#URL_GOOGLE#"));
            }
            catch (Exception)
            {
            }
        }
        
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SiteItemViewModel model = e.ClickedItem as SiteItemViewModel;
            this.Navigation.NavigateTo<DetailPage>(model.SiteID);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Navigation.NavigateTo<View.SearchPage>();
        }

        private async void btnAddSite_Click(object sender, RoutedEventArgs e)
        {

            this.Navigation.NavigateTo<View.DetailPage>((await this.ViewModelLocator.Sites.CreateNewSite()).SiteID);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private async void wvMaps_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _mapLoaded = true;
            SiteViewModel model = this.DataContext as SiteViewModel;
            foreach (SiteItemViewModel item in model.Groups[2].Items)
            {
                if(!string.IsNullOrEmpty(item.Address))
                    await wvMaps.InvokeScriptAsync("setMarker", new string[] { item.Address, item.Number, item.Description });
            }
        }

        private void wvMaps_ScriptNotify(object sender, NotifyEventArgs e)
        {

        }

        private void wvMaps_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {

        }

        async void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_mapLoaded)
            {
                await wvMaps.InvokeScriptAsync("clearMarkers", new string[] { });
                SiteViewModel model = this.DataContext as SiteViewModel;
                foreach (SiteItemViewModel item in model.Groups[2].Items)
                {
                    if (!string.IsNullOrEmpty(item.Address))
                        await wvMaps.InvokeScriptAsync("setMarker", new string[] { item.Address, item.Number, item.Description });
                }
            }
        }
    }
}
