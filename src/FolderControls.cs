using System;
using System.Windows.Forms;
using System.IO;

namespace _3DFileManager
{
	/// <summary>
	/// Summary description for FolderControls.
	/// </summary>
	public class FolderControls
	{
		public string CurrentPath;
		public FolderControls(string inPath)
		{
			CurrentPath = inPath;
		}

		public bool AddFolder(string newFolder)
		{
			//if the folder already exists
			if(Directory.Exists(CurrentPath+"\\"+newFolder))
			{
				MessageBox.Show("Folder name " +newFolder +" Already Exists", "Add Folder Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else
			{
				try
				{
					//try to create the new directory
					Directory.CreateDirectory(CurrentPath+"\\"+newFolder);
					return true;
				}
				//catch exceptions
				catch (UnauthorizedAccessException)
				{
					MessageBox.Show("You do not have permission to add a new folder", "Add Folder Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				catch (ArgumentException)
				{
					MessageBox.Show("New name contains invalid characters", "Add Folder Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				catch(PathTooLongException)
				{
					MessageBox.Show("Folder name is too long", "Add Folder Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				catch(NotSupportedException)
				{
					MessageBox.Show("A folder cannot contain the character :", "Add Folder Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
		}
		public bool DeleteFolder(string folderToRemove, string shortName)
		{
		DialogResult dlgRes;
		dlgRes=	MessageBox.Show("are you sure you want to delete "+shortName+ " and all of its contents?", "Delete folder",
				MessageBoxButtons.YesNo,MessageBoxIcon.Question);
			
			if(dlgRes == DialogResult.Yes)
			{
				Directory.Delete(folderToRemove,true);
				return true;
			}
			else
			{
				return false;
			}
			
		}
		public void properties()
		{
			FolderProperties fp = new FolderProperties(CurrentPath);
			fp.Show();			
		}
		public bool RenameFolder(string OldName, string NewName)
		{
			try
			{
				Directory.Move(OldName,NewName);
				return true;
			}
			catch(PathTooLongException)
			{
				MessageBox.Show("Folder name is too long", "Rename Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			catch(IOException)
			{
				MessageBox.Show("The Folder name already Exists", "Rename Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show("You do not have permission to rename this folder", "Rename Error",
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
