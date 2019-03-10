using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TetrisEngine;
using System.Resources;

namespace TetrisWinForms
{
    public partial class TetrisControl : Control
    {
        TetrisBoard Board;

        public TetrisControl()
        {
            InitializeComponent();

            Board = new TetrisBoard();
            DoubleBuffered = true;
            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.Clear(Color.FromArgb(5, 5, 20));

            Brush b;
            foreach (Block block in Board.GetBlocks())
            {
                pe.Graphics.DrawImage(GetImageFromColor(block.Color), block.X * 25, block.Y * 25, 25, 25);
            }

            Piece ghostPiece = Board.FallLocation();
            foreach (Block block in ghostPiece.Blocks)
                //pe.Graphics.DrawImage(GetImageFromColor(block.Color), (block.X + ghostPiece.X) * 25, (block.Y + ghostPiece.Y) * 25, 25, 25);
                pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(77, ColorFromInt(block.Color))), (block.X + ghostPiece.X) * 25, (block.Y + ghostPiece.Y) * 25, 25, 25);

            b = new SolidBrush(Color.FromArgb(5, 10, 40));
            for (int x=1; x<10; x++)
            {
                pe.Graphics.DrawLine(new Pen(b), x * 25, 0, x * 25, Height);
            }
            for (int y=1; y<20; y++)
            {
                pe.Graphics.DrawLine(new Pen(b), 0, y*25, Width, y*25);
            }
        }

        private static Color ColorFromInt(int color)
        {
            switch (color) {
                case 0:
                    return Color.Red;
                case 1:
                    return Color.Orange;
                case 2:
                    return Color.Aqua;
                case 3:
                    return Color.LimeGreen;
                case 4:
                    return Color.Yellow;
                case 5:
                    return Color.MediumPurple;
                default:
                    return Color.MediumBlue;
            }
        }

        private static Image GetImageFromColor(int color)
        {
            switch (color)
            {
                case 0:
                    return Tetris.Properties.Resources.tetrisblocks_0;
                case 1:
                    return Tetris.Properties.Resources.tetrisblocks_1;
                case 2:
                    return Tetris.Properties.Resources.tetrisblocks_2;
                case 3:
                    return Tetris.Properties.Resources.tetrisblocks_3;
                case 4:
                    return Tetris.Properties.Resources.tetrisblocks_4;
                case 5:
                    return Tetris.Properties.Resources.tetrisblocks_5;
                default:
                    return Tetris.Properties.Resources.tetrisblocks_6;
            }
        }

        public void MoveRight()
        {
            Board.MoveRight();
            Invalidate(true);
        }

        public void MoveLeft()
        {
            Board.MoveLeft();
            Invalidate(true);
        }

        public void Tick(int Delta)
        {
            Board.Tick(Delta);
            Invalidate(true);
        }

        public void Rotate()
        {
            Board.Rotate();
            Invalidate(true);
        }

        public void PlaceDown()
        {
            Board.PlaceDown();
            Invalidate(true);
        }

        public void GoDown()
        {
            Board.GoDown();
            Invalidate(true);
        }
    }
}
