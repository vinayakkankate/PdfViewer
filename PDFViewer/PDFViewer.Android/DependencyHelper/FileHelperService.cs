using PDFViewer.Droid;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelperService))]
namespace PDFViewer.Droid
{
    public class FileHelperService : IFileHelperService
    {
        public string SaveFile(string filename, byte[] bytes)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllBytes(filePath, bytes);
            return filePath;
        }
    }
}