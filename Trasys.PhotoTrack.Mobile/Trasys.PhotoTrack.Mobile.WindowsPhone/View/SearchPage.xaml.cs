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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Trasys.PhotoTrack.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : PageBase
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = ViewModelLocator.Sites;
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SiteViewModel model = this.DataContext as SiteViewModel;
            model.Search(((TextBox)sender).Text);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SiteItemViewModel model = e.ClickedItem as SiteItemViewModel;
            this.Navigation.NavigateTo<DetailPage>(model.SiteID);
        }
    }
}
