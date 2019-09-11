using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using PDFViewer;
using PDFViewer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedImage), typeof(ExtendedImageRenderer))]
namespace PDFViewer.iOS
{
    internal class ExtendedImageRenderer : ImageRenderer
    {
        public ExtendedImage ImageElement { get { return this.Element as ExtendedImage; } }

        public ExtendedImageRenderer()
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.ImageElement != null && e.PropertyName == nameof(this.ImageElement.BitMapImage))
            {
                var uIImage = this.ImageElement.BitMapImage as UIImage;

                if (uIImage != null)
                    this.Control.Image = uIImage;
            }
        }
    }
}