using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Tetris_game.Models
{
    public class JBlock : Block
    {

        public JBlock() : base()
        {
            Color = new SolidColorBrush(Colors.Blue);
            PossiblePositions = new ObservableCollection<ObservableCollection<Position>>()
            {
                new ObservableCollection<Position>{ new(0,0), new(1,0), new(1,1), new(1,2)},
                new ObservableCollection<Position>{ new(0,1), new(0,2), new(1,1), new(2,1)},
                new ObservableCollection<Position>{ new(1,0), new(1,1), new(1,2), new(2,2)},
                new ObservableCollection<Position>{ new(0,1), new(1,1), new(2,0), new(2,1)}
            };
        }

    }
}
