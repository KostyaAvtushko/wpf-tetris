using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tetris_game.Models;

namespace Tetris_game.CustomControls
{
    public class NextGrid : Grid
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
        name: "NextBlock",
        propertyType: typeof(Block),
        ownerType: typeof(NextGrid),
        typeMetadata: new PropertyMetadata(
        defaultValue: null,
        propertyChangedCallback: new PropertyChangedCallback(NextBlockPropertyChangedCallback))
        );

        private static void NextBlockPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            NextGrid nextGrid = dependencyObject as NextGrid;

            if (e.OldValue != null)
            {
                Block oldNextBlock = e.OldValue as Block;
                oldNextBlock.PropertyChanged -= nextGrid.NextBlockPropertyChanged;

            }
            if (e.NewValue != null)
            {
                Block newNextBlock = e.NewValue as Block;
                newNextBlock.PropertyChanged += nextGrid.NextBlockPropertyChanged;
            }
            nextGrid.UpdateFromNextBlock();
        }

        private void NextBlockPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFromNextBlock();
        }

        private void UpdateFromNextBlock()
        {

            Block nextBlock = this.NextBlock;
            if (nextBlock != null)
            {
                ClearPreviousCells();

                foreach (Position tile in nextBlock.PossiblePositions[0])
                {
                    var cell = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = nextBlock.Color,
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
        public Block NextBlock
        {
            get => (Block)GetValue(HeldBlockProperty);
            set => SetValue(HeldBlockProperty, value);
        }

        public NextGrid()
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
