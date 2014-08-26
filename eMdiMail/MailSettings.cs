
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using System.Windows.Forms;
using Microsoft.WindowsMobile.PocketOutlook;

namespace eMdiMail
{
    public partial class MailSettings : Form
    {
        OutlookSession _session;
        EmailAccount _account;
        eMDIeMailSettings _settings;

        public string sAccountName
        {
            get { return account.Name; }
        }
        public EmailAccount account
        {
            get { return _account; }
            set
            {
                int i = selectAccount(value.Name);
                if (i >= 0)
                    _account = value;
            }

        }
        string _sRecipient = "heinz-josef.gode@intermec.com";
        public string sRecipient
        {
            get {
                _sRecipient = txtRecipient.Text;
                return _sRecipient; }
            set { 
                    _sRecipient = value;
                    txtRecipient.Text = value;
                }
        }
        public string AppPath
        {
            get
            {
                string sAppPath;
                sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                if (!sAppPath.EndsWith(@"\"))
                    sAppPath += @"\";
                Uri uri = new Uri(AppPath);
                sAppPath = uri.AbsolutePath;
                return sAppPath;
            }
        }

        public MailSettings(ref string sAccount)
        {
            InitializeComponent();

            _settings = new eMDIeMailSettings();
            _sRecipient = _settings.Recipient;

            _session = new OutlookSession();
            _account = _session.EmailAccounts[_settings.MailAccount];
            
            if (_settings.syncInBackground)
                chkSendRcvInBackground.Checked = true;
            else
                chkSendRcvInBackground.Checked = false;

            fillAccountList();
        }
        private int selectAccount(string sAccountName)
        {
            if (cboMailAccounts.Items.Count == 0)
                return -1;
            int idx = -1;
            bool bFound = false;
            foreach (Account acc in _session.EmailAccounts)
            {
                System.Diagnostics.Debug.WriteLine(acc.Name);
                for (int i = 0; i < cboMailAccounts.Items.Count; i++)
                {
                    if (sAccountName == cboMailAccounts.Items[i].ToString())
                    {
                        bFound = true;
                        idx = i;
                    }
                }
            }
            if (bFound)
            {
                cboMailAccounts.SelectedIndex = idx;
            }
            return idx;
        }

        private void fillAccountList()
        {
            cboMailAccounts.Items.Clear();
            //eMail = new EmailMessage();
            bool bFound = false;
            int idx = -1;
            foreach (Account acc in _session.EmailAccounts)
            {
                System.Diagnostics.Debug.WriteLine(acc.Name);
                idx = cboMailAccounts.Items.Add(acc.Name);
                if (_account != null)
                {
                    if (acc.Name == _account.Name)
                    {
                        bFound = true;
                    }
                }
            }
            if (bFound)
            {
                _account = _session.EmailAccounts[_account.Name];
                cboMailAccounts.SelectedIndex = idx;
            }
            else
            {
                cboMailAccounts.SelectedIndex = 0;
            }

        }

        private void createGmail()
        {
            sendMail _sendMail = new sendMail();
            if (_sendMail.createAccountGoogle())
                _account = _session.EmailAccounts["Google Mail"];
            _sendMail.Dispose();
        }

        private void mnuOK_Click(object sender, EventArgs e)
        {
            _settings.Recipient = _sRecipient;
            _settings.MailAccount = cboMailAccounts.Items[cboMailAccounts.SelectedIndex].ToString();
            _settings.syncInBackground = chkSendRcvInBackground.Checked;

            _settings.saveAll();

            account = _session.EmailAccounts[cboMailAccounts.Items[cboMailAccounts.SelectedIndex].ToString()];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            _session.Dispose();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnGmail_Click(object sender, EventArgs e)
        {
            sendMail _sendMail = new sendMail();
            _sendMail.createAccountGoogle();
            _sendMail.Dispose();
            mnuCancel_Click(null, null);
        }

        private void btnHotmail_Click(object sender, EventArgs e)
        {
            sendMail _sendMail = new sendMail();
            _sendMail.createAccountGoogle();
            _sendMail.Dispose();
            mnuCancel_Click(null, null);
        }
    }
}