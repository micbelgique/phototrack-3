using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Trasys.PhotoTrack.Mobile.ViewModel;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
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
    public sealed partial class EditDetailPage : PageBase
    {
        public EditDetailPage()
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
            SiteItemViewModel model = e.Parameter as SiteItemViewModel;            
            this.DataContext = model;
            if (!string.IsNullOrEmpty(model.Number))
                txtNumber.Text = model.Number;
            if (!string.IsNullOrEmpty(model.Address))
                txtAddress.Text = model.Address;
            if (!string.IsNullOrEmpty(model.Company))
                txtCompany.Text = model.Company;
            if (!string.IsNullOrEmpty(model.Coordonates))
                txtCoordinate.Text = model.Coordonates;
        }

        private async void btnGeoLocalize_Click(object sender, RoutedEventArgs e)
        {
            pgbWaiting.Visibility = Windows.UI.Xaml.Visibility.Visible;
            txtAcquiring.Visibility = Windows.UI.Xaml.Visibility.Visible;
            var geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;
            Geoposition position = await geolocator.GetGeopositionAsync();

            // reverse geocoding
            BasicGeoposition myLocation = new BasicGeoposition
            {
                Longitude = position.Coordinate.Longitude,
                Latitude = position.Coordinate.Latitude
            };
            Geopoint pointToReverseGeocode = new Geopoint(myLocation);

            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

            // here also it should be checked if there result isn't null and what to do in such a case
            MapAddress address = result.Locations[0].Address;

            txtCoordinate.Text = string.Format("{0} - {1}", myLocation.Longitude, myLocation.Latitude);

            txtAddress.Text = string.Format("{0} {1}, {2} {3}", address.Street, address.StreetNumber, address.PostCode, address.Town);
            pgbWaiting.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            txtAcquiring.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            SiteItemViewModel model = this.DataContext as SiteItemViewModel;
            model.Address = txtAddress.Text;
            model.Number = txtNumber.Text;
            model.Company = txtCompany.Text;
            string[] splitted = txtCoordinate.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            model.Longitude = Convert.ToDouble(splitted[0]);
            model.Latitude = Convert.ToDouble(splitted[1]);
            this.Navigation.GoBack();
        }
    }
}
