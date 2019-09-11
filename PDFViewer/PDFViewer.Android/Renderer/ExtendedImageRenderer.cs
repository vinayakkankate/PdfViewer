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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.IO;
using System.Reflection;
using Android.Graphics;
using System.ComponentModel;
using System.Threading.Tasks;
using Java.Lang;
using PDFViewer;
using PDFViewer.Droid;

[assembly: ExportRenderer(typeof(ExtendedImage), typeof(ExtendedImageRenderer))] 
namespace PDFViewer.Droid
{
    internal class ExtendedImageRenderer : ViewRenderer<ExtendedImage, ImageView>
    {
        #region Fields

        private Handler mainHandler;

        #endregion

        #region Properties

        private ImageView NativeImage { get; set; }

        #endregion

        #region Constructor

        public ExtendedImageRenderer(Context context) : base(context)
        {
            AutoPackage = false;
            mainHandler = new Handler(Looper.MainLooper);
        }

        #endregion

        #region Override Methods

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedImage> e)
        {
            if (e.NewElement != null)
            {
                NativeImage = new ImageView(Context);
                NativeImage.DrawingCacheEnabled = true;
                this.SetNativeControl(NativeImage);
                SetImageSourceToNativeView();
                UpdateAspect();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Element.BitMapImage))
            {
                SetImageSourceToNativeView();
            }
            else if (e.PropertyName == "Aspect")
            {
                UpdateAspect();
            }
            else
                base.OnElementPropertyChanged(sender, e);
        }

        #endregion

        #region Private Methods

        private void UpdateAspect()
        {
            if (Element == null || NativeImage == null)
                return;

            ImageView.ScaleType type = this.GetScaleType(Element.Aspect);
            NativeImage.SetScaleType(type);
        }

        private void SetImageSourceToNativeView()
        {
            var bitmapimage = this.Element.BitMapImage as Bitmap;

            if (bitmapimage == null)
                return;

            Runnable myRunnable = new Runnable(() =>
            {
                if (this.Control != null && this.Control.Handle != IntPtr.Zero)
                {
                    this.Control.SetImageBitmap(bitmapimage);
                    ((IVisualElementController)Element).NativeSizeChanged();
                    bitmapimage = null;
                }
            });

            if (mainHandler != null)
                mainHandler.Post(myRunnable);
        }

        public ImageView.ScaleType GetScaleType(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.Fill:
                    return ImageView.ScaleType.FitXy;
                case Aspect.AspectFill:
                    return ImageView.ScaleType.CenterCrop;
                default:
                case Aspect.AspectFit:
                    return ImageView.ScaleType.FitCenter;
            }
        }

        #endregion

        #region Dispose Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NativeImage = null;
                mainHandler = null;
                GC.SuppressFinalize(this);
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}