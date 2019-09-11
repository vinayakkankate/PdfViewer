using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PDFViewer
{
    public partial class MainPage : ContentPage
    {
        double startScale;
        double currentScale;
        double xOffset;
        double yOffset;
        Uri PDFUri { get; set; }

        public MainPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.ViewModel.NeedToVisibleEntry = true;
        }

        private async void OnLoadDocument(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var filedata = await CrossFilePicker.Current.PickFile();
                    if (filedata != null)
                    {
                        var filetype = Path.GetExtension(filedata.FilePath);
                        if (filetype == null || filetype.ToLower() != ".pdf")
                        {
                            await Application.Current.MainPage.DisplayAlert("Warning!", "You are allowed to pick PDF file only", "Back");
                            return;
                        }

                        PDFViewer.FilePath = DependencyService.Get<IFileHelperService>().SaveFile(filedata.FileName, filedata.DataArray);
                        PDFViewer.FileName = filedata.FileName;
                      
                        filedata = null;
                    }
                }
                else
                {
                    //string file = SearchEntry.Text.ToLower(); //"https://www.tutorialspoint.com/xamarin/xamarin_tutorial.pdf";
                    string file = "https://www.tutorialspoint.com/xamarin/xamarin_tutorial.pdf";
                    PDFUri = new Uri(file);
                    PDFViewer.PageStream = await this.GetPDFStream(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Choosing File: " + ex.ToString());
                await Application.Current.MainPage.DisplayAlert("Error!", "Unable to load the file", "Close");
            }
        }

        #region Private Methods

        private async Task<Stream> GetPDFStream(string URL) 
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(URL);
           
            if ((int)response.StatusCode == 302)
            {
                HttpResponseMessage redirectedResponse = await httpClient.GetAsync(response.Headers.Location.AbsoluteUri);
                return await redirectedResponse.Content.ReadAsStreamAsync();
            }
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion

        #region CallBacks Methods

        private void CarouselView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var index = e.HorizontalDelta >= 0 && e.HorizontalOffset != 0 ? e.CenterItemIndex : e.FirstVisibleItemIndex;
                PDFViewer.CurrentPage = index;
            }
            else
                PDFViewer.CurrentPage = e.LastVisibleItemIndex;
        }

        private void RightArrow_Tapped(object sender, EventArgs e)
        {
            var nextPage = PDFViewer.PageCount > PDFViewer.CurrentPage + 1 ? PDFViewer.CurrentPage + 1 : PDFViewer.CurrentPage;
            CarouselView.ScrollTo(nextPage, -1, ScrollToPosition.End, false);
        }

        private void LeftArrow_Tapped(object sender, EventArgs e)
        {
            var previousPage = PDFViewer.CurrentPage - 1 < 0 ? PDFViewer.CurrentPage : PDFViewer.CurrentPage - 1;
            CarouselView.ScrollTo(previousPage, -1, ScrollToPosition.Start, false);
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);
                Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }

        }

        #endregion  
    }
}