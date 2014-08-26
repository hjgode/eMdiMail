using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

using System.Reflection;

using Intermec.Multimedia;

namespace hgo.CameraSettings
{
    public class CamSettings
    {
        Camera _cam;
        public CamSettings(ref Camera cam){
            _cam = cam;
        }
        public string dump
        {
            get
            {
                string sOut = "";
                Type type = _cam.GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    try
                    {
                        sOut += propertyInfo.Name;
                        try
                        {
                            sOut += "|'" + propertyInfo.GetValue(_cam, null).ToString() + "' / '" + propertyInfo.PropertyType.FullName + "'\r\n";
                            ;
                        }
                        catch (Exception)
                        {
                            sOut += "|''\r\n";
                        }
                    }
                    catch (InvalidProgramException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
                return sOut;
            }
        }
        public void saveBIN()
        {
            Type type = _cam.GetType();
            PropertyInfo[] properties = type.GetProperties();
            TextWriter tw = new StreamWriter(appPath + "settings.dat");
            foreach (PropertyInfo propertyInfo in properties)
            {
                try
                {
                    tw.Write(propertyInfo.Name);
                    System.Diagnostics.Debug.WriteLine("++++++ Got '" + propertyInfo.Name + "'");
                    try
                    {
                        tw.WriteLine ("|" + propertyInfo.GetValue(_cam, null));
                    }
                    catch (InvalidProgramException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("InvalidProgramException in Properties Name for '" + propertyInfo.Name + "', " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception in Properties Name for '" + propertyInfo.Name + "', " + ex.Message);
                    }
                }
                catch (InvalidProgramException ex)
                {
                    System.Diagnostics.Debug.WriteLine("InvalidProgramException in Properties GetValue for '" + propertyInfo.Name + "', " + ex.Message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in Properties GetValue for '" + propertyInfo.Name + "', " + ex.Message);
                }
            }
            tw.Close();
        }
        private string appPath
        {
            get {
                string AppPath;
                AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                if (!AppPath.EndsWith(@"\"))
                    AppPath += @"\";
                return AppPath;
            }
        }
        
        [XmlRootAttribute("CameraSettings")]
        public class settings
        {
            #region properties
            [XmlElement("brightness")]
            public int Brightness;

            [XmlElement]
            public string brightnessCmt = "the brightness setting";
            [XmlElement("BrightnessAuto")]
            public bool BrightnessAuto;

            [XmlElement("flash")]
            public int flash;
            [XmlElement]
            public string flashCmt = "'Off' Position='0', 'On' Position='1', 'Auto' Position='2'";

            [XmlElement("torch")]     
            public int torch;
            [XmlElement]
            public string torchCmt = "'Off' Position='0' , 'On' Position='1'  , 'Auto' Position='2'";
            
            [XmlElement("focus")]
            public int focus;
            [XmlElement]
            public string focusCmt = "'Autofocus - continuous' Position='1', 'Autofocus - single try' Position='2', 'Fixed focus - manual' Position='0'";

            [XmlElement("ManualFocusValue")]
            public int ManualFocusValue = 100;
            [XmlElement]
            public string manualFocusCmt = "manual focus: Min='0' Max='255'";

            [XmlElement("viewfinder")]
            public bool viewfinder = false;

            [XmlElement("whitebalance")]
            public int whitebalance;
            [XmlElement("WhitebalanceAutoMode")]
            public bool WhitebalanceAutoMode;

            [XmlElement("resolution")]
            public Intermec.Multimedia.Camera.Resolution resolution;

            [XmlElement("ViewFinderResolution")]
            public Intermec.Multimedia.Camera.ImageResolutionType viewfinderresolution;
//###
            [XmlElement("Snapshot.FileDirectory")]
            public string SnapshotFileDirectory="\\";
            
            [XmlElement("SnapshotFile.Filename")]
            public string SnapshotFileFilename = "snapshot";
            
            [XmlElement("SnapshotFile.FilenamePadding")]
            public Camera.FilenamePaddingType SnapshotFileFilenamePadding;

            [XmlElement("SnapshotFile.ImageFormatType")]
            public Camera.ImageType SnapshotFileImageFormatType;

            //[XmlElement("SnapshotFile.ImageResolution")]
            //public Camera.ImageResolutionType SnapshotFileImageResolution;

            [XmlElement("SnapshotFile.JPGQuality")]
            public int SnapshotFileJPGQuality;

            //######### Imprints ############
            [XmlElement("ImprintCaptionPos")]
            public Camera.ImprintCaptionPosType ImprintCaptionPos;
            /*
            Disabled: No caption imprint.
            LowerRight
            LowerLeft
            LowerCenter
            UpperRight
            UpperLeft
            UpperCenter
            Center
            */
            [XmlElement("ImprintCaptionString")]
            public string ImprintCaptionString="";
            
            [XmlElement("ImprintCompassPosType")]
            public Camera.ImprintCompassPosType ImprintCompassPosType=Camera.ImprintCompassPosType.Disabled;
            /*
            Disabled: No compass direction is displayed.
            UpperRight
            LowerRight
            UpperLeft
            LowerLeft
            */

            [XmlElement("Camera.ImprintDateTimePos")]
            public Camera.ImprintDateTimePosType ImprintDateTimePos;
            /*
            Disabled: No imprint.
            LowerRight
            LowerLeft
            UpperRight
            UpperLeft
            */

            //########## Onscreen Infos ###########
            [XmlElement("DisplayCameraInfo")]
            public bool DisplayCameraInfo;

            [XmlElement("DisplayHistogram")]
            public bool DisplayHistogram;

            [XmlElement("ImprintInfo")]
            public bool ImprintInfo;

            private double _DigitalZoomFactor=1.0f;
            [XmlElement("DigitalZoomFactor")]
            public double DigitalZoomFactor
            {
                get
                {
                    return _DigitalZoomFactor;
                }
                set
                {
                    if (value < 1.00f)
                        _DigitalZoomFactor = 1.00f;
                    if (value > 5.00f)
                        _DigitalZoomFactor = 5.00f;
                }
            }

            [XmlElement("PlaySoundOnCapture")]
            public string PlaySoundOnCapture;

            #endregion
            public override string ToString()
            {
                string s = "";
                s += "brightness: '" + this.Brightness + "'\r\n";
                s += "BrightnessAuto: '" + this.BrightnessAuto + "'\r\n";
                s += "DigitalZoomFactor: '" + this.DigitalZoomFactor + "'\r\n";
                s += "DisplayCameraInfo: '" + this.DisplayCameraInfo + "'\r\n";
                s += "DisplayHistogram: '" + this.DisplayHistogram + "'\r\n";
                s += "flash: '" + this.flash + "'\r\n";
                s += "focus: '" + this.focus + "'\r\n";
                s += "ImprintCaptionPos: '" + this.ImprintCaptionPos + "'\r\n";
                s += "ImprintCaptionString: '" + this.ImprintCaptionString + "'\r\n";
                s += "ImprintCompassPosType: '" + this.ImprintCompassPosType + "'\r\n";
                s += "ImprintDateTimePos: '" + this.ImprintDateTimePos + "'\r\n";
                s += "ImprintInfo: '" + this.ImprintInfo + "'\r\n";
                s += "ManualFocusValue: '" + this.ManualFocusValue + "'\r\n";
                s += "resolution: '" + this.resolution.Width + "x" + this.resolution.Height + "'\r\n";
                s += "SnapshotFileDirectory: '" + this.SnapshotFileDirectory + "'\r\n";
                s += "SnapshotFileFilename: '" + this.SnapshotFileFilename + "'\r\n";
                s += "SnapshotFileFilenamePadding: '" + this.SnapshotFileFilenamePadding + "'\r\n";
                s += "SnapshotFileImageFormatType: '" + this.SnapshotFileImageFormatType + "'\r\n";
                s += "SnapshotFileJPGQuality: '" + this.SnapshotFileJPGQuality + "'\r\n";
                s += "torch: '" + this.torch + "'\r\n";
                s += "viewfinder: '" + this.viewfinder + "'\r\n";
                s += "viewfinderresolution: '" + this.viewfinderresolution + "'\r\n";
                s += "whitebalance: '" + this.whitebalance + "'\r\n";
                s += "PlaySoundOnCapture: '" + this.PlaySoundOnCapture + "'\r\n";
                return s;
            }
            #region serialization
            public static settings deserialize(System.IO.Stream xmlStream)
            {
                XmlSerializer xs = new XmlSerializer(typeof(settings));
                settings s=null;
                try
                {
                    //StreamReader sr = new StreamReader("./SystemHealth.xml");
                    StreamReader sr = new StreamReader(xmlStream);
                    s = (settings)xs.Deserialize(sr);
                    sr.Close();
                }
                catch (Exception) { }
                return s;
            }
            public static settings deserialize(string sXMLfile)
            {
                XmlSerializer xs = new XmlSerializer(typeof(settings));
                settings s=null;
                try
                {
                    //StreamReader sr = new StreamReader("./SystemHealth.xml");
                    StreamReader sr = new StreamReader(sXMLfile);
                    s = (settings)xs.Deserialize(sr);
                    sr.Close();
                }
                catch (Exception) { }
                return s;
            }
            public static void serialize(settings datamodel, string sXMLfile)
            {
                XmlSerializer xs = new XmlSerializer(typeof(settings));
                //omit xmlns:xsi from xml output
                //Create our own namespaces for the output
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add("", "");
                //StreamWriter sw = new StreamWriter("./SystemHealth.out.xml");
                StreamWriter sw = new StreamWriter(sXMLfile);
                xs.Serialize(sw, datamodel, ns);
                sw.Flush();
                sw.Close();
            }
            #endregion
            [NonSerialized]
            private Camera _cam;

            public settings()
            {
                viewfinderresolution = Camera.ImageResolutionType.Medium;
                resolution = new Camera.Resolution();
                resolution.BPP = 24;
                resolution.Height = 640;
                resolution.Width = 480;
                resolution.ResolutionID = 10;

                ImprintCaptionPos = Camera.ImprintCaptionPosType.Disabled;
                ImprintCaptionString = "";

                ImprintCompassPosType = Camera.ImprintCompassPosType.Disabled;

                ImprintDateTimePos = Camera.ImprintDateTimePosType.Disabled;
                
                DisplayCameraInfo = false;
                DisplayHistogram = false;
                
                _DigitalZoomFactor = 1.00f;
                ImprintInfo = false;

            }
            public settings(Camera cam, Camera.ImageResolutionType _viewfinderResolution, Camera.Resolution _snapshotResolution)
            {
                _cam = cam;
                viewfinderresolution = _viewfinderResolution;
                resolution = _snapshotResolution;
                readFromCam();
            }

            private void readFromCam(){
                if (_cam.Features.Brightness.Available)
                {
                    Brightness = _cam.Features.Brightness.CurrentValue;
                    BrightnessAuto = _cam.Features.Brightness.Auto;
                }
                else
                {
                    Brightness = -1;
                    BrightnessAuto = false;
                }

                if (_cam.Features.Flash.Available)
                    flash = _cam.Features.Flash.CurrentValue;
                else
                    flash = -1;
                
                if (_cam.Features.Torch.Available)
                    torch = _cam.Features.Torch.CurrentValue;
                else
                    torch = -1;
                
                if (_cam.Features.Focus.Available)
                {
                    focus = _cam.Features.Focus.CurrentValue;
                    //ManualFocusValue = _cam.Features.Focus.???:
                }
                else
                {
                    focus = -1;
                }
                
                if (_cam.Features.WhiteBalance.Available)
                    whitebalance = (int)_cam.Features.WhiteBalance.PresetValue;
                else
                    whitebalance = -1;

                if (_cam.SnapshotFile.Directory == "")
                    _cam.SnapshotFile.Directory = "\\";

                _DigitalZoomFactor = _cam.DigitalZoomFactor;
                DisplayCameraInfo = _cam.DisplayCameraInfo;
                DisplayHistogram = _cam.DisplayHistogram;

                ImprintCaptionPos = _cam.ImprintCaptionPos;
                ImprintCaptionString = _cam.ImprintCaptionString;
                ImprintCompassPosType = _cam.ImprintCompassPos;
                ImprintDateTimePos = _cam.ImprintDateTimePos;
                DisplayCameraInfo = _cam.DisplayCameraInfo;
                DisplayHistogram = _cam.DisplayHistogram;
                ImprintInfo = _cam.ImprintInfo;
                
                SnapshotFileDirectory = _cam.SnapshotFile.Directory;
                SnapshotFileFilename = _cam.SnapshotFile.Filename;
                SnapshotFileFilenamePadding = _cam.SnapshotFile.FilenamePadding;
                SnapshotFileImageFormatType = _cam.SnapshotFile.ImageFormatType;
                SnapshotFileJPGQuality = _cam.SnapshotFile.JPGQuality;

            }

            private bool readIntermecXML()
            {
                bool bRet = false;
                Intermec.DeviceManagement.SmartSystem.ITCSSApi _ssAPI = new Intermec.DeviceManagement.SmartSystem.ITCSSApi();
                string sXML = "";

                int sbSize = 4000;
                StringBuilder sbRet = new StringBuilder(sbSize);
                uint uRes = _ssAPI.Get(sXML, sbRet, ref sbSize, 3000);
                if (uRes == Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
                {
                    bRet = true;
                }
                return bRet;
            }

            public void save(string xmlFile)
            {
                settings.serialize(this, xmlFile);
            }
            public settings load(string xmlFile)
            {
                settings sett = settings.deserialize(xmlFile);
                return sett;
            }
            public settings load(System.IO.Stream xmlFile)
            {
                settings sett = settings.deserialize(xmlFile);
                return sett;
            }

            public string binaryDump
            {
                get
                {
                    string sOut = "";
                    Type type = _cam.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        try
                        {
                            sOut += propertyInfo.Name;
                            try
                            {
                                sOut += "|'" + propertyInfo.GetValue(_cam, null).ToString() + "' / '" + propertyInfo.PropertyType.FullName + "'\r\n";
                                ;
                            }
                            catch (Exception)
                            {
                                sOut += "|''\r\n";
                            }
                        }
                        catch (InvalidProgramException)
                        {
                        }
                        catch (Exception)
                        {
                        }
                    }
                    return sOut;
                }
            }
            public enum flashValues
            {
                none = -1,
                off = 0,
                on = 1,
                auto = 2

            }
            public enum torchValues
            {
                none = -1,
                off = 0,
                on = 1,
                auto = 2
            }
            public enum focusValues
            {
                FixedManual = 0,
                AutoContinues = 1,
                AutoSingleTry = 2,
            }
        }
    }
}
