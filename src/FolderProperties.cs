using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace _3DFileManager
{

	public class FolderProperties : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox properties;

		private System.ComponentModel.Container components = null;

		public string directory;
		public string location;
		public long size;
		public long sizeInMb;
		public DateTime folderCreated;
		public DateTime lastAccess;
		public int numberOfFiles = 0;
		public int numberOfDirectories= 0;
		public string name;

		public FolderProperties(string inDirectory)
		{
			directory = inDirectory;
			getProperties();
			InitializeComponent();
		}

		public void getProperties()
		{
			DirectoryInfo d = new DirectoryInfo(directory);
			location = Directory.GetParent(directory).ToString();
			name = d.FullName;
			size = DirSize(d);
			folderCreated=Directory.GetCreationTime(directory);
			lastAccess=Directory.GetLastAccessTime(directory);

			
			//convert bytes into megabytes
			long temp = (size/1024);
			sizeInMb = (temp/1024);
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

		public long DirSize(DirectoryInfo d) 
		{    
			long Size = 0;    
			// Add file sizes.
			FileInfo[] fis = d.GetFiles();
			foreach (FileInfo fi in fis) 
			{    
				numberOfFiles = numberOfFiles+1;
				Size += fi.Length;  
			}
			// Add subdirectory sizes.
			DirectoryInfo[] dis = d.GetDirectories();
			
			foreach (DirectoryInfo di in dis) 
			{
				numberOfDirectories = numberOfDirectories +1; 
				Size += DirSize(di); 
			}
			return(Size);  
		}


		private void InitializeComponent()
		{
			this.properties = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// properties
			// 
			this.properties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.properties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.properties.HorizontalScrollbar = true;
			this.properties.ItemHeight = 16;
			this.properties.Items.AddRange(new object[] {
															"Type: Folder",
															"",
															"Name: " + name,
															"",
															"Location: "+ location,
															"",
															"Size:          " + sizeInMb + " MB (" + size + " bytes)",
															"",
															"Contains: " + numberOfFiles + "Files, " + numberOfDirectories+ " Folders",
															"",
															"Created:               " +  folderCreated,
															"Last Accessed: " +  lastAccess										
														});;
			this.properties.Location = new System.Drawing.Point(0, 0);
			this.properties.Name = "properties";
			this.properties.Size = new System.Drawing.Size(350, 220);
			this.properties.TabIndex = 0;
			//this.properties.SelectedIndexChanged += new System.EventHandler(this.properties_SelectedIndexChanged);
			// 
			// FolderProperties
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(350, 220);
			this.Controls.Add(this.properties);
			this.Name = "FolderProperties";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Folder Properties";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
	}
}
