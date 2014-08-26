namespace MDIdemo
{
    partial class DirChooser
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnu_OK = new System.Windows.Forms.MenuItem();
            this.btnCDdir = new System.Windows.Forms.Button();
            this.btnUpDir = new System.Windows.Forms.Button();
            this.chkUseCustomDir = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuCancel);
            this.mainMenu1.MenuItems.Add(this.mnu_OK);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(234, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "textBox1";
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(3, 31);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(234, 142);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnu_OK
            // 
            this.mnu_OK.Text = "OK";
            this.mnu_OK.Click += new System.EventHandler(this.mnu_OK_Click);
            // 
            // btnCDdir
            // 
            this.btnCDdir.Location = new System.Drawing.Point(3, 179);
            this.btnCDdir.Name = "btnCDdir";
            this.btnCDdir.Size = new System.Drawing.Size(72, 20);
            this.btnCDdir.TabIndex = 2;
            this.btnCDdir.Text = "chDir";
            this.btnCDdir.Click += new System.EventHandler(this.btnCDdir_Click);
            // 
            // btnUpDir
            // 
            this.btnUpDir.Location = new System.Drawing.Point(85, 179);
            this.btnUpDir.Name = "btnUpDir";
            this.btnUpDir.Size = new System.Drawing.Size(72, 20);
            this.btnUpDir.TabIndex = 2;
            this.btnUpDir.Text = "upDir";
            this.btnUpDir.Click += new System.EventHandler(this.btnUpDir_Click);
            // 
            // chkUseCustomDir
            // 
            this.chkUseCustomDir.Location = new System.Drawing.Point(5, 214);
            this.chkUseCustomDir.Name = "chkUseCustomDir";
            this.chkUseCustomDir.Size = new System.Drawing.Size(151, 19);
            this.chkUseCustomDir.TabIndex = 3;
            this.chkUseCustomDir.Text = "use custom dir";
            this.chkUseCustomDir.CheckStateChanged += new System.EventHandler(this.chkUseCustomDir_CheckStateChanged);
            // 
            // DirChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.chkUseCustomDir);
            this.Controls.Add(this.btnUpDir);
            this.Controls.Add(this.btnCDdir);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox1);
            this.Menu = this.mainMenu1;
            this.Name = "DirChooser";
            this.Text = "DirChooser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnu_OK;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnCDdir;
        private System.Windows.Forms.Button btnUpDir;
        private System.Windows.Forms.CheckBox chkUseCustomDir;
    }
}