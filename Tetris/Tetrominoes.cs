using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    class Tetrominoes
    {
        public static Piece T = new Piece(Color.MediumPurple, new[] { new Block(-1, 0), new Block(0, 0), new Block(1, 0), new Block(0, -1) });
        public static Piece J = new Piece(Color.MediumBlue, new[] { new Block(-1, -1), new Block(-1, 0), new Block(0, 0), new Block(1, 0) });
        public static Piece L = new Piece(Color.Orange, new[] { new Block(1, -1), new Block(-1, 0), new Block(0, 0), new Block(1, 0) });
        public static Piece O = new Piece(Color.Yellow, new[] { new Block(1, -1), new Block(0, -1), new Block(0, 0), new Block(1, 0) })
        {
            X = 5,
            Rotatable = false
        };
        public static Piece S = new Piece(Color.LimeGreen, new[] { new Block(-1, 0), new Block(0, 0), new Block(0, -1), new Block(1, -1) });
        public static Piece Z = new Piece(Color.Red, new[] { new Block(1, 0), new Block(0, 0), new Block(0, -1), new Block(-1, -1) });
        public static Piece I = new Piece(Color.Aqua, new[] { new Block(-2, 0), new Block(-1, 0), new Block(0, 0), new Block(1, 0) })
        {
            X = 5,
            IRotate = true,
        };
    }

    public struct Piece
    {
        public Block[] Blocks;
        public int X, Y;
        public bool Rotatable, IRotate;
        private int RStage;

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
            X = 4;
            Y = 0;
            Rotatable = true;
            IRotate = false;
            RStage = 0;

            for (int x = 0; x < Blocks.Length; x++)
            {
                Blocks[x].Color = C;
            }
        }

        public Piece Rotate()
        {
            if (Rotatable)
            {
                Block[] b;

                if (!IRotate)
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
                    IRotate = this.IRotate
                };

                return p;
            }
            else
            {
                return this;
            }
        }
    }
}
