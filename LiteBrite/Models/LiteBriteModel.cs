/*	
	Author: Derek Ackworth
	Date: April 5th, 2019
	File: LiteBriteModel.cs
    Purpose: LiteBriteModel class implementation
*/

using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LiteBrite.Models
{
    [Serializable]
    public class LiteBriteModel
    {
        // Variables
        public ObservableCollection<MenuItem> Menu { get; set; }
        public ObservableCollection<Ellipse> Palette { get; set; }
        public ObservableCollection<ObservableCollection<Ellipse>> Board { get; set; }

        // Default Constructor
        public LiteBriteModel()
        {
            Menu = new ObservableCollection<MenuItem>();
            Palette = new ObservableCollection<Ellipse>();
            Board = new ObservableCollection<ObservableCollection<Ellipse>>();
        }
    }
}
