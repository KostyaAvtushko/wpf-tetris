using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_game.Models
{
    public class BlockQueue : BaseModel
    {
        private readonly Random random = new Random();

        private Block nextBlock;

        private Block currentBlock;

        private readonly Block[] blocks = new Block[]
        {
            new JBlock(),
            new LBlock(),
            new TBlock(),
            new LiBlock(),
            new SBlock(),
            new ZBlock(),
            new OBlock()
        };
        
        public Block NextBlock
        {
            get => nextBlock;
            set
            {
                nextBlock = value;
                OnPropertyChanged();
            }
        }

        public Block CurrentBlock
        {
            get => currentBlock;
            set
            {
                currentBlock = value;
                OnPropertyChanged();
            }
        }
        public BlockQueue()
        {
            CurrentBlock = RandomBlock();
            NextBlock = RandomBlock();
        }
        public Block RandomBlock()
        {
            Block block = random.Next(7) switch
            {
                0 => new JBlock(),
                1 => new LBlock(),
                2 => new TBlock(),
                3 => new OBlock(),
                4 => new LiBlock(),
                5 => new SBlock(),
                6 => new ZBlock(),
                _ => new LiBlock()
            };
            return block;
        }

        public Block GenerateBlock()
        {
            
            CurrentBlock = NextBlock;
            NextBlock = RandomBlock();
            return CurrentBlock;
        }

    }


}
