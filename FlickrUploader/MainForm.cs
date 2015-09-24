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
        private BackgroundWorker _worker;
        private string _path;
        public MainForm()
        {
            InitializeComponent();
            InitFlickr();
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.WorkerReportsProgress = true;
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

        private void btnProcess_Click(object sender, EventArgs e)
        {
            InitProgressBar();
            DisableOrEnableAllControls(false);
            _worker.RunWorkerAsync();
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
            int totalFiles = proBar.Maximum;
            if (totalFiles == 0) return;
            int currentFile = 0;
            //go through all dirs in the root path
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

                        //upload
                        Stream s = null;
                        string pid = null;
                        try
                        {
                            s = new FileStream(p.FileInfo.FullName, FileMode.Open);
                            pid = flickr.UploadPicture(s, fi.Name, fi.Name, string.Empty,
                                string.IsNullOrWhiteSpace(txtTag.Text) ? string.Empty : txtTag.Text, false, false, false,
                                ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Hidden);
                        }
                        catch (Exception e)
                        {
                            LogError(string.Format("[{0}] Error:{1} \nFilename:{2} Album:{3}", DateTime.Now, e.Message, fi.Name, i.Name));
                            Thread.Sleep(1000);
                        }


                        if (!string.IsNullOrWhiteSpace(pid))
                        {
                            p.PhotoId = pid;

                            //report progress
                            currentFile++;
                            _worker.ReportProgress(currentFile);
                        }
                    }

                    //ensure all uploaded
                    Photo tmpFi = a.UploadFailedPhotoInfo();
                    string tmpPId = null;
                    while (null != tmpFi)
                    {
                        try
                        {
                            tmpPId = flickr.UploadPicture(new FileStream(tmpFi.FileInfo.FullName, FileMode.Open),
                            tmpFi.FileInfo.Name, tmpFi.FileInfo.Name, string.Empty,
                            string.IsNullOrWhiteSpace(txtTag.Text) ? string.Empty : txtTag.Text, false, false, false,
                            ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Hidden);
                        }
                        catch (Exception e)
                        {
                            Thread.Sleep(1000);
                            LogError(string.Format("[{0}] Error:{1} \nFilename:{2} Album:{3}", DateTime.Now, e.Message, tmpFi.FileInfo.Name, i.Name));
                        }

                        if (!string.IsNullOrWhiteSpace(tmpPId))
                        {
                            tmpFi.PhotoId = tmpPId;

                            //report progress
                            currentFile++;
                            _worker.ReportProgress(currentFile);
                        }

                        tmpFi = a.UploadFailedPhotoInfo();
                    }

                    //create album
                    Photoset ps = null;
                    while (null == ps)
                    {
                        try
                        {
                            ps = flickr.PhotosetsCreate(i.Name, a.FindFirstPhotoId());
                        }
                        catch (Exception e)
                        {
                            LogError(string.Format("[{0}-create album] Error:{1} \nAlbum:{2}\n", DateTime.Now, e.Message, i.Name));
                            Thread.Sleep(1000);
                        }
                    }
                    a.SetId = ps.PhotosetId;

                    //add photos
                    Photo tmpP = a.NotAddedToAlbum();
                    while (null != tmpP)
                    {
                        try
                        {
                            if (a.IsNotTheFirstPhoto(tmpP.PhotoId))
                            {
                                flickr.PhotosetsAddPhoto(a.SetId, tmpP.PhotoId);
                                tmpP.IsAddedToAlbum = true;
                            }
                            else
                            {
                                tmpP.IsAddedToAlbum = true;
                            }
                        }
                        catch (Exception e)
                        {
                            LogError(string.Format("[{0}-add to album] Error:{1} \nFilename:{2} Album:{3}", DateTime.Now, e.Message, tmpFi.FileInfo.Name, i.Name));
                            Thread.Sleep(1000);
                        }
                        tmpP = a.NotAddedToAlbum();
                    }
                    Console.WriteLine("Folder:{0} uploaded successfully!", i.Name);
                }
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
