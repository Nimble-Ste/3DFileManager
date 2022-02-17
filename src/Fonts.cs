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

	public class Fonts
	{
		private static IntPtr hDC;                   // Private GDI Device Context
		private static IntPtr hRC;                   // Permanent Rendering Context
		private static int fontbase;
		TextureLoader textures  = new TextureLoader();
		public Fonts()
		{
			textures.LoadGLTextures();
		}

		public void BuildFont() 
		{
			
			Gdi.GLYPHMETRICSFLOAT[] gmf = new Gdi.GLYPHMETRICSFLOAT[256];       // Address Buffer For Font Storage
			IntPtr font;                                                        // Windows Font ID
			fontbase = Gl.glGenLists(256);                                      // Storage For 256 Characters

			font = Gdi.CreateFont(                                              // Create The Font
				-12,                                                            // Height Of Font
				0,                                                              // Width Of Font
				0,                                                              // Angle Of Escapement
				0,                                                              // Orientation Angle
				Gdi.FW_BOLD,                                                    // Font Weight
				false,                                                          // Italic
				false,                                                          // Underline
				false,                                                          // Strikeout
				Gdi.ANSI_CHARSET,                                            // Character Set Identifier
				Gdi.OUT_TT_PRECIS,                                              // Output Precision
				Gdi.CLIP_DEFAULT_PRECIS,                                        // Clipping Precision
				Gdi.ANTIALIASED_QUALITY,                                        // Output Quality
				Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,                            // Family And Pitch
				"Century Gothic");                                                   // Font Name

			Gdi.SelectObject(hDC, font);                                        // Selects The Font We Created
			Wgl.wglUseFontOutlines(
				hDC,                                                            // Select The Current DC
				0,                                                              // Starting Character
				255,                                                            // Number Of Display Lists To Build
				fontbase,                                                       // Starting Display Lists
				0.1f,                                                           // Deviation From The True Outlines
				0.1f,                                                           // Font Thickness In The Z Direction
				Wgl.WGL_FONT_POLYGONS,                                          // Use Polygons, Not Lines
				gmf);                                                           // Address Of Buffer To Recieve Data
		}

		public void glPrint(string text) 
		{
			Gl.glScaled(0.8,0.8,0.8);
		
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[1]);
			if(text == null || text.Length == 0) 
			{                              // If There's No Text
				return;                                                         // Do Nothing
			}

			Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
			Gl.glListBase(fontbase);                                        // Sets The Base Character to 0
			// .NET: We can't draw text directly, it's a string!
			byte [] textbytes = new byte [text.Length];
			for (int i = 0; i < text.Length; i++) textbytes[i] = (byte) text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);    // Draws The Display List Text
			Gl.glPopAttrib();                                                   // Pops The Display List Bits
		}

		public void glPrintFileName(string text) 
		{
			
			Gl.glScaled(0.13,0.13,0.13);

			//Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[1]);
			if(text == null || text.Length == 0) 
			{                              // If There's No Text
				return;                                                         // Do Nothing
			}

			Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
			Gl.glListBase(fontbase);                                        // Sets The Base Character to 0
			// .NET: We can't draw text directly, it's a string!
			byte [] textbytes = new byte [text.Length];
			for (int i = 0; i < text.Length; i++) textbytes[i] = (byte) text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);    // Draws The Display List Text
			
			Gl.glPopAttrib();                                                   // Pops The Display List Bits
		}


	}
}
