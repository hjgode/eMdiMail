using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace ITCSSsettings
{
	[XmlRootAttribute("DevInfo")]
	public class DevInfo
	{
		[XmlElement("Subsystem")]
		public SubSystem[] subsystems;
		[XmlAttribute("Action")]
		public ActionEnum eAction{get;set;}
		[XmlIgnore()]//"Persist")]
		public string Persist;
		[XmlIgnore()]//"SeqNo")]
		public string SeqNo;
			
		public static DevInfo deserialize(string sXMLfile){
			XmlSerializer xs = new XmlSerializer(typeof(DevInfo));
			StreamReader sr = new StreamReader(sXMLfile);
			DevInfo s = (DevInfo)xs.Deserialize(sr);
			sr.Close();
			return s;
		}
		public static void serialize(DevInfo subsys, string sXMLfile){
			XmlSerializer xs = new XmlSerializer(typeof(DevInfo));
			//omit xmlns:xsi from xml output
			//Create our own namespaces for the output
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			//Add an empty namespace and empty value
			ns.Add("", "");
			//StreamWriter sw = new StreamWriter("./SystemHealth.out.xml");
			StreamWriter sw = new StreamWriter(sXMLfile);
			xs.Serialize(sw, subsys, ns);
		}
	}
	public enum ActionEnum{
		[XmlEnum("Set")]
		@Set,
		[XmlEnum("Get")]
		@Get
	}
	
	public class SubSystem{
		[XmlElement("Group")]
		public Group[] groups;
		
		[XmlElement("Field")]
		public Field[] fields;
		
		[XmlAttribute("Name")]
		public string sName{get; set;}
		
	}
	
	public class Group{
		[XmlElement("Group")]
		public Group[] groups;
		
		[XmlElement("Field")]	
		public Field[] fields;

		[XmlAttribute("Name")]
		public string sName{get; set;}

		[XmlAttribute("Instance")]
		public string sInstance{get; set;}
	}
	
	public class Field{
		private string _shortName;
		public string sShortName{
			get{return _shortName;}
		}
		private string _sName;
		[XmlAttribute("Name")]
		public string sName{
			get{return _sName;} 
			set{
				_sName=value;
				string[] s = value.Split(new char[]{'\\'});
				_shortName=s[s.Length-1];
			}
		}
		[XmlIgnore()]//"Path")]
		public string Path{get; set;}
		
		[XmlIgnore()]//"Encrypt")]
		public string Encrypt{get; set;}

		[XmlText(typeof(string))]
		public string sValue{get; set;}
		
		// This field shouldn't be serialized 
   		// if it is uninitialized.
		[XmlIgnore()]//"Error")]
		public string Error{get;set;}
	}
}