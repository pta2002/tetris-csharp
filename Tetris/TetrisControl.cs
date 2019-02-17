﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TetrisEngine;

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
            pe.Graphics.Clear(Color.Black);

            Brush b;
            foreach (Block block in Board.GetBlocks())
            {
                b = new SolidBrush(ColorFromInt(block.Color));
                pe.Graphics.FillRectangle(b, block.X * 25, block.Y * 25, 25, 25);
                b.Dispose();
            }

            b = new SolidBrush(Color.FromArgb(20, Color.White));
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
    }
}
