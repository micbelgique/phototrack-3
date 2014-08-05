using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Trasys.Dev.Tools.Apps;
using System.Collections;
using System.Diagnostics;
using Trasys.PhotoTrack.Mobile.Model;
using Windows.Storage;
using Windows.Storage.Pickers;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Trasys.PhotoTrack.Mobile.View
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : PageBase
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
