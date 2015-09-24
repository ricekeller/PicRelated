using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;


namespace PicDown
{
    public class BrowserPicDown
    {
        private static BlockingCollection<string> _urlQueue = new BlockingCollection<string>();
		private const int MAXWAITSECOND = 60;

		static BrowserPicDown()
		{
			Task.Run(() => { 
				while(true)
				{
					string url=null;
					try
					{
						url = _urlQueue.Take();
					}
					catch (InvalidOperationException ioe) 
					{
						Console.WriteLine(ioe.Message);
					}
					
					if(!string.IsNullOrWhiteSpace(url))
					{
						NavigateUrl(url);
					}
				}
			});
		}

		private static void NavigateUrl(string url)
		{
			Console.WriteLine(url);
			Thread th = new Thread(() => {
				PhantomJSDriverService svc = PhantomJSDriverService.CreateDefaultService();
				svc.HideCommandPromptWindow = true;//hide the PhantomJS cmd window
				PhantomJSDriver driver = new PhantomJSDriver(svc);
				driver.Navigate().GoToUrl(url);
				
				//wait until target exists, then find them
				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
				//wait.Until(ExpectedConditions.ElementExists(By.Id("")));
				
				ReadOnlyCollection<IWebElement> imgs= driver.FindElements(By.TagName("img"));
				Console.WriteLine(imgs.Count);

				foreach (IWebElement ele in imgs)
				{
					Console.WriteLine(ele);
				}

				driver.Quit();
			});
			th.SetApartmentState(ApartmentState.STA);
			th.Start();
		}

		public static void GetUrlFromCurrentActiveChromeTab()
		{
			Task.Run(() =>
			{
				string url = GetUrlFromCurrentActiveChromeTabHelper();
				
				if(!string.IsNullOrWhiteSpace(url))
				{
					_urlQueue.Add(url);
				}
			});
		}

		private static string GetUrlFromCurrentActiveChromeTabHelper()
		{
			// there are always multiple chrome processes, so we have to loop through all of them to find the
			// process with a Window Handle and an automation element of name "Address and search bar"
			Process[] procsChrome = Process.GetProcessesByName("chrome");
			string url = string.Empty;
			foreach (Process chrome in procsChrome)
			{
				// the chrome process must have a window
				if (chrome.MainWindowHandle == IntPtr.Zero)
				{
					continue;
				}

				// find the automation element
				AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);

				// manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
				AutomationElement elmUrlBar = null;
				try
				{
					var propFindText = new PropertyCondition(AutomationElement.NameProperty, "Address and search bar");
					elmUrlBar = elm.FindFirst(TreeScope.Descendants, propFindText);
				}
				catch
				{
					// Chrome has probably changed something, and above walking needs to be modified. :(
					// put an assertion here or something to make sure you don't miss it
					continue;
				}

				// make sure it's valid
				if (elmUrlBar == null)
				{
					continue;
				}

				// elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
				if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
				{
					continue;
				}

				url = elmUrlBar.GetCurrentPropertyValue(ValuePattern.ValueProperty).ToString();
				if (!string.IsNullOrEmpty(url) && Regex.IsMatch(url, @"^(https?:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$"))
				{
					// prepend http:// to the url, because Chrome hides it if it's not SSL
					if (!url.StartsWith("http"))
					{
						url = "http://" + url;
					}
					Console.WriteLine("Open Chrome URL found: '" + url + "'");
					return url;
				}
				continue;
			}
			return null;
		}
    }
}
