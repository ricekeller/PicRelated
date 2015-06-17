using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PicDown
{
	public class WebUtils
	{
		public static void DownloadFile(string fileUrl,string localPath)
		{
			try
			{
				using(WebClient wc = new WebClient())
				{
					wc.DownloadFileAsync(new Uri(fileUrl), localPath);
				}
				
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
