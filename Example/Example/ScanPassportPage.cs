using System;
using Xamarin.Forms;

namespace Example
{
    public class ScanPassportPage : ContentPage
    {
        public ScanPassportPage()
        {
            Title = "Mrz Scan Page";

            NavigationPage.SetHasBackButton(this, true);
        }
    }
}
