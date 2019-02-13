using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
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
            pe.Graphics.Clear(Color.Black);

            Brush b = new SolidBrush(Color.DarkGray);
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    pe.Graphics.FillRectangle(b, x * 25 - 1, y * 25 - 1, 2, 2);
                }
            }

            foreach (Block block in Board.GetBlocks())
            {
                b.Dispose();
                b = new SolidBrush(block.Color);
                pe.Graphics.FillRectangle(b, block.X * 25, block.Y * 25, 25, 25);
            }

            b.Dispose();
        }

        public void Tick()
        {
            Board.Tick();
            Invalidate(true);
        }
    }
}
