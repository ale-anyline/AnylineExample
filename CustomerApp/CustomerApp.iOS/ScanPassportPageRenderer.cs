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
        private UIAlertView _alert;
        AnylineMRZModuleView _anylineMrzView;
        NSError _error;
        bool _success;

        public UIAlertView Alert { get; private set; }

        // temp license
        private string _licenseKey = "eyJzY29wZSI6WyJBTEwiXSwicGxhdGZvcm0iOlsiaU9TIiwiQW5kcm9pZCIsIldpbmRvd3MiXSwidmFsaWQiOiIyMDE4LTA1LTIwIiwibWFqb3JWZXJzaW9uIjoiMyIsImlzQ29tbWVyY2lhbCI6ZmFsc2UsInRvbGVyYW5jZURheXMiOjMwLCJzaG93UG9wVXBBZnRlckV4cGlyeSI6dHJ1ZSwiaW9zSWRlbnRpZmllciI6WyJjb20uc3RhbXAuY3VzdG9tZXJhcHAiXSwiYW5kcm9pZElkZW50aWZpZXIiOlsiY29tLnN0YW1wLmN1c3RvbWVyYXBwIl0sIndpbmRvd3NJZGVudGlmaWVyIjpbImNvbS5zdGFtcC5jdXN0b21lcmFwcCJdfQpPZS9iTng3dk5GN2U1aEVTcTZ4L0d0MU5iaVpjWGo5M0haS3FWQ3dzdHJXTkErcDdHdlg0ajh5MzNIRGRnRmRjSng3MVl3NHlvR0N6SlRoWU5TbEJ4L2lYVFJoRWI1d0k2UGNYaXpwVU43eHZhL0JUUjlJc3NEdnlZWDRTR1ZZdCtHMm9KWFhXdUttRXVFV3ZNMHNGUWo4WHZwbHFZdW5vV2gyelZMcmpibEVEVVdzajd6bmNWUHpML0NOZGs0Y2M5emNPY09Hdjc1Y0pGUDdyeFc5cG42bkE1eTFnOU91OEIvTkcxNHVkNG9wSFVRQ056aEsrT3pZVk9rOEQ4TE51aEV5SXVFandTS2ZiNkQ3eFBTNXYxUFoxWitRcCtRY3ZGNnIzbmd1VGozUHVMRzlaa2I4ZytORDMzNDRRZ0RsazZ0TTYzTjg5THVhWDNJN0lEUkUxTnc9PQ==";


        // Version 1 without Async
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CGRect frame = UIScreen.MainScreen.ApplicationFrame;
            frame = new CGRect(frame.X,
                frame.Y - 20,
                frame.Width,
                frame.Height);

            _anylineMrzView = new AnylineMRZModuleView(frame);

            _error = null;

            _success = _anylineMrzView.SetupWithLicenseKey(_licenseKey, Self, out _error);

            if (!_success)
            {
                // Something went wrong. The error object contains the error description
                (Alert = new UIAlertView("Error", _error.DebugDescription, (IUIAlertViewDelegate)null, "OK", null)).Show();
            }

            // After setup is complete we add the module to the view of this view controller
            View.AddSubview(_anylineMrzView);

            //we'll manually cancel scanning
            _anylineMrzView.CancelOnResult = true;

            //set strict mode here
            _anylineMrzView.StrictMode = true;

            StartScanning();
        }

        //Version 2 Async that doesn't scan
        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    CGRect frame = UIScreen.MainScreen.ApplicationFrame;
        //    frame = new CGRect(frame.X,
        //        frame.Y - 20,
        //        frame.Width,
        //        frame.Height);

        //    _anylineMrzView = new AnylineMRZModuleView(frame);

        //    // After setup is complete we add the module to the view of this view controller
        //    View.AddSubview(_anylineMrzView);

        //    _anylineMrzView.SetupAsyncWithLicenseKey(_licenseKey, Self, (b, error) =>
        //    {
        //        StartScanning();
        //    });
        //}

        // Version 3 Async that doesn't work
        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    CGRect frame = UIScreen.MainScreen.ApplicationFrame;
        //    frame = new CGRect(frame.X,
        //        frame.Y - 20,
        //        frame.Width,
        //        frame.Height);

        //    _anylineMrzView = new AnylineMRZModuleView(frame);

        //    _anylineMrzView.SetupAsyncWithLicenseKey(_licenseKey, Self, (b, error) =>
        //    {
        //        // After setup is complete we add the module to the view of this view controller
        //        View.AddSubview(_anylineMrzView);

        //        StartScanning();
        //    });
        //}

        void StartScanning()
        {
            var success = _anylineMrzView.StartScanningAndReturnError(out _error);

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

            NavigationController.PopViewController(true);
        }
    }
}
