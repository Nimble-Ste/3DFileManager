using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace _3DFileManager
{
	/// <summary>
	/// Summary description for InputBox.
	/// </summary>
	public class FileRename : System.Windows.Forms.Form
	{
		FileManager fm;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FileName;
		public System.Windows.Forms.Button OkButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public string fileToRename;
		public string shortName;
		private System.Windows.Forms.Button Cancel;
		public string FilePath;

		public FileRename(string inFileToRename, string inShortName, string inFilePath)
		{
			fm = new FileManager();
			InitializeComponent();
			FileName.Text = inShortName;
			shortName = inShortName;
			fileToRename = inFileToRename;
			FilePath = inFilePath;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.FileName = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter new file name";
			// 
			// FileName
			// 
			this.FileName.Location = new System.Drawing.Point(24, 56);
			this.FileName.Name = "FileName";
			this.FileName.Size = new System.Drawing.Size(168, 20);
			this.FileName.TabIndex = 1;
			this.FileName.Text = "NewFile";
			// 
			// OkButton
			// 
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.Location = new System.Drawing.Point(32, 104);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(48, 24);
			this.OkButton.TabIndex = 2;
			this.OkButton.Text = "Ok";
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// Cancel
			// 
			this.Cancel.Location = new System.Drawing.Point(152, 104);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(64, 24);
			this.Cancel.TabIndex = 3;
			this.Cancel.Text = "Cancel";
			this.Cancel.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// FileRename
			// 
			this.AcceptButton = this.OkButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 141);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.FileName);
			this.Controls.Add(this.label1);
			this.Name = "FileRename";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Rename File";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		private void CancelButton_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
			
		}

		private void OkButton_Click(object sender, System.EventArgs e)
		{	
			FileControls f = new FileControls(FilePath);

			string newName = FilePath + "\\" +FileName.Text; 

			if((FileName.Text.Equals(shortName))||(FileName.Text.Equals("")))
			{
				this.Dispose();
			}
			else
			{
				if(f.RenameFile(FilePath+"\\"+shortName, newName))
				{
					fm.ScanPath();
					this.Dispose();
				}
			}
		}
	}
}

