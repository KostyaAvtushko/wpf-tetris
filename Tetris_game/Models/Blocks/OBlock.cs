using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris_game.Models
{
    public class OBlock : Block
    {
        public OBlock() : base()
        {
            Color = new SolidColorBrush(Colors.Yellow);
            PossiblePositions = new ObservableCollection<ObservableCollection<Position>>()
            {
                new ObservableCollection<Position>{ new(0,0), new(0,1), new(1,0), new(1,1)}
            };
        }

    }
}
