namespace FlickrUploader
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblVerifier = new System.Windows.Forms.Label();
			this.txtVerifier = new System.Windows.Forms.TextBox();
			this.btnVerify = new System.Windows.Forms.Button();
			this.lblPath = new System.Windows.Forms.Label();
			this.btnChooseDir = new System.Windows.Forms.Button();
			this.btnProcess = new System.Windows.Forms.Button();
			this.txtTag = new System.Windows.Forms.TextBox();
			this.lblTag = new System.Windows.Forms.Label();
			this.proBar = new System.Windows.Forms.ProgressBar();
			this.lblStatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblVerifier
			// 
			this.lblVerifier.AutoSize = true;
			this.lblVerifier.Location = new System.Drawing.Point(12, 9);
			this.lblVerifier.Name = "lblVerifier";
			this.lblVerifier.Size = new System.Drawing.Size(42, 13);
			this.lblVerifier.TabIndex = 0;
			this.lblVerifier.Text = "Verifier:";
			// 
			// txtVerifier
			// 
			this.txtVerifier.Location = new System.Drawing.Point(60, 6);
			this.txtVerifier.Name = "txtVerifier";
			this.txtVerifier.Size = new System.Drawing.Size(100, 20);
			this.txtVerifier.TabIndex = 1;
			// 
			// btnVerify
			// 
			this.btnVerify.Location = new System.Drawing.Point(166, 4);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(75, 23);
			this.btnVerify.TabIndex = 2;
			this.btnVerify.Text = "Verify";
			this.btnVerify.UseVisualStyleBackColor = true;
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			// 
			// lblPath
			// 
			this.lblPath.AutoSize = true;
			this.lblPath.Location = new System.Drawing.Point(12, 38);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(58, 13);
			this.lblPath.TabIndex = 3;
			this.lblPath.Text = "Root Path:";
			// 
			// btnChooseDir
			// 
			this.btnChooseDir.Location = new System.Drawing.Point(12, 63);
			this.btnChooseDir.Name = "btnChooseDir";
			this.btnChooseDir.Size = new System.Drawing.Size(229, 23);
			this.btnChooseDir.TabIndex = 4;
			this.btnChooseDir.Text = "Choose Folder";
			this.btnChooseDir.UseVisualStyleBackColor = true;
			this.btnChooseDir.Click += new System.EventHandler(this.btnChooseDir_Click);
			// 
			// btnProcess
			// 
			this.btnProcess.Location = new System.Drawing.Point(12, 118);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.Size = new System.Drawing.Size(229, 23);
			this.btnProcess.TabIndex = 5;
			this.btnProcess.Text = "Process";
			this.btnProcess.UseVisualStyleBackColor = true;
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// txtTag
			// 
			this.txtTag.Location = new System.Drawing.Point(60, 92);
			this.txtTag.Name = "txtTag";
			this.txtTag.Size = new System.Drawing.Size(100, 20);
			this.txtTag.TabIndex = 7;
			// 
			// lblTag
			// 
			this.lblTag.AutoSize = true;
			this.lblTag.Location = new System.Drawing.Point(12, 95);
			this.lblTag.Name = "lblTag";
			this.lblTag.Size = new System.Drawing.Size(29, 13);
			this.lblTag.TabIndex = 6;
			this.lblTag.Text = "Tag:";
			// 
			// proBar
			// 
			this.proBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.proBar.Location = new System.Drawing.Point(0, 239);
			this.proBar.Name = "proBar";
			this.proBar.Size = new System.Drawing.Size(284, 23);
			this.proBar.TabIndex = 8;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(9, 223);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(0, 13);
			this.lblStatus.TabIndex = 9;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.proBar);
			this.Controls.Add(this.txtTag);
			this.Controls.Add(this.lblTag);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.btnChooseDir);
			this.Controls.Add(this.lblPath);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.txtVerifier);
			this.Controls.Add(this.lblVerifier);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblVerifier;
		private System.Windows.Forms.TextBox txtVerifier;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.Label lblPath;
		private System.Windows.Forms.Button btnChooseDir;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.TextBox txtTag;
		private System.Windows.Forms.Label lblTag;
		private System.Windows.Forms.ProgressBar proBar;
		private System.Windows.Forms.Label lblStatus;
	}
}

