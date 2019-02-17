using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            tetris.Tick(GameTimer.Interval);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
                tetris.MoveLeft();
            else if (keyData == Keys.Right)
                tetris.MoveRight();
            if (keyData == Keys.Up)
                tetris.Rotate();
            if (keyData == Keys.Space)
                tetris.PlaceDown();
            if (keyData == Keys.Down)
                tetris.GoDown();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
