using System;

namespace _3DFileManager
{
	/// <summary>
	/// 
	/// </summary>
	public class DriveObject
	{
		internal double xPosition;
		internal double yPosition;
		internal double zPosition;
		internal string name;
		

		public DriveObject(double inXposition,double inYposition,double inZposition, string inDriveName)
		{
			xPosition=inXposition;
			yPosition=inYposition;
			zPosition=inZposition;
			name=inDriveName;
		}
	}
}
