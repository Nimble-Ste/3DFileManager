using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace _3DFileManager
{
	/// <summary>
	/// 
	/// </summary>
	public class AddFolder : System.Windows.Forms.Form
	{
		FileManager fm;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FolderName;
		private System.Windows.Forms.Button OkButton;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button CancelButton;
		private string currentPath;

		public AddFolder(string inPath)
		{
			fm = new FileManager();
			currentPath = inPath;
			InitializeComponent();	
			FolderName.Text = "new folder";				
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
			this.FolderName = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter new folder name";
			// 
			// FolderName
			// 
			this.FolderName.Location = new System.Drawing.Point(24, 56);
			this.FolderName.Name = "FolderName";
			this.FolderName.Size = new System.Drawing.Size(168, 20);
			this.FolderName.TabIndex = 1;
			this.FolderName.Text = "NewFolder";
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
			// CancelButton
			// 
			this.CancelButton.Location = new System.Drawing.Point(144, 104);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(64, 24);
			this.CancelButton.TabIndex = 3;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// addFolder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 141);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.FolderName);
			this.Controls.Add(this.label1);
			this.Name = "addFolder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Folder";
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
			FolderControls FoControls = new FolderControls(currentPath);
			
			if(FoControls.AddFolder(FolderName.Text))
			{
				fm.ScanPath();
				this.Dispose();
			}
		}
	}
}