namespace ImagePanel
{
    partial class ImagePanel
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnZoom0 = new System.Windows.Forms.Button();
            this.btnRotateRight = new System.Windows.Forms.Button();
            this.btnRotateLeft = new System.Windows.Forms.Button();
            this.btnFit = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.btnZoom0);
            this.panel1.Controls.Add(this.btnRotateRight);
            this.panel1.Controls.Add(this.btnRotateLeft);
            this.panel1.Controls.Add(this.btnFit);
            this.panel1.Controls.Add(this.btnZoomIn);
            this.panel1.Controls.Add(this.btnZoomOut);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 204);
            // 
            // btnZoom0
            // 
            this.btnZoom0.Location = new System.Drawing.Point(21, 3);
            this.btnZoom0.Name = "btnZoom0";
            this.btnZoom0.Size = new System.Drawing.Size(16, 16);
            this.btnZoom0.TabIndex = 15;
            this.btnZoom0.Text = "1";
            this.btnZoom0.Click += new System.EventHandler(this.btnZoom0_Click);
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Location = new System.Drawing.Point(105, 3);
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(16, 16);
            this.btnRotateRight.TabIndex = 15;
            this.btnRotateRight.Text = "R";
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Location = new System.Drawing.Point(83, 3);
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(16, 16);
            this.btnRotateLeft.TabIndex = 15;
            this.btnRotateLeft.Text = "L";
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // btnFit
            // 
            this.btnFit.Location = new System.Drawing.Point(61, 3);
            this.btnFit.Name = "btnFit";
            this.btnFit.Size = new System.Drawing.Size(16, 16);
            this.btnFit.TabIndex = 15;
            this.btnFit.Text = "F";
            this.btnFit.Click += new System.EventHandler(this.btnFit_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(40, 3);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(16, 16);
            this.btnZoomIn.TabIndex = 15;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(3, 3);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(16, 16);
            this.btnZoomOut.TabIndex = 13;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(308, 204);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // ImagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panel1);
            this.Name = "ImagePanel";
            this.Size = new System.Drawing.Size(308, 204);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnZoom0;
        private System.Windows.Forms.Button btnFit;
        private System.Windows.Forms.Button btnRotateRight;
        private System.Windows.Forms.Button btnRotateLeft;

    }
}
