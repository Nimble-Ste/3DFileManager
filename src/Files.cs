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
	/// <summary>
	/// Summary description for File.
	/// </summary>
	public class Files
	{
		public Files()
		{

		}
		public void DrawFile()
		{
			Gl.glBegin(Gl.GL_QUADS);
			// Front Face

			Gl.glNormal3f( 0.0f, 0.0f, 1.0f);// Normal Pointing Towards Viewer
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.2f, -0.2f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.2f, -0.2f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.2f,  0.2f,  0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.2f,  0.2f,  0.05f);	// Top Left Of The Texture and Quad
			// Back Face
			Gl.glNormal3f( 0.0f, 0.0f,-1.0f);// Normal Pointing Away From Viewer
			Gl.glVertex3f(-0.2f, -0.2f, -0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glVertex3f(-0.2f,  0.2f, -0.05f);	// Top Right Of The Texture and Quad
			Gl.glVertex3f( 0.2f,  0.2f, -0.05f);	// Top Left Of The Texture and Quad
		    Gl.glVertex3f( 0.2f, -0.2f, -0.05f);	// Bottom Left Of The Texture and Quad
			// Top Face
			Gl.glNormal3f( 0.0f, 1.0f,0.0f);
			Gl.glVertex3f(-0.2f,  0.2f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glVertex3f(-0.2f,  0.2f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glVertex3f( 0.2f,  0.2f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glVertex3f( 0.2f,  0.2f, -0.05f);	// Top Right Of The Texture and Quad
			// Bottom Face
			Gl.glNormal3f( 0.0f, -1.0f,0.0f);
			Gl.glVertex3f(-0.2f, -0.2f, -0.05f);	// Top Right Of The Texture and Quad
		    Gl.glVertex3f( 0.2f, -0.2f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glVertex3f( 0.2f, -0.2f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glVertex3f(-0.2f, -0.2f,  0.05f);	// Bottom Right Of The Texture and Quad
			// Right face
			Gl.glNormal3f( 1.0f, 0.0f,0.0f);
			Gl.glVertex3f( 0.2f, -0.2f, -0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glVertex3f( 0.2f,  0.2f, -0.05f);	// Top Right Of The Texture and Quad
			Gl.glVertex3f( 0.2f,  0.2f,  0.05f);	// Top Left Of The Texture and Quad
			Gl.glVertex3f( 0.2f, -0.2f,  0.05f);	// Bottom Left Of The Texture and Quad
			// Left Face
			Gl.glNormal3f( -1.0f, 0.0f,0.0f);
			Gl.glVertex3f(-0.2f, -0.2f, -0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glVertex3f(-0.2f, -0.2f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glVertex3f(-0.2f,  0.2f,  0.05f);	// Top Right Of The Texture and Quad
			Gl.glVertex3f(-0.2f,  0.2f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glEnd();
		}
	
		public void DrawSelectedFile()
		{

			Gl.glDisable(Gl.GL_LIGHTING);
			//Gl.glDisable(Gl.GL_LIGHT1);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glColor4f(0.9f, 0.9f, 0.9f, 0.3f);
			Gl.glPushMatrix();
			Gl.glScaled(1.05,1.05,1.05); //make the selected file a little bit bigger
			DrawFile();
			Gl.glPopMatrix();
			Gl.glColor4f(1f, 1f, 1f, 1f);
			Gl.glEnable(Gl.GL_LIGHTING);
			//Gl.glEnable(Gl.GL_LIGHT1);

		}
	}
}
