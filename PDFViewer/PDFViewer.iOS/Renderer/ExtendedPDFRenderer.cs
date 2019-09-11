using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using PdfKit;
using PDFViewer;
using PDFViewer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedPDFViewer), typeof(ExtendedPDFRenderer))]
namespace PDFViewer.iOS
{
    public class ExtendedPDFRenderer : VisualElementRenderer<ExtendedPDFViewer>
    {
        CGPDFDocument PdfDocument { get; set; }

        public ExtendedPDFRenderer()
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(ExtendedPDFViewer.PageStream))
            {
                if (PdfDocument != null)
                {
                    PdfDocument.Dispose();
                    PdfDocument = null;
                    this.Element.Dispose();
                }

                this.LoadDocument(this.Element.PageStream);
            }
            else if (e.PropertyName == nameof(ExtendedPDFViewer.CurrentPage))
                this.ConvertPagetoImage(this.Element.CurrentPage);
        }

        internal void LoadDocument(Stream stream)
        {
            if (stream.CanSeek)
                stream.Position = 0;

            var tempStream = new MemoryStream();
            stream.CopyTo(tempStream);
            tempStream.Position = 0;
            CGDataProvider provider = new CGDataProvider((tempStream as MemoryStream).ToArray());
            PdfDocument = new CGPDFDocument(provider);
            this.Element.PageCount = (int)PdfDocument.Pages;
            this.Element.GenerateItems();
            this.ConvertPagetoImage(this.Element.CurrentPage);
        }

        internal void ConvertPagetoImage(int index)
        {
            if (this.PdfDocument == null || this.Element.ImageStreams.ContainsKey(index))
                return;

            float scaleFactor = 1;
            UIImage image = null;
            using (CGPDFPage pdfPage = this.PdfDocument.GetPage(index + 1))
            {
                if (pdfPage != null)
                {
                    CGRect rect = pdfPage.GetBoxRect(CGPDFBox.Media);
                    nfloat factor = (nfloat)scaleFactor;
                    CGRect bounds = new CGRect(rect.X * factor, rect.Y * factor, rect.Width * factor, rect.Height * factor);
                    UIGraphics.BeginImageContext(bounds.Size);
                    CGContext context = UIGraphics.GetCurrentContext();
                    context.SetFillColor(1.0f, 1.0f, 1.0f, 1.0f);
                    context.FillRect(bounds);
                    context.TranslateCTM(0, bounds.Height);
                    context.ScaleCTM(factor, -factor);
                    context.ConcatCTM(pdfPage.GetDrawingTransform(CGPDFBox.Crop, rect, 0, true));
                    context.SetRenderingIntent(CGColorRenderingIntent.Default);
                    context.InterpolationQuality = CGInterpolationQuality.Default;
                    context.DrawPDFPage(pdfPage);
                    image = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                }
            }

            if (image != null)
            {
                this.Element.ImageStreams.Add(index, image);
                this.Element.PDFViewModel.Items[index].PageData = image;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (PdfDocument != null)
                {
                    PdfDocument.Dispose();
                    PdfDocument = null;
                }

                base.Dispose(disposing);
            }
        }
    }
}