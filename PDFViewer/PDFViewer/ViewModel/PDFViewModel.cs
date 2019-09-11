using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace PDFViewer
{
    public class PDFViewModel : INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<PDFModel> items;
        private bool needtoVisiblefooter;
        private bool needToVisibleEntry;
         
        #endregion

        #region Constructor

        public PDFViewModel()
        {
            items = new ObservableCollection<PDFModel>();
        }

        #endregion

        #region Properties

        public ObservableCollection<PDFModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public bool NeedToVisibleFooter
        {
            get { return needtoVisiblefooter; }
            set
            {
                needtoVisiblefooter = value;
                OnPropertyChanged("NeedToVisibleFooter");
            }
        }

        public bool NeedToVisibleEntry
        {
            get { return needToVisibleEntry; }
            set
            {
                needToVisibleEntry = value;
                OnPropertyChanged("NeedToVisibleEntry");
            }
        }

        #endregion

        #region Methods

        internal void GeneratePDFItems(int count)
        {
            for (int i = 0; i < count; i++)
                items.Add(new PDFModel());

            NeedToVisibleFooter = true;
        }

        #endregion

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