using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AppSettings
{
    /// <summary>
    /// Useful to load and store data aplication configuration (settings or any other)
    /// Specially in Compact Framework development SmartDevice applications
    /// More fast than using SQLCE client, and also ore lighweight and more compatible
    /// </summary>
    /// <remarks>
    /// By Alejandro Barrada Martín
    /// Some pieces extracted from other sources: pbrroks at http://www.codeproject.com/KB/mobile/SaveSettings.aspx, Microsoft at http://msdn.microsoft.com/en-us/library/aa446526.aspx
    /// </remarks>
    public class MyAppSettings
    {
	    private System.Xml.XmlDocument XmlDoc = new System.Xml.XmlDocument();
	    //This will be your default application configuration file. 
	    //You can change or add your own custom ListItems (that belongs from <appSettings> tag)
	    private string XmlDefaultConfigurationFileString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" 
            + "<configuration>" 
            + "<appSettings>" 
            + "<add key=\"FileCreated\" value=\"" + DateTime.Now.ToShortDateString() + "\" />" 
            + "<add key=\"FileModified\" value=\"\" />" 
            + "<add key=\"MailAccount\" value=\"Google Mail\" />"
            + "<add key=\"MailRecipient\" value=\"heinz-josef.gode@intermec.com\" />" 
            + "</appSettings>" 
            + "</configuration>";
	    //Kind of data access storage method
	    public enum enumXmlSaveMethod
	    {
		    StreamWrite = 1,
		    //Basic
		    XmlTextWriter = 2,
		    //Good
		    XmlDocument = 3
		    //Good as knowledgement

	    }
	    //               'Try with the 3

	    private enumXmlSaveMethod XmlSaveMethod = enumXmlSaveMethod.XmlDocument;

	    private System.Collections.Specialized.NameValueCollection _ListItems = new System.Collections.Specialized.NameValueCollection();


#region AppProperties
	    public System.Collections.Specialized.NameValueCollection ListItems {
		    get { return _ListItems; }
		    set { _ListItems = value; }
	    }
	    //If you write the custom properties then they will be available as intellisense typo
	    //As an example:
	    public string MailAccount {
		    get { return this.ListItems.Get("MailAccount"); }
		    set { this.ListItems.Set("MailAccount", value); }
	    }
	    public string MailRecipient {
            get { return this.ListItems.Get("MailRecipient"); }
            set { this.ListItems.Set("MailRecipient", value); }
	    }
        //public int LoginCount {
        //    get { return Convert.ToInt32(this.ListItems.Get("LoginCount")); }
        //    set { this.ListItems.Set("LoginCount", value.ToString()); }
        //}

	    //If you don't want to complicated, don't write custom properties code, or write the ones you can -as it by time or priority needed-
	    //                                'in that case, no intellinse will be available on forms for that xmlElements
	    //                                'W/O properties you'll neeed to use in forms, MyAppSettings.

#endregion
#region Properties
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
        string _path;
        public string filePath
        {
            get { return _path; }
            set { _path = value; }
        }
        string _fileName = "AppSettings.xml";
        public string fileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
#endregion
	    /// <summary>
	    /// Load an .xml configuration file, if not exits code will create one
	    /// </summary>
	    /// <param name="StrPath">If not specified, the constructor will look at root path for</param>
	    /// <param name="StrFilename">If not specified, AppSettings.xml will be used</param>
	    /// <remarks></remarks>
	    public MyAppSettings(string StrPath, string StrFilename)
	    {
            if (StrFilename == null)
                StrFilename = _fileName;
            else
                fileName = StrFilename;
		    //Conform the Path
            string FilePath="";
            if (StrPath == null)
                FilePath = AppPath + StrFilename;
            else
                FilePath = StrPath;

            //string FilePath = (!(StrPath == null) ? StrPath : AppPath + StrFilename);
            
            filePath = FilePath;
		    //Check for the existence of a valid XML configuration file
		    if (!System.IO.File.Exists(FilePath)) {
			    //Try to save the XmlDefaultConfigurationFile
			    if (SaveXmlDefaultConfigurationFile(FilePath, null) == false) {
				    throw new System.IO.IOException();
			    } else {
				    XmlDoc.Load(new System.IO.StringReader(XmlDefaultConfigurationFileString));
			    }
		    } else {
			    XmlDoc.Load(FilePath);
		    }

		    //Load Settings
		    System.Xml.XmlElement xRoot = XmlDoc.DocumentElement;
		    //                                        'Configuration   .(appSettings).Elements
		    System.Xml.XmlNodeList xNodeList = xRoot.ChildNodes.Item(0).ChildNodes;
		    foreach (System.Xml.XmlNode xNode in xNodeList) {
			    ListItems.Add(xNode.Attributes["key"].Value, xNode.Attributes["value"].Value);
		    }
	    }


	    /// <summary>
	    /// Create and Save a default configuration Xml file based on a easily specific or default defined xml string
	    /// </summary>
	    /// <param name="FilePath">The fullpath including filename.xml to save to</param>
	    /// <param name="XmlConfigurationFileString">If not specified, then will use the XmlDefaultConfigurationFileString</param>
	    /// <returns>True if succesfull created</returns>
	    /// <remarks></remarks>
	    public bool SaveXmlDefaultConfigurationFile(string FilePath, string XmlConfigurationFileString)
	    {
		    string theXmlString = ((XmlConfigurationFileString != null) ? XmlConfigurationFileString : XmlDefaultConfigurationFileString);
		    //What Method You want to use ?, try any by simply change the var XmlSaveMethod declared at top
		    switch (XmlSaveMethod) {
			    case enumXmlSaveMethod.StreamWrite:
				    //Easy Method
				    using (System.IO.StreamWriter StreamWriter = new System.IO.StreamWriter(FilePath)) {
					    //Without Compact Framework more easily using file.WriteAllText but, CF doesn't have this method
					    StreamWriter.Write(theXmlString);
					    StreamWriter.Close();
				    }


				    return true;

			    case enumXmlSaveMethod.XmlTextWriter:
				    //Alternative Method
				    using (System.Xml.XmlTextWriter XmlTextWriter = new System.Xml.XmlTextWriter(FilePath, System.Text.UTF8Encoding.UTF8)) {
					    XmlTextWriter.WriteStartDocument();
					    XmlTextWriter.WriteStartElement("configuration");
					    XmlTextWriter.WriteStartElement("appSettings");
					    foreach (string Item in GetListItems(theXmlString)) {
						    XmlTextWriter.WriteStartElement("add");

						    XmlTextWriter.WriteStartAttribute("key", string.Empty);
						    XmlTextWriter.WriteRaw(GetKey(Item));
						    XmlTextWriter.WriteEndAttribute();

						    XmlTextWriter.WriteStartAttribute("value", string.Empty);
						    XmlTextWriter.WriteRaw(GetValue(Item));
						    XmlTextWriter.WriteEndAttribute();

						    XmlTextWriter.WriteEndElement();

					    }

					    XmlTextWriter.WriteEndElement();
					    XmlTextWriter.WriteEndElement();
					    //XmlTextWriter.WriteEndDocument()
					    XmlTextWriter.Close();

				    }

				    return true;

			    case enumXmlSaveMethod.XmlDocument:
				    //Method you will practice
				    System.Xml.XmlDocument XmlDoc = new System.Xml.XmlDocument();
				    System.Xml.XmlElement xRoot = XmlDoc.CreateElement("configuration");
				    XmlDoc.AppendChild(xRoot);
				    System.Xml.XmlElement xAppSettingsElement = XmlDoc.CreateElement("appSettings");
				    xRoot.AppendChild(xAppSettingsElement);

				    System.Xml.XmlElement xElement = default(System.Xml.XmlElement);
				    System.Xml.XmlAttribute xAttrKey = default(System.Xml.XmlAttribute);
				    System.Xml.XmlAttribute xAttrValue = default(System.Xml.XmlAttribute);
				    foreach (string Item in GetListItems(theXmlString)) {
					    xElement = XmlDoc.CreateElement("add");
					    xAttrKey = XmlDoc.CreateAttribute("key");
					    xAttrValue = XmlDoc.CreateAttribute("value");
					    xAttrKey.InnerText = GetKey(Item);
					    xElement.SetAttributeNode(xAttrKey);
					    xAttrValue.InnerText = GetValue(Item);
					    xElement.SetAttributeNode(xAttrValue);
					    xAppSettingsElement.AppendChild(xElement);
				    }

				    System.Xml.XmlProcessingInstruction XmlPI = XmlDoc.CreateProcessingInstruction("xml", "version='1.0' encoding='utf-8'");
				    XmlDoc.InsertBefore(XmlPI, XmlDoc.ChildNodes[0]);
				    XmlDoc.Save(FilePath);

				    return true;
		    }
            return false;
	    }


	    public object SaveXmlCurrentConfiguration(string StrPath, string StrFilename /*= "AppSettings.xml"*/)
	    {
		    //Conform the Path
		    string FilePath = (!(StrPath == null) ? StrPath : System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)) + "\\" + StrFilename;

		    System.Xml.XmlDocument XmlDoc = new System.Xml.XmlDocument();
		    System.Xml.XmlElement xRoot = XmlDoc.CreateElement("configuration");
		    XmlDoc.AppendChild(xRoot);
		    System.Xml.XmlElement xAppSettingsElement = XmlDoc.CreateElement("appSettings");
		    xRoot.AppendChild(xAppSettingsElement);


		    System.Xml.XmlElement xElement = default(System.Xml.XmlElement);
		    System.Xml.XmlAttribute xAttrKey = default(System.Xml.XmlAttribute);
		    System.Xml.XmlAttribute xAttrValue = default(System.Xml.XmlAttribute);
		    //For Each Item As String In Me.ListItems
		    for (int i = 0; i <= this.ListItems.Count - 1; i++) {
			    xElement = XmlDoc.CreateElement("add");
			    xAttrKey = XmlDoc.CreateAttribute("key");
			    xAttrValue = XmlDoc.CreateAttribute("value");
			    xAttrKey.InnerText = this.ListItems.GetKey(i);
			    xElement.SetAttributeNode(xAttrKey);
			    xAttrValue.InnerText = this.ListItems[i];
			    xElement.SetAttributeNode(xAttrValue);
			    xAppSettingsElement.AppendChild(xElement);
		    }
		    System.Xml.XmlProcessingInstruction XmlPI = XmlDoc.CreateProcessingInstruction("xml", "version='1.0' encoding='utf-8'");
		    XmlDoc.InsertBefore(XmlPI, XmlDoc.ChildNodes[0]);
		    XmlDoc.Save(FilePath);
		    return true;

	    }


	    #region "Element (Items) Functions to retrieve"
	    public ArrayList GetListItems(string XmlConfigurationFileString)
	    {
		    string ListItemsString = XmlConfigurationFileString.Substring(XmlConfigurationFileString.IndexOf("<appSettings>") + "<appSettings>".Length, XmlConfigurationFileString.IndexOf("</appSettings>") - (XmlConfigurationFileString.IndexOf("<appSettings>") + "<appSettings>".Length));
		    ListItemsString = ListItemsString.Replace("<add", "|<add");
		    ListItemsString = ListItemsString.Substring(1);
		    ArrayList result = new ArrayList();
            string[] sList=ListItemsString.Split('|');
		    for (int i = 0; i < sList.Length; i++) {
			    result.Add(sList[i]);
		    }
		    return result;



	    }

	    private string GetKey(string Item)
	    {
		    return Item.Substring(Item.IndexOf("=\"") + "=\"".Length, Item.IndexOf("\" value=\"") - (Item.IndexOf("=\"") + "=\"".Length));

	    }

	    private string GetValue(string Item)
	    {
		    // Return Item.Substring(Item.IndexOf("value=""") + "value=""".Length, Item.IndexOf("""") - (Item.IndexOf("=""") + "=""".Length))
		    return Item.Substring(Item.IndexOf("value=\"") + "value=\"".Length, Item.IndexOf("\"", Item.IndexOf("value=\"") + "value=\"".Length) - (Item.IndexOf("value=\"") + "value=\"".Length));

	    }

	    #endregion


    }

    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by NRefactory.
    //Twitter: @telerik, @toddanglin
    //Facebook: facebook.com/telerik
    //=======================================================
}