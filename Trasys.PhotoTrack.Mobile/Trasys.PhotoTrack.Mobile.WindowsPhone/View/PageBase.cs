using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trasys.PhotoTrack.Mobile.Dev.Apps.Tools.Apps;

namespace Trasys.PhotoTrack.Mobile.View
{
    public class PageBase : Trasys.Dev.Views.Apps.PageBase<ViewModel.ViewModelLocator>
    {        
        /// <summary>
        /// Gets a reference to the all ViewModels.
        /// </summary>
        public override ViewModel.ViewModelLocator ViewModelLocator
        {
            get
            {
                return (global::Windows.UI.Xaml.Application.Current as Trasys.PhotoTrack.Mobile.App).ViewModelLocator;
            }
        }

    }
}
