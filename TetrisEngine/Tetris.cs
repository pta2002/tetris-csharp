using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisEngine
{
    public class TetrisBoard
    {
        Block?[,] Blocks { get; set; }
        Piece FallingPiece;
        List<Piece> PiecePool;
        public List<Piece> PieceQueue;
        Random R;
        public Piece? HeldPiece { get; private set; }
        bool changedHold = false;
        int LockOutTimer = 500;
        int rots = 0;
        int gravityTimer = 0;
        public bool IsOver = false;

        public TetrisBoard()
        {
            Blocks = new Block?[10, 20];

            R = new Random();

            PiecePool = new List<Piece>(new[]
            {
                Tetrominoes.I,
                Tetrominoes.J,
                Tetrominoes.L,
                Tetrominoes.O,
                Tetrominoes.S,
                Tetrominoes.T,
                Tetrominoes.Z
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

        public void Tick(int Delta)
        {
            if (!IsOver)
            {
                if (ShouldPlace(Delta))
                {
                    PlacePiece(FallingPiece);
                    NewPiece();
                }
                else if (!Collides(FallingPiece, FallingPiece.X, FallingPiece.Y + 1) && gravityTimer >= 200)
                {
                    gravityTimer = 0;
                    rots = 0;
                    FallingPiece.Y += 1;
                }
                else
                {
                    gravityTimer += Delta;
                }
            }
        }

        public void GoDown()
        {
            if (!Collides(FallingPiece, FallingPiece.X, FallingPiece.Y + 1))
            {
                gravityTimer = 0;
                FallingPiece.Y += 1;
            }
        }

        private void PlacePiece(Piece P)
        {
            foreach (Block block in P.Blocks)
            {
                Block b = block;
                b.X = P.X + b.X;
                b.Y = P.Y + b.Y;
                if (b.X < 0 || b.X > 9 || b.Y < 0 || b.Y > 19)
                    continue;
                Blocks[b.X, b.Y] = b;
            }

            List<int> linesToClear = new List<int>();
            for (int y = 0; y < Blocks.GetLength(1); y++)
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

            for (int i = 0; i < linesToClear.Count(); i++)
            {
                int y = linesToClear[i];
                for (int x = 0; x < Blocks.GetLength(0); x++)
                    Blocks[x, y] = null;

                for (int line = y - 1; line >= 0; line--)
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

        public void Hold()
        {
            if (!changedHold)
            {
                if (HeldPiece.HasValue)
                {
                    Piece tmp = FallingPiece;
                    FallingPiece = HeldPiece.Value;
                    HeldPiece = tmp;
                    FallingPiece.Y = 0;
                    FallingPiece.X = FallingPiece.DefaultX;
                }
                else
                {
                    HeldPiece = FallingPiece;
                    NewPiece();
                }
                changedHold = true;
            }
        }

        private void NewPiece()
        {
            if (PieceQueue == null)
                PieceQueue = new List<Piece>();
            while (PieceQueue.Count() <= 6)
            {
                List<Piece> pieces = PiecePool.OrderBy(x => R.Next()).ToList();
                PieceQueue.AddRange(pieces);
            }

            FallingPiece = PieceQueue[0];
            PieceQueue.RemoveAt(0);
            changedHold = false;

            if (Collides(FallingPiece, FallingPiece.X, FallingPiece.Y) || IsOver)
            {
                IsOver = true;
            }
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

        private bool ShouldPlace(int Delta)
        {
            if (Collides(FallingPiece, FallingPiece.X, FallingPiece.Y + 1))
                LockOutTimer -= Delta;
            else
                LockOutTimer = 500;
            if (LockOutTimer < 0)
            {
                foreach (Block block in FallingPiece.Blocks)
                {
                    if (block.X + FallingPiece.X > 9 || block.X + FallingPiece.X < 0 || block.Y + FallingPiece.Y < 0)
                        continue;
                    if (block.Y + FallingPiece.Y + 1 > 19 || Blocks[block.X + FallingPiece.X, block.Y + FallingPiece.Y + 1].HasValue)
                        return true;
                }
                LockOutTimer = 500;
            }
            return false;
        }

        public void PlaceDown()
        {
            PlacePiece(FallLocation());
            NewPiece();
        }

        public Piece FallLocation()
        {
            Piece P = FallingPiece;
            while (!Collides(P, P.X, P.Y + 1))
                P.Y++;
            return P;
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

            // Check wall kicks
            WallKick[] kicks;
            if (p.IsIPiece)
                kicks = WallKick.IKicks[p.RStage];
            else
                kicks = WallKick.Normal[p.RStage];
            foreach (WallKick wk in kicks)
            {
                if (!Collides(p, p.X + wk.CheckX, p.Y + wk.CheckY))
                {
                    FallingPiece = p;
                    FallingPiece.X += wk.CheckX;
                    FallingPiece.Y += wk.CheckY;
                    rots++;

                    if (rots < 8)
                        LockOutTimer = 500;
                    break;
                }
            }


        }
    }

    public struct Block
    {
        public int Color;
        public int X, Y;

        public Block(int C, int x, int y)
        {
            Color = C;
            X = x;
            Y = y;
        }

        public Block(int x, int y)
        {
            this = new Block(0, x, y);
        }
    }

    public class WallKick
    {
        // https://tetris.fandom.com/wiki/SRS
        public static WallKick[][] Normal = new[]
        {
            new[] { new WallKick(0, 0, 0), new WallKick(0, -1, 0), new WallKick(0, -1, -1), new WallKick(0, 0, 2), new WallKick(0, -1, 2) },
            new[] { new WallKick(1, 0, 0), new WallKick(1, 1, 0), new WallKick(1, 1, 1), new WallKick(1, 0, -2), new WallKick(1, 1, -2) },
            new[] { new WallKick(2, 0, 0), new WallKick(2, 1, 0), new WallKick(2, 1, -1), new WallKick(2, 0, 2), new WallKick(2, 1, 2) },
            new[] { new WallKick(3, 0, 0), new WallKick(3, -1, 0), new WallKick(3, -1, 1), new WallKick(3, 0, -2), new WallKick(3, -1, -2) },
        };

        public static WallKick[][] IKicks = new[]
        {
            new[] { new WallKick(0, 0, 0), new WallKick(0, -2, 0), new WallKick(0, 1, -1), new WallKick(0, -2, 1), new WallKick(0, 1, -2) },
            new[] { new WallKick(1, 0, 0), new WallKick(1, -1, 0), new WallKick(1, 2, 0), new WallKick(1, -1, -2), new WallKick(1, 2, 1) },
            new[] { new WallKick(2, 0, 0), new WallKick(2, 2, 0), new WallKick(2, -1, 0), new WallKick(2, 2, -1), new WallKick(2, -1, 2) },
            new[] { new WallKick(3, 0, 0), new WallKick(3, 1, 0), new WallKick(3, -2, 0), new WallKick(3, 1, 2), new WallKick(3, -2, -1) },
        };

        public int SourceRot { get; }
        public int CheckX { get; }
        public int CheckY { get; }

        public WallKick(int SourceRot, int CheckX, int CheckY)
        {
            this.SourceRot = SourceRot;
            this.CheckX = CheckX;
            this.CheckY = CheckY;
        }
    }
}
