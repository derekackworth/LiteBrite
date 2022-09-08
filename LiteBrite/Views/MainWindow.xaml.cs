/*	
	Author: Derek Ackworth
	Date: April 5th, 2019
	File: MainWindow.xaml.cs
    Purpose: MainWindow class implementation
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LiteBrite.Views
{
    public partial class MainWindow : Window
    {
        // Variables
        private Point _startPoint;

        // Default Contructor
        public MainWindow()
        {
            InitializeComponent();
        }

        // Get the starting point of initiating the drag
        private void Palette_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        // Pallette mouse move
        private void Palette_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the mouse position and difference since starting the drag
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged listbox item
                ListBox listBox = sender as ListBox;
                // Use findAncestor to take the OriginalSource, go up the visual tree
                // until a ListBoxItem is hit. Need a ListBoxItem to get the Ellipse
                ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
                if (listBoxItem != null)
                {
                    // Find the data behind the listBoxItem
                    Ellipse theEllipse = new Ellipse
                    {
                        Fill = (listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as Ellipse).Fill,
                        Width = (listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as Ellipse).Width,
                        Height = (listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as Ellipse).Height
                    };
                    // Initialize drag and drop
                    DataObject dragData = new DataObject(typeof(Ellipse), theEllipse);
                    DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                }
            }
        }

        // Disable selecting cells
        private void Board_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            (sender as DataGrid).UnselectAll();
        }

        // Don't allow if the Data is not an Ellipse or the source is the same as the sender
        private void Ellipse_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Ellipse)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        // Drop into a DataGrid's ContentControl
        private void Ellipse_Drop(object sender, DragEventArgs e)
        {
            // If the data is an Ellipse
            if (e.Data.GetDataPresent(typeof(Ellipse)))
            {
                // Get the Ellipse from the DataObject and add it to the ContentControl
                Ellipse theEllipse = (Ellipse)e.Data.GetData(typeof(Ellipse));
                ContentControl cc = sender as ContentControl;
                GetChildOfType<Ellipse>(cc).Fill = theEllipse.Fill;
                GetChildOfType<Ellipse>(cc).Width = theEllipse.Width;
                GetChildOfType<Ellipse>(cc).Height = theEllipse.Height;
            }
        }

        // Find the child of a DependencyObject
        private static T GetChildOfType<T>(DependencyObject depObj)
    where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        // Find the ancestor of a DependencyObject
        private static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
