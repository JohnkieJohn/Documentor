using Documentor.Models;
using Documentor.ViewModels.Details;
using Documentor.ViewModels.Document;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Documentor.Views.Details
{
    /// <summary>
    /// Logique d'interaction pour DocumentInsight.xaml
    /// </summary>
    public partial class DocumentInsight : Page
    {
        private TemplateDetailsViewModel ViewModel;

        private Point startPoint; // Point de départ du dessin de la Border de sélection
        private Border selectionBorder; // La Border de sélection

        private Thumb? NotHittableThumb;

        private double OriginalCanvasWidth;
        private double OriginalCanvasHeight;

        public DocumentInsight(TemplateDetailsViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
            this.DataContext = ViewModel;
            this.Insight.Loaded += Canvas_Loaded;
            this.Document.Loaded += Document_Loaded;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            Insight.Width = this.ActualWidth;
            Insight.Height = this.ActualHeight;

            OriginalCanvasWidth = this.ActualWidth;
            OriginalCanvasHeight = this.ActualHeight;

            DocumentZoombox.CurrentViewChanged += DocumentZoombox_CurrentViewChanged;
        }

        private void Document_Loaded(object sender, RoutedEventArgs e)
        {
            Document.Width = ViewModel.CanvasWidth;
            Document.Height = ViewModel.CanvasHeight;
        }

        private void DocumentZoombox_CurrentViewChanged(object sender, RoutedEventArgs e)
        {
            double zoomFactor = DocumentZoombox.Scale;

            var top = Canvas.GetTop(DocumentContainer);
            var left = Canvas.GetLeft(DocumentContainer);

            //// Replace correctement le document en cas de zoom
            //if(top > 0)
            //{
            //    Canvas.SetTop(DocumentContainer, (top - (Insight.Height - (OriginalCanvasHeight / zoomFactor)) / 2));
            //}
            //else
            //{
            //    Canvas.SetTop(DocumentContainer, (top + (Insight.Height - (OriginalCanvasHeight / zoomFactor)) / 2));
            //}

            //if(left > 0)
            //{
            //    Canvas.SetLeft(DocumentContainer, (left - (Insight.Width - (OriginalCanvasWidth / zoomFactor)) / 2));
            //}
            //else
            //{
            //    Canvas.SetLeft(DocumentContainer, (left + (Insight.Width - (OriginalCanvasWidth / zoomFactor)) / 2));
            //}

            // Ajuste la taille du Canvas de fond en fonction du facteur de zoom actuel
            Insight.Width = OriginalCanvasWidth / zoomFactor;
            Insight.Height = OriginalCanvasHeight / zoomFactor;
        }

        private void DocumentThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb? thumb = sender as Thumb;
            Grid? grid = VisualTreeHelper.GetParent(thumb) as Grid;
            var left = Canvas.GetLeft(grid);
            var top = Canvas.GetTop(grid);
            left += e.HorizontalChange;
            top += e.VerticalChange;
            Canvas.SetLeft(grid, left);
            Canvas.SetTop(grid, top);
            ViewModel.documentLeft = left;
            ViewModel.documentTop = top;
        }

        private void RichTextBox_SelectionChanged(object sender,RoutedEventArgs e)
        {
            ViewModel.GetPropertiesViewModel().GetSelectedTextPropertyValue();
        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            MElement element = (MElement)((FrameworkElement)sender).DataContext;
            ViewModel.UpdateElement(element);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.Focus(FocusThis);
            ViewModel.DisplayProperties(null, null);
            ClearThumbsRectangles();

            // Si l'utilisateur n'est pas en train d'utiliser l'outil de déplacement du document(ctrl + clique)
            if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
                startPoint = e.GetPosition(Insight);

                // Création de la bordure de sélection
                selectionBorder = new Border
                {
                    BorderBrush = Brushes.DodgerBlue,
                    BorderThickness = new Thickness(1),
                    Background = Brushes.LightBlue,
                    Opacity = 0.5,
                    Visibility = Visibility.Visible
                };

                Canvas.SetLeft(selectionBorder, startPoint.X);
                Canvas.SetTop(selectionBorder, startPoint.Y);

                // Ajoutez la Border de sélection au Canvas.
                Insight.Children.Add(selectionBorder);

                Insight.MouseMove += Canvas_MouseMove;
                Insight.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                Insight.CaptureMouse();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Mettez à jour la taille et la position de la Border de sélection en fonction de la souris.
                Point currentPoint = e.GetPosition(Insight);

                double width = Math.Abs(currentPoint.X - startPoint.X);
                double height = Math.Abs(currentPoint.Y - startPoint.Y);

                Canvas.SetLeft(selectionBorder, Math.Min(startPoint.X, currentPoint.X));
                Canvas.SetTop(selectionBorder, Math.Min(startPoint.Y, currentPoint.Y));

                selectionBorder.Width = width;
                selectionBorder.Height = height;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectElementsInsideBorder();
            Insight.ReleaseMouseCapture();
        }

        private void SelectElementsInsideBorder()
        {
            // Parcours les éléments et détermine s'ils sont à l'intérieur de la Border de sélection.
            Rect selectionRect = new Rect(Canvas.GetLeft(selectionBorder), Canvas.GetTop(selectionBorder), selectionBorder.Width, selectionBorder.Height);

            foreach (ItemsControl item in Document.Children.OfType<ItemsControl>())
            {
                foreach (MElement element in item.ItemsSource)
                {
                    // On prend en compte le positionnement du document entier, au cas où celui-ci aurait été déplacé
                    Rect elementRect = new Rect(element.Position.Left + ViewModel.documentLeft, element.Position.Top + ViewModel.documentTop, element.Size.Width, element.Size.Height);

                    if (selectionRect.IntersectsWith(elementRect))
                    {
                        element.IsSelected = true;
                    }
                }
            }

            // Supprimez la Border de sélection.
            Insight.Children.Remove(selectionBorder);
            Insight.MouseMove -= Canvas_MouseMove;
            Insight.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
        }

        private void Thumb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestVisibleThumb();
            MElement element = (MElement)((FrameworkElement)sender).DataContext;
            FrameworkElement control = sender as FrameworkElement;
            control.IsHitTestVisible = false;
            NotHittableThumb = control as Thumb;
            element.IsSelected = true;
            ViewModel.DisplayProperties(element, ReturnRichTextBox(control));
            ReturnRichTextBox(control).Focus();
            ViewModel.GetPropertiesViewModel().GetSelectedTextPropertyValue();
            e.Handled = true;
        }

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            Keyboard.ClearFocus();
            MElement element = (MElement)((FrameworkElement)sender).DataContext;
            element.IsSelected = true;
            HitTestVisibleThumb();
            ViewModel.DisplayProperties(element, null);
        }

        private void ThumbResize_DragStarted(Object sender, DragStartedEventArgs e)
        {
            Keyboard.ClearFocus();
            MElement element = (MElement)((FrameworkElement)sender).DataContext;
            Thumb thumb = sender as Thumb;
            thumb.Opacity = 1;
            HitTestVisibleThumb();
            ViewModel.DisplayProperties(element, null);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            foreach (ItemsControl item in Document.Children.OfType<ItemsControl>())
            {
                foreach (MElement element in item.ItemsSource)
                {
                    if (element.IsSelected)
                    {
                        double newLeft = element.Position.Left + e.HorizontalChange;
                        double newTop = element.Position.Top + e.VerticalChange;

                        //Suite de conditions pour faire en sorte que l'élément ne dépasse pas du canvas

                        if (newLeft < 0)
                        {
                            newLeft = 0;
                        }
                        else if (newLeft > Document.ActualWidth - element.Size.Width)
                        {
                            newLeft = Document.ActualWidth - element.Size.Width;
                        }

                        if (newTop < 0)
                        {
                            newTop = 0;
                        }
                        else if (newTop > Document.ActualHeight - element.Size.Height)
                        {
                            newTop = Document.ActualHeight - element.Size.Height;
                        }

                        element.Position.Left = newLeft;
                        element.Position.Top = newTop;
                    }
                }
            }
        }

        private void ThumbResize_DragDelta(object sender, DragDeltaEventArgs e)
        {
            foreach (ItemsControl item in Document.Children.OfType<ItemsControl>())
            {
                foreach (MElement element in item.ItemsSource)
                {
                    if (element.IsSelected)
                    {
                        // vérifie que l'élément ne dépasse pas le canvas
                        if((e.VerticalChange + (element.Size.Height + element.Position.Top)) < Document.ActualHeight)
                        {
                            // donne une valeur minimale de height de 15
                            element.Size.Height = element.Size.Height + e.VerticalChange < 15 ? 15 : element.Size.Height + e.VerticalChange;
                        }
                        // vérifie que l'élément ne dépasse pas le canvas
                        if ((e.HorizontalChange + (element.Size.Width + element.Position.Left)) < Document.ActualWidth)
                        {
                            // donne une valeur minimale de width de 30
                            element.Size.Width = element.Size.Width + e.HorizontalChange < 30 ? 30 : element.Size.Width + e.HorizontalChange;
                        }
                    }
                }
            }
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            MElement element = (MElement)((FrameworkElement)sender).DataContext;
        }

        private void ThumbResize_DragCompleted(Object sender, DragCompletedEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            thumb.Opacity = 0.2;
        }

        private void ClearThumbsRectangles()
        {
            foreach (ItemsControl item in Document.Children.OfType<ItemsControl>())
            {
                foreach (MElement element in item.ItemsSource)
                {
                    if (element != null)
                    {
                        element.IsSelected = false;
                        HitTestVisibleThumb();
                    }
                }
            }
        }

        private void HitTestVisibleThumb()
        {
            if(NotHittableThumb != null)
            {
                NotHittableThumb.IsHitTestVisible = true;
                NotHittableThumb = null;
            }
        }

        private static RichTextBox? ReturnRichTextBox(FrameworkElement control)
        {
            var parent = VisualTreeHelper.GetParent(control);
            RichTextBox? box = VisualTreeHelper.GetChild(parent, 0) as RichTextBox;
            return box;
        }
    }
}
