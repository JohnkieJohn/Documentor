using Documentor.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Documentor.Models
{
    public class MElement : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int ControlId { get; set; }
        public int PageId { get; set; }
        public MSize Size { get; set; }
        public MPosition Position { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        // préviens la vue de l'élément sélectionné
        private bool isSelected;
        public bool IsSelected 
        { 
            get { return isSelected; }
            set
            {
                if(isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public MElement(int id, int controlId, int pageId, MSize size, MPosition position, string name, string content)
        {
            this.Id = id;
            this.ControlId = controlId;
            this.PageId = pageId;
            this.Size = size;
            this.Position = position;
            this.Name = name;
            this.Content = content;
            this.IsSelected = false;
        }

        // DEFAULT ELEMENT

        public MElement(MSize defaultSize, MPosition defaultPosition)
        {
            this.Size = defaultSize;
            this.Position = defaultPosition;
        }

        // EVENT HANDLER
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
