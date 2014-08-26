namespace eMdiMail
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuSettings = new System.Windows.Forms.MenuItem();
            this.mnuMail = new System.Windows.Forms.MenuItem();
            this.mnuEnableDocCap = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.imagePanel1 = new ImagePanel.ImagePanel();
            this.mnuSendRecv = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuExit);
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.mnuSettings);
            this.menuItem1.MenuItems.Add(this.mnuMail);
            this.menuItem1.MenuItems.Add(this.mnuEnableDocCap);
            this.menuItem1.MenuItems.Add(this.mnuSendRecv);
            this.menuItem1.Text = "Options";
            // 
            // mnuSettings
            // 
            this.mnuSettings.Text = "Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuMail
            // 
            this.mnuMail.Text = "Mail";
            this.mnuMail.Click += new System.EventHandler(this.mnuMail_Click);
            // 
            // mnuEnableDocCap
            // 
            this.mnuEnableDocCap.Checked = true;
            this.mnuEnableDocCap.Text = "Enable DocCap";
            this.mnuEnableDocCap.Click += new System.EventHandler(this.mnuEnableDocCap_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(7, 227);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(218, 40);
            this.statusBar1.Text = "label1";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(3, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 21);
            this.label1.Text = "Press center Scan button to capture document";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnSend.Location = new System.Drawing.Point(156, 181);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 27);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // imagePanel1
            // 
            this.imagePanel1.Location = new System.Drawing.Point(0, 0);
            this.imagePanel1.Name = "imagePanel1";
            this.imagePanel1.Size = new System.Drawing.Size(240, 150);
            this.imagePanel1.TabIndex = 5;
            // 
            // mnuSendRecv
            // 
            this.mnuSendRecv.Text = "Send/Recv immediately";
            this.mnuSendRecv.Click += new System.EventHandler(this.mnuSendRecv_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.imagePanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.btnSend);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "eMDI to eMail";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.Label statusBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mnuSettings;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.MenuItem mnuMail;
        private ImagePanel.ImagePanel imagePanel1;
        private System.Windows.Forms.MenuItem mnuEnableDocCap;
        private System.Windows.Forms.MenuItem mnuSendRecv;
    }
}

