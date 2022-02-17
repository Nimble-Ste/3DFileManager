using System;

namespace _3DFileManager
{
	public class FileObject
	{
		internal double xPosition;
		internal double yPosition;
		internal double zPosition;
		internal string name;
		internal string formattedName;
		internal string fileType;
		internal DateTime FileLastAccessed;

		public FileObject(double inXposition,double inYposition,double inZposition, string inFileName, string inFormattedName, string inFileType, DateTime inDateTime)
		{
			xPosition=inXposition;
			yPosition=inYposition;
			zPosition=inZposition;
			name=inFileName;
			formattedName=inFormattedName;	
			fileType = inFileType.ToLower();
			FileLastAccessed = inDateTime;
		}
	}
}