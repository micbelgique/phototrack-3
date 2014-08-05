using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Activation;
using Trasys.PhotoTrack.Mobile.Dev.Apps.Tools.Apps;
using Trasys.PhotoTrack.Mobile.ViewModel;
using Trasys.PhotoTrack.Mobile.Model;
using Windows.UI.Xaml.Controls.Maps;
using Trasys.PhotoTrack.Mobile.Dev.Apps.Tools.Converter;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Trasys.PhotoTrack.Mobile.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : PageBase, IFileOpenPickerContinuable
    {

        private PhotoViewModel _currentPhoto = null;
        private Windows.Media.Capture.MediaCaptureInitializationSettings _captureInitSettings = null;

        private bool _mapLoaded = false;

        public DetailPage()
        {
            this.InitializeComponent();
            Loaded += DetailPage_Loaded;
        }

        void DetailPage_Loaded(object sender, RoutedEventArgs e)
        {

            SiteItemViewModel model = this.DataContext as SiteItemViewModel;
            //ucMap.Children.Add(new MapIcon() { Location = model.Points[0], NormalizedAnchorPoint = new Point(0.5, 1.0), Title = "test" });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter==null)
            {
                this.DataContext = await ViewModelLocator.Sites.CreateNewSite();
            }
            else
                this.DataContext = await ViewModelLocator.Sites.GetSite((int)e.Parameter);
            ((SiteItemViewModel)this.DataContext).LoadDetailsAsync();
            ((SiteItemViewModel)this.DataContext).PropertyChanged += DetailPage_PropertyChanged;
            try
            {
                wvMaps.Navigate(new Uri("#GoogleMap_URL#"));    // Set the URL to GoogleMap html page.
            }
            catch (Exception)
            {
            }
        }

        async void DetailPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_mapLoaded)
            {
                SiteItemViewModel model = this.DataContext as SiteItemViewModel;
                if (!string.IsNullOrEmpty(model.Address))
                    await wvMaps.InvokeScriptAsync("setMarker", new string[] { model.Address, string.IsNullOrEmpty(model.Number) ? string.Empty : model.Number, string.IsNullOrEmpty(model.Description) ? string.Empty : model.Description });
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            AppBarButton button = sender as AppBarButton;
            button.Flyout.ShowAt(button);
        }

        private async void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.ContinuationData["Operation"] = "AddPhotoToSite";
                // Launch file open picker and caller app is suspended and may be terminated if required 
                openPicker.PickSingleFileAndContinue();
                //var storageFile = await openPicker.PickSingleFileAsync();
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(ex.Message , "Error");
                dialog.ShowAsync();
            }
        }


        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {
                //var stream = await args.Files[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
                //var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                //await bitmapImage.SetSourceAsync(stream);

                SiteItemViewModel model = this.DataContext as SiteItemViewModel;
                model.AddNewPhotoAsync(args.Files[0]);
            }
            else
            {

            }
            App.ContinuationManager.MarkAsStale();
        }

        private void mnuRemove_Click(object sender, RoutedEventArgs e)
        {
            SiteItemViewModel model = this.DataContext as SiteItemViewModel;
            model.Photos.Remove(_currentPhoto);
        }

        private void mnuClycle_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentPhoto.Tag)
            {
                case Configuration.PhotoStateBefore:
                    _currentPhoto.Tag = Configuration.PhotoStateDuring;
                    break;
                case Configuration.PhotoStateDuring:
                    _currentPhoto.Tag = Configuration.PhotoStateAfter;
                    break;
                default:
                    _currentPhoto.Tag = Configuration.PhotoStateBefore;
                    break;
            }
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            _currentPhoto = senderElement.DataContext as PhotoViewModel;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
            SiteItemViewModel model = this.DataContext as SiteItemViewModel;
            model.IsFavorite = !model.IsFavorite;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SiteItemViewModel model = this.DataContext as SiteItemViewModel;
            this.Navigation.NavigateTo<View.EditDetailPage>(model);
        }

        private async void wvMaps_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _mapLoaded = true;
            SiteItemViewModel model = this.DataContext as SiteItemViewModel;
            if (!string.IsNullOrEmpty(model.Address))
                await wvMaps.InvokeScriptAsync("setMarker", new string[] { model.Address, string.IsNullOrEmpty(model.Number) ? string.Empty : model.Number, string.IsNullOrEmpty(model.Description) ? string.Empty : model.Description });
        }

        private void wvMaps_ScriptNotify(object sender, NotifyEventArgs e)
        {

        }

        private void wvMaps_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {

        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            PhotoViewModel model = e.ClickedItem as PhotoViewModel;
            this.Navigation.NavigateTo<View.PhotoViewer>(model);
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {

        }
    }
}
