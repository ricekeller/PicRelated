namespace MoveDir
{
	partial class Form1
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSourceDir = new System.Windows.Forms.Button();
			this.btnDestDir = new System.Windows.Forms.Button();
			this.btnMove = new System.Windows.Forms.Button();
			this.lblSource = new System.Windows.Forms.Label();
			this.lblDest = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "SourceDir:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "DestDir:";
			// 
			// btnSourceDir
			// 
			this.btnSourceDir.Location = new System.Drawing.Point(13, 59);
			this.btnSourceDir.Name = "btnSourceDir";
			this.btnSourceDir.Size = new System.Drawing.Size(92, 23);
			this.btnSourceDir.TabIndex = 2;
			this.btnSourceDir.Text = "Choose Source";
			this.btnSourceDir.UseVisualStyleBackColor = true;
			this.btnSourceDir.Click += new System.EventHandler(this.btnSourceDir_Click);
			// 
			// btnDestDir
			// 
			this.btnDestDir.Location = new System.Drawing.Point(121, 59);
			this.btnDestDir.Name = "btnDestDir";
			this.btnDestDir.Size = new System.Drawing.Size(92, 23);
			this.btnDestDir.TabIndex = 3;
			this.btnDestDir.Text = "Choose Dest";
			this.btnDestDir.UseVisualStyleBackColor = true;
			this.btnDestDir.Click += new System.EventHandler(this.btnDestDir_Click);
			// 
			// btnMove
			// 
			this.btnMove.Location = new System.Drawing.Point(66, 88);
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(92, 23);
			this.btnMove.TabIndex = 4;
			this.btnMove.Text = "Move";
			this.btnMove.UseVisualStyleBackColor = true;
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// lblSource
			// 
			this.lblSource.AutoSize = true;
			this.lblSource.Location = new System.Drawing.Point(75, 9);
			this.lblSource.Name = "lblSource";
			this.lblSource.Size = new System.Drawing.Size(0, 13);
			this.lblSource.TabIndex = 5;
			// 
			// lblDest
			// 
			this.lblDest.AutoSize = true;
			this.lblDest.Location = new System.Drawing.Point(75, 43);
			this.lblDest.Name = "lblDest";
			this.lblDest.Size = new System.Drawing.Size(0, 13);
			this.lblDest.TabIndex = 6;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.lblDest);
			this.Controls.Add(this.lblSource);
			this.Controls.Add(this.btnMove);
			this.Controls.Add(this.btnDestDir);
			this.Controls.Add(this.btnSourceDir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSourceDir;
		private System.Windows.Forms.Button btnDestDir;
		private System.Windows.Forms.Button btnMove;
		private System.Windows.Forms.Label lblSource;
		private System.Windows.Forms.Label lblDest;
	}
}

