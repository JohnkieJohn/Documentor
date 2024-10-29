 using Documentor.Interfaces;
using Documentor.Models;
using Documentor.Repository;
using Documentor.Tools;
using Documentor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Documentor.ViewModels.Document
{
    public class TemplatesViewModel : INotifyPropertyChanged
    {
        public ICommand NewDocument { get; set; }
        public ICommand DetailsView { get; set; }

        private repDocument repDocument;
        private repPage repPage;

        private ObservableCollection<MDocument> documentList;
        public ObservableCollection<MDocument> DocumentsList
        {
            get { return documentList; }
            set
            {
                if (documentList != value)
                {
                    documentList = value;
                    OnPropertyChanged(nameof(DocumentsList));
                }
            }
        }

        //private ObservableCollection<string> pageModelList;
        //public ObservableCollection<string> PageModelList
        //{
        //    get { return pageModelList; }
        //    set
        //    {
        //        if (pageModelList != value)
        //        {
        //            pageModelList = value;
        //            OnPropertyChanged(nameof(PageModelList));
        //        }
        //    }
        //}

        private int comboBoxSelectedIndex;
        public int ComboBoxSelectedIndex
        {
            get { return comboBoxSelectedIndex; }
            set
            {
                if (comboBoxSelectedIndex != value)
                {
                    comboBoxSelectedIndex = value;
                    OnPropertyChanged(nameof(ComboBoxSelectedIndex));
                }
            }
        }

        private string textFieldValue;
        public string TextFieldValue
        {
            get { return textFieldValue; }
            set
            {
                if (textFieldValue != value)
                {
                    textFieldValue = value;
                    OnPropertyChanged(nameof(TextFieldValue));
                }
            }
        }

        private MDocument selectedDocument;

        public MDocument SelectedDocument
        {
            get { return selectedDocument; }
            set
            {
                if (selectedDocument != value)
                {
                    selectedDocument = value;
                    OnPropertyChanged(nameof(SelectedDocument));
                }
            }
        }

        public TemplatesViewModel() 
        {
            DocumentsList = new ObservableCollection<MDocument>();
            GetDocuments();
            GetPages();
            NewDocument = new RelayCommand(CreateDocument);
            DetailsView = new RelayCommand(OpenDocumentDetailsView);
        }

        public MDocument? GetDocument()
        {
            if(SelectedDocument != null)
            {
                using (repDocument = new repDocument())
                {
                    return repDocument.GetById(SelectedDocument.Id);
                }
            }
            return null;
        }

        public void GetDocuments()
        {
            using (repDocument = new repDocument())
            {
                foreach (MDocument document in repDocument.GetAll())
                {
                    DocumentsList.Add(document);
                }
            }
        }

        public void GetPages()
        {
            using (repPage = new repPage())
            {
                foreach(MDocument document in DocumentsList)
                {
                    document.Pages = repPage.GetAll(document.Id);
                }
            }
        }

        public void CreateDocument()
        {
            using (repDocument = new repDocument())
            {
                MDocument newDocument = new MDocument
                (
                    null,
                    1,
                    TextFieldValue
                );

                repDocument.Create(newDocument);
                DocumentsList.Clear();
                GetDocuments();
            }
        }

        public void UpdateDocument(MDocument template)
        {

        }

        public void DeleteDocument(int id)
        {

        }

        public void OpenDocumentDetailsView()
        {
            DetailsWindow detailsWindow = new DetailsWindow(GetDocument());
            detailsWindow.DetailsWindowClosed += DetailsWindow_DetailsWindowClosed;
            detailsWindow.ShowDialog();
        }

        private void DetailsWindow_DetailsWindowClosed(object sender, EventArgs e)
        {
            DetailsWindow detailsWindow = (DetailsWindow)sender; // Récupère la référence à DetailsWindow
            detailsWindow.DetailsWindowClosed -= DetailsWindow_DetailsWindowClosed; // Désabonnement de l'événement

            // Instructions effectuées à la fermeture de la DetailsWindow
            DocumentsList.Clear();
            GetDocuments();
        }

        // EVENT HANDLER
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
