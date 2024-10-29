using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Documentor.Models
{
    public class MPosition : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private double top;
        public double Top
        {
            get { return top; }
            set
            {
                if (top != value)
                {
                    top = value;
                    OnPropertyChanged(nameof(Top));
                }
            }
        }

        private double left;
        public double Left
        {
            get { return left; }
            set
            {
                if(left != value)
                {
                    left = value;
                    OnPropertyChanged(nameof(Left));
                }
            }
        }

        public MPosition(int id, double top, double left)
        {
            Id = id;
            Top = top;
            Left = left;
        }

        // EVENT HANDLER
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
