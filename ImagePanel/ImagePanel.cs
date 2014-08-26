using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using System.IO;

using OpenNETCF.Drawing;
using OpenNETCF.Drawing.Imaging;

namespace ImagePanel
{
    public partial class ImagePanel : UserControl
    {
        public Image image { 
            get {
                return pictureBox1.Image;
            }
            set {
                pictureBox1.Image = value;
            }
        }
        private int m_idx = 0;
        private string _filePath = "";

        public void loadImage(string sFilePath)
        {
            if (this.pictureBox1.Image != null)
                this.pictureBox1.Image.Dispose();

            this.pictureBox1.Image = new Bitmap(sFilePath);
            _filePath = sFilePath;
            m_idx = 0;
            ImgFit();
        }
        public void loadImage(string sFilePath, bool bFit)
        {
            if (this.pictureBox1.Image != null)
                this.pictureBox1.Image.Dispose();

            this.pictureBox1.Image = new Bitmap(sFilePath);
            _filePath = sFilePath;
            m_idx = 0;
            if (bFit)
                ImgFit();
            else
                ImgNormal();
        }

        public void loadImage(System.IO.FileInfo[] fileInfos, int idx){
            if (this.pictureBox1.Image != null)
                this.pictureBox1.Image.Dispose();
            this.pictureBox1.Image=new Bitmap(fileInfos[idx].DirectoryName + "\\" + fileInfos[idx].Name);
            _filePath = fileInfos[idx].DirectoryName + "\\" + fileInfos[idx].Name;
            m_idx = idx;
        }
        public int imageIdx = 0;
        public ImagePanel()
        {
            InitializeComponent();
            //theScreen = Screen.PrimaryScreen.Bounds;
            //theScreen = new Rectangle(3, 3, 234, 130);
            //234; 130
            ImgNormal();
        }
        private void reSizeMe(){

            return;
        }
        private void ImgFit(){
            //vScrollBar1.Visible = false;
            //hScrollBar1.Visible = false;
            pictureBox1.Left = this.Left;
            pictureBox1.Top = this.Top;
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox1.SizeMode=PictureBoxSizeMode.StretchImage;
            pictureBox1.Refresh();
        }
        private void ImgNormal()
        {
            //vScrollBar1.Visible = true;
            //hScrollBar1.Visible = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;// Normal;

            pictureBox1.Refresh();
        }
        private void ImgZoomIn()
        {
            pictureBox1.Width = pictureBox1.Width + (int)(pictureBox1.Width * 0.2);
            pictureBox1.Height = pictureBox1.Height + (int)(pictureBox1.Height * 0.2);
            pictureBox1.Refresh();
        }
        private void ImgZoomOut()
        {
            pictureBox1.Width = pictureBox1.Width - (int)(pictureBox1.Width * 0.2);
            pictureBox1.Height = pictureBox1.Height - (int)(pictureBox1.Height * 0.2);
            pictureBox1.Refresh();
        }
        public void ReShow_Image()
        {
            pictureBox1.Refresh();
        }
        private bool bPanning = false;
        int sx, sy, nx, ny;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bPanning = true;
            sx = e.X;
            sy = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bPanning = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bPanning)
            {
                nx = pictureBox1.Left - sx + e.X;
                ny = pictureBox1.Top - sy + e.Y;
                pictureBox1.Location = new System.Drawing.Point(nx, ny);
                pictureBox1.Refresh();
            }
        }
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ImgZoomIn();
            this.Refresh();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ImgZoomOut();
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            reSizeMe();
        }

        private void btnZoom0_Click(object sender, EventArgs e)
        {
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;            
        }

        private void btnFit_Click(object sender, EventArgs e)
        {
            pictureBox1.Left = this.Left;
            pictureBox1.Top = this.Top;
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Refresh();
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            //create a rotated down image file
            Bitmap _Bitmap = ImageUtils.Rotate((Bitmap)this.pictureBox1.Image, 270);// ImageHelper.getScaledBitmap(_stream, 50);// new Bitmap(filePath);
            _Bitmap.Save(_filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            //show image
            this.loadImage(_filePath, false);            
            
            //this.pictureBox1.Image = _Bitmap;
            this.pictureBox1.Refresh();
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            //create a rotated down image file
            Bitmap _Bitmap = ImageUtils.Rotate((Bitmap)this.pictureBox1.Image, 90);// ImageHelper.getScaledBitmap(_stream, 50);// new Bitmap(filePath);
            _Bitmap.Save(_filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            //show image
            this.loadImage(_filePath, false);

            //this.pictureBox1.Image = _Bitmap;
            this.pictureBox1.Refresh();
        }

    }
}