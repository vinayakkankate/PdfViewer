using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PDFViewer
{
    public class ExtendedPDFViewer : ContentView, IDisposable
    {
        public static readonly BindableProperty FilePathProperty = BindableProperty.Create(
            "FilePath",
            typeof(string),
            typeof(ExtendedPDFViewer),
            string.Empty,
            BindingMode.Default,
            null);

        public static readonly BindableProperty FileNameProperty = BindableProperty.Create(
           "FileName",
           typeof(string),
           typeof(ExtendedPDFViewer),
           string.Empty,
           BindingMode.Default,
           null);

        public static readonly BindableProperty PageCountProperty = BindableProperty.Create(
          "PageCount",
          typeof(int),
          typeof(ExtendedPDFViewer),
          0,
          BindingMode.Default,
          null);


        public static readonly BindableProperty CurrentPageProperty = BindableProperty.Create(
          "CurrentPage",
          typeof(int),
          typeof(ExtendedPDFViewer),
          0,
          BindingMode.Default,
          null);

        public static readonly BindableProperty PageStreamProperty = BindableProperty.Create(
        "PageStream",
        typeof(Stream),
        typeof(ExtendedPDFViewer),
        null,
        BindingMode.Default,
        null);

        public Stream PageStream
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

        public string FilePath
        {
            get
            {
                return (string)GetValue(FilePathProperty);
            }

            set
            {
                SetValue(FilePathProperty, value);
            }
        }

        public string FileName
        {
            get
            {
                return (string)GetValue(FileNameProperty);
            }

            set
            {
                SetValue(FileNameProperty, value);
            }
        }

        public int PageCount 
        {
            get
            {
                return (int)GetValue(PageCountProperty);
            }

            set
            {
                SetValue(PageCountProperty, value);
            }
        }

        public int CurrentPage 
        {
            get
            {
                return (int)GetValue(CurrentPageProperty);
            }

            set
            {
                SetValue(CurrentPageProperty, value);
            }
        }

        public Dictionary<int, object> ImageStreams { get; set; }

        public PDFViewModel PDFViewModel 
        {
            get { return this.BindingContext as PDFViewModel; }
        }
    
        public ExtendedPDFViewer()
        {
        }

        public void GenerateItems()
        {
            ImageStreams = new Dictionary<int, object>();
            if (PDFViewModel.Items.Count > 0)
                PDFViewModel.Items.Clear();
            PDFViewModel.GeneratePDFItems(this.PageCount);
        }

        public void Dispose()
        {
            if (this.ImageStreams != null)
            {
                this.ImageStreams.Clear();
                this.ImageStreams = null;
            }
        }
    }
}
