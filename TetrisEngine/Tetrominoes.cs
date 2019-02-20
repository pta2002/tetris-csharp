namespace TetrisEngine
{
    class Tetrominoes
    {
        public static Piece T = new Piece(Colors.Purple, new[] { new Block(-1, 0), new Block(0, 0), new Block(1, 0), new Block(0, -1) });
        public static Piece J = new Piece(Colors.DarkBlue, new[] { new Block(-1, -1), new Block(-1, 0), new Block(0, 0), new Block(1, 0) });
        public static Piece L = new Piece(Colors.Orange, new[] { new Block(1, -1), new Block(-1, 0), new Block(0, 0), new Block(1, 0) });
        public static Piece O = new Piece(Colors.Yellow, new[] { new Block(1, -1), new Block(0, -1), new Block(0, 0), new Block(1, 0) })
        {
            Rotatable = false
        };
        public static Piece S = new Piece(Colors.Green, new[] { new Block(-1, 0), new Block(0, 0), new Block(0, -1), new Block(1, -1) });
        public static Piece Z = new Piece(Colors.Red, new[] { new Block(1, 0), new Block(0, 0), new Block(0, -1), new Block(-1, -1) });
        public static Piece I = new Piece(Colors.Aqua, new[] { new Block(-2, 0), new Block(-1, 0), new Block(0, 0), new Block(1, 0) }, 5)
        {
            IsIPiece = true,
        };
    }

    public struct Piece
    {
        public Block[] Blocks;
        public int X, Y;
        public int DefaultX;
        public bool Rotatable, IsIPiece;
        public int RStage { get; private set; }

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

        public Piece(int C, Block[] blocks, int defX = 4)
        {
            Blocks = blocks;
            DefaultX = defX;
            X = DefaultX;
            Y = 0;
            Rotatable = true;
            IsIPiece = false;
            RStage = 0;

            for (int x = 0; x < Blocks.Length; x++)
            {
                Blocks[x].Color = C;
            }
        }

        public Piece Normalize()
        {
            int lowestX = int.MaxValue;
            int lowestY = int.MaxValue;

            foreach (Block blocks in Blocks)
            {
                if (lowestX > blocks.X)
                    lowestX = blocks.X;
                if (lowestY > blocks.Y)
                    lowestY = blocks.Y;
            }

            Block[] b = new Block[Blocks.Length];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Blocks[i];
                b[i].X -= lowestX;
                b[i].Y -= lowestY;
            }

            return new Piece(b[0].Color, b);
        }

        public Piece Rotate()
        {
            if (Rotatable)
            {
                Block[] b;

                if (!IsIPiece)
                {
                    b = new Block[Blocks.Length];
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i] = Blocks[i];
                        b[i].X = -Blocks[i].Y;
                        b[i].Y = Blocks[i].X;
                    }
                }
                else
                {
                    // TODO Arranjar uma maneira mais inteligente de rodar esta peça direito...
                    b = new[] {
                        new[] { new Block(0, -1), new Block(0, 0), new Block(0, 1), new Block(0, 2) },
                        new[] { new Block(-2, 1), new Block(-1, 1), new Block(0, 1), new Block(1, 1) },
                        new[] { new Block(-1, -1), new Block(-1, 0), new Block(-1, 1), new Block(-1, 2) },
                        new[] { new Block(-2, 0), new Block(-1, 0), new Block(0, 0), new Block(1, 0) },
                        }[RStage];
                }

                Piece p = new Piece(Blocks[0].Color, b)
                {
                    X = this.X,
                    Y = this.Y,
                    RStage = (this.RStage + 1) % 4,
                    IsIPiece = this.IsIPiece
                };

                return p;
            }
            else
            {
                return this;
            }
        }
    }

    public struct Colors
    {
        public static int Red = 0;
        public static int Orange = 1;
        public static int Aqua = 2;
        public static int Green = 3;
        public static int Yellow = 4;
        public static int Purple = 5;
        public static int DarkBlue = 6;
    }
}
