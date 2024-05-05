using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tetris_game.Models;

namespace Tetris_game.CustomControls
{
    public class HeldGrid : Grid
    {
        public UIElement this[int r, int c]
        {
            get
            {
                return this.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == r && Grid.GetColumn(e) == c);
            }
        }

        public static readonly DependencyProperty HeldBlockProperty =
        DependencyProperty.Register(
        name: "HeldBlock",
        propertyType: typeof(Block),
        ownerType: typeof(HeldGrid),
        typeMetadata: new PropertyMetadata(
        defaultValue: null,
        propertyChangedCallback: new PropertyChangedCallback(HeldBlockPropertyChangedCallback))
        );
        private static void HeldBlockPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            HeldGrid heldGrid = dependencyObject as HeldGrid;

            if (e.OldValue != null)
            {
                Block oldHeldBlock = e.OldValue as Block;
                oldHeldBlock.PropertyChanged -= heldGrid.HeldBlockPropertyChanged;

            }
            if (e.NewValue != null)
            {
                Block newHeldBlock = e.NewValue as Block;
                newHeldBlock.PropertyChanged += heldGrid.HeldBlockPropertyChanged;
            }
            heldGrid.UpdateFromHeldBlock();
        }

        private void HeldBlockPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFromHeldBlock();
        }

        private void UpdateFromHeldBlock()
        {

            Block heldBlock = this.HeldBlock;
            if (heldBlock != null)
            {
                ClearPreviousCells();

                foreach (Position tile in heldBlock.PossiblePositions[0])
                {
                    var cell = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = heldBlock.Color,
                    };
                    Grid.SetColumn(cell, tile.Column);
                    Grid.SetRow(cell, tile.Row);

                    this.Children.Add(cell);
                }
            }
            else
            {
                this.Children.Clear();
            }
        }
        public Block HeldBlock
        {
            get => (Block)GetValue(HeldBlockProperty);
            set => SetValue(HeldBlockProperty, value);
        }

        public HeldGrid()
        {
            for (int i = 0; i < 2; i++)
            {

                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1, GridUnitType.Star);
                this.RowDefinitions.Add(rd);

            }
            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(1, GridUnitType.Star);
                this.ColumnDefinitions.Add(cd);
            }
        }

        private void ClearPreviousCells()
        {

            this.Children.Clear();
        }
    }
}
