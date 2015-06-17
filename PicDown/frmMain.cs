using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicDown
{
	public partial class frmMain : Form
	{
		public string RootURL { get; set; }
		public int StartIdx { get; set; }
		public int EndIdx { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }

		
		public frmMain()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			WebUtils.DownloadFile(textBox1.Text, textBox2.Text);
		}
	}
}
