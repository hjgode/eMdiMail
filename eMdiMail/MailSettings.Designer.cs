namespace eMdiMail
{
    partial class MailSettings
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
                _session.Dispose();
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
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnuOK = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRecipient = new System.Windows.Forms.TextBox();
            this.cboMailAccounts = new System.Windows.Forms.ComboBox();
            this.btnGmail = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnHotmail = new System.Windows.Forms.Button();
            this.chkSendRcvInBackground = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuCancel);
            this.mainMenu1.MenuItems.Add(this.mnuOK);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuOK
            // 
            this.mnuOK.Text = "OK";
            this.mnuOK.Click += new System.EventHandler(this.mnuOK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.Text = "Mail account:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.Text = "Recipient:";
            // 
            // txtRecipient
            // 
            this.txtRecipient.Location = new System.Drawing.Point(14, 86);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.Size = new System.Drawing.Size(208, 21);
            this.txtRecipient.TabIndex = 2;
            // 
            // cboMailAccounts
            // 
            this.cboMailAccounts.Location = new System.Drawing.Point(111, 21);
            this.cboMailAccounts.Name = "cboMailAccounts";
            this.cboMailAccounts.Size = new System.Drawing.Size(111, 22);
            this.cboMailAccounts.TabIndex = 3;
            // 
            // btnGmail
            // 
            this.btnGmail.Location = new System.Drawing.Point(14, 195);
            this.btnGmail.Name = "btnGmail";
            this.btnGmail.Size = new System.Drawing.Size(96, 27);
            this.btnGmail.TabIndex = 6;
            this.btnGmail.Text = "Gmail";
            this.btnGmail.Click += new System.EventHandler(this.btnGmail_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 18);
            this.label3.Text = "Create Account:";
            // 
            // btnHotmail
            // 
            this.btnHotmail.Location = new System.Drawing.Point(116, 195);
            this.btnHotmail.Name = "btnHotmail";
            this.btnHotmail.Size = new System.Drawing.Size(96, 27);
            this.btnHotmail.TabIndex = 6;
            this.btnHotmail.Text = "Hotmail";
            this.btnHotmail.Click += new System.EventHandler(this.btnHotmail_Click);
            // 
            // chkSendRcvInBackground
            // 
            this.chkSendRcvInBackground.Location = new System.Drawing.Point(14, 125);
            this.chkSendRcvInBackground.Name = "chkSendRcvInBackground";
            this.chkSendRcvInBackground.Size = new System.Drawing.Size(208, 21);
            this.chkSendRcvInBackground.TabIndex = 9;
            this.chkSendRcvInBackground.Text = "send/recv in background";
            // 
            // MailSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.chkSendRcvInBackground);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnHotmail);
            this.Controls.Add(this.btnGmail);
            this.Controls.Add(this.cboMailAccounts);
            this.Controls.Add(this.txtRecipient);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "MailSettings";
            this.Text = "MailSettings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRecipient;
        private System.Windows.Forms.ComboBox cboMailAccounts;
        private System.Windows.Forms.Button btnGmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnHotmail;
        private System.Windows.Forms.CheckBox chkSendRcvInBackground;
    }
}