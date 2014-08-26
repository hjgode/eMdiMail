using System;
using System.Collections.Generic;
using System.Text;
using Intermec.DeviceManagement.SmartSystem;
using Intermec.DataCollection;
using System.IO;

//using System.Runtime.InteropServices;
/*
				<Group Name="Document Imaging">
					<Field Name="Enable Document Imaging">0</Field>
					<Field Name="Focus Check">0</Field>
					<Field Name="Image/Area to Capture Ratio">80</Field>
					<Field Name="Output Compression">1</Field>
					<Field Name="Output Compression Quality">90</Field>
					<Field Name="Perspective Correction">1</Field>
					<Field Name="Output Format">1</Field>
					<Field Name="Color Mode Brightness Threshold">2</Field>
					<Field Name="Contrast Enhancement">2</Field>
					<Field Name="Text Enhancement">2</Field>
					<Field Name="Noise Reduction">6</Field>
					<Field Name="Image Rotation">0</Field>
					<Field Name="Anti-vignetting">1</Field>
					<Field Name="Image File Location">\My Documents\MDI</Field>
					<Field Name="Document File Name">doc_$(num)</Field>
					<Field Name="Folder Memory Limit">15</Field>
				</Group>
*/
namespace SSAPI2
{
    class SSapi2
    {
        public static string sMDIFolder;
        public static bool saveSettings(ref DocumentCapture docCap, string sXMLfile)
        {
            //ITCSSApi ss = new ITCSSApi();
            string ssConfig = "";
            ssConfig = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><DevInfo Action=\"Set\">\r\n";
            ssConfig = "<Subsystem Name=\"Data Collection\">\r\n";
            ssConfig += "	<Group Name=\"Scanners\" Instance=\"0\">\r\n";
            ssConfig += "		<Group Name=\"Imager Settings\">\r\n";
            ssConfig += "			<Group Name=\"Document Imaging\">\r\n";
            ssConfig += string.Format("				<Field Name=\"Focus Check\">{0}</Field>\r\n", (int)docCap.FocusCheck);
            ssConfig += string.Format("				<Field Name=\"Image/Area to Capture Ratio\">{0}</Field>\r\n", (int)docCap.ImageAreaCaptureRatio);
            ssConfig += string.Format("				<Field Name=\"Output Compression\">{0}</Field>\r\n",(int)docCap.OutputCompression);
            ssConfig += string.Format("				<Field Name=\"Output Compression Quality\">{0}</Field>\r\n",(int)docCap.OutputCompressionQuality);
            ssConfig += string.Format("				<Field Name=\"Perspective Correction\">{0}</Field>\r\n", (int)docCap.PerspectiveCorrection);
            ssConfig += string.Format("				<Field Name=\"Output Format\">0</Field>\r\n", (int)docCap.OutputCompression); //see docCap.OutputCompression, looks like used twice
            ssConfig += string.Format("				<Field Name=\"Color Mode Brightness Threshold\">{0}</Field>\r\n", (int)docCap.ColorModeBrightnessThreshold);
            ssConfig += string.Format("				<Field Name=\"Contrast Enhancement\">{0}</Field>\r\n", (int)docCap.ContrastEnhancement);
            ssConfig += string.Format("				<Field Name=\"Text Enhancement\">{0}</Field>\r\n",(int)docCap.TextEnhancement);
            ssConfig += string.Format("				<Field Name=\"Noise Reduction\">{0}</Field>\r\n",(int)docCap.NoiseReduction);
            ssConfig += string.Format("				<Field Name=\"Image Rotation\">{0}</Field>\r\n", (int)docCap.ImageRotation);
            //ssConfig += string.Format("				<Field Name=\"Anti-vignetting\">1</Field>\r\n", (int)docCap.;
            ssConfig += string.Format("				<Field Name=\"Image File Location\">{0}</Field>\r\n", escapeXML(docCap.CapturedDocumentLocation)); //do not use quotes around strings
            ssConfig += string.Format("				<Field Name=\"Document File Name\">{0}</Field>\r\n", escapeXML(docCap.FileNameTemplate)); //do not use quotes around strings
            ssConfig += string.Format("				<Field Name=\"Folder Memory Limit\">{0}</Field>\r\n", (int)docCap.FolderMemoryLimit);

            ssConfig += "			</Group>\r\n";
            ssConfig += "		</Group>\r\n";
            ssConfig += "	</Group>\r\n";
            ssConfig += "</Subsystem>\r\n";


            try
            {
                using (StreamWriter sw = new StreamWriter(sXMLfile))
                {
                    sw.WriteLine(ssConfig);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception) { }

            return true;
        }

        public static bool loadSettings(ref DocumentCapture docCap, string sXMLfile)
        {
            bool bRet = false;
            loadSettings(sXMLfile);
            return bRet;
        }

        public static bool loadSettings(string sXMLfile)
        {
            try
            {
                ITCSSApi ss;
                ss = new ITCSSApi();
                string strXml="";
                string line = "";
                int ssSize = 4096;
                StringBuilder sbRetData = new StringBuilder(ssSize);
                uint uiRet = 0;
                //get the settings from file
                //ss.ConfigFromFile(sXMLfile, sRet, sbRetData, ref ssSize, 2000);

                //read settings from file into string and apply them
                StreamReader sr = new StreamReader(sXMLfile);
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        strXml += line + "\r\n";
                    };
                    sr.Close();
                }
                catch (Exception x) {
                    System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()\r\nException during readFile: " + x.Message);
                    return false;
                }
                uiRet = ss.Set(strXml, sbRetData, ref ssSize, 2000);
                if (uiRet != ITCSSErrors.E_SS_SUCCESS)
                {
                    if(uiRet == ITCSSErrors.E_SSAPI_FUNCTION_UNAVAILABLE){
                            System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()\r\nFunction is unavailable via the SmartSystems API.");
                    }
                    else if(uiRet == ITCSSErrors.E_SSAPI_OPERATION_FAILED){
                            System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()\r\nThe operation failed.");
                    }
                    else if(uiRet == ITCSSErrors.E_SSAPI_MISSING_REQUIRED_PARM){
                            System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()\r\nRequired parameter is missing.");
                    }
                    else if(uiRet == ITCSSErrors.E_SSAPI_MALFORMED_XML){
                            System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()\r\nXML is incorrectly formatted.");
                    }
                    else if(uiRet == ITCSSErrors.E_SSAPI_TIMEOUT){
                            System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()\r\nOperation timed out.");
                    }
                    // System.Windows.Forms.MessageBox.Show("Error in ConfigFromFile()");
                }
            }
            catch (Exception x) {
                System.Windows.Forms.MessageBox.Show("Exception in loadSettings: "+x.Message);
            }
            return true;
        }
        public static string getSettings(string s)
        {
            string sRet = "";
            string ssConfig = "";
            ssConfig = "<Subsystem Name=\"Data Collection\">";
            ssConfig += "	<Group Name=\"Scanners\" Instance=\"0\">";
            ssConfig += "		<Group Name=\"Imager Settings\">";
            ssConfig += "			<Group Name=\"Document Imaging\">";
            ssConfig += "				<Field Name=\"Enable Document Imaging\">0</Field>";
            ssConfig += "				<Field Name=\"Focus Check\">1</Field>";
            ssConfig += "				<Field Name=\"Image/Area to Capture Ratio\">15</Field>";
            ssConfig += "				<Field Name=\"Output Compression\">1</Field>";
            ssConfig += "				<Field Name=\"Output Compression Quality\">50</Field>";
            ssConfig += "				<Field Name=\"Perspective Correction\">1</Field>";
            ssConfig += "				<Field Name=\"Output Format\">0</Field>";
            ssConfig += "				<Field Name=\"Color Mode Brightness Threshold\">2</Field>";
            ssConfig += "				<Field Name=\"Contrast Enhancement\">2</Field>";
            ssConfig += "				<Field Name=\"Text Enhancement\">2</Field>";
            ssConfig += "				<Field Name=\"Noise Reduction\">7</Field>";
            ssConfig += "				<Field Name=\"Image Rotation\">0</Field>";
            ssConfig += "				<Field Name=\"Anti-vignetting\">1</Field>";
            ssConfig += "				<Field Name=\"Image File Location\">\\My Documents\\MDI</Field>";
            ssConfig += "				<Field Name=\"Document File Name\">doc_$(num)</Field>";
            ssConfig += "				<Field Name=\"Folder Memory Limit\">15</Field>";
            ssConfig += "			</Group>";
            ssConfig += "		</Group>";
            ssConfig += "	</Group>";
            ssConfig += "</Subsystem>";

            try
            {
                ITCSSApi ss;
                ss = new ITCSSApi();
                int ssSize = 4096;
                StringBuilder sbRetData = new StringBuilder(ssSize);
                uint uiRet = 0;
                ss.Get(ssConfig, sbRetData, ref ssSize, 2000);
                if (uiRet == ITCSSErrors.E_SS_SUCCESS)
                {
                    //bool bFocusCheck;
                    sRet += "Focus Check: " + SSapi2.getBoolSetting(sbRetData, "Focus Check").ToString();

                    int iAreaRatio = getIntSetting(sbRetData, "Image/Area to Capture Ratio");
                    sRet += "\r\nImage/Area Ratio: " + iAreaRatio.ToString();

                    int iOutComp = SSapi2.getIntSetting(sbRetData, "Output Compression");
                    sRet += "\r\nOutput Compression: " + iOutComp.ToString();

                    sRet += "\r\nOutput Compression Quality: " + SSapi2.getIntSetting(sbRetData, "Output Compression Quality").ToString();

                    sRet += "\r\nPerspective Correction: " + SSapi2.getBoolSetting(sbRetData, "Perspective Correction").ToString();

                    //Color Mode Brightness Threshold
                    sRet += "\r\nColor Mode Brightness Threshold: " + SSapi2.getIntSetting(sbRetData, "Color Mode Brightness Threshold").ToString();

                    sRet += "\r\nContrast Enhancement: " + SSapi2.getIntSetting(sbRetData, "Contrast Enhancement").ToString();

                    //Text Enhancement
                    sRet += "\r\nText Enhancement: " + SSapi2.getIntSetting(sbRetData, "Text Enhancement").ToString();
                    //Noise Reduction
                    sRet += "\r\nNoise Reduction: " + SSapi2.getIntSetting(sbRetData, "Noise Reduction").ToString();
                    //Image Rotation
                    sRet += "\r\nImage Rotation: " + SSapi2.getIntSetting(sbRetData, "Image Rotation").ToString();
                    //Anti-vignetting
                    sRet += "\r\nAnti-vignetting: " + SSapi2.getIntSetting(sbRetData, "Anti-vignetting").ToString();
                    //Image File Location getStrSetting
                    sMDIFolder = SSapi2.getStrSetting(sbRetData, "Image File Location");
                    //if (!sMDIFolder.EndsWith("\\"))
                    //    sMDIFolder+="\\";
                    sRet += "\r\nImage File Location: " + sMDIFolder;
                }
                else
                {
                    sRet = "Error reading Settings: " + uiRet.ToString();
                }
            }
            catch (SystemException sx)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + sx.Message + ". Did you miss to install the runtimes?");
            }
            //ssConfigText = sRet;
            s = sRet;
            return s;
        }

        public static string getStrSetting(StringBuilder sb, String sField)
        {
            string sVal = sb.ToString();
            string sFieldString = "<Field Name=\"" + sField + "\">";
            int iIdx = sVal.IndexOf(sFieldString);
            string sResult = "";
            if (iIdx >= 0)
            {
                string fieldStr = sVal.Substring(iIdx);
                string ssidStr = fieldStr.Remove(0, sFieldString.Length);
                iIdx = ssidStr.IndexOf("<");
                sResult = ssidStr.Substring(0, iIdx);
            }
            return sResult;
        }

        public static int getIntSetting(StringBuilder sb, String sField)
        {
            string sVal = sb.ToString();
            string sFieldString = "<Field Name=\"" + sField + "\">";
            int iIdx = sVal.IndexOf(sFieldString);
            int iRes = -1;
            if (iIdx >= 0)
            {
                string fieldStr = sVal.Substring(iIdx);
                string ssidStr = fieldStr.Remove(0, sFieldString.Length);
                iIdx = ssidStr.IndexOf("<");
                string sResult = ssidStr.Substring(0, iIdx);
                try
                {
                    iRes = Convert.ToInt16(sResult);
                }
                catch (Exception)
                {
                }
            }
            return iRes;
        }
        public static bool getBoolSetting(StringBuilder sb, String sField)
        {
            string sVal = sb.ToString();
            string sFieldString = "<Field Name=\"" + sField + "\">";
            int iIdx = sVal.IndexOf(sFieldString);
            if (iIdx >= 0)
            {
                string fieldStr = sVal.Substring(iIdx);
                string ssidStr = fieldStr.Remove(0, sFieldString.Length);
                iIdx = ssidStr.IndexOf("<");
                string sResult = ssidStr.Substring(0, iIdx);
                if (sResult.Equals("1"))
                    return true;
            }
            return false;
        }
        public static string unEscapeXML(string xmlString)
        {
            //Dim xmlString As String = "&lt;&gt;&amp;"
            xmlString = xmlString.Replace("&lt;", "<");
            xmlString = xmlString.Replace("&gt;", ">");
            xmlString = xmlString.Replace("&amp;", "&");
            xmlString = xmlString.Replace("&quot;", "\"");
            xmlString = xmlString.Replace("&apos;", "'");
            //xmlString now is "<>&"

            //Dim i As Integer = 0
            //Dim sRet As String = sXML
            //sRet = sRet.Replace("&amp;", "&")
            //sRet = sRet.Replace("&gt;", ">")
            //sRet = sRet.Replace("&lt;", "<")
            //sRet = sRet.Replace("&quot;", """")
            //Return sRet
            return xmlString;
        }

        public static string escapeXML(string xmlString)
        {
            //Dim xmlString As String = "<>&"
            xmlString = xmlString.Replace("<", "&lt;");
            xmlString = xmlString.Replace(">", "&gt;");
            xmlString = xmlString.Replace("&", "&amp;");
            xmlString = xmlString.Replace("\"", "&quot;");
            xmlString = xmlString.Replace("'", "&apos;");
            //xmlString now is "&lt;&gt;&amp;"

            //Dim i As Integer = 0
            //Dim sRet As String = ""
            //Dim c As String = ""
            //Do
            //    c = sXML.Substring(i, 1)
            //    If c = "&" Then
            //        c = "&amp;"
            //    ElseIf c = "<" Then
            //        c = "&lt;"
            //    ElseIf c = ">" Then
            //        c = "&gt;"
            //    ElseIf c = """" Then
            //        c = "&quot;"
            //    End If
            //    sRet += c
            //    i = i + 1
            //Loop While i < sXML.Length
            //Return sRet
            return xmlString;
        }

    }
}

//namespace CFBeep
//{
//    class Beeper
//    {
//        [DllImport("coredll.dll")]
//        public static extern int PlaySound(
//            string szSound,
//            IntPtr hModule,
//            int flags);


//        private void button1_Click(object sender, EventArgs e)
//        {
//            PlaySound(@"\Windows\Voicbeep", IntPtr.Zero, (int)(PlaySoundFlags.SND_FILENAME | PlaySoundFlags.SND_SYNC));

//        }
//        public enum MessageBeepType
//        {
//            Default = -1,
//            Ok = 0x00000000,
//            Error = 0x00000010,
//            Question = 0x00000020,
//            Warning = 0x00000030,
//            Information = 0x00000040,
//        }

//        [DllImport("coredll.dll", SetLastError = true)]
//        public static extern bool MessageBeep(
//            MessageBeepType type
//        );
//    }

//    public enum PlaySoundFlags : int
//    {
//        SND_SYNC = 0x0,     // play synchronously (default)
//        SND_ASYNC = 0x1,    // play asynchronously
//        SND_NODEFAULT = 0x2,    // silence (!default) if sound not found
//        SND_MEMORY = 0x4,       // pszSound points to a memory file
//        SND_LOOP = 0x8,     // loop the sound until next sndPlaySound
//        SND_NOSTOP = 0x10,      // don't stop any currently playing sound
//        SND_NOWAIT = 0x2000,    // don't wait if the driver is busy
//        SND_ALIAS = 0x10000,    // name is a registry alias
//        SND_ALIAS_ID = 0x110000,// alias is a predefined ID
//        SND_FILENAME = 0x20000, // name is file name
//        SND_RESOURCE = 0x40004, // name is resource name or atom
//    }
//}