using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Intermec.DataCollection;
using System.IO;

using OpenNETCF.Drawing;
using OpenNETCF.Drawing.Imaging;

using System.Runtime.InteropServices;

namespace eMdiMail
{
    public partial class Form1 : Form
    {
        [DllImport("coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes); 
        
        DocumentCapture doccap;
        private Bitmap _Bitmap;
        sendMail _sendMail;
        eMDIeMailSettings _mySettings;
        string _filePath = "";

        public Form1()
        {
            InitializeComponent();
            if (startMDI())
            {
                _mySettings = new eMDIeMailSettings();
                _sendMail = new sendMail(_mySettings.MailAccount);
                if (!initMail())
                {
                    MessageBox.Show("eMail account invalid. Setup an account and restart!");
                    stopMDI();
                    Application.Exit();
                }
                System.Diagnostics.Debug.WriteLine(_mySettings.Recipient);
            }
            btnSend.Enabled = false;
        }
        
        private bool stopMDI()
        {
            bool bRet = false;
            if (doccap != null)
            {
                doccap.Dispose();
                doccap = null;
                bRet = true;
            }
            return bRet;
        }

        private bool initMail()
        {
            bool bRet = false;
            _sendMail = new sendMail(_mySettings.MailAccount);
            if (!_sendMail.bIsValidAccount)
            {
                MessageBox.Show("eMail account invalid. Setup an account and restart!");
                string sAccount = _mySettings.MailAccount;
                MailSettings ms = new MailSettings(ref sAccount);
                if (ms.ShowDialog() == DialogResult.OK)
                {
                    _sendMail.setAccount(sAccount);// ms.account.Name);
                    bRet = true;
                }
                ms.Dispose();
            }
            else
                bRet = true;

            return bRet;
        }
        /// <summary>
        /// read the full model code of device
        /// </summary>
        /// <returns>a model code like CK3B36211058091</returns>
        public static string getModelNumber()
        {
            /*
            [HKEY_LOCAL_MACHINE\Ident]
            "Desc"="Intermec CK3 Device"
            "Name"="CK3A36010858282"
            "OrigName"="IntermecCK3"
             * */
            string sTemp = "";
            try
            {
                Microsoft.Win32.RegistryKey tempKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"\Ident");
                sTemp = (string)tempKey.GetValue("Name", "");
            }
            catch (Exception)
            {

            }
            return sTemp;
        }

        private bool startMDI()
        {
            bool bRet = false;
            try
            {
                string sModel = getModelNumber();
                if (sModel.StartsWith("CK7") || sModel.StartsWith("CN7") || sModel.StartsWith("CS40"))
                    doccap = new DocumentCapture("Camera");
                else
                    throw new NotSupportedException("Only Cozumel or CS40 supported");

                //get free space
                ulong freeBytesAvail, totalBytes, totalFree;
                if (GetDiskFreeSpaceEx("\\My Documents", out freeBytesAvail, out totalBytes, out totalFree))
                    statusBar1.Text = "Memory Limit=" + doccap.FolderMemoryLimit.ToString() + " free mem=" + totalFree.ToString();
                else
                    statusBar1.Text = "Memory Limit=" + doccap.FolderMemoryLimit.ToString();

                doccap.FolderMemoryLimit = 0;
                doccap.Capture += new DocumentCaptureEventHandler(doccap_Capture);
                doccap.Error += new DocumentCaptureErrorEventHandler(doccap_Error);
                doccap.Guidance += new DocumentCaptureGuidanceEventHandler(doccap_Guidance);
                doccap.CapturedDocumentLocation = @"\My Documents";
                
                try { doccap.ViewFinderEnable = true; }catch{}
                
                
                try
                {                    
                    doccap.ManualTorchControl(DocumentCapture.ManualTorchControlType.TorchOn);
                    short torchIntensStep = doccap.TorchIncrementValue;
                    short torchMaxVal = doccap.TorchIntensityMaximumValue;
                    short torchMinVal = doccap.TorchIntensityMinimumValue;
                    short torchCurVal = doccap.TorchIntensitySetting;
                    short torchNew = (short)( torchMaxVal % torchIntensStep );
                    torchNew = (short)(torchNew * torchIntensStep);
                }
                catch (DocumentCaptureException ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception " + ex.Message + " in ManualTorchControl");
                }

                doccap.ImageRotation = ImageConditioning.ImageRotationValue.Degrees_90;
                doccap.OutputCompression = ImageConditioning.OutputCompressionValue.Jpeg;
                doccap.OutputCompressionQuality = 100;
                if(doccap.isFlashConfigSupported)
                    doccap.FlashMethod = DocumentCapture.FlashMethodType.NoFlash;

                //disable hardware trigger
                doccap.TriggerEnable = DocumentCapture.TriggerEnableValue.Disable;

                doccap.EnableDocumentCapture = DocumentCapture.EnableDocumentCaptureValue.Disable;

                try { doccap.SetButtonAction(DocumentCapture.ButtonID.Center, DocumentCapture.ButtonActionType.Scan); }
                catch { }

                enableCapture(true);
                bRet = true;
            }
            catch (DocumentCaptureFirmwareSupportException ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to init DocCapture: " + ex.Message + "\nDocumentCaptureFirmwareSupportException");
            }
            catch (DocumentCaptureException ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to init DocCapture: " + ex.Message + "\nDid you install DC_NET.CAB");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to init DocCapture: " + ex.Message + "\nDid you install DC_NET.CAB");
            }
            if (doccap == null)
            {
                MessageBox.Show("Cannot start without DocCap object. Will exit now!");
                bRet = false;
                Application.Exit();
            }
            return bRet;
        }

        void doccap_Guidance(object sender, GuidanceEventArgs DocCaptureGuidanceArgs)
        {
            try
            {
                //failed on focus 
                if (DocCaptureGuidanceArgs.FocusCheckFailure)
                    this.statusBar1.Text = "focus failure";

                //image far
                if (DocCaptureGuidanceArgs.ImagerFar)
                    this.statusBar1.Text = "imager far";

                //imager sharp
                if (DocCaptureGuidanceArgs.ImagerSharp)
                    this.statusBar1.Text = "imager sharp";

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        void doccap_Error(object sender, ErrorEventArgs DocCapErrorArgs)
        {
            try
            {
                if (DocCapErrorArgs.LimitStorageWarning)
                    this.statusBar1.Text = "Storage Limit Warning: " + DocCapErrorArgs.ErrorMessage;

                if (DocCapErrorArgs.ExceedStorageLimit)
                    this.statusBar1.Text = "Exceed Storage Limit: " + DocCapErrorArgs.ErrorMessage;

                if (DocCapErrorArgs.FileSaveError)
                    this.statusBar1.Text = "Failed on save file: " + DocCapErrorArgs.ErrorMessage;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        void doccap_Capture(object sender, DocumentCaptureEventArgs DocCaptureEventArgs)
        {
            if (DocCaptureEventArgs.DocCapInProcess)
            {
                this.statusBar1.Text = "DocCap in process, please wait...";
            }

            //display captured doucment image file
            if (DocCaptureEventArgs.DocCapFile.Length > 0)
            {
                string filePath = DocCaptureEventArgs.DocCapFile.ToString();

                this.statusBar1.Text = "File Path: " + filePath;

                if (File.Exists(filePath))
                {
                    if (_Bitmap != null)
                        _Bitmap.Dispose();
                    
                    //create a scaled down image file
                    var stream = File.Open(filePath, FileMode.Open);
                    StreamOnFile _stream = new StreamOnFile(stream);
                    _Bitmap = ImageHelper.getScaledBitmap(_stream, 50);// new Bitmap(filePath);
                    _stream.Dispose();
                    _Bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //show image
                    imagePanel1.loadImage(filePath);
                    //this.pictureBox1.Image = (Image)_Bitmap;
                    _filePath = filePath;
                    if(_sendMail.bIsValidAccount)
                        btnSend.Enabled = true;
                    else
                        btnSend.Enabled = false;
                }
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("eXit?", "eMDI 2 eMail", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            if (doccap != null)
            {
                doccap.EnableDocumentCapture = DocumentCapture.EnableDocumentCaptureValue.Disable;
                try { doccap.SetButtonAction(DocumentCapture.ButtonID.Center, DocumentCapture.ButtonActionType.Scan); }catch{}
                doccap.Dispose();
            }
            if (_sendMail != null)
                _sendMail.Dispose();
            Cursor.Current = Cursors.Default;
            Application.Exit();
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            MDIdemo.mdiSettings sett = new MDIdemo.mdiSettings(doccap);
            sett.ShowDialog();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_filePath.Length > 0)
            {
                try
                {
                    if (!System.IO.File.Exists(_filePath))
                        return;
                }
                catch (Exception)
                {
                }
            }
            _mySettings.syncInBackground = !_sendMail.syncImmediately;
            
            if (_sendMail.send(_filePath))
                this.statusBar1.Text = "File sent: " + _filePath;
            else
                this.statusBar1.Text = "File sent FAILED: " + _filePath;

            if (_mySettings.syncInBackground)
                _sendMail.syncNow(this.Handle);
            else
                _sendMail.syncNow();
        }

        private void mnuMail_Click(object sender, EventArgs e)
        {
            string sAccountIn = "";
            MailSettings ms = new MailSettings(ref sAccountIn);
            ms.account = _sendMail.account;
            ms.sRecipient = _sendMail._to;
            if (ms.ShowDialog() == DialogResult.OK)
            {
                _sendMail.Dispose();
                _sendMail = new sendMail(sAccountIn);// ms.account.Name);
                if (!_sendMail.bIsValidAccount)
                {
                    MessageBox.Show("eMail account invalid. Please setup valid eMail account!");
                }
                else
                {
                    //string sAccount = ms.sAccountName;
                    _sendMail = new sendMail(sAccountIn);
                    //_sendMail.account = ms.account;
                    _sendMail._to = ms.sRecipient;
                }
            }
            if(_sendMail.bIsValidAccount)
                btnSend.Enabled = true;
            else
                btnSend.Enabled = false;
            ms.Dispose();
        }

        private void mnuEnableDocCap_Click(object sender, EventArgs e)
        {
            mnuEnableDocCap.Checked = !mnuEnableDocCap.Checked;
            enableCapture(mnuEnableDocCap.Checked);
        }
        private void enableCapture(bool bEnable)
        {
            if (doccap != null)
            {
                try
                {
                    if (bEnable)
                    {
                        //enable hardware trigger
                        doccap.TriggerEnable = DocumentCapture.TriggerEnableValue.Enable;
                        doccap.EnableDocumentCapture = DocumentCapture.EnableDocumentCaptureValue.EnableWithoutDecodes;
                        try { doccap.SetButtonAction(DocumentCapture.ButtonID.Center, DocumentCapture.ButtonActionType.Camera); }
                        catch { }
                    }
                    else
                    {
                        //disable hardware trigger
                        doccap.TriggerEnable = DocumentCapture.TriggerEnableValue.Disable;
                        doccap.EnableDocumentCapture = DocumentCapture.EnableDocumentCaptureValue.Disable;
                        try { doccap.SetButtonAction(DocumentCapture.ButtonID.Center, DocumentCapture.ButtonActionType.Scan); }
                        catch { }
                    }
                    //doccap.SnapShot();
                }
                catch (DocumentCaptureException ex)
                {
                    System.Windows.Forms.MessageBox.Show("SnapShot() failed: " + ex.Message);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("SnapShot() failed: " + ex.Message);
                }
            }
        }

        private void mnuSendRecv_Click(object sender, EventArgs e)
        {
            mnuSendRecv.Checked = !mnuSendRecv.Checked;
            _sendMail.syncImmediately = mnuSendRecv.Checked;
        }
    }
}