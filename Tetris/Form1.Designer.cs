﻿namespace Tetris
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tetrisControl1 = new Tetris.TetrisControl();
            this.SuspendLayout();
            // 
            // tetrisControl1
            // 
            this.tetrisControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tetrisControl1.Location = new System.Drawing.Point(0, 0);
            this.tetrisControl1.Name = "tetrisControl1";
            this.tetrisControl1.Size = new System.Drawing.Size(250, 500);
            this.tetrisControl1.TabIndex = 0;
            this.tetrisControl1.Text = "tetrisControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 500);
            this.Controls.Add(this.tetrisControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Tetris";
            this.ResumeLayout(false);

        }

        #endregion

        private TetrisControl tetrisControl1;
    }
}

