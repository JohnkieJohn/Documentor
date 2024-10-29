using Documentor.Models;
using Documentor.Repository;
using Documentor.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Documentor.ViewModels.Details
{
    public class TemplateDetailsViewModel : INotifyPropertyChanged
    {

        private readonly MElement defaultElement;

        private readonly PropertiesViewModel properties;

        public MDocument CurrentDocument { get; set; }

        private ObservableCollection<MElement> richTextBoxes;
        public ObservableCollection<MElement> RichTextBoxes
        {
            get { return richTextBoxes; }
            set
            {
                if (richTextBoxes != value)
                {
                    richTextBoxes = value;
                    OnPropertyChanged(nameof(RichTextBoxes));
                }
            }
        }

        private ObservableCollection<MElement> textboxes;
        public ObservableCollection<MElement> Textboxes
        {
            get { return textboxes; }
            set
            {
                if (textboxes != value)
                {
                    textboxes = value;
                    OnPropertyChanged(nameof(Textboxes));
                }
            }
        }

        private ObservableCollection<MElement> images;
        public ObservableCollection<MElement> Images
        {
            get { return images; }
            set
            {
                if (images != value)
                {
                    images = value;
                    OnPropertyChanged(nameof(Images));
                }
            }
        }

        private ObservableCollection<MElement> checkboxes;
        public ObservableCollection<MElement> Checkboxes
        {
            get { return checkboxes; }
            set
            {
                if (checkboxes != value)
                {
                    checkboxes = value;
                    OnPropertyChanged(nameof(Checkboxes));
                }
            }
        }

        private repElement repElement;
        private repPage repPage;

        public ICommand AddRichTextBox { get; set; }
        public ICommand Save { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Next { get; set; }
        public ICommand Previous { get; set; }
        public ICommand AddPage { get; set; }
        public ICommand DeletePage { get; set; }

        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }
        private List<MElement> elementsToCreate;
        private List<MElement> elementsToDelete;
        private List<MPage> pagesToCreate;
        private int documentThumbZIndex;
        public int DocumentThumbZIndex
        {
            get { return documentThumbZIndex; }
            set
            {
                if (documentThumbZIndex != value)
                {
                    documentThumbZIndex = value;
                    OnPropertyChanged(nameof(DocumentThumbZIndex));
                }
            }
        }

        private bool isZooming;
        public bool IsZooming
        {
            get { return isZooming; }
            set
            {
                if (isZooming != value)
                {
                    isZooming = value;
                    OnPropertyChanged(nameof(IsZooming));
                }
            }
        }

        public double documentTop = 0;
        public double documentLeft = 0;

        private int currentPageIndex = 0;

        public TemplateDetailsViewModel(MDocument document)
        {
            this.CurrentDocument = document;

            using (repPage = new repPage())
            {
                this.CurrentDocument.Pages = repPage.GetAll(CurrentDocument.Id);
            }

            using (repElement = new repElement())
            {
                this.defaultElement = repElement.GetDefault();
            }

            this.properties = new PropertiesViewModel();

            this.RichTextBoxes = new ObservableCollection<MElement>();
            this.Textboxes = new ObservableCollection<MElement>();
            this.Images = new ObservableCollection<MElement>();
            this.Checkboxes = new ObservableCollection<MElement>();

            this.AddRichTextBox = new RelayCommand(AddRichTextBoxToDocument);
            this.Save = new RelayCommand(SaveDocument);
            this.Delete = new RelayCommand(DeleteFromDocument);
            this.Next = new RelayCommand(NextPage);
            this.Previous = new RelayCommand(PreviousPage);
            this.AddPage = new RelayCommand(AddPageToDocument);
            this.DeletePage = new RelayCommand(DeletePageFromDocument);

            CanvasWidth = 793.7007874;
            CanvasHeight = 1122.519685;

            this.elementsToCreate = new List<MElement>();
            this.elementsToDelete = new List<MElement>();
            this.pagesToCreate = new List<MPage>();
            this.DocumentThumbZIndex = -1;
            this.IsZooming = false;

            GetElements();
        }

        private void NextPage()
        {
            RichTextBoxes.Clear();
            if(this.currentPageIndex < this.CurrentDocument.Pages.Count - 1)
            {
                this.currentPageIndex++;
            }
            GetElements();
        }

        private void PreviousPage()
        {
            RichTextBoxes.Clear();
            if(this.currentPageIndex > 0)
            {
                this.currentPageIndex--;
            }
            GetElements();
        }

        private void AddPageToDocument()
        {
            MPage newPage = new MPage(
                this.CurrentDocument.Id,
                this.CurrentDocument.Pages.Count + 1
            );

            using (repPage = new repPage())
            {
                repPage.Create(newPage);
            }

            this.CurrentDocument.Pages.Add(newPage);
            this.pagesToCreate.Add(newPage);
            NextPage();
        }

        private void DeletePageFromDocument()
        {
            using(repPage = new repPage())
            {
                repPage.Delete(this.CurrentDocument.Pages[this.currentPageIndex]);
            }
            PreviousPage();
        }

        public void UpdateElement(MElement element)
        {
            using(repElement = new repElement())
            {
                repElement.Update(element);
            }
        }

        private void GetElements()
        {
            using (repElement = new repElement())
            {
                foreach(MElement element in repElement.GetAll(CurrentDocument.Pages[currentPageIndex].Id))
                {
                    switch (element.ControlId)
                    {
                        case 1:
                            this.RichTextBoxes.Add(element); 
                            break;
                        case 2:
                            this.Textboxes.Add(element);
                            break;
                        case 3:
                            this.Images.Add(element);
                            break;
                        case 4:
                            this.Checkboxes.Add(element);
                            break;
                    }
                }
            }
        }

        //RETOURNE LE VIEW MODEL DES PROPRIETES
        public PropertiesViewModel GetPropertiesViewModel()
        {
            return this.properties;
        }

        //AFFICHER LES PROPRIETEES DE L'ELEMENT CLIQUE ET ASSIGNE LA RICHTEXTBOX EN CAS DE DOUBLE-CLIQUE
        public void DisplayProperties(MElement selectedElement, RichTextBox? richTextBox)
        {
            this.properties.SelectedElement = selectedElement;
            this.properties.SelectedRichTextBox = richTextBox;
        }

        // ELEMENTS METHODS
        public void AddRichTextBoxToDocument()
        {
            MElement newRichTextBox = new MElement
            (
                0,
                1,
                CurrentDocument.Pages[this.currentPageIndex].Id,
                new MSize
                (
                    defaultElement.Size.Id,
                    defaultElement.Size.Width,
                    defaultElement.Size.Height,
                    defaultElement.Size.Padding
                ),
                new MPosition
                (
                    defaultElement.Position.Id,
                    defaultElement.Position.Top,
                    defaultElement.Position.Left
                ),
                "richTextBox" + (RichTextBoxes.Count + 1).ToString(),
                "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2{\\fonttbl{\\f0\\fcharset0 Times New Roman;}{\\f2\\fcharset0 Arial;}}{\\colortbl\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0{\\lang1033\\fs18\\f2\\cf0 \\cf0\\ql{\\f2 {\\lang1036\\ltrch Votre texte ici...}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}\r\n}\r\n}"
            );
            this.RichTextBoxes.Add(newRichTextBox);
            this.elementsToCreate.Add(newRichTextBox);
        }

        // Enregistre les modifications effectuées
        private void SaveDocument()
        {
            List<MElement> elementsToUpdate = new List<MElement>();

            foreach(MElement element in this.RichTextBoxes)
            {
                // Si ce n'est pas un élément ajouté temporairement, alors c'est un élément déjà présent sur la bdd
                if (!this.elementsToCreate.Contains(element))
                {
                    elementsToUpdate.Add(element);
                }
            }

            foreach(MElement element in this.elementsToCreate)
            {
                using (repElement = new repElement())
                {
                    this.repElement.Create(element);
                }
            }

            foreach(MElement element in elementsToUpdate)
            {
                using (repElement = new repElement())
                {
                    this.repElement.Update(element);
                }
            }

            foreach (MElement element in this.elementsToDelete)
            {
                using (repElement = new repElement())
                {
                    this.repElement.Delete(element);
                }
            }
            this.elementsToCreate.Clear();
            this.elementsToDelete.Clear();
        }

        private void DeleteFromDocument()
        {
            List<MElement> elementsRemovedFromList = new List<MElement>();

            foreach (MElement element in this.RichTextBoxes)
            {
                if (element.IsSelected)
                {
                    // Vérifie si l'élément est un élément ajouté temporairement et le supprime si c'est le cas
                    if(this.elementsToCreate.Contains(element))
                    {
                        this.elementsToCreate.Remove(element);
                        elementsRemovedFromList.Add(element);
                    }
                    // Sinon ajoute l'élément à la liste des éléments à supprimer de la bdd
                    else
                    {
                        this.elementsToDelete.Add(element);
                    }
                }
            }
            foreach (MElement element in this.elementsToDelete)
            {
                if(this.RichTextBoxes.Contains(element))
                {
                    // Supprime l'élément visuellement en le retirant de la liste
                    this.RichTextBoxes.Remove(element);
                }
            }
            foreach(MElement element in elementsRemovedFromList)
            {
                if (this.RichTextBoxes.Contains(element))
                {
                    // Supprime l'élément visuellement en le retirant de la liste
                    this.RichTextBoxes.Remove(element);
                }
            }
            this.properties.SelectedElement = null;
        }

        // EVENT HANDLER
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
