﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ILLC.Encoder;
using NHtmlUnit;
using NHtmlUnit.Html;
using NHtmlUnit.W3C.Dom;

namespace PicDown
{
	public partial class frmMain : Form
	{
		public string RootURL { get; set; }
		public int StartIdx { get; set; }
		public int EndIdx { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }

        private bool IsCtrlDown { get; set; }
        private bool IsTDown { get; set; }
		
		public frmMain()
		{
			InitializeComponent();
		}

        void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Modifiers==Keys.ControlKey)
            {
                IsCtrlDown = false;
            }
            else if(e.KeyCode==Keys.T)
            {
                IsTDown = false;
            }
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.ControlKey)
            {
                IsCtrlDown = true;
            }
            else if (e.KeyCode == Keys.T)
            {
                IsTDown = true;
            }

            if(IsCtrlDown&&IsTDown)
            {
                var bro = new BrowserPicDown();
                bro.GetUrl();
            }
        }

		private void button1_Click(object sender, EventArgs e)
		{
			WebUtils.DownloadFile(textBox1.Text, textBox2.Text);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			byte[] b = new ILLCCodec().Encode(textBox1.Text);
			FileStream fs = new FileStream(@"D:\t1.ilc",FileMode.Create);
			fs.Write(b,0,b.Length);
			fs.Flush();
			fs.Close();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			byte[] b = new ILLCCodec().Decode(textBox1.Text);
			FileStream fs = new FileStream(@"D:\t1.jpg", FileMode.Create);
			fs.Write(b, 0, b.Length);
			fs.Flush();
			fs.Close();
		}

		private void btnDownloadFromFile_Click(object sender, EventArgs e)
		{
			if(string.IsNullOrWhiteSpace(textBox1.Text))
			{
				return;
			}
			if(string.IsNullOrWhiteSpace(textBox2.Text))
			{
				return;
			}
			try
			{
				StreamReader sr = new StreamReader(textBox1.Text);
				if(!Directory.Exists(textBox2.Text))
				{
					Directory.CreateDirectory(textBox2.Text);
				}

				string url = null;
				string filename = null;
				while(!sr.EndOfStream)
				{
					url = sr.ReadLine();
					if(string.IsNullOrWhiteSpace(url))
					{
						continue;
					}
					filename = Path.GetFileName(url);
					filename = Path.Combine(textBox2.Text, filename);
					WebUtils.DownloadFile(url, filename);
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

        private void btnBrowserTest_Click(object sender, EventArgs e)
        {
            WebClient cli = new WebClient(BrowserVersion.CHROME);
            HtmlPage page = (HtmlPage)cli.GetPage("http://www.google.com");
            IList<INode> imgs= page.GetElementsByTagName("img");
        }
	}
}
