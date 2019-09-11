using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PDFViewer
{
    public interface IFileHelperService
    {
        string SaveFile(string filename, byte[] bytes);
    }
}
