using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PDFViewer
{
    public class ExtendedImage : Image, IDisposable
    {
        public static readonly BindableProperty BitMapImageProperty = BindableProperty.Create(
          "BitMapImage",
          typeof(object),
          typeof(ExtendedPDFViewer),
          null,
          BindingMode.Default,
          null);

        public object BitMapImage 
        {
            get
            {
                return GetValue(BitMapImageProperty);
            }

            set
            {
                SetValue(BitMapImageProperty, value);
            }
        }

        public static readonly BindableProperty PageStreamProperty = BindableProperty.Create(
         "PageStream",
         typeof(Stream),
         typeof(ExtendedPDFViewer),
         null,
         BindingMode.Default,
         null);

        public object PageStream
        {
            get
            {
                return (Stream)GetValue(PageStreamProperty);
            }

            set
            {
                SetValue(PageStreamProperty, value);
            }
        }

        public void Dispose()
        {
            this.PageStream = null;
            this.BitMapImage = null;
        }
    }
}
