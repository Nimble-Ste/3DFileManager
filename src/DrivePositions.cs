using System;
using System.IO;
using System.Collections; 
using System.Text;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace _3DFileManager
{
    
	public class DrivePositions
	{
	
		public DriveObject[] DriveObjects;
		public ArrayList Names = new ArrayList();
		public ArrayList store = new ArrayList();
		public int DriveCount;
		double C = 0.0;
		public double rad = 0.0;
		public double spacing = 5.0;

		public DriveObject dr; //array for the objects

		public DrivePositions()
		{
			if(GetDrives())
			{
				DriveObjects = new DriveObject [DriveCount];
				SetDrivePositions();
			}
		}

		public bool GetDrives()
		{
			string[] tempDrives = Directory.GetLogicalDrives();
			foreach(string drive in tempDrives)
			{
				Names.Add(drive); 
				DriveCount++;
			}
			C  = DriveCount*spacing;
			rad = (C/3.14)/2;
			return true;
		}

		public void SetDrivePositions()
		{   		
			for (int i = 0; i< DriveCount; i++)
			{
				double ypos = rad * System.Math.Cos(i*2*Math.PI/DriveCount);
				double zpos = rad * System.Math.Sin(i*2*Math.PI/DriveCount);

				string temp = "";
				temp = Names[i]+temp;
				dr = new DriveObject(-1,ypos,zpos, temp);
				
				StoreDrive(dr);
			}
		}

		public bool StoreDrive(DriveObject input ) 
		{
			for ( int i = 0 ; i < DriveObjects.Length ; i = i + 1 )
			{
				if (DriveObjects [i] == null ) 
				{
	
					// found an empty space
					DriveObjects [i] = input ;
					return true ;
				}
			}
			return false ;
		}

		public double GetRad()
		{
			return rad;
		}

		//gets how many folders there are
		public int GetDriveCount()
		{
			return DriveObjects.Length;
		}

		public double GetYposition(int DriveNumber)
		{
			return DriveObjects[DriveNumber].yPosition;
		}

		public double GetZposition(int DriveNumber)
		{
			return DriveObjects[DriveNumber].zPosition;
		}

		public string GetName(int DriveNumber)
		{		
			return DriveObjects[DriveNumber].name;
		}
	}
}
