using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace eMdiMail
{
    [Serializable]
    public class eMDIeMailSettings
    {
        private bool _syncInBackground = true;
        public bool syncInBackground
        {
            get { return _syncInBackground; }
            set { _syncInBackground = value; }
        }
        private string _Recipient = "heinz-josef.gode@intermec.com";
        public string Recipient
        {
            get { return _Recipient; }
            set { _Recipient = value; }
        }
        private string _MailAccount = "Google Mail";
        public string MailAccount
        {
            get { return _MailAccount; }
            set { _MailAccount = value; }
        }

        private string readReg(string sValue)
        {
            string sTemp = "";
            try
            {
                sTemp = (string) Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\eMdiMail", sValue, "");
                if (sTemp == null)
                    return "";
                else
                    return sTemp;
            }
            catch (Exception)
            {
            }
            return "";
        }
        private bool writeReg(string sValueName, string sValue)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\eMdiMail", sValueName, sValue, Microsoft.Win32.RegistryValueKind.String);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        private void readAll()
        {
            string sTemp = "";
            sTemp = readReg("MailAccount");
            if (sTemp.Length > 0)
                _MailAccount = sTemp;
            sTemp = readReg("Recipient");
            if (sTemp.Length > 0)
                _Recipient = sTemp;
            sTemp = readReg("syncInBackground");
            if (sTemp.Length > 0)
            {
                if(sTemp.ToUpper()=="TRUE")
                    _syncInBackground = true;
                else
                    _syncInBackground = false;
            }
        }
        private bool writeAll()
        {
            bool b = writeReg("MailAccount", _MailAccount);
            
            if(b)
                writeReg("Recipient", _Recipient);

            if (b)
            {
                if (_syncInBackground)
                    b = writeReg("syncInBackground", "True");
                else
                    b = writeReg("syncInBackground", "False");
            }
            return b;
        }
        public bool saveAll()
        {
            return writeAll();
        }
        public eMDIeMailSettings()
        {
            string AppPath;
            AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (!AppPath.EndsWith(@"\"))
                AppPath += @"\";

            readAll();
        }

    }
}
