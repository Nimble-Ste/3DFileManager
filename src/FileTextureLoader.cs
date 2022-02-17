using System;
using System.IO;
using System.Collections; 
using System.Drawing;
using System.Drawing.Imaging;
using Tao.OpenGl;

namespace _3DFileManager
{
	/// <summary>
	/// Summary description for FileTextureLoader.
	/// </summary>
	public class FileTextureLoader
	{
		public ArrayList fileTypes = new ArrayList();
		public ArrayList texturePaths = new ArrayList();
		public int [] texture; //texture = new int[10];
		public int numberOfTextures = 0;
		public string directory = "FileTextures";

		public FileTextureLoader()
		{
			string[] fileList = Directory.GetFiles(directory);

			foreach(string dir in fileList)
			{
				string [] temp=  dir.Split('.','\\');
				if(temp[1].Equals("Thumbs"))
				{
					Console.WriteLine("ignore");
				}
				else
				{
					fileTypes.Add(temp [1]);
					texturePaths.Add(temp[0]+"\\"+temp[1]+"."+temp[2]);
					numberOfTextures  = numberOfTextures  +1;
				}			
			}
			texture = new int[numberOfTextures];
		}

		public Bitmap LoadBMP(string fileName) 
		{
			if(fileName == null || fileName == string.Empty) 
			{                  // Make Sure A Filename Was Given
				return null;                                                    // If Not Return Null
			}

			string fileName1 = string.Format("Data{0}{1}",                      // Look For Data\Filename
				Path.DirectorySeparatorChar, fileName);
			string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          // Look For ..\..\Data\Filename
				"..", Path.DirectorySeparatorChar, fileName);

			// Make Sure The File Exists In One Of The Usual Directories
			if(!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2)) 
			{
				return null;                                                    // If Not Return Null
			}

			if(File.Exists(fileName)) 
			{                                         // Does The File Exist Here?
				return new Bitmap(fileName);                                    // Load The Bitmap
			}
			else if(File.Exists(fileName1)) 
			{                                   // Does The File Exist Here?
				return new Bitmap(fileName1);                                   // Load The Bitmap
			}
			else if(File.Exists(fileName2)) 
			{                                   // Does The File Exist Here?
				return new Bitmap(fileName2);                                   // Load The Bitmap
			}

			return null;                                                        // If Load Failed Return Null
		}

		public bool LoadGLTextures() 
		{
			bool status = false;                                                // Status Indicator
			Bitmap[] textureImage = new Bitmap[texture.Length];                              // Create Storage Space For The Texture

			for(int i = 0; i<texture.Length;i++)
			{
				string texPath = texturePaths[i].ToString(); 
				textureImage[i] = LoadBMP(texPath); 
			}
	
			// Check For Errors, If Bitmap's Not Found, Quit
			if(textureImage[0] != null)
			{

				status = true;
				// Set The Status To True
				for (int loop=0; loop<textureImage.Length; loop++)
				{
 
					Gl.glGenTextures(1, out texture[loop]);                            // Create The Texture

					textureImage[loop].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
					// Rectangle For Locking The Bitmap In Memory
					Rectangle rectangle = new Rectangle(0, 0, textureImage[loop].Width, textureImage[loop].Height);
					// Get The Bitmap's Pixel Data From The Locked Bitmap
					BitmapData bitmapData = textureImage[loop].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

					// Typical Texture Generation Using Data From The Bitmap
					Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[loop]);
					Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[loop].Width, textureImage[loop].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
					Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
					Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
					Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
					Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);

					if(textureImage[loop] != null) 
					{                                   // If Texture Exists
						textureImage[loop].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
						textureImage[loop].Dispose();                                  // Dispose The Bitmap
					}
				}
			}	
			return status;                                                      // Return The Status
		}
	}
}
