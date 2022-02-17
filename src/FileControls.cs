using System;
using System.Windows.Forms;
using System.IO;

namespace _3DFileManager
{

	public class FileControls
	{
		public string CurrentPath;
		public FileControls(string inPath)
		{
			CurrentPath = inPath;
		}


		public bool DeleteFile(string fileToRemove, string shortName)
		{
			DialogResult dlgRes;
			dlgRes=	MessageBox.Show("are you sure you want to delete "+shortName+ "?", "Delete File",
				MessageBoxButtons.YesNo,MessageBoxIcon.Question);
			
			if(dlgRes == DialogResult.Yes)
			{
				try
				{
					File.Delete(fileToRemove);
				}
				catch(UnauthorizedAccessException)
				{
					MessageBox.Show("You do not have permission to delete this File", "delete Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch(Exception) 
				{
					MessageBox.Show("The File is in use", "Delete Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				return true;
			}
			else
			{
				Console.WriteLine("cancelled");
				return false;
			}
			
		}

		public void FileProperties()
		{
			FileProperties fp = new FileProperties(CurrentPath);
			fp.Show();	
		}
		public bool RenameFile(string OldName, string NewName)
		{
			try
			{
				File.Move(OldName,NewName);
				return true;
			}
			catch(PathTooLongException)
			{
				MessageBox.Show("File name is too long", "Rename Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			catch(IOException)
			{
				MessageBox.Show("The File name already Exists", "Rename Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show("You do not have permission to rename this File", "Rename Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			catch(ArgumentException)
			{
				MessageBox.Show("New name contains invalid characters", "Rename Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
	}
}
