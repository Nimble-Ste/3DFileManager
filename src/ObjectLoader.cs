using System;
using System.IO;
using System.Collections; 
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace _3DFileManager
{
	/// <summary>
	/// Summary description for ObjectLoader.
	/// </summary>
	public class ObjectLoader
	{
		public string FileName;

		static ArrayList vz = new ArrayList();
		static ArrayList vx = new ArrayList();
		static ArrayList vy = new ArrayList();
		static ArrayList nz = new ArrayList();
		static ArrayList nx = new ArrayList();
		static ArrayList ny = new ArrayList();

		static ArrayList faces = new ArrayList();

		public ObjectLoader(string inFileName)
		{
			FileName = inFileName;
			LoadFile();
		}

		public void LoadFile()
		{
			string line;
			char [] wspace = {' ','\t'};
			char [] separator = {' ','/','f'};
			string [] tokens;
			string [] indices;

			StreamReader file = new StreamReader(FileName);
			while ((line = file.ReadLine()) != null) 
			{
				// first, strip off comments
				int comment = line.IndexOf('#');
				if (comment >= 0) 
				{
					line = line.Substring(0, comment);
				}
				tokens = line.Split(wspace);
				switch (tokens[0]) 
				{
					case "v" :
						vx.Add(Double.Parse(tokens[2]));
						vy.Add(Double.Parse(tokens[3]));
						vz.Add(Double.Parse(tokens[4]));
						break;
					
					case "vn" :
						nx.Add(Double.Parse(tokens[2]));
						ny.Add(Double.Parse(tokens[3]));
						nz.Add(Double.Parse(tokens[4]));
						break;
					
					case "vt" :
						break;

					case "f" :
						indices = line.Split(separator);
						for(int i = 0;i<indices .Length;i++)
						{
							if(indices[i].Length>0)
							{
								faces.Add(indices[i]);
							}
						}
						break;
					case "g":   // group
						break;
					case "s":   // smoothing group
						break;
					case "":    // blank line
						break;
					default :
						break;
				}
			}
			file.Close(); //close the file
		}

		public void drawModel()
		{
			int number = 0;
			Gl.glColor3d(1,0,0);
			Gl.glBegin(Gl.GL_TRIANGLES);
			for(int i = 0;i<faces.Count;i++)
			{

				Gl.glTexCoord2f(1.0f, 1.0f);

				number = System.Convert.ToInt32(faces[i]);
				double x = System.Convert.ToDouble(vx[number-1]);
				double y = System.Convert.ToDouble(vy[number-1]);
				double z = System.Convert.ToDouble(vz[number-1]);

				double normalX =System.Convert.ToDouble(nx[number-1]);
				double normalY =System.Convert.ToDouble(ny[number-1]);
				double normalZ =System.Convert.ToDouble(nz[number-1]);
				Gl.glNormal3d(normalX,normalY,normalZ);
				Gl.glVertex3d(x,y,z);
			}
			Gl.glEnd();	
		}
	}
}
