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
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.Clear(Color.Black);

            foreach (Block block in Board.GetBlocks())
            {
                Brush b = new SolidBrush(block.Color);
                pe.Graphics.FillRectangle(b, block.X * 25, block.Y * 25, 25, 25);
            }
        }

        public void Tick()
        {
            Board.Tick();
            Invalidate(true);
        }
    }
}
