using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.ViewModel.SampleData
{
    /// <summary>
    /// Sample data for drawing a Site Listing.
    /// </summary>
    /// <remarks>
    /// Usage in Page or UserControl Resources:
    ///   <d:Page.DataContext>
    ///     <sample:SiteSample />
    ///   </d:Page.DataContext>
    /// </remarks>
    public class SiteSample : SiteViewModel
    {
        /// <summary>
        /// Initializes a new Site Sample
        /// </summary>
        public SiteSample()
        {
            Uri resourceUri = new Uri("ms-appx:/ViewModel/SampleData/SiteSample.xaml", UriKind.RelativeOrAbsolute);

            #if WINDOWS_PHONE_APP || WINDOWS_APP
            Windows.UI.Xaml.Application.LoadComponent(this, resourceUri);
            #else
            #endif
        }

        
    }
}
