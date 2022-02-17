using System;
using System.IO;
using System.Collections; 
using System.Text;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace _3DFileManager
{
    
	public class FilePositions
	{
		public FileObject[] FileObjects;
		public ArrayList FormattedName = new ArrayList();
		public ArrayList files = new ArrayList();
		public ArrayList store = new ArrayList();
		public ArrayList FileType = new ArrayList();

		public string winDir;
		public int fileCount=0;
		string fileName = "";
		public FileObject fi; //array for the objects

		public FilePositions(string currentPath)
		{
			winDir = currentPath;
			if(GetFiles())
			{
				FileObjects = new FileObject [fileCount];
				CreateFile();
				sort();
				SetPositions();
			}
		}

		//gets the names of the files and formats
		//the names 
		public bool GetFiles()
		{	
			try
			{	
				string[] fileList = Directory.GetFiles(winDir);
				foreach (string file in fileList)
				{
					char[] delimit = new char[] { '\\' };
					foreach (string substr in file.Split(delimit))
					{
						fileName = substr;		
					}
					if(fileName.Length>8)
					{
						StringBuilder MyStringBuilder = new StringBuilder(fileName);
						//get the number of letters to remove
						int end = MyStringBuilder.Length-8;
						//remove the letters starting from position 8
						string formatted = MyStringBuilder.Remove(8,end).ToString();
						//add to the array
						FormattedName.Add(formatted+ "...");
					}
					else
					{
						//file name is at acceptable length
						FormattedName.Add(fileName);
					}

					files.Add(fileName);
	
					string [] temp=  fileName.Split('.');

					if(temp.Length>1)
					{
						FileType.Add(temp [1]);
					}
					else
					{
						//some files have no extension type
						FileType.Add("none");
					}
					fileCount++;
				}
			}	
			catch (Exception)
			{
				return false;
			}
			
			return true;
		}

		//gets the names and the last access date
		//of the files and set up the file objects
		public void CreateFile()
		{
			for (int i = 0; i< fileCount; i++)
			{			
				string formatted="";
				formatted=FormattedName[i]+formatted; 

				string Unformatted =""; 
				Unformatted = files[i]+Unformatted;

				string type = "";
				type = FileType[i] + type;
				
				DateTime dt = System.Convert.ToDateTime(Directory.GetLastAccessTime(winDir+"\\"+Unformatted));

				//set up the file object
				fi  = new FileObject(0,0,0.0,Unformatted,formatted,type, dt);
				
				//store the file object
				StoreFile(fi);			
			}
		}

		//stores the files in the FileObject array
		public bool StoreFile(FileObject input ) 
		{
			for ( int i = 0 ; i < FileObjects.Length ; i = i + 1 )
			{
				if (FileObjects [i] == null ) 
				{
					// found an empty space
					FileObjects [i] = input ;
					return true ;
				}
			}
			return false ;
		}

		//use the bubble sort algorithm
		//to order the files starting with the
		//file that was accessed last
		//(the knife draw effect)
		public void sort()
		{
			for (int i = FileObjects.Length;--i>=0;) 
			{
				bool flipped = false;
				for (int j = 0; j<i; j++) 
				{
					if (FileObjects[j].FileLastAccessed < FileObjects[j+1].FileLastAccessed) 
					{
						FileObject Temp = FileObjects[j];
						FileObjects[j] = FileObjects[j+1];
						FileObjects[j+1] = Temp;
						flipped = true;
					}
				}
				if (!flipped) 
				{
					return;
				}
			}
		}

		//set the file positions
		public void SetPositions()
		{
			double offset = 0.0;
			int counter = 0;
		
			for (int i = 0; i< FileObjects.Length; i++)
			{			
				if(counter == 5)
				{
					offset = offset-0.7;
					counter = 0;
				}

				FileObjects[i].xPosition=counter*+0.7;
				FileObjects[i].yPosition=offset*1.1;
				counter = counter +1;
			}
		}

		//gets how many files there are
		public int GetFileCount()
		{
			return fileCount;
		}

		//returns x position of the files
		public double GetXposition(int FileNumber)
		{
			return FileObjects[FileNumber].xPosition;	
		}

		//returns y position of the files
		public double GetYposition(int FileNumber)
		{
			return FileObjects[FileNumber].yPosition;
		}

		//returns z position of the files
		public double GetZposition(int FileNumber)
		{
			return FileObjects[FileNumber].zPosition;
		}

		//returns the formatted filename
		public string GetFormattedName(int FileNumber)
		{		
			
		   return FileObjects[FileNumber].formattedName;
		}

		//returns unformatted filename
		public string GetUnformattedName(int FileNumber)
		{
			return FileObjects[FileNumber].name;
		}

		//returns the file type/extension
		public string GetFileType(int FileNumber)
		{
           return FileObjects[FileNumber].fileType;
		}
	}
}
