using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlickrNet;

namespace FlickrUploader
{
	public partial class MainForm : Form
	{
		private OAuthRequestToken _requestToken;
		private string _path;
		public MainForm()
		{
			InitializeComponent();
			InitFlickr();
		}

		void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Console.WriteLine("Completed!");
			CleanAfterComplete();
		}

		void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			proBar.Value = e.ProgressPercentage;
			lblStatus.Text = string.Format("Files uploaded:{0}", e.ProgressPercentage);
		}

		void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			DoJob();
		}

		private void InitFlickr()
		{
			if (null == FlickrManager.OAuthToken)
			{
				Flickr f = FlickrManager.GetInstance();
				_requestToken = f.OAuthGetRequestToken("oob");

				string url = f.OAuthCalculateAuthorizationUrl(_requestToken.Token, AuthLevel.Write);
				Process.Start(url);
				ChangeAuthControlVisibleState(true);
			}
			else
			{
				ChangeAuthControlVisibleState(false);
				this.Text = LoggedInString();
			}
		}

		private string LoggedInString()
		{
			return string.Format("Current User:{0}", FlickrManager.OAuthToken.FullName);
		}

		private void ChangeAuthControlVisibleState(bool toShow)
		{
			lblVerifier.Visible = toShow;
			txtVerifier.Visible = toShow;
			btnVerify.Visible = toShow;
		}

		private void btnVerify_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(txtVerifier.Text))
			{
				MessageBox.Show("You must paste the verifier code into the textbox above.");
				return;
			}

			Flickr f = FlickrManager.GetInstance();
			try
			{
				var accessToken = f.OAuthGetAccessToken(_requestToken, txtVerifier.Text);
				FlickrManager.OAuthToken = accessToken;
				this.Text = LoggedInString();
			}
			catch (FlickrApiException ex)
			{
				MessageBox.Show("Failed to get access token. Error message: " + ex.Message);
			}
		}

		private void btnChooseDir_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				lblPath.Text = fbd.SelectedPath;
				_path = fbd.SelectedPath;
			}
			else
			{
				lblPath.Text = string.Empty;
				_path = string.Empty;
			}
		}

		private async void btnProcess_Click(object sender, EventArgs e)
		{
			InitProgressBar();
			DisableOrEnableAllControls(false);

			if (proBar.Maximum == 0)
			{
				return;
			}
			await Task.Run(() => { DoJob(); });

			//task completed
			DisableOrEnableAllControls(true);
			lblStatus.Text = "Task Completed! " + proBar.Maximum + " files uploaded!";
		}

		private void InitProgressBar()
		{
			proBar.Maximum = GetTotalFilesCount(_path);
			proBar.Value = 0;
			lblStatus.Text = string.Empty;
		}

		private void DoJob()
		{
			if (string.IsNullOrWhiteSpace(_path))
			{
				return;
			}

			Flickr flickr = FlickrManager.GetAuthInstance();
			//key: folder name, value: album
			Dictionary<string, Album> map = new Dictionary<string, Album>();
			DirectoryInfo d = new DirectoryInfo(_path);
			DirectoryInfo[] subs = d.GetDirectories();

			//build data
			foreach (DirectoryInfo i in subs)
			{
				if (!map.ContainsKey(i.Name))
				{
					Album a = new Album();
					map.Add(i.Name, a);

					//go through all files in each dir
					foreach (FileInfo fi in i.GetFiles())
					{
						if (!IsImageFile(fi))
						{
							continue;
						}

						Photo p = new Photo();
						p.FileInfo = fi;

						a.Add(p);
					}
				}
			}

			//upload and add to set
			foreach (KeyValuePair<string, Album> kv in map)
			{
				//1. upload
				foreach (Photo p in kv.Value)
				{
					lblStatus.Invoke(new Action(() => { lblStatus.Text = proBar.Value + ":" + p.FileInfo.Name; }));
					Stream s = null;
					string pid = null;
					while (string.IsNullOrWhiteSpace(pid))
					{
						try
						{
							s = new FileStream(p.FileInfo.FullName, FileMode.Open);
							pid = flickr.UploadPicture(s, p.FileInfo.Name, p.FileInfo.Name, string.Empty,
								string.IsNullOrWhiteSpace(txtTag.Text) ? string.Empty : txtTag.Text, false, false, false,
								ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Hidden);
						}
						catch (Exception e)
						{
							LogError(string.Format("[{0} Upload Error] Message:{1} \nFilename:{2} Album:{3}", DateTime.Now, e.Message, p.FileInfo.Name, kv.Key));
							Thread.Sleep(1000);
						}
					}
					p.PhotoId = pid;
					proBar.Invoke(new Action(() => { proBar.Value++; }));
				}

				//2. create album
				Photoset ps = null;
				while (null == ps)
				{
					try
					{
						ps = flickr.PhotosetsCreate(kv.Key, kv.Value.FindFirstPhotoId());
					}
					catch (Exception e)
					{
						LogError(string.Format("[{0} Create Album] Error:{1} \nAlbum:{2}\n", DateTime.Now, e.Message, kv.Key));
						Thread.Sleep(1000);
					}
				}
				kv.Value.SetId = ps.PhotosetId;

				//3. add photos
				foreach (Photo p in kv.Value)
				{
					while (!p.IsAddedToAlbum)
					{
						try
						{
							flickr.PhotosetsAddPhoto(kv.Value.SetId, p.PhotoId);
						}
						catch (Exception e)
						{
							LogError(string.Format("[{0} Add to Album] Error:{1} \nFilename:{2} Album:{3}", DateTime.Now, e.Message, p.FileInfo.Name, kv.Key));
							Thread.Sleep(1000);
						}

						AllContexts ctx = null;
						while (null == ctx)
						{
							try
							{
								ctx = flickr.PhotosGetAllContexts(p.PhotoId);
							}
							catch (Exception e)
							{
								LogError(string.Format("[{0} GetAllContext] Error:{1} \nFilename:{2} Album:{3}", DateTime.Now, e.Message, p.FileInfo.Name, kv.Key));
								Thread.Sleep(1000);
							}
						}

						foreach (ContextSet cs in ctx.Sets)
						{
							if (cs.PhotosetId.Equals(kv.Value.SetId))
							{
								p.IsAddedToAlbum = true;
							}
						}
					}
				}
				Console.WriteLine("Folder:{0} uploaded successfully!", kv.Key);
			}
		}

		private void LogError(string p)
		{
			using (StreamWriter sw = File.AppendText("log.dat"))
			{
				sw.WriteLine(p);
				sw.Flush();
			}
		}

		private int GetTotalFilesCount(string root)
		{
			if (Directory.Exists(root))
			{
				return Directory.GetFiles(root, "*.*", SearchOption.AllDirectories).Length;
			}
			return 0;
		}
		private bool IsImageFile(FileInfo fi)
		{
			string[] valid = { "jpg", "jpeg", "bmp", "png", "gif" };
			foreach (string s in valid)
			{
				if (fi.Extension.ToLowerInvariant().Contains(s))
				{
					return true;
				}
			}
			return false;
		}

		private void DisableOrEnableAllControls(bool toEnable)
		{
			txtVerifier.Enabled = toEnable;
			btnVerify.Enabled = toEnable;
			btnChooseDir.Enabled = toEnable;
			txtTag.Enabled = toEnable;
			btnProcess.Enabled = toEnable;
		}

		private void CleanAfterComplete()
		{
			DisableOrEnableAllControls(true);
			txtTag.Text = string.Empty;
			_path = string.Empty;
			lblPath.Text = "Root path:";
		}
	}
}
