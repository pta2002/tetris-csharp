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
        List<Piece> PiecePool;
        List<Piece> PieceQueue;
        Random R;

        public TetrisBoard()
        {
            Blocks = new Block?[10, 20];

            R = new Random();

            PiecePool = new List<Piece>(new []
            {
                new Piece(Color.Purple, new[] { new Block(-1, 0), new Block(0, 0), new Block(1, 0), new Block(0, -1) }), // T
                new Piece(Color.Aqua, new[] { new Block(-1, 0), new Block(0, 0), new Block(1, 0), new Block(2, 0) }),   // I
                new Piece(Color.Yellow, new[] { new Block(1, 0), new Block(0, 0), new Block(1, 1), new Block(0, 1) }),  // O
                new Piece(Color.DarkBlue, new[] { new Block(-1, -1), new Block(0, 0), new Block(1, 0), new Block(-1, 0) }),  // J
                new Piece(Color.Orange, new[] { new Block(1, -1), new Block(0, 0), new Block(1, 0), new Block(-1, 0) }),  // L
                new Piece(Color.Red, new[] { new Block(1, 1), new Block(0, 0), new Block(-1, 0), new Block(0, 1) }),  // Z
                new Piece(Color.Green, new[] { new Block(-1, 1), new Block(0, 0), new Block(1, 0), new Block(0, 1) }),  // S
            });

            NewPiece();
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
            if (ShouldPlace())
            {
                PlacePiece();
                NewPiece();
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
                if (b.X < 0 || b.X > 9 || b.Y < 0 || b.Y > 19)
                    continue;
                Blocks[b.X, b.Y] = b;
            }

            List<int> linesToClear = new List<int>();
            for (int y=0;y<Blocks.GetLength(1);y++)
            {
                bool clear = true;
                for (int x = 0; x < Blocks.GetLength(0); x++)
                {
                    if (!Blocks[x, y].HasValue)
                    {
                        clear = false;
                        break;
                    }
                }

                if (clear)
                    linesToClear.Add(y);
            }

            for (int i = 0; i<linesToClear.Count(); i++)
            {
                int y = linesToClear[i];
                for (int x = 0; x < Blocks.GetLength(0); x++)
                    Blocks[x, y] = null;

                for (int line = y-1; line >= 0; line--)
                {
                    for (int x = 0; x < Blocks.GetLength(0); x++)
                    {
                        if (Blocks[x, line] != null)
                        {
                            Block b = Blocks[x, line].Value;
                            b.Y = line + 1;
                            Blocks[x, line] = b;
                        }
                        Blocks[x, line + 1] = Blocks[x, line];
                    }
                }
            }
            
        }

        private void NewPiece()
        {
            if (PieceQueue == null)
                PieceQueue = new List<Piece>();
            while (PieceQueue.Count() < 5)
            {
                List<Piece> pieces = PiecePool.OrderBy(x => R.Next()).ToList();
                PieceQueue.AddRange(pieces);
            }

            FallingPiece = PieceQueue[0];
            PieceQueue.RemoveAt(0);
        }

        private bool Collides(Piece P, int X, int Y)
        {
            foreach (Block block in P.Blocks)
            {
                if (block.Y + Y < 0)
                    continue;
                if (block.Y + Y > 19 || block.X + X < 0 || block.X + X > 9 || Blocks[block.X + X, block.Y + Y].HasValue)
                    return true;
            }
            return false;
        }

        private bool ShouldPlace()
        {
            foreach (Block block in FallingPiece.Blocks)
            {
                if (block.X + FallingPiece.X > 9 || block.X + FallingPiece.X < 0 || block.Y + FallingPiece.Y < 0)
                    continue;
                if (block.Y + FallingPiece.Y + 1> 19 || Blocks[block.X + FallingPiece.X, block.Y + FallingPiece.Y + 1].HasValue)
                    return true;
            }
            return false;
        }

        public void MoveLeft()
        {
            if (!Collides(FallingPiece, FallingPiece.X - 1, FallingPiece.Y))
                FallingPiece.X -= 1;
        }

        public void MoveRight()
        {
            if (!Collides(FallingPiece, FallingPiece.X + 1, FallingPiece.Y))
                FallingPiece.X += 1;
        }

        public void Rotate()
        {
            Piece p = FallingPiece.Rotate();
            if (!Collides(p, p.X, p.Y))
                FallingPiece = p;
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

        public Piece Rotate()
        {
            Block[] b = new Block[Blocks.Length];

            for (int i=0; i<b.Length; i++)
            {
                b[i] = Blocks[i];
                b[i].X = Blocks[i].Y;
                b[i].Y = -Blocks[i].X;
            }

            Piece p = new Piece(b[0].Color, b)
            {
                X = this.X,
                Y = this.Y
            };

            return p;
        }
    }
}
