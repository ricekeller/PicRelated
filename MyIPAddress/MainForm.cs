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
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace MyIPAddress
{
	public partial class MainForm : Form
	{
		private static string s_IPUrl = "http://whatismyipaddress.com/";
		private string _dbPath;
		private static string s_FileName = "MyIPAddress.txt";

		public MainForm()
		{
			InitializeComponent();
			timer.Interval = 600000;
			//timer.Interval = 60000;
			FindDBPath();
			if(!string.IsNullOrWhiteSpace(_dbPath))
			{
				timer.Start();
			}
		}

		private async void timer_Tick(object sender, EventArgs e)
		{
			await Task.Run(() =>
			{
				try
				{
					PhantomJSDriverService svc = PhantomJSDriverService.CreateDefaultService();
					svc.HideCommandPromptWindow = true;//hide the PhantomJS cmd window
					PhantomJSDriver driver = new PhantomJSDriver(svc);
					driver.Navigate().GoToUrl(s_IPUrl);

					//wait until target exists, then find them
					WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
					wait.Until(ExpectedConditions.ElementExists(By.Id("section_left")));

					IWebElement div = driver.FindElementById("section_left");
					Console.WriteLine(div.Text);
					using (StreamWriter sw = new StreamWriter(_dbPath + Path.PathSeparator + s_FileName))
					{
						sw.WriteLine(div.Text);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			});
		}

		private void FindDBPath()
		{
			string appDataPath = Environment.GetFolderPath(
								   Environment.SpecialFolder.ApplicationData);
			string dbPath = System.IO.Path.Combine(appDataPath, "Dropbox\\host.db");
			string[] lines = null;
			try
			{
				lines = System.IO.File.ReadAllLines(dbPath);
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message);
				return;
			}
			byte[] dbBase64Text = Convert.FromBase64String(lines[1]);
			string folderPath = System.Text.ASCIIEncoding.ASCII.GetString(dbBase64Text);
			Console.WriteLine(folderPath);
			this._dbPath = folderPath;
		}
	}
}
