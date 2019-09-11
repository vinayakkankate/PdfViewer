using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Graphics.Pdf;
using PDFViewer;
using PDFViewer.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.IO;
using Android.Graphics;
using Java.Lang;

[assembly: ExportRenderer(typeof(ExtendedPDFViewer), typeof(ExtendedPDFRenderer))]
namespace PDFViewer.Droid
{
    public class ExtendedPDFRenderer : VisualElementRenderer<ExtendedPDFViewer>
    {
        PdfRenderer PdfRenderer { get; set; }

        public ExtendedPDFRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(ExtendedPDFViewer.FileName))
            {
                if (PdfRenderer != null)
                {
                    PdfRenderer.Dispose();
                    PdfRenderer = null;
                    this.Element.Dispose();
                }

                var fileStreamPath = this.Context.GetFileStreamPath(this.Element.FileName);
                ParcelFileDescriptor fileDescriptor = ParcelFileDescriptor.Open(fileStreamPath, ParcelFileMode.ReadOnly);
                PdfRenderer = new PdfRenderer(fileDescriptor);
                this.Element.PageCount = PdfRenderer.PageCount;
                this.Element.GenerateItems();

                //For better performance, if page count is lesser than 10, create entire items to avoid loading time.
                if (this.Element.PageCount > 10)
                    this.ConvertPagetoBitMap(this.Element.CurrentPage);
                else
                {
                    for (int i = 0; i < this.Element.PageCount; i++)
                        this.ConvertPagetoBitMap(i);
                }
            }
            else if (e.PropertyName == nameof(ExtendedPDFViewer.CurrentPage))
                this.ConvertPagetoBitMap(this.Element.CurrentPage);
        }

        public void ConvertPagetoBitMap(int index)
        {
            if (this.Element.ImageStreams.ContainsKey(index))
                return;

            int scaleFactor = 1;
            Stream stream = null;
            PdfRenderer.Page page = PdfRenderer.OpenPage(index);
            Bitmap bitmap = Bitmap.CreateBitmap((int)(page.Width * scaleFactor), (int)(page.Height * scaleFactor), Bitmap.Config.Argb8888);
            bitmap.EraseColor(Android.Graphics.Color.White);
            page.Render(bitmap, new Rect(0, 0, (int)(page.Width * scaleFactor), (int)(page.Height * scaleFactor)), null, PdfRenderMode.ForDisplay);
            page.Close();
            stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            this.Element.ImageStreams.Add(index, bitmap);
            this.Element.PDFViewModel.Items[index].PageData = bitmap;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (PdfRenderer != null)
                {
                    PdfRenderer.Dispose();
                    PdfRenderer = null;
                }

                base.Dispose(disposing);
            }
        }
    }
}