using Documentor.Models;
using Documentor.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Documentor.ViewModels.Details
{
    public class PropertiesViewModel : INotifyPropertyChanged
    {
        private MElement? selectedElement;
        public MElement? SelectedElement
        {
            get { return selectedElement; }
            set
            {
                if (selectedElement != value)
                {
                    selectedElement = value;
                    OnPropertyChanged(nameof(SelectedElement));
                }
            }
        }

        private RichTextBox? selectedRichTextBox;
        public RichTextBox? SelectedRichTextBox
        {
            get { return selectedRichTextBox; }
            set
            {
                if (selectedRichTextBox != value)
                {
                    selectedRichTextBox = value;
                    OnPropertyChanged(nameof(this.SelectedRichTextBox));
                }
            }
        }

        public List<HorizontalAlignment> HorizontalAlignments { get; set; }
        public List<VerticalAlignment> VerticalAlignments { get; set; }
        public Dictionary<FontFamily, string> FontFamilies { get; set; }
        public List<FontWeight> FontWeights { get; set; }
        public List<FontStyle> FontStyles { get; set; }
        public List<TextAlignment> TextAlignments { get; set; }

        private Color selectedColor;
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if(selectedColor != value)
                {
                    selectedColor = value;
                    OnPropertyChanged(nameof(SelectedColor));
                }
            }
        }

        private string selectedFontFamily;
        public string SelectedFontFamily
        {
            get { return selectedFontFamily; }
            set
            {
                if (selectedFontFamily != value)
                {
                    selectedFontFamily = value;
                    OnPropertyChanged(nameof(SelectedFontFamily));
                }
            }
        }

        public List<double> FontSizes { get; set; }

        private double selectedFontSize;
        public double SelectedFontSize
        {
            get { return selectedFontSize; }
            set
            {
                if (selectedFontSize != value)
                {
                    selectedFontSize = value;
                    OnPropertyChanged(nameof(SelectedFontSize));
                }
            }
        }

        private TextSelection text;
        public TextSelection Text
        {
            get { return text; }
            set
            {
                if(text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        // changer la couleur du texte souligné dans la richtextbox
        public ICommand ApplyTextColor { get; set; }

        // changer la font family du texte souligné dans la richtextbox
        public ICommand ApplyFontFamily { get; set; }

        // changer la font size du texte souligné dans la richtextbox
        public ICommand ApplyFontSize { get; set; }
        public PropertiesViewModel()
        {
            this.HorizontalAlignments = new List<HorizontalAlignment>();
            this.HorizontalAlignments = Enum.GetValues(typeof(System.Windows.HorizontalAlignment)).Cast<System.Windows.HorizontalAlignment>().ToList();

            this.VerticalAlignments = new List<VerticalAlignment>();
            this.VerticalAlignments = Enum.GetValues(typeof(System.Windows.VerticalAlignment)).Cast<System.Windows.VerticalAlignment>().ToList();

            this.FontFamilies = new Dictionary<FontFamily, string>();
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                FontFamilies.Add(fontFamily, fontFamily.Source);
            }

            this.FontWeights = new List<FontWeight> 
            {
                System.Windows.FontWeights.Thin,
                System.Windows.FontWeights.Normal, 
                System.Windows.FontWeights.Bold,
                System.Windows.FontWeights.ExtraBold
            };

            this.FontStyles = new List<FontStyle>
            {
                System.Windows.FontStyles.Normal,
                System.Windows.FontStyles.Italic,
                System.Windows.FontStyles.Oblique
            };

            this.TextAlignments = new List<TextAlignment>
            {
                TextAlignment.Left,
                TextAlignment.Center,
                TextAlignment.Right,
                TextAlignment.Justify
            };

            this.SelectedElement = null;
            this.SelectedRichTextBox = null;
            this.FontSizes = new List<double>();
            for(double i = 10; i <= 42; i++)
            {
                this.FontSizes.Add(i);
            }
            this.selectedFontFamily = "Arial";
            this.ApplyTextColor = new RelayCommand(ApplyTextColorCommand);
            this.ApplyFontFamily = new RelayCommand(ApplyFontFamilyCommand);
            this.ApplyFontSize = new RelayCommand(ApplyFontSizeCommand);
        }

        private void ApplyTextColorCommand()
        {
            if (this.SelectedRichTextBox != null && this.SelectedRichTextBox.Selection.Text.Length > 0)
            {
                SolidColorBrush brush = new SolidColorBrush(SelectedColor);
                SelectedRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                GetSelectedTextPropertyValue();
            }
        }

        private void ApplyFontFamilyCommand()
        {
            if (this.SelectedRichTextBox != null && this.SelectedRichTextBox.Selection.Text.Length > 0)
            {
                FontFamily selectedFontFamily = new FontFamily(SelectedFontFamily);
                // Appliquez la police de caractères à la sélection de texte
                SelectedRichTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, selectedFontFamily);
                GetSelectedTextPropertyValue();
            }
        }

        private void ApplyFontSizeCommand()
        {
            if (this.SelectedRichTextBox != null && this.SelectedRichTextBox.Selection.Text.Length > 0)
            {
                SelectedRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, SelectedFontSize);
                GetSelectedTextPropertyValue();
            }
        }

        public void GetSelectedTextPropertyValue()
        {
            if(SelectedRichTextBox != null)
            {
                if (this.SelectedRichTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty) is SolidColorBrush brush)
                {
                    this.SelectedColor = brush.Color;
                }
                this.SelectedFontFamily = this.SelectedRichTextBox.Selection.GetPropertyValue(TextElement.FontFamilyProperty).ToString();
                if(this.SelectedRichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty) is double fontSize)
                {
                    this.SelectedFontSize = fontSize;
                }
            }
        }

        // EVENT HANDLER
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
