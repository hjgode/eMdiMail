using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDIdemo
{
    public partial class DirChooser : Form
    {
        private string m_directory = "\\";
        public string directory
        {
            get { return m_directory; }
            set { 
                if (System.IO.Directory.Exists(value)){
                    m_directory = value;
                    textBox1.Text = value;
                    fillList(m_directory);
                }
                else {
                    throw new IndexOutOfRangeException("Dir does not exist");
                }
            }
        }
        private bool m_bUseCustomDir = false;
        public bool bUseCustomDir
        {
            get { return m_bUseCustomDir; }
            set { m_bUseCustomDir = value; }
        }
        public DirChooser(string startDir)
        {
            InitializeComponent();
            directory = startDir;
            fillList(m_directory);
        }

        public DirChooser()
        {
            InitializeComponent();
            directory = m_directory;
            fillList(m_directory);
        }
        private void fillList(string sDir){
            listBox1.Items.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sDir);
            System.IO.DirectoryInfo[] diList = di.GetDirectories();
            listBox1.BeginUpdate();
            foreach(System.IO.DirectoryInfo diNext in diList)
            {
                listBox1.Items.Add(diNext);
            }
            listBox1.EndUpdate();
            //textBox1.Text = ((System.IO.DirectoryInfo)(listBox1.Items[0])).FullName;
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void mnu_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = ((System.IO.DirectoryInfo)(listBox1.Items[listBox1.SelectedIndex])).FullName;
        }

        private void btnCDdir_Click(object sender, EventArgs e)
        {
            directory = textBox1.Text;
            //fillList(directory);
        }

        private void btnUpDir_Click(object sender, EventArgs e)
        {
            if (directory == "\\")
                return;
            string temp = directory;
            int slashCount = 0;
            if(temp.EndsWith("\\"))
                temp = temp.Remove(temp.Length - 1,1);
            for (int i = 0; i < temp.Length; i++){
                if (temp.Substring(i, 1) == "\\")
                    slashCount++;
            }
            if (slashCount>1){
                while(!temp.EndsWith("\\")){
                    temp = temp.Remove(temp.Length - 1,1);
                }
            }
            //root dir
            if ((slashCount == 1) && (temp.Length > 1))
                temp = "\\";
            directory = temp;
        }

        private void chkUseCustomDir_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkUseCustomDir.Checked)
                m_bUseCustomDir = true;
            else
                m_bUseCustomDir = false;
        }
    }
}