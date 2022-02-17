using System;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace _3DFileManager
{

	public class navigationButtons
	{
		public navigationButtons()
		{


		}


      	public void DrawButton()
		{
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glColor4d(0.7,0.7,0.7,0.7);
			Gl.glBegin(Gl.GL_QUADS);
			// Front Face

			Gl.glNormal3f( 0.0f, 0.0f, 1.0f);// Normal Pointing Towards Viewer
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-0.5f, -0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.5f, -0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.5f,  0.5f,  0.05f);	// Top Right Of The Texture and Quad
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-0.5f,  0.5f,  0.05f);	// Top Left Of The Texture and Quad
			// Back Face
			Gl.glNormal3f( 0.0f, 0.0f,-1.0f);// Normal Pointing Away From Viewer
			 Gl.glVertex3f(-0.5f, -0.5f, -0.05f);	// Bottom Right Of The Texture and Quad
			 Gl.glVertex3f(-0.5f,  0.5f, -0.05f);	// Top Right Of The Texture and Quad
			 Gl.glVertex3f( 0.5f,  0.5f, -0.05f);	// Top Left Of The Texture and Quad
			 Gl.glVertex3f( 0.5f, -0.5f, -0.05f);	// Bottom Left Of The Texture and Quad
			// Top Face
			 Gl.glVertex3f(-0.5f,  0.5f, -0.05f);	// Top Left Of The Texture and Quad
			 Gl.glVertex3f(-0.5f,  0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			 Gl.glVertex3f( 0.5f,  0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			 Gl.glVertex3f( 0.5f,  0.5f, -0.05f);	// Top Right Of The Texture and Quad
			// Bottom Face
			 Gl.glVertex3f(-0.5f, -0.5f, -0.05f);	// Top Right Of The Texture and Quad
			 Gl.glVertex3f( 0.5f, -0.5f, -0.05f);	// Top Left Of The Texture and Quad
		     Gl.glVertex3f( 0.5f, -0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			 Gl.glVertex3f(-0.5f, -0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			// Right face
			 Gl.glVertex3f( 0.5f, -0.5f, -0.05f);	// Bottom Right Of The Texture and Quad
			 Gl.glVertex3f( 0.5f,  0.5f, -0.05f);	// Top Right Of The Texture and Quad
			 Gl.glVertex3f( 0.5f,  0.5f,  0.05f);	// Top Left Of The Texture and Quad
			 Gl.glVertex3f( 0.5f, -0.5f,  0.05f);	// Bottom Left Of The Texture and Quad
			// Left Face
			 Gl.glVertex3f(-0.5f, -0.5f, -0.05f);	// Bottom Left Of The Texture and Quad
			 Gl.glVertex3f(-0.5f, -0.5f,  0.05f);	// Bottom Right Of The Texture and Quad
			 Gl.glVertex3f(-0.5f,  0.5f,  0.05f);	// Top Right Of The Texture and Quad
			 Gl.glVertex3f(-0.5f,  0.5f, -0.05f);	// Top Left Of The Texture and Quad
			Gl.glEnd();
			Gl.glColor4d(1,1,1,1);
		}
	}
}
