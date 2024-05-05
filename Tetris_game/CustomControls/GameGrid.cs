using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tetris_game.Models;

namespace Tetris_game.CustomControls
{
    public class GameGrid : Grid
    {
        public UIElement this[int r, int c]
        {
            get
            {
                return this.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == r && Grid.GetColumn(e) == c);
            }
        }

        public static readonly DependencyProperty OccupiedCellsProperty =
        DependencyProperty.Register(
        name: "OccupiedCells",
        propertyType: typeof(ObservableCollection<ObservableCollection<(bool,Color)>>),
        ownerType: typeof(GameGrid),
        typeMetadata: new PropertyMetadata(
        defaultValue: null,
        propertyChangedCallback: new PropertyChangedCallback(IsOccupiedPropertyChangedCallback))
        );

        public ObservableCollection<ObservableCollection<(bool, Color)>> OccupiedCells
        {
            get => (ObservableCollection<ObservableCollection<(bool,Color)>>)GetValue( OccupiedCellsProperty);
            set => SetValue( OccupiedCellsProperty, value );
        }

        private static void IsOccupiedPropertyChangedCallback(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
        {
            GameGrid gameGrid = dependencyObject as GameGrid;

            if (e.OldValue != null)
            {
                ObservableCollection<ObservableCollection<(bool,Color)>> oldOccupiedCells = e.OldValue as ObservableCollection<ObservableCollection<(bool, Color)>>;
                oldOccupiedCells.CollectionChanged -= gameGrid.IsOccupiedPropertyChanged;
                foreach (var row in oldOccupiedCells)
                    row.CollectionChanged -= gameGrid.IsOccupiedPropertyChanged;
            }
            if (e.NewValue != null)
            {
                ObservableCollection<ObservableCollection<(bool, Color)>> newOccupiedCells = e.NewValue as ObservableCollection<ObservableCollection<(bool, Color)>>;
                newOccupiedCells.CollectionChanged += gameGrid.IsOccupiedPropertyChanged;
                foreach (var row in newOccupiedCells)
                    row.CollectionChanged += gameGrid.IsOccupiedPropertyChanged;
            }
            gameGrid.UpdateFromOccupiedCells();
        }
        private void IsOccupiedPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateFromOccupiedCells();
        }
        
        private void UpdateFromOccupiedCells()
        {
            for (int row = 0; row < 22; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    var rectangle = this[row, col];
                    if (OccupiedCells[row][col].Item1)
                    {
                        if (rectangle != null)
                        {
                            this.Children.Remove(rectangle);
                        }
                        var newRectangle = new Rectangle()
                        {
                            Stroke = new SolidColorBrush(Colors.Black),
                            Fill = new SolidColorBrush(OccupiedCells[row][col].Item2)
                        };

                        Grid.SetRow(newRectangle, row);
                        Grid.SetColumn(newRectangle, col);

                        this.Children.Add(newRectangle);
                    }
                    else
                    {
                        while (rectangle != null)
                        {
                            this.Children.Remove(rectangle);
                            rectangle = this[row, col];
                        }
                    }
                }
            }
        }


        public static readonly DependencyProperty ActiveBlockProperty =
        DependencyProperty.Register(
        name: "ActiveBlock",
        propertyType: typeof(Block),
        ownerType: typeof(GameGrid),
        typeMetadata: new PropertyMetadata(
        defaultValue: null,
        propertyChangedCallback: new PropertyChangedCallback(ActiveBlockPropertyChangedCallback))
        );

        public Block ActiveBlock
        {
            get=>(Block)GetValue( ActiveBlockProperty);
            set=>SetValue( ActiveBlockProperty, value);
        }

        private static void ActiveBlockPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GameGrid gameGrid = dependencyObject as GameGrid;

            if (e.OldValue != null)
            {
                Block oldActiveBlock = e.OldValue as Block;
                oldActiveBlock.PropertyChanged -= gameGrid.ActiveBlockPropertyChanged;
                oldActiveBlock.Offset.PropertyChanged -= gameGrid.ActiveBlockPropertyChanged;
                oldActiveBlock.PreviousPosition.CollectionChanged -= gameGrid.CurrentPosition_CollectionChanged;

            }
            if (e.NewValue != null)
            {
                Block newActiveBlock = e.NewValue as Block;
                newActiveBlock.PropertyChanged += gameGrid.ActiveBlockPropertyChanged;
                newActiveBlock.Offset.PropertyChanged += gameGrid.ActiveBlockPropertyChanged;
                newActiveBlock.PreviousPosition.CollectionChanged += gameGrid.CurrentPosition_CollectionChanged;
            }
            gameGrid.UpdateFromActiveBlock();
        }

        private void ActiveBlockPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFromActiveBlock();
        }
        private void CurrentPosition_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateFromActiveBlock();
        }

        private IList<Rectangle> previewBlocks;
        private void UpdateFromActiveBlock()
        {
            Block activeBlock = this.ActiveBlock;
            if (activeBlock != null)
            {
                ClearPreviousCells();


                foreach (Position tile in activeBlock.PossiblePositions[activeBlock.CurrentPosition])
                {
                    var cell = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = activeBlock.Color,

                    };
                    Grid.SetColumn(cell, tile.Column + activeBlock.Offset.Column);
                    Grid.SetRow(cell, tile.Row + activeBlock.Offset.Row);

                    this.Children.Add(cell);

                    int row = tile.Row + activeBlock.Offset.Row;

                    while (row < 22 && !OccupiedCells[row][tile.Column + activeBlock.Offset.Column].Item1)
                    {
                        row++;
                    }
                }
            }
        }


        public GameGrid()
        {

            
            for (int i =0; i<22; i++)
            {

                RowDefinition rd = new RowDefinition();
                rd.SharedSizeGroup = "TetrisGrid";
                rd.Height = (i < 2) ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
                this.RowDefinitions.Add(rd);

            }
            for (int i = 0; i<10; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.SharedSizeGroup = "TetrisGrid";
                cd.Width = new GridLength(1, GridUnitType.Star);
                this.ColumnDefinitions.Add(cd);
            }
        }

        private void ClearPreviousCells()
        {
            foreach(Position p in ActiveBlock.PreviousPosition)
            {
                this.Children.Remove(this[p.Row, p.Column]);
            }
            if(previewBlocks != null)
            {
                foreach (Rectangle p in previewBlocks)
                {
                    this.Children.Remove(p);
                }
                previewBlocks.Clear();
            }
        }
    }
}
