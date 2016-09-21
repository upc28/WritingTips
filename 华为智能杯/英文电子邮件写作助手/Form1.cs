using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using huaLib;

namespace 单词提示窗口
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            finding = new Finding();
            finding.init();
        }
        string lastStr;
        Finding finding;
        string[] returnStr;
        private void Form1_Load(object sender, EventArgs e)
        {
            string str = textBox1.Text.ToString();
            listBox1.Items.Clear();

            if (str!=null&&str.Length>1&&str[str.Length-1]==' ')
            {
                int locat = 0;
                Boolean firtsFlag = false;
                for (int i=str.Length-2;i>=0;i--)
                {
                    if(str[i]==' '||str[i] == ',' || str[i] == '.' || str[i] == '?' || str[i] == '!' ||
                    str[i] == ';' || str[i] == ':' || str[i] == '"')
                    {
                        locat = i;
                        firtsFlag = true;
                        break;
                    }
                }
                string words ;
                if (firtsFlag)
                {
                    words = str.Substring(locat + 1, str.Length - locat - 2);
                }
                else words = str.Substring(0, str.Length - 1);
                    returnStr = finding.finds(words);
                    if (returnStr != null)
                    {
                        for (int j = 0; j < returnStr.Length; j++)
                            if (!returnStr[j].Equals("0")&&!returnStr[j].Equals(""))
                                listBox1.Items.Add(returnStr[j]);
                    }
                if (lastStr != null)
                    finding.choice(lastStr, words);
                lastStr = words;
            }
            if (str.Length>0&&(str[str.Length - 1] == ',' || str[str.Length - 1] == '.' || str[str.Length - 1] == '?' || str[str.Length - 1] == '!' ||
                str[str.Length - 1] == ';' || str[str.Length - 1] == ':' || str[str.Length - 1] == '"'))
            {
                int locat = 0;
                for (int i = str.Length - 2; i >= 0; i--)
                {
                    if (str[i] == ' ')
                    {
                        locat = i;
                        break;
                    }
                }
                string words;
                if (locat != 0)
                {
                    words = str.Substring(locat + 1, str.Length - locat - 2);
                }
                else words = str.Substring(0, str.Length - 1);
                if (lastStr != null)
                {
                    finding.choice(lastStr, words);
                    lastStr = null;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            this.textBox1.AppendText((string)this.listBox1.Items[i]);
            this.textBox1.Focus();
        }
        private void form_exit(object sender,EventArgs e)
        {
            finding.close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
