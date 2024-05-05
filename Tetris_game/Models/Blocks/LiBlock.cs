using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris_game.Models
{
    public class LiBlock : Block
    {
        public LiBlock() : base()
        {
            Color = new SolidColorBrush(Colors.Cyan);
            PossiblePositions = new ObservableCollection<ObservableCollection<Position>>()
            {
                new ObservableCollection<Position>{ new(1,0), new(1,1), new(1,2), new(1,3)},
                new ObservableCollection<Position>{ new(0,2), new(1,2), new(2,2), new(3,2)},
                new ObservableCollection<Position>{ new(2,0), new(2,1), new(2,2), new(2,3)},
                new ObservableCollection<Position>{ new(0,1), new(1,1), new(2,1), new(3,1)}
            };
        }
    }
}
