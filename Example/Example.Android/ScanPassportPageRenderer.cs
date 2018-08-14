using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AT.Nineyards.Anyline.Camera;
using AT.Nineyards.Anyline.Modules.Mrz;
using AT.Nineyards.Anylinexamarin.Support.Modules.Mrz;
using Example;
using Example.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ScanPassportPage), typeof(ScanPassportPageRenderer))]
namespace Example.Droid
{
    public class ScanPassportPageRenderer : PageRenderer, IMrzResultListener
    {
        private Android.Views.View _view;
        private MrzScanView _scanView;

        // Stamp License
        private readonly string _licenseKey = "<A License Key>";

        public ScanPassportPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (_view == null)
            {
                var activity = Context as Activity;
                _view = activity.LayoutInflater.Inflate(Resource.Layout.MrzLayout, this, false);

                AddView(_view);

                _scanView = _view.FindViewById<MrzScanView>(Resource.Id.mrz_scan_view);
                _scanView.InitAnyline(_licenseKey, this);

                _scanView.SetConfigFromAsset("MrzConfig.json");

                // The config can be changed via code directly through the Config property, for example:
                var config = _scanView.Config;
                config.SetFlashMode(AnylineViewConfig.FlashMode.Auto);
                _scanView.Config.SetFlashAlignment(AnylineViewConfig.FlashAlignment.TopRight);

                // Important: Once the config is changed, it has to be set again explicitly:
                _scanView.Config = config;

                _scanView.SetCancelOnResult(false);

                _scanView.StartScanning();

                _scanView.CameraOpened += _scanView_CameraOpened;
            }
        }

        private void _scanView_CameraOpened(object sender, CameraOpenedEventArgs e)
        {
            //_scanView.StartScanning();
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
            _view.Measure(msw, msh);
            _view.Layout(0, 0, r - l, b - t);
        }

        void IMrzResultListener.OnResult(MrzResult scanResult)
        {
            Toast.MakeText(Context, scanResult.Result.ToString(), ToastLength.Long).Show();

            _scanView.StartScanning();
        }
    }
}