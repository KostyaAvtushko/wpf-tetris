using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Media;

namespace Tetris_game.Models
{
    public abstract class Block : BaseModel
    {
        private int currentPosition = 0;

        private Position offset;
        public SolidColorBrush Color { get; set; }
        public ObservableCollection<ObservableCollection<Position>> PossiblePositions { get; set; }

        private ObservableCollection<Position> previousPosition = new ObservableCollection<Position>();

        public Block()
        {
            Offset = new Position(0, 3);
        }
        public ObservableCollection<Position> PreviousPosition
        {
            get => previousPosition;
            set
            {
                previousPosition = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPosition
        {
            get => currentPosition;
            set
            {
                currentPosition = value;
                OnPropertyChanged();
            }
        }

        public Position Offset
        {
            get => offset;
            set
            {
                offset = value;
                OnPropertyChanged();
            }
        }

        public void SavePreviousPosition(object sender, PropertyChangedEventArgs e)
        {
            ObservableCollection<Position> positions = new ObservableCollection<Position>();
            PreviousPosition.Clear();
            foreach (Position p in PossiblePositions[CurrentPosition])
            {
                positions.Add(new Position(p.Row + Offset.Row, p.Column + Offset.Column));
            }
            PreviousPosition = positions;
        }

        public void ResetPositions()
        {
            CurrentPosition = 0;
            Offset.Row = 0;
            Offset.Column = 3;
        }
    }
}
