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

namespace MoveDir
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnSourceDir_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			lblSource.Text = string.Empty;
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				lblSource.Text = fbd.SelectedPath;
			}
		}

		private void btnDestDir_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			lblDest.Text = string.Empty;
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				lblDest.Text = fbd.SelectedPath;
			}
		}

		private void btnMove_Click(object sender, EventArgs e)
		{
			string[] subs = Directory.GetDirectories(lblSource.Text);
			foreach (string pa in subs)
			{
				string[] subs2 = Directory.GetDirectories(pa);
				foreach (string pa2 in subs2)
				{
					DirectoryInfo di = new DirectoryInfo(pa2);
					Directory.Move(pa2, lblDest.Text + Path.DirectorySeparatorChar + di.Name);
				}
			}
		}
	}
}
