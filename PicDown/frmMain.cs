using System;
using System.IO;
using System.Windows.Forms;
using ILLC.Encoder;
using PNG;

namespace PicDown
{
	public partial class frmMain : Form
	{
		private const int s_id = 100;

		public frmMain()
		{
			InitializeComponent();
			GlobalHotKeyHelper.RegisterHotKey(Handle, s_id, GlobalHotKeyHelper.KeyModifiers.Shift, Keys.T);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			WebUtils.DownloadFile(textBox1.Text, textBox2.Text);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			byte[] b = new ILLCCodec().Encode(textBox1.Text);
			FileStream fs = new FileStream(@"D:\t1.ilc", FileMode.Create);
			fs.Write(b, 0, b.Length);
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
			if (string.IsNullOrWhiteSpace(textBox1.Text))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(textBox2.Text))
			{
				return;
			}
			try
			{
				StreamReader sr = new StreamReader(textBox1.Text);
				if (!Directory.Exists(textBox2.Text))
				{
					Directory.CreateDirectory(textBox2.Text);
				}

				string url = null;
				string filename = null;
				while (!sr.EndOfStream)
				{
					url = sr.ReadLine();
					if (string.IsNullOrWhiteSpace(url))
					{
						continue;
					}
					filename = Path.GetFileName(url);
					filename = Path.Combine(textBox2.Text, filename);
					WebUtils.DownloadFile(url, filename);
				}
			}
			catch (Exception ex)
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
							BrowserPicDown.GetUrlFromCurrentActiveChromeTab();
							break;
					}
					break;
			}
			base.WndProc(ref m);
		}

		protected override void OnClosed(EventArgs e)
		{
			//unregister global key hook
			GlobalHotKeyHelper.UnregisterHotKey(Handle, s_id);
			base.OnClosed(e);
		}

		private void frmMain_DragDrop(object sender, DragEventArgs e)
		{
			string path = null;
			if (GetFilename(out path, e))
			{
				FileStream fs = new FileStream(path, FileMode.Open);
				PNGDecoder pd = new PNGDecoder(fs);
				if (pd.IsPNG())
				{
					Console.WriteLine("File {0} is PNG file!", path);
				}
				else
				{
					Console.WriteLine("File {0} is NOT PNG file!", path);
				}
			}
		}

		private void frmMain_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.All;
			else
				e.Effect = DragDropEffects.None;
		}

		protected bool GetFilename(out string filename, DragEventArgs e)
		{
			bool ret = false;
			filename = String.Empty;

			if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
			{
				Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
				if (data != null)
				{
					if ((data.Length == 1) && (data.GetValue(0) is String))
					{
						filename = ((string[])data)[0];
						string ext = Path.GetExtension(filename).ToLower();
						if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
						{
							ret = true;
						}
					}
				}
			}
			return ret;
		}

		private bool ByteArrayEqual(byte[] b1,byte[] b2)
		{
			if(b1.Length!=b2.Length)
			{
				return false;
			}

			for(int i=0;i<b1.Length;i++)
			{
				if(b1[i]!=b2[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
