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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            tetris.Tick();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
                tetris.MoveLeft();
            else if (keyData == Keys.Right)
                tetris.MoveRight();
            else if (keyData == Keys.Up)
                tetris.Rotate();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
