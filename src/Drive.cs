using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Collections; 

namespace _3DFileManager
{

	public class Drive
	{
		public Drive()
		{


		}

		public static void DrawDrive()
		{
		
			
			Gl.glBegin(Gl.GL_QUADS);
			// Front Face

			Gl.glNormal3f( 0.0f, 0.0f, 1.0f);// Normal Pointing Towards Viewer
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.05f);	// Top Left Of The Texture and Quad
			// Back Face
			Gl.glNormal3f( 0.0f, 0.0f,-1.0f);// Normal Pointing Away From Viewer
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f, -0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f, -0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f, -0.05f);	// Bottom Left Of The Texture and Quad
			// Top Face
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f, -0.05f);	// Top Right Of The Texture and Quad
			// Bottom Face
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-0.5f, -0.5f, -0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 0.5f, -0.5f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			// Right face
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f, -0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f, -0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.05f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			// Left Face
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f, -0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glEnd();


		
		}
	}
}
