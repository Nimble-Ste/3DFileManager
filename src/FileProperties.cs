using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace _3DFileManager
{
	public class FileProperties : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox Properties;

		private System.ComponentModel.Container components = null;

		public string fileName;
		public DateTime fileCreated;
		public DateTime fileLastAccess;
		public string name;
		public long size;
		public long sizeInMb;
		public string location;

		public FileProperties(string inFileName)
		{
			fileName = inFileName;
			GetProperties();
			InitializeComponent();
		}

		public void GetProperties()
		{
			FileInfo f = new FileInfo(fileName);
			fileCreated =  File.GetCreationTime(fileName);
			fileLastAccess =File.GetLastAccessTime(fileName);
			name= f.Name;
			size = f.Length;

			//convert bytes into megabytes
			long temp = (size/1024);
			sizeInMb = (temp/1024);

			location = f.DirectoryName;
			
		}

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


		private void InitializeComponent()
		{
			this.Properties = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// Properties
			// 
			this.Properties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Properties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Properties.HorizontalScrollbar = true;
			this.Properties.ItemHeight = 16;
			this.Properties.Items.AddRange(new object[] {
															"Name: " + name,
															"",
															"Location:" + location,
															"",
															"Full path:" + fileName,
															"",
															"Size: " + sizeInMb + " MB (" +size+" bytes)",
															"",
															"Created:" + fileCreated,
															"Last Access: "+ fileLastAccess});
			this.Properties.Location = new System.Drawing.Point(0, 0);
			this.Properties.Name = "Properties";
			this.Properties.Size = new System.Drawing.Size(296, 308);
			this.Properties.TabIndex = 0;
			// 
			// FileProperties
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 317);
			this.Controls.Add(this.Properties);
			this.Name = "FileProperties";
			this.Text = "FileProperties";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
	}
}
