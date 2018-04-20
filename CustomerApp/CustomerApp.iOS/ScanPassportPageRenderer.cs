using System;
using AnylineXamarinSDK.iOS;
using CoreGraphics;
using CustomerApp.iOS;
using CustomerApp.Views;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ScanPassportPage), typeof(ScanPassportPageRenderer))]
namespace CustomerApp.iOS
{
    public class ScanPassportPageRenderer : PageRenderer, IAnylineMRZModuleDelegate
    {
        private ScanPassportPage _scanPassportPage;
        private AnylineMRZModuleView _scanView;
        private CGRect _frame;
        private NSError _error;
        private UIAlertView _alert;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            _scanPassportPage = (ScanPassportPage)Element;

            try
            {
                var licenseKey = _scanPassportPage.LicenseKey;

                // Initializing the view.
                _frame = UIScreen.MainScreen.ApplicationFrame;

                _frame = new CGRect(_frame.X,
                    _frame.Y - 20,
                    _frame.Width,
                    _frame.Height);

                _scanView = new AnylineMRZModuleView(_frame);

                // Initializing the module.
                _error = null;
                var success = _scanView.SetupWithLicenseKey(licenseKey, Self, out _error);

                if (!success)
                {
                    (_alert = new UIAlertView("Error", _error.DebugDescription, (IUIAlertViewDelegate)null, "OK", null)).Show();
                }

                _scanView.CancelOnResult = true;

                // After setup, add the scanView to the view
                View.AddSubview(_scanView);

                _error = null;
                if (_error != null)
                    (_alert = new UIAlertView("Error", _error.DebugDescription, (IUIAlertViewDelegate)null, "OK", null)).Show();

                // Start scanning
                StartScanning();
            }
            catch (Exception exception)
            {
                (_alert = new UIAlertView("Error", "Unexpected Error Occured", (IUIAlertViewDelegate)null, "OK", null)).Show();
            }
        }

        void StartScanning()
        {
            var success = _scanView.StartScanningAndReturnError(out _error);

            if (!success)
                (_alert = new UIAlertView("Error", _error.DebugDescription, (IUIAlertViewDelegate)null, "OK", null)).Show();
        }

        public void DidFindResult(AnylineMRZModuleView anylineMRZModuleView, ALMRZResult scanResult)
        {
            var passportData = (ALIdentification) scanResult?.Result;

            if (passportData != null)
            {
                (_alert = new UIAlertView("Passport", $"The passport id: {passportData.DocumentNumber}", (IUIAlertViewDelegate)null, "OK", null)).Show();
            }

            //NavigationController.PopViewController(true);
        }
    }
}
