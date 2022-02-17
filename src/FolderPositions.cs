using System;
using System.IO;
using System.Collections; 
using System.Text;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Windows.Forms;

namespace _3DFileManager
{
    
	public class FolderPositions
	{
	
		public FolderObject[] FolderObjects;
		public ArrayList formattedNames = new ArrayList();
		public ArrayList UnFormattedName = new ArrayList();
		public ArrayList Names = new ArrayList();
		public ArrayList store = new ArrayList();
		public string winDir;
		public int folderCount=0;
		double C = 0.0;
		public double rad = 0.0;
		public double spacing = 3.0;

		public FolderObject fo; //array for the objects

		public FolderPositions(string currentPath)
		{
			winDir = currentPath;
			if(getFolders())
			{
				FolderObjects = new FolderObject [folderCount];
				CreateFolder();
				Sort();
				SetFolderPositions();
			}
			else
			{
				MessageBox.Show("Disk drive is empty, insert a disk and try again", "Drive Empty",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public bool getFolders()
		{
			try
			{	
				string[] dirs = Directory.GetDirectories(winDir);
			
				char[] delimit = new char[] { '\\' };
			
				foreach(string dir in dirs)
				{
				     Names.Add(dir);          
					string [] substr = dir.Split(delimit);

					string temp = substr[substr.Length-1];
					if(temp.Length>14)
					{
						StringBuilder MyStringBuilder = new StringBuilder(temp);
						//get the number of letters to remove
						int end = MyStringBuilder.Length-14;
						//remove the letters starting from position 8
						string formatted = MyStringBuilder.Remove(14,end).ToString();
						//add to the array
						formattedNames.Add(formatted+ "...");
					}
					else
					{
						//file name is at acceptable length
						formattedNames.Add(temp);
					}
					
					UnFormattedName.Add(substr[substr.Length-1]);

					folderCount++;				
				}

				C  = folderCount*spacing;
				rad = (C/3.14)/2;
			}
			catch (Exception)
			{
				//return false if cant access the drive
				return false;
			}	
			return true;
		}

		public void CreateFolder()
		{   		
			for (int i = 0; i< folderCount; i++)
			{
				string formatted="";
				formatted=formattedNames[i]+formatted; 

				string Unformatted =""; 
				Unformatted = UnFormattedName[i]+Unformatted;

				string fullName =""; 
				fullName = Names[i]+fullName;

				DateTime dt = System.Convert.ToDateTime(Directory.GetLastAccessTime(winDir+"\\"+Unformatted));

				fo  = new FolderObject(0,0,fullName,Unformatted,formatted, dt);
			
				storeFolder(fo);
			}
		}

		public bool storeFolder(FolderObject input ) 
		{
			for ( int i = 0 ; i < FolderObjects.Length ; i = i + 1 )
			{
				if (FolderObjects [i] == null ) 
				{
	
					// found an empty space
					FolderObjects [i] = input ;
					return true ;
				}
			}
			return false ;
		}

		//use the bubble sort algorithm
		//to order the folders starting with the
		//folder that was accessed last
		//(the knife draw effect)
		public void Sort()
		{
			for (int i = FolderObjects.Length;--i>=0;) 
			{
				bool flipped = false;
				for (int j = 0; j<i; j++) 
				{
					if (FolderObjects[j].FolderLastAccessed < FolderObjects[j+1].FolderLastAccessed) 
					{
						FolderObject Temp = FolderObjects[j];
						FolderObjects[j] = FolderObjects[j+1];
						FolderObjects[j+1] = Temp;
						flipped = true;
					}
				}
				if (!flipped) 
				{
					return;
				}
			}
		}

		public void SetFolderPositions()
		{
			for (int i = 0; i< folderCount; i++)
			{
				double ypos = rad * System.Math.Cos(i*2*Math.PI/folderCount);
				double zpos = rad * System.Math.Sin(i*2*Math.PI/folderCount);
				
				FolderObjects[i].yPosition=ypos;
				FolderObjects[i].zPosition=zpos;
			}
		}
		public double GetRad()
		{
			return rad;
		}

		public double GetYposition(int FolderNumber)
		{
			return FolderObjects[FolderNumber].yPosition;
		}

		public double GetZposition(int FolderNumber)
		{
			return FolderObjects[FolderNumber].zPosition;
		}

		public string GetFormattedName(int FolderNumber)
		{		
			return FolderObjects[FolderNumber].formattedName;
		}

		public string GetUnformattedName(int FolderNumber)
		{
			return FolderObjects[FolderNumber].name;
		}

		public string GetFullName(int FolderNumber)
		{
			return FolderObjects[FolderNumber].fullName;
		}
	}
}
