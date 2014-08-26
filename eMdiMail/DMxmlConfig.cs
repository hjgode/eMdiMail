using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace XMLConfig
{
    public class Settings
    {
        public Settings()
        {
            m_appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (!m_appPath.EndsWith("\\"))
                m_appPath += @"\";
        }
        private string m_appPath;

        public string appPath
        {
            get
            {
                m_appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                if (!m_appPath.EndsWith("\\"))
                    m_appPath += @"\";
                return m_appPath;
            }
        }
    }
    /// <summary>
    /// Summary description for DMProcessConfig.
    /// </summary>
    public class DMProcessConfig
    {
        [DllImport("aygshell.dll")]
        private static extern int DMProcessConfigXML(string pszWXMLin, int dwFlags, IntPtr ppszwXMLout);

        [DllImport("coredll.dll")]
        private static extern void free(int buffer);

        public DMProcessConfig()
        {
        }

        unsafe static public bool ProcessXML(string Filename)
        {
            StreamReader sr = new StreamReader(Filename, Encoding.ASCII);
            string XML = sr.ReadToEnd();
            bool bRet = false;
            fixed (int* OutPtr = new int[1])
            {
                IntPtr outptr = (IntPtr)OutPtr;
                try
                {
                    int result = DMProcessConfigXML(XML, 1, outptr);
                    if (result != 0)
                    {
                        bRet = false;
                    }
                    else
                    {
                        free(*OutPtr);
                        bRet = true;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            return bRet;
        }
    }
}
