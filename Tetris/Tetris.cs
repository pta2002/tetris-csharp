using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    public class TetrisBoard
    {
        Block?[,] Blocks { get; set; }

        public TetrisBoard()
        {
            Blocks = new Block?[10, 20];

            Blocks[4, 19] = new Block(Color.Red, 4, 19);
            Blocks[5, 19] = new Block(Color.Blue, 5, 19);
            Blocks[6, 19] = new Block(Color.Yellow, 6, 19);
            Blocks[9, 19] = new Block(Color.Green, 9, 19);
        }

        public List<Block> GetBlocks()
        {
            List<Block> blocks = new List<Block>();

            for (int x=0; x<Blocks.GetLength(0); x++)
            {
                for (int y=0; y<Blocks.GetLength(1); y++)
                {
                    if (Blocks[x, y] != null)
                        blocks.Add(Blocks[x, y].Value);
                }
            }

            return blocks;
        }
    }

    public struct Block
    {
        public Color Color;
        public int X, Y;
        public Block(Color C, int x, int y)
        {
            Color = C;
            X = x;
            Y = y;
        }
    }
}
