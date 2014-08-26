using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Intermec.DataCollection;

namespace MDIdemo
{
    public partial class mdiSettings : Form
    {
        private DocumentCapture m_docCap;
        private string AppPath;
        bool m_manualTorchSupported = false;
        eMdiMail.ViewFinderSettings viewFinderSettings = new eMdiMail.ViewFinderSettings();

        private void readSettings()
        {
            numBrightness.Value = m_docCap.Brightness;
            numCompQuality.Value = m_docCap.OutputCompressionQuality;
            //colorMode
            //get color mode
            cboColorMode.SelectedItem = m_docCap.ColorMode.ToString();

            //contrast
            comboContrast.SelectedItem = m_docCap.ContrastEnhancement.ToString();

            //ColorModeBrightnessThreshold
            comboBrightnessTreshold.SelectedItem = m_docCap.ColorModeBrightnessThreshold.ToString();

            //ImageLightingCorrection
            if (m_docCap.ImageLightingCorrection == ImageConditioning.ImageLightingCorrectionValue.Enabled)
                checkLightingCorrection.Checked = true;
            else
                checkLightingCorrection.Checked = false;

            //ImageRotation
            comboImageRotation.SelectedItem = m_docCap.ImageRotation.ToString();

            //NoiseReduction
            numNoiseReduction.Value = m_docCap.NoiseReduction;

            //OutputCompression File Type
            comboOutputCompression.SelectedIndex = (int)m_docCap.OutputCompression;

            //TextEnhancement
            comboTextEnhancement.SelectedItem = m_docCap.TextEnhancement.ToString();

            //ImageAreaCaptureRatio
            numImageAreaRatio.Value = m_docCap.ImageAreaCaptureRatio;

            //PerspectiveCorrection
            if (m_docCap.PerspectiveCorrection == DocumentCapture.PerspectiveCorrectionValue.Enable)
                checkPerspectiveCorr.Checked = true;
            else
                checkPerspectiveCorr.Checked = false;

            //Subsampling
            comboSubsampling.Items.Clear();
            byte[] bIn = new byte[4]; int bSize=4;
            byte bGID=0, bFID=0;
            Intermec.DataCollection.ImageConditioning imgCond = new ImageConditioning(bIn, bSize, bGID, bFID);
            
            if (imgCond.ImageConditioningAvailable())
            {
                for (int i = 0; i < 8; i++)
                {
                    comboSubsampling.Items.Add((ImageConditioning.SubsamplingValue)i);
                }
            }
            else
                comboSubsampling.Enabled = false;
            imgCond = null;

            //Next file number (1 to 4294967295)
            numNextFileNum.Value = m_docCap.NextFileNumber;

            txtFileDir.Text = m_docCap.CapturedDocumentLocation;
            txtFileTemplate.Text = m_docCap.FileNameTemplate;

            try
            {
                m_docCap.ManualTorchControl(DocumentCapture.ManualTorchControlType.TorchOn);
                m_manualTorchSupported = true;
            }
            catch (Exception){}
            if (m_manualTorchSupported)
            {
                panelLightning.Enabled = true;
                short torchIntensStep = m_docCap.TorchIncrementValue;
                short torchMaxVal = m_docCap.TorchIntensityMaximumValue;
                short torchMinVal = m_docCap.TorchIntensityMinimumValue;
                short torchCurVal = m_docCap.TorchIntensitySetting;
                numTorchIntensity.Maximum = torchMaxVal;
                numTorchIntensity.Minimum = torchMinVal;
                numTorchIntensity.Value = torchCurVal;
                numTorchIntensity.Increment = torchIntensStep;                
            }
            else
            {
                panelLightning.Enabled = false;
            }

            if (m_docCap.isAutomaticFlashSupported)
                optFlashAuto.Enabled = true;

            if (m_docCap.isManualFlashSupported)
                optFlashAlways.Enabled = true;
            if (m_docCap.isManualFlashSupported && m_docCap.isAutomaticFlashSupported)
            {
                optFlashNone.Enabled = true;
                optFlashAuto.Checked = true;
            }

            if (m_docCap.isFlashConfigSupported)
            {
                numFlashSett.Minimum = m_docCap.MinimumFlashValue;
                numFlashSett.Maximum = m_docCap.MaximumFlashValue;
                numFlashSett.Increment = m_docCap.FlashIncrementValue;
                numFlashSett.Value = m_docCap.FlashSetting;
            }
            else
                numFlashSett.Enabled = false;

            //using xml for Imager Settings - Viewfinder
            chkEnableViewFinder.Checked = viewFinderSettings.EnableViewfinder;
            comboFocusMode.Items.Clear();
            for (int i = 0; i < 3; i++)
            {
                comboFocusMode.Items.Insert(i, (eMdiMail.ViewFinderSettings.eFocusMode)i);
            }
            int iFocusMode = (int)viewFinderSettings.FocusMode;
            comboFocusMode.SelectedIndex = iFocusMode;

            numFocusManual.Value = viewFinderSettings.Manual_focus_value;
            
            if (comboFocusMode.SelectedIndex == (int)eMdiMail.ViewFinderSettings.eFocusMode.fixedFocus)
                numFocusManual.Enabled = true;
            else
                numFocusManual.Enabled = false;

            comboTorchMode.Items.Clear();
            for (int i = 0; i < 3; i++)
            {
                comboTorchMode.Items.Insert(i, (eMdiMail.ViewFinderSettings.eTorchMode)i);
            }
            comboTorchMode.SelectedIndex = (int)viewFinderSettings.TorchMode;

            comboFlashMode.Items.Clear();
            for (int i = 0; i < 3; i++)
                comboFlashMode.Items.Insert(i, (eMdiMail.ViewFinderSettings.eFlashMode)i);
            comboFlashMode.SelectedIndex = (int)viewFinderSettings.FlashMode;
            //####### end of XML based settings

            //Focus Tab
            comboFocusCheck.SelectedItem = m_docCap.FocusCheck.ToString();

            if (m_docCap.isFocusConfigSupported)
            {
                panelFocus.Enabled = true;

                if (m_docCap.isAutomaticSingleFocusSupported)
                {
                    chkFocusSingle.Enabled = true;
                    if (m_docCap.FocusMethod == DocumentCapture.FocusMethodType.AutomaticSingleFocus)
                        chkFocusSingle.Checked = true;
                    else
                        chkFocusSingle.Checked = false;
                }
                else
                    chkFocusSingle.Enabled = false;

                if (m_docCap.isAutomaticContinuousFocusSupported)
                {
                    chkFocusAuto.Enabled = true;
                    if (m_docCap.FocusMethod == DocumentCapture.FocusMethodType.AutomaticContinuousFocus)
                        chkFocusAuto.Checked = true;
                    else
                        chkFocusAuto.Checked = false;
                }
                else
                    chkFocusAuto.Enabled = false;

                if (m_docCap.isManualFocusSupported)
                {
                    chkFocusManual.Enabled = true;
                    if (m_docCap.FocusMethod == DocumentCapture.FocusMethodType.ManualFocus)
                    {
                        chkFocusManual.Checked = true;
                        numFocusManual1.Value = m_docCap.ManualFocusSetting;
                    }
                    else
                        chkFocusManual.Checked = false;
                }
                else
                {
                    chkFocusSingle.Enabled = false;
                    numFocusManual1.Enabled = false;
                }
            }
            else
            {
                panelFocus.Enabled = false;
            }//isFocusConfigSupported

        }
        public mdiSettings(DocumentCapture docCap)
        {
            //get AppPath
            AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (!AppPath.EndsWith(@"\"))
                AppPath += @"\";

            InitializeComponent();
            //open html file
            webBrowser1.Url = new Uri(@"file:\\" +AppPath + @"MDIhelp.htm"); 

            //label17.Text = "Samples:\r\nTime/Date stamp $(time), in the format YYYY-MM-DD HH_MM_SS. ";
            //Document number $(num), which is an automatically incremented number for every file stored.

            cboColorMode.Items.Add("None");
            cboColorMode.Items.Add("Monochrome_Normal");
            cboColorMode.Items.Add("Monochrome_Enhanced_TIFF_Quality");

            comboContrast.Items.Add(ImageConditioning.ContrastEnhancementValue.None.ToString());
            comboContrast.Items.Add(ImageConditioning.ContrastEnhancementValue.Photo.ToString());
            comboContrast.Items.Add(ImageConditioning.ContrastEnhancementValue.Black_text_on_white_background.ToString());
            comboContrast.Items.Add(ImageConditioning.ContrastEnhancementValue.White_text_on_black_background.ToString());

            comboBrightnessTreshold.Items.Add(ImageConditioning.ColorModeBrightnessThresholdValue.VeryDark.ToString());
            comboBrightnessTreshold.Items.Add(ImageConditioning.ColorModeBrightnessThresholdValue.Dark.ToString());
            comboBrightnessTreshold.Items.Add(ImageConditioning.ColorModeBrightnessThresholdValue.Normal.ToString());
            comboBrightnessTreshold.Items.Add(ImageConditioning.ColorModeBrightnessThresholdValue.Bright.ToString());
            comboBrightnessTreshold.Items.Add(ImageConditioning.ColorModeBrightnessThresholdValue.VeryBright.ToString());

            comboImageRotation.Items.Add(ImageConditioning.ImageRotationValue.None.ToString());
            comboImageRotation.Items.Add(ImageConditioning.ImageRotationValue.Degrees_90.ToString());
            comboImageRotation.Items.Add(ImageConditioning.ImageRotationValue.Degrees_180.ToString());
            comboImageRotation.Items.Add(ImageConditioning.ImageRotationValue.Degrees_270.ToString());

            comboOutputCompression.Items.Insert(0, "None");
            comboOutputCompression.Items.Insert(1, "JPEG");
            comboOutputCompression.Items.Insert(2, "TIFF");
            //the following did not work:
            //comboOutputCompression.Items.Add(ImageConditioning.OutputCompressionValue.Jpeg.ToString());
            //comboOutputCompression.Items.Add(ImageConditioning.OutputCompressionValue.Bitmap.ToString());
            //comboOutputCompression.Items.Add(ImageConditioning.OutputCompressionValue.TIFF.ToString());

            comboTextEnhancement.Items.Add(ImageConditioning.TextEnhancementValue.None.ToString());
            comboTextEnhancement.Items.Add(ImageConditioning.TextEnhancementValue.Low.ToString());
            comboTextEnhancement.Items.Add(ImageConditioning.TextEnhancementValue.Medium.ToString());
            comboTextEnhancement.Items.Add(ImageConditioning.TextEnhancementValue.High.ToString());

            comboFocusCheck.Items.Add(DocumentCapture.FocusCheckValue.Disable.ToString());
            comboFocusCheck.Items.Add(DocumentCapture.FocusCheckValue.Medium.ToString());
            comboFocusCheck.Items.Add(DocumentCapture.FocusCheckValue.High.ToString());

            m_docCap = docCap;

            //eMdiMail.ViewFinderSettings vSett = new eMdiMail.ViewFinderSettings();

            readSettings();
        }

        private void applySettings(){
            m_docCap.OutputCompressionQuality = (int)numCompQuality.Value;
            m_docCap.Brightness = (int)numBrightness.Value;
            //set color mode
            m_docCap.ColorMode = (ImageConditioning.ColorModeValue)this.cboColorMode.SelectedIndex;
            //set contrast
            m_docCap.ContrastEnhancement = (ImageConditioning.ContrastEnhancementValue)comboContrast.SelectedIndex;
            //set ColorModeBrightnessThreshold
            m_docCap.ColorModeBrightnessThreshold = (ImageConditioning.ColorModeBrightnessThresholdValue)comboBrightnessTreshold.SelectedIndex;
            //set ImageLightingCorrectionValue
            if(checkLightingCorrection.Checked)
                m_docCap.ImageLightingCorrection = ImageConditioning.ImageLightingCorrectionValue.Enabled;
            else
                m_docCap.ImageLightingCorrection = ImageConditioning.ImageLightingCorrectionValue.None;
            //ImageRotation
            m_docCap.ImageRotation = (ImageConditioning.ImageRotationValue)comboImageRotation.SelectedIndex;
            //NoiseReduction
            m_docCap.NoiseReduction = (int)numNoiseReduction.Value;
            //OutputCompressionValue
            m_docCap.OutputCompression = (ImageConditioning.OutputCompressionValue)comboOutputCompression.SelectedIndex;
            //TextEnhancement
            m_docCap.TextEnhancement = (ImageConditioning.TextEnhancementValue)comboTextEnhancement.SelectedIndex;
            //FocusCheck
            m_docCap.FocusCheck = (DocumentCapture.FocusCheckValue)comboFocusCheck.SelectedIndex;
            //ImageAreaCaptureRatio
            m_docCap.ImageAreaCaptureRatio = (int)numImageAreaRatio.Value;
            //PerspectiveCorrection
            if(checkPerspectiveCorr.Checked)
                m_docCap.PerspectiveCorrection= DocumentCapture.PerspectiveCorrectionValue.Enable;
            else
                m_docCap.PerspectiveCorrection = DocumentCapture.PerspectiveCorrectionValue.Disable;

            //Next file number (1 to 4294967295)
            m_docCap.NextFileNumber = (int)numNextFileNum.Value;

            m_docCap.CapturedDocumentLocation = txtFileDir.Text;
            m_docCap.FileNameTemplate=txtFileTemplate.Text ;

            //######### torch
            if (m_manualTorchSupported)
            {
                if(chkTorchControl.Checked)
                    m_docCap.ManualTorchControl(DocumentCapture.ManualTorchControlType.TorchOn);
                else
                    m_docCap.ManualTorchControl(DocumentCapture.ManualTorchControlType.TorchOff);
                m_docCap.TorchIntensitySetting = (short)numTorchIntensity.Value;
            }

            //######### FLASH
            if (m_docCap.isAutomaticFlashSupported)
                optFlashAuto.Enabled = true;

            if (m_docCap.isManualFlashSupported)
            {
                m_docCap.FlashMethod = DocumentCapture.FlashMethodType.AlwaysFlash;                
            }
            if (m_docCap.isManualFlashSupported && m_docCap.isAutomaticFlashSupported)
            {
                if (optFlashNone.Enabled)
                    if (optFlashAuto.Checked)
                        m_docCap.FlashMethod = DocumentCapture.FlashMethodType.AutomaticallyFlash;
            }

            if (m_docCap.isFlashConfigSupported)
            {
                m_docCap.FlashSetting = (short)numFlashSett.Value;
            }
            else
                numFlashSett.Enabled = false;

            viewFinderSettings.EnableViewfinder = chkEnableViewFinder.Checked;
            viewFinderSettings.FlashMode = (eMdiMail.ViewFinderSettings.eFlashMode) comboFlashMode.SelectedIndex;
            viewFinderSettings.FocusMode = (eMdiMail.ViewFinderSettings.eFocusMode)comboFocusMode.SelectedIndex;
            viewFinderSettings.Manual_focus_value = (int)numFocusManual.Value;
            viewFinderSettings.TorchMode = (eMdiMail.ViewFinderSettings.eTorchMode)comboTorchMode.SelectedIndex;

            //########## focus
            if (m_docCap.isFocusConfigSupported)
            {
                panelFocus.Enabled = true;

                if (m_docCap.isAutomaticSingleFocusSupported)
                {
                    if (chkFocusSingle.Checked)
                        m_docCap.FocusMethod = DocumentCapture.FocusMethodType.AutomaticSingleFocus;
                }

                if (m_docCap.isAutomaticContinuousFocusSupported)
                {
                    if (chkFocusAuto.Checked)
                        m_docCap.FocusMethod = DocumentCapture.FocusMethodType.AutomaticContinuousFocus;
                }

                if (m_docCap.isManualFocusSupported)
                {
                    if (chkFocusManual.Checked)
                    {
                        m_docCap.FocusMethod = DocumentCapture.FocusMethodType.ManualFocus;
                        m_docCap.ManualFocusSetting = (short)numFocusManual1.Value;
                    }
                }
            }//isFocusConfigSupported
            
        }

        private void mnuOK_Click(object sender, EventArgs e)
        {
            applySettings();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            DirChooser dlg = new DirChooser(txtFileDir.Text);
            if (dlg.ShowDialog() == DialogResult.OK)
                txtFileDir.Text = dlg.directory;
            this.Close();
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSelectDir_Click_1(object sender, EventArgs e)
        {
            DirChooser myDir = new DirChooser(m_docCap.CapturedDocumentLocation);
            if (myDir.ShowDialog() == DialogResult.OK)
            {
                m_docCap.CapturedDocumentLocation = myDir.directory;
                txtFileDir.Text = m_docCap.CapturedDocumentLocation;
            }
            myDir.Dispose();
        }

        private void mButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter="*.mdi|*.mdi";
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string xmlFile = dlg.FileName;
                SSAPI2.SSapi2.saveSettings(ref m_docCap, xmlFile); 
            }
            dlg.Dispose();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter="*.mdi|*.mdi";
            if(dlg.ShowDialog()==DialogResult.OK){
                string xmlFile=dlg.FileName;
                if (SSAPI2.SSapi2.loadSettings(ref m_docCap, xmlFile))
                    readSettings();
                else
                    MessageBox.Show("Could not load settings");
            }
            dlg.Dispose();
        }

        private void btnIntermecSettings_Click(object sender, EventArgs e)
        {
            /*
            //FAILS:
            \windows\intermecsettings.exe /P\Windows\itcReaderDataModel.xml /ip=127.0.0.1 "/k Data Collection\Scanners\Imager Settings\Document Imaging"

            //OK:
            \windows\intermecsettings.exe /P\Windows\itcReaderDataModel.xml /ip=127.0.0.1 "/kData Collection\Scanners" \"/kData Collection\\Scanners\"
            */
            MessageBox.Show("If you changed settings, please apply first with [OK] and re-open this dialog.\nAfter changing settings using IntermecSettings, this dialog will be closed automatically!\nIn Intermec Settings navigate to /Data Collection/Camera/Camera Settings/Document Imaging");
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("\\Windows\\intermecSettings.exe", "/P\\Windows\\itcReaderDataModel.xml /ip=127.0.0.1 ");
            System.Diagnostics.Process.Start(psi);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void numFocusManual_ValueChanged(object sender, EventArgs e)
        {
            labelDistance.Text = viewFinderSettings.FocusDistanceStr((int)numFocusManual.Value);
        }

        private void comboFocusMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFocusMode.SelectedIndex == (int)eMdiMail.ViewFinderSettings.eFocusMode.fixedFocus)
            {
                numFocusManual.Enabled = true;
                labelDistance.Text = viewFinderSettings.FocusDistanceStr((int)numFocusManual.Value);
            }
        }

        private void chkFocusManual_CheckStateChanged(object sender, EventArgs e)
        {
            numFocusManual1.Enabled = chkFocusManual.Checked;

        }
    }
}