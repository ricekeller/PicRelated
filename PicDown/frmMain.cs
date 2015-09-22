using System;
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
		private const int s_id = 100;
		
		public frmMain()
		{
			InitializeComponent();
			GlobalHotKeyHelper.RegisterHotKey(Handle, s_id, GlobalHotKeyHelper.KeyModifiers.Shift, Keys.T);
			////注销Id号为100的热键设定
			//HotKey.UnregisterHotKey(Handle, 100);
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
		protected override void WndProc(ref Message m)
		{
			const int WM_HOTKEY = 0x0312;
			//按快捷键 
			switch (m.Msg)
			{
				case WM_HOTKEY:
					switch (m.WParam.ToInt32())
					{
						case s_id:
							PicRelatedHelper.GetUrlFromCurrentActiveChromeTab();
							break;

					}
					break;
			}
			base.WndProc(ref m);
		}
	}
}
