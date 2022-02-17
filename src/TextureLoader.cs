using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Collections; 
using System.Text;

namespace _3DFileManager
{
	/// <summary>
	/// Summary description for TextureLoader.
	/// </summary>
	public class TextureLoader
	{
		public int[] texture = new int[5];  //array for storing textures
		public TextureLoader()
		{
	
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

			textureImage[0] = LoadBMP("DefaultTextures\\folder.bmp"); 
			textureImage[1] = LoadBMP("DefaultTextures\\text.bmp");
			textureImage[2] = LoadBMP("DefaultTextures\\select.bmp");
			textureImage[3] = LoadBMP("DefaultTextures\\none.jpg");
			textureImage[4] = LoadBMP("DefaultTextures\\Drive.bmp");

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
