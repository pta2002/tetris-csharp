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
        Piece FallingPiece;

        public TetrisBoard()
        {
            Blocks = new Block?[10, 20];

            FallingPiece = new Piece(Color.Yellow, new[] { new Block(0, 0), new Block(0, 1), new Block(1, 1), new Block(1, 0) });
        }

        public List<Block> GetBlocks()
        {
            List<Block> blocks = new List<Block>();

            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    if (Blocks[x, y] != null)
                        blocks.Add(Blocks[x, y].Value);
                }
            }

            foreach (Block block in FallingPiece.Blocks)
            {
                Block b = block;
                b.X += FallingPiece.X;
                b.Y += FallingPiece.Y;

                blocks.Add(b);
            }

            return blocks;
        }

        public void Tick()
        {
            if (Collides(FallingPiece, FallingPiece.X, FallingPiece.Y + 1))
            {
                PlacePiece();
                FallingPiece.Y = 0;
            }
            else
            {
                FallingPiece.Y += 1;
            }
        }

        private void PlacePiece()
        {
            foreach (Block block in FallingPiece.Blocks)
            {
                Block b = block;
                b.X = FallingPiece.X + b.X;
                b.Y = FallingPiece.Y + b.Y;
                Blocks[b.X, b.Y] = b;
            }
        }

        private bool Collides(Piece P, int X, int Y)
        {
            foreach (Block block in P.Blocks)
            {
                if (block.Y + Y > 19 || block.X + X < 0 || block.X + X > 9 || Blocks[block.X + X, block.Y + Y].HasValue)
                    return true;
            }
            return false;
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

        public Block(int x, int y)
        {
            this = new Block(Color.Red, x, y);
        }
    }

    public struct Piece
    {
        public Block[] Blocks;
        public int X, Y;

        public int Height {
            get {
                int h = 0;

                foreach (Block b in Blocks)
                {
                    if (b.Y > h)
                        h = b.Y;
                }

                return h;
            }
        }

        public int Width {
            get {
                int w = 0;

                foreach (Block b in Blocks)
                {
                    if (b.X > w)
                        w = b.X;
                }

                return w;
            }
        }

        public Piece(Color C, Block[] blocks)
        {
            Blocks = blocks;
            X = 5;
            Y = 0;

            for (int x = 0; x < Blocks.Length; x++)
            {
                Blocks[x].Color = C;
            }
        }
    }
}
