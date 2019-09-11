using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using PDFViewer.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelperService))]
namespace PDFViewer.iOS
{
    public class FileHelperService : IFileHelperService
    {
        public string SaveFile(string filename, byte[] bytes)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllBytes(filePath, bytes);
            return filePath;
        }
    }
}