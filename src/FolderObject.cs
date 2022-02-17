using System;

namespace _3DFileManager
{
	public class FolderObject
	{
		internal double yPosition;
		internal double zPosition;
		internal string fullName;
		internal string name;
		internal string formattedName;
		internal DateTime FolderLastAccessed;

		public FolderObject(double inYposition,double inZposition,string inFullName, string inFolderName, string inFormattedName, DateTime inDateTime)
		{
			yPosition=inYposition;
			zPosition=inZposition;
			fullName = inFullName;
			name=inFolderName;
			formattedName=inFormattedName;	
			FolderLastAccessed = inDateTime;
		}
	}
}
