using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Intermec.DeviceManagement.SmartSystem;
using SSAPI2;

namespace eMdiMail
{
    /*
    <Subsystem Name="Data Collection">
        <Group Name="Scanners" Instance="18">
            <Group Name="Imager Settings">
				<Group Name="Viewfinder Properties">
					<Field Name="Enable viewfinder">0</Field>
					<Field Name="Focus mode">2</Field>
<!--    
                        <Enumeration Value="Autofocus - continuous" Position="1"  />
                        <Enumeration Value="Autofocus - single try" Position="2"  />
                        <Enumeration Value="Fixed focus - manual" Position="0"  />
-->
					<Field Name="Manual focus value">65</Field>
				</Group>
     * 
				<Group Name="Lighting">
					<Field Name="Flash">2</Field>
<!--    

                        <Enumeration Value="Off" Position="0"  />
                        <Enumeration Value="On" Position="1"  />
                        <Enumeration Value="Auto" Position="2"  />
-->
 
					<Field Name="Torch">2</Field>

<!--    
                        <Enumeration Value="Off" Position="0"  />
                        <Enumeration Value="On" Position="1"  />
                        <Enumeration Value="Auto" Position="2"  />
-->

				</Group>
			</Group>
		</Group>
    </Subsystem>
    */
    public class ViewFinderSettings
    {
        public enum eFocusMode
        {
            none = -1,
            fixedFocus = 0,
            singleTry = 1,
            continuous = 2,
        }
        bool _EnableViewfinder = false;
        public bool EnableViewfinder
        {
            get {
                string s = readSettings();
                int iSett=-1;
                if(s!="")
                    iSett = SSAPI2.SSapi2.getIntSetting(new StringBuilder(s), "Enable viewfinder");
                if (iSett > 0)
                    _EnableViewfinder = true;
                else
                    _EnableViewfinder = false;

                return _EnableViewfinder; 
            }
            set {
                _EnableViewfinder = value;
                string sXml = "";
                sXml += "    <Subsystem Name=\"Data Collection\">\r\n";
                sXml += "        <Group Name=\"Scanners\" Instance=\"18\">\r\n";
                sXml += "            <Group Name=\"Imager Settings\">\r\n";
                sXml += "				<Group Name=\"Viewfinder Properties\">\r\n";
                int iViewFinder;
                if (_EnableViewfinder)
                    iViewFinder = 1;
                else
                    iViewFinder = 0;
                sXml += "				<Field Name=\"Enable viewfinder\">" + iViewFinder.ToString() + "</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "			</Group>\r\n";
                sXml += "		</Group>\r\n";
                sXml += "    </Subsystem>\r\n";
                changeSetting(sXml);
            }
        }
        eFocusMode _FocusMode = eFocusMode.none;
        public eFocusMode FocusMode
        {
            get {
                string s = readSettings(); //"\r\n <Subsystem Name=\"Data Collection\">\r\n  <Group Name=\"Scanners\" Instance=\"18\">\r\n   <Group Name=\"Imager Settings\">\r\n    <Group Name=\"Viewfinder Properties\">\r\n     <Field Name=\"Enable viewfinder\">1</Field>\r\n     <Field Name=\"Focus mode\">0</Field>\r\n     <Field Name=\"Manual focus value\">130</Field>\r\n    </Group>\r\n    <Group Name=\"Lighting\">\r\n     <Field Name=\"Flash\">2</Field>\r\n     <Field Name=\"Torch\">2</Field>\r\n    </Group>\r\n   </Group>\r\n  </Group>\r\n </Subsystem>\r\n"
                int iSett = -1;
                if (s != "")
                    iSett = SSAPI2.SSapi2.getIntSetting(new StringBuilder(s), "Focus mode");
                if (iSett >= 0)
                    _FocusMode = (eFocusMode)iSett;
                else
                    _FocusMode=eFocusMode.none;

                return _FocusMode;
            }
            set {
                _FocusMode = value;
                string sXml = "";
                sXml += "    <Subsystem Name=\"Data Collection\">\r\n";
                sXml += "        <Group Name=\"Scanners\" Instance=\"18\">\r\n";
                sXml += "            <Group Name=\"Imager Settings\">\r\n";
                sXml += "				<Group Name=\"Viewfinder Properties\">\r\n";
                int iVal =(int)_FocusMode;
                sXml += "					<Field Name=\"Focus mode\">"+iVal.ToString()+"</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "			</Group>\r\n";
                sXml += "		</Group>\r\n";
                sXml += "    </Subsystem>\r\n";
                changeSetting(sXml);
            }
        }

        int _manualFocusValue=65;
        /*
        Focus Value	Distance to Target Object
        255		< 2.5"
        200 – 255 (225)	2.5" (Postage Stamp)
        150 – 200 (175)	3.5"
        140		3.5 – 4"
        130		4.5" (Business Card)
        120		5.5"
        110		6.5"
        100		7.5" (Shipping Label)
        90		8.5"
        80		9.5"
        65		10.5 – 32" (Document)
        45		32 – 90" (Poster)
        15		> 90" 
        */

        public string FocusDistanceStr(int focusValue)
        {
            string s = "n/a";
            if (focusValue==255)
                    s = "<2.5\"";   //< 5cm
            else if(focusValue<255 && focusValue>=200)
                    s="2.5\"";      //  5cm
            else if (focusValue >= 150 && focusValue < 200)
                s = "3.5\"";        //  9cm
            else if (focusValue >= 140 && focusValue < 150)
                s = "3.5-4.0\"";    //9-10cm
            else if (focusValue >= 130 && focusValue < 140)
                s = "4.5\"";        //11,4cm
            else if (focusValue >= 120 && focusValue < 130)
                s = "5.5\"";        //14cm
            else if (focusValue >= 110 && focusValue < 120)
                s = "6.5\"";        //17cm
            else if (focusValue >= 100 && focusValue < 110)
                s = "7.5\"";        //19cm
            else if (focusValue >= 90 && focusValue < 100)
                s = "8.5\"";        //22cm
            else if (focusValue >= 80 && focusValue < 90)
                s = "9.5\"";        //24cm
            else if (focusValue >= 65 && focusValue < 80)
                s = "10.5-32\"";    //26-80cm
            else if (focusValue >= 45 && focusValue < 65)
                s = "32-90\"";
            else if (focusValue >= 15 && focusValue < 45)
                s = ">90\"";
            return s;
        }
        public int Manual_focus_value
        {
            get
            {
                string s = readSettings();
                int iSett = -1;
                if (s != "")
                    iSett = SSAPI2.SSapi2.getIntSetting(new StringBuilder(s), "Manual focus value");
                if (iSett >= 0)
                    _manualFocusValue = iSett;
                else
                    _manualFocusValue = 65;
                return _manualFocusValue;
            }
            set
            {
                if (value < 15 || value > 255)
                    return;
                _manualFocusValue = value;
                string sXml = "";
                sXml += "    <Subsystem Name=\"Data Collection\">\r\n";
                sXml += "        <Group Name=\"Scanners\" Instance=\"18\">\r\n";
                sXml += "            <Group Name=\"Imager Settings\">\r\n";
                sXml += "				<Group Name=\"Viewfinder Properties\">\r\n";
                int iVal =(int)_manualFocusValue;
                sXml += "					<Field Name=\"Manual focus value\">" + iVal.ToString() + "</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "			</Group>\r\n";
                sXml += "		</Group>\r\n";
                sXml += "    </Subsystem>\r\n";
                changeSetting(sXml);
            }
        }

        //######## Lightning modes
        public enum eFlashMode
        {
            none = -1,
            Off = 0,
            On = 1,
            Auto = 2,
        }
        public enum eTorchMode
        {
            none = -1,
            Off = 0,
            On = 1,
            Auto = 2,
        }
        eFlashMode _FlashMode = eFlashMode.Off;
        public eFlashMode FlashMode
        {
            get
            {
                string s = readSettings();
                int iSett = -1;
                if (s != "")
                    iSett = SSAPI2.SSapi2.getIntSetting(new StringBuilder(s), "Flash");
                if (iSett >= 0)
                    _FlashMode = (eFlashMode)iSett;
                else
                    _FlashMode = eFlashMode.Off;
                return _FlashMode;
            }
            set
            {
                _FlashMode = value;
                string sXml = "";
                sXml += "    <Subsystem Name=\"Data Collection\">\r\n";
                sXml += "        <Group Name=\"Scanners\" Instance=\"18\">\r\n";
                sXml += "            <Group Name=\"Imager Settings\">\r\n";
                sXml += "				<Group Name=\"Lighting\">\r\n";
                int iVal = (int)_FlashMode;
                sXml += "					<Field Name=\"Flash\">" + iVal.ToString() + "</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "			</Group>\r\n";
                sXml += "		</Group>\r\n";
                sXml += "    </Subsystem>\r\n";
                changeSetting(sXml);
            }
        }
        eTorchMode _TorchMode = eTorchMode.Off;
        public eTorchMode TorchMode
        {
            get
            {
                string s = readSettings();
                int iSett = -1;
                if (s != "")
                    iSett = SSAPI2.SSapi2.getIntSetting(new StringBuilder(s), "Flash");
                if (iSett >= 0)
                    _TorchMode = (eTorchMode)iSett;
                else
                    _TorchMode = eTorchMode.Off;
                return _TorchMode;
            }
            set
            {
                _TorchMode = value;
                string sXml = "";
                sXml += "    <Subsystem Name=\"Data Collection\">\r\n";
                sXml += "        <Group Name=\"Scanners\" Instance=\"18\">\r\n";
                sXml += "            <Group Name=\"Imager Settings\">\r\n";
                sXml += "				<Group Name=\"Lighting\">\r\n";
                int iVal = (int)_TorchMode;
                sXml += "					<Field Name=\"Torch\">" + iVal.ToString() + "</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "			</Group>\r\n";
                sXml += "		</Group>\r\n";
                sXml += "    </Subsystem>\r\n";
                changeSetting(sXml);
            }

        }
        string readSettings()
        {
            string s = sXmlViewfinderLightning.sXML;
            StringBuilder sb = new StringBuilder(1024);
            int sbSize = 1024;
            Intermec.DeviceManagement.SmartSystem.ITCSSApi ssAPI = new ITCSSApi();
            uint uiRet = 0;
            if ((uiRet = ssAPI.Get(s, sb, ref sbSize, 0)) == Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
                return sb.ToString();
            else
            {
                System.Diagnostics.Debug.WriteLine("SSAPI Error: " + uiRet.ToString());
                return "";
            }
        }
        bool changeSetting(string xml)
        {
            StringBuilder sb = new StringBuilder(1024);
            int sbSize = 1024;
            Intermec.DeviceManagement.SmartSystem.ITCSSApi ssAPI = new ITCSSApi();
            uint uiRet = 0;
            if ((uiRet = ssAPI.Set(xml, sb, ref sbSize, 0)) == Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
                return true;
            else
            {
                System.Diagnostics.Debug.WriteLine("SSAPI Error: " + uiRet.ToString());
                return false;
            }
        }

    }
    static class sXmlViewfinderLightning
    {
        public static string sXML
        {
            get
            {
                string sXml="";
                sXml += "    <Subsystem Name=\"Data Collection\">\r\n";
                sXml += "        <Group Name=\"Scanners\" Instance=\"18\">\r\n";
                sXml += "            <Group Name=\"Imager Settings\">\r\n";
                sXml += "				<Group Name=\"Viewfinder Properties\">\r\n";
                sXml += "					<Field Name=\"Enable viewfinder\">0</Field>\r\n";
                sXml += "					<Field Name=\"Focus mode\">2</Field>\r\n";
                sXml += "					<Field Name=\"Manual focus value\">65</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "				<Group Name=\"Lighting\">\r\n";
                sXml += "					<Field Name=\"Flash\">2</Field>\r\n";
                sXml += "					<Field Name=\"Torch\">2</Field>\r\n";
                sXml += "				</Group>\r\n";
                sXml += "			</Group>\r\n";
                sXml += "		</Group>\r\n";
                sXml += "    </Subsystem>\r\n";
                return sXml;
            }
        }
    }
}
