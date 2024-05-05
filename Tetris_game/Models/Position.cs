using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_game.Models
{
    public class Position : BaseModel
    {
        private int row = 0;
        private int column = 0;
        public int Row
        {
            get => row;
            set
            {
                row=value;
                OnPropertyChanged();
            }
        }
        public int Column
        {
            get => column;
            set
            {
                column=value;
                OnPropertyChanged();
            }
        }
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public Position(int row, int column, int row1, int column1)
        {
            Row = row + row1;
            Column = column + column1;
        }
    }
}
