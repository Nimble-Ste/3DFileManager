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
	public class Folders
	{
		

		public Folders()
		{

		}
		public void DrawFolder(){
		
			
			Gl.glBegin(Gl.GL_QUADS);
			// Front Face

			Gl.glNormal3f( 0.0f, 0.0f, 1.0f);// Normal Pointing Towards Viewer
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.2f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.2f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.2f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.2f);	// Top Left Of The Texture and Quad
			// Back Face
			Gl.glNormal3f( 0.0f, 0.0f,-1.0f);// Normal Pointing Away From Viewer
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f, -0.2f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f, -0.2f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f, -0.2f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f, -0.2f);	// Bottom Left Of The Texture and Quad
			// Top Face
			Gl.glNormal3f( 0.0f, 1.0f,0.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f, -0.2f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.2f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.2f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f, -0.2f);	// Top Right Of The Texture and Quad
			// Bottom Face
			Gl.glNormal3f( 0.0f, -1.0f,0.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-0.5f, -0.5f, -0.2f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 0.5f, -0.5f, -0.2f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.2f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.2f);	// Bottom Right Of The Texture and Quad
			// Right face
			Gl.glNormal3f( 1.0f, 0.0f,0.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f, -0.2f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f, -0.2f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.2f);	// Top Left Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.2f);	// Bottom Left Of The Texture and Quad
			// Left Face
			Gl.glNormal3f( -1.0f, 0.0f,0.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f, -0.2f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.2f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.2f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f, -0.2f);	// Top Left Of The Texture and Quad
			Gl.glEnd();


		
		}

		public void DrawSelectedFolder()
		{

		Gl.glDisable(Gl.GL_LIGHTING);
		Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
		Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.7f);
		Gl.glPushMatrix();
		Gl.glTranslated(0.1,1,0.0);
		Gl.glScaled(2.5,2.5,2.5); //make the selected folder a little bit bigger
		DrawFolder();
		Gl.glPopMatrix();
		Gl.glColor4f(1f, 1f, 1f, 1f);
		Gl.glEnable(Gl.GL_LIGHTING);


		}

	}
}
