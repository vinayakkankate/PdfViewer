using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PDFViewer
{
    public class PDFModel : INotifyPropertyChanged
    {
        private object pagedata; 

        public object PageData
        {
            get { return pagedata; }
            set
            {
                pagedata = value;
                OnPropertyChanged("PageData");
            }
        }

        #region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
