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

	public class FileManager : System.Windows.Forms.Form
	{
	
		static string path = "";
		private static bool active = true;			 //make the window active                                   
		private static bool done = false;			 //control for the main loop

		private static IntPtr hDC;                   // Private GDI Device Context
		private static IntPtr hRC;                   // Permanent Rendering Context
                                
		private static int fontbase;
    
		static Form form;							 //the form that is being used                           
		private static bool[] keys = new bool[256];  //array for the keyboard

		static int W = 0;
		static int H = 0;

		private static int DriveModel;
		private static int SelectedDrive;
		private static int folder;
		private static int fileObjects;
		private static int selected;
		private static int selectedFile;

		

		static Folders f;
		static FolderPositions fo;
		static Files fi;
		static FilePositions files;
		static Drive d;
		static DriveModel driveM;
		static DrivePositions drive;
		static ObjectLoader loader;
		static FileTextureLoader ft;

		static double zoom = -30.0;
		static double rot = 5.0;
		static double angle = 60.0;
		static double filePositions = -1; 

		static int fov = 60;

		static double mouseX , mouseY;
		static int mWheel = 0;
		
	
		static bool select = false;

		private static int ObjectSelected=-1;
		private static int [] selectBuf = new int [512];

		private static bool fileSelected = false;
		static bool folderSelected =false;
		static bool driveSelected = false;
		static bool showDrives = false;

		static int folderCount = 0;
		static int fileCount = 0;
		static int clickCount = 0;
		
		static bool RightClick = false;

		static bool rotateUp = false;
		static bool rotateDown = false;
		static bool zoomIn = false;
		static bool zoomOut = false;
		static bool filesUp = false;
		static bool filesDown = false;
		
		static ContextMenu cmenu = new ContextMenu();	
		static TextureLoader textures = new TextureLoader();

		static float [] light0_position = {0.0f, 2.0f, 2.0f, 0.0f}; 
	

		private System.Windows.Forms.Panel leftPanel;
		private System.Windows.Forms.Panel bottomPanel;
		static System.Windows.Forms.Label address; //address;
		private System.Windows.Forms.Button BackButton;
		private System.Windows.Forms.Button upArrow;
		private System.Windows.Forms.Button downArrow;
		private System.Windows.Forms.Button zoomInButton;
		private System.Windows.Forms.Button zoomOutButton;
		private System.Windows.Forms.Button filesUpButton;
		private System.Windows.Forms.Button filesDownButton;

		public FileManager() 
		{ 

			address = new Label();
			InitializeComponent();
			this.CreateParams.ClassStyle = this.CreateParams.ClassStyle |       // Redraw On Size, And Own DC For Window.
				User.CS_HREDRAW | User.CS_VREDRAW | User.CS_OWNDC;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
			this.SetStyle(ControlStyles.DoubleBuffer, true);                    // Buffer Control
			this.SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
			this.SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
			this.SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves

			this.Activated += new EventHandler(this.Form_Activated);            // On Activate Event Call Form_Activated
			this.Closing += new CancelEventHandler(this.Form_Closing);          // On Closing Event Call Form_Closing
			this.Deactivate += new EventHandler(this.Form_Deactivate);  
           	this.Resize += new EventHandler(this.Form_Resize); // On Resize Event Call Form_Resize
	 
		}



		[STAThread]
		public static void Main(string[] commandLineArguments) 
		{
			if(!CreateGLWindow("3D File Manager", 16)) 
			{
				return;                                                         // Quit If Window Was Not Created
			}

			while(!done) 
			{                                                      // Loop That Runs While done = false
				Application.DoEvents();                                         // Process Events

				// Draw The Scene.  Watch For ESC Key And Quit Messages From DrawGLScene()
				if((active && (form != null) && !DrawGLScene()) || keys[(int) Keys.Escape]) 
				{   //set the finish flag to true
					done = true;                                                // ESC Or DrawGLScene Signalled A Quit
				}
				else 
				{     
					// Not Time To Quit, Update Screen
					Gdi.SwapBuffers(hDC); // Swap Buffers (Double Buffering)
			
					//if F1 is pressed end the program
					if(keys[(int) Keys.F1]) 
					{                    
						KillGLWindow();                                                       
					}

					if(keys[(int) Keys.Q]) 
					{    
						zoom = zoom +0.25;                        
					}

					if(keys[(int) Keys.E]) 
					{                                 
						zoom = zoom -0.25;  
					}

					if(keys[(int) Keys.D]) 
					{                                 
						rot = rot +0.55;                                                       
					}
						
					if(keys[(int) Keys.A]) 
					{                                 
						rot = rot -0.55;                                                       
					}

					if(keys[(int) Keys.S]) 
					{                                 
						angle = angle +0.55;                                                       
					}
					if(keys[(int) Keys.W]) 
					{                                 
						angle = angle -0.55;                                                       
					}
					if(keys[(int) Keys.PageUp]) 
					{                                 
						filePositions = filePositions +0.05;                                                       
					}
					if(keys[(int) Keys.PageDown]) 
					{                                 
						filePositions = filePositions -0.05;                                                       
					}
					if(keys[(int) Keys.B]) 
					{                                 
						                                                     
					}
			
				}
			}

			// Shutdown
			KillGLWindow();                                                     // Kill The Window
			return;                                                             // Exit The Program
		}


		private static bool CreateGLWindow(string title, int bits) 
		{
			int pixelFormat;                                                    // Holds The Results After Searching For A Match
			//fullscreen = fullscreenflag;                                        // Set The Global Fullscreen Flag
			form = null;                                                        // Null The Form

			GC.Collect();                                                       // Request A Collection
			// This Forces A Swap
			Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

			form = new FileManager();                                              // Create The Window

			//make the form sizable
			form.FormBorderStyle = FormBorderStyle.Sizable;
			form.Text = title; 
			//show the cursor
			Cursor.Show();

			Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();    // pfd Tells Windows How We Want Things To Be
			pfd.nSize = (short) Marshal.SizeOf(pfd);                            // Size Of This Pixel Format Descriptor
			pfd.nVersion = 1;                                                   // Version Number
			pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW |                              // Format Must Support Window
				Gdi.PFD_SUPPORT_OPENGL |                                        // Format Must Support OpenGL
				Gdi.PFD_DOUBLEBUFFER;                                           // Format Must Support Double Buffering
			pfd.iPixelType = (byte) Gdi.PFD_TYPE_RGBA;                          // Request An RGBA Format
			pfd.cColorBits = (byte) bits;                                       // Select Our Color Depth
			pfd.cRedBits = 0;                                                   // Color Bits Ignored
			pfd.cRedShift = 0;
			pfd.cGreenBits = 0;
			pfd.cGreenShift = 0;
			pfd.cBlueBits = 0;
			pfd.cBlueShift = 0;
			pfd.cAlphaBits = 0;                                                 // No Alpha Buffer
			pfd.cAlphaShift = 0;                                                // Shift Bit Ignored
			pfd.cAccumBits = 0;                                                 // No Accumulation Buffer
			pfd.cAccumRedBits = 0;                                              // Accumulation Bits Ignored
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits = 0;
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits = 16;                                                // 16Bit Z-Buffer (Depth Buffer)
			pfd.cStencilBits = 0;                                               // No Stencil Buffer
			pfd.cAuxBuffers = 0;                                                // No Auxiliary Buffer
			pfd.iLayerType = (byte) Gdi.PFD_MAIN_PLANE;                         // Main Drawing Layer
			pfd.bReserved = 0;                                                  // Reserved
			pfd.dwLayerMask = 0;                                                // Layer Masks Ignored
			pfd.dwVisibleMask = 0;
			pfd.dwDamageMask = 0;

			hDC = User.GetDC(form.Handle);                                      // Attempt To Get A Device Context
			if(hDC == IntPtr.Zero) 
			{                                            // Did We Get A Device Context?
				KillGLWindow();                                                 // Reset The Display
				MessageBox.Show("Can't Create A GL Device Context.", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);                  // Attempt To Find An Appropriate Pixel Format
			if(pixelFormat == 0) 
			{                                              // Did Windows Find A Matching Pixel Format?
				KillGLWindow();                                                 // Reset The Display
				MessageBox.Show("Can't Find A Suitable PixelFormat.", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if(!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd)) 
			{                // Are We Able To Set The Pixel Format?
				KillGLWindow();                                                 // Reset The Display
				MessageBox.Show("Can't Set The PixelFormat.", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			hRC = Wgl.wglCreateContext(hDC);                                    // Attempt To Get The Rendering Context
			if(hRC == IntPtr.Zero) 
			{                                            // Are We Able To Get A Rendering Context?
				KillGLWindow();                                                 // Reset The Display
				MessageBox.Show("Can't Create A GL Rendering Context.", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if(!Wgl.wglMakeCurrent(hDC, hRC)) 
			{                                 // Try To Activate The Rendering Context
				KillGLWindow();                                                 // Reset The Display
				MessageBox.Show("Can't Activate The GL Rendering Context.", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			form.Show();                                                        // Show The Window
			form.TopMost = true;                                                // Topmost Window
			form.Focus();                                                       // Focus The Window

			ReSizeGLScene(form.Width, form.Height);                                       // Set Up Our Perspective GL Screen

			if(!InitGL()) 
			{                                                     // Initialize Our Newly Created GL Window
				KillGLWindow();                                                 // Reset The Display
				MessageBox.Show("Initialization Failed.", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;                                                        // Success
		}

		private static void BuildLists() 
		{
			DriveModel = Gl.glGenLists(1); 
			Gl.glNewList(DriveModel, Gl.GL_COMPILE);
			driveM.DrawDrive();
			Gl.glEndList();

			SelectedDrive = Gl.glGenLists(1); 
			Gl.glNewList(SelectedDrive, Gl.GL_COMPILE);
			driveM.DrawSelectedDrive();
			Gl.glEndList();

			folder = Gl.glGenLists(1); 
			Gl.glNewList(folder, Gl.GL_COMPILE);
			loader.drawModel();
			Gl.glEndList();

			fileObjects = Gl.glGenLists(1); 
			Gl.glNewList(fileObjects, Gl.GL_COMPILE);
			fi.DrawFile();
			Gl.glEndList();
			
			selected = Gl.glGenLists(1); 
			Gl.glNewList(selected, Gl.GL_COMPILE);
			Gl.glPushMatrix();
			f.DrawSelectedFolder();
			Gl.glPopMatrix();
			Gl.glEndList();

			selectedFile = Gl.glGenLists(1); 
			Gl.glNewList(selectedFile, Gl.GL_COMPILE);
			fi.DrawSelectedFile();
			Gl.glEndList();
			
		}

		static bool CheckUserInput()
		{
			if(contextMenu())
			{
				RightClick = false;
			}

			if(filesUp)
			{
				filePositions = filePositions +0.05; 
			}

			if(filesDown)
			{
				filePositions = filePositions -0.05; 
			}

			if(rotateUp)
			{
				angle = angle -0.55; 
			}

			if(rotateDown)
			{
				angle = angle +0.55; 
			}

			if(zoomIn)
			{
				zoom = zoom +0.25; 
			}

			if(zoomOut)
			{
				zoom = zoom -0.25; 
			}
			if((clickCount ==2)&&(folderSelected))//if the user double clicked on a folder
			{
				clickCount = 0; //reset the click count
				path = fo.GetFullName(ObjectSelected);//get which one was selected
				angle = 60.0;
				scanPath();	
				address.Text= getPath();								
			}

			if((clickCount ==2)&&(fileSelected))//if the user double clicked on a folder
			{
				//open the file
				System.Diagnostics.Process psi =   new System.Diagnostics.Process();
				psi.StartInfo.FileName= path+"\\"+files.GetUnformattedName(ObjectSelected-folderCount);
				try
				{
					psi.Start();
					form.SendToBack();
				}
				catch (System.ComponentModel.Win32Exception)
				{
					MessageBox.Show("No program specified to the file type", "Unable to open file",MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				clickCount = 0; //reset the click count
			}

			if((clickCount ==2)&&(driveSelected))//if the user double clicked on a drive
			{				
				clickCount = 0; //reset the click count
				driveSelected = false;
				showDrives = false; //dont want to see them anymore
				path = drive.GetName(ObjectSelected);//get which one was selected				folderSelected = false;
				scanPath();
				ObjectSelected = -1;
				folderSelected = false;
				fileSelected = false;
				driveSelected = false;
				address.Text= getPath();
			}		
			return true;
		}

		private static void scanPath()
		{
			clickCount = 0; 
	
			fo = new FolderPositions(path);
			folderCount = fo.folderCount;
		
			files = new FilePositions(path);
			fileCount = files.fileCount;
			filePositions = -1;

			ObjectSelected = -1;
			folderSelected = false;
			fileSelected = false;
			driveSelected = false;

		}

		private static void drawDrives()
		{
			Gl.glInitNames();
			Gl.glPushMatrix();
			Gl.glTranslated(-15,0,zoom-drive.rad);
			
			Gl.glRotated(rot,0.0,1.0,0.0);
			Gl.glRotated(angle,1.0,0.0,0.0);

			for(int j=0; j< drive.DriveCount; j++)
			{			
				Gl.glPushMatrix();
				Gl.glPushName(j);
				Gl.glTranslated(0, drive.GetYposition(j), drive.GetZposition(j));
				Gl.glRotated(-angle,1.0,0.0,0.0);
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[4]);
				Gl.glCallList(DriveModel);

				if(driveSelected)
				{
					if(j == ObjectSelected)
					{
						Gl.glDisable(Gl.GL_LIGHTING);
						Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[2]);
						Gl.glCallList(SelectedDrive);
						Gl.glEnable(Gl.GL_LIGHTING);
					}
				}
				
				Gl.glPushMatrix();
				Gl.glTranslated(1.3,0,0);
				Gl.glScaled(1.3,1.3,1.3);
				glPrint(drive.GetName(j));
				Gl.glPopMatrix();
				Gl.glPopName();
				Gl.glPopMatrix();
						
				
			}
			Gl.glPopMatrix();
		}
		private static void drawFolders()
		{

			Gl.glPushMatrix();
			Gl.glTranslated(-15,0,zoom-fo.GetRad());
			
			Gl.glRotated(rot,0.0,1.0,0.0);
			Gl.glRotated(angle,1.0,0.0,0.0);

			for(int j=0; j< fo.folderCount; j++)
			{			
				Gl.glPushMatrix();
				Gl.glPushName(j);
				Gl.glTranslated(0, fo.GetYposition(j), fo.GetZposition(j));
				Gl.glRotated(-angle,1.0,0.0,0.0);
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[0]);
				Gl.glCallList(folder);

				if(folderSelected)
				{
					if(j == ObjectSelected)
					{
						Gl.glDisable(Gl.GL_LIGHTING);
						Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[2]);
						Gl.glCallList(selected);
						Gl.glEnable(Gl.GL_LIGHTING);
					}
				}
				
				Gl.glPushMatrix();
				Gl.glTranslated(1.3,0,0);
				glPrint(fo.GetFormattedName(j));
				Gl.glPopMatrix();
				Gl.glPopName();
				Gl.glPopMatrix();
						
				
			}
			Gl.glPopMatrix();
		}

		public void ScanPath()
		{
			scanPath();
		}


		private static void drawFiles()
		{
			Gl.glInitNames();
			Gl.glPushMatrix();
			Gl.glTranslated(0,filePositions,-5);		
			for(int i=folderCount; i<fileCount+fo.folderCount; i++)
			{
				Gl.glPushMatrix();
				Gl.glPushName(i);

				Gl.glTranslated( files.GetXposition(i-fo.folderCount),files.GetYposition(i-fo.folderCount),files.GetZposition(i-fo.folderCount));
				Gl.glRotated(-60,1,0,0);
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[3]);
				
				for(int textureNumber= 0; textureNumber < ft.numberOfTextures; textureNumber++)
				{		
					if (ft.fileTypes[textureNumber].Equals(files.GetFileType(i-fo.folderCount)))
					{					
						Gl.glBindTexture(Gl.GL_TEXTURE_2D, ft.texture[textureNumber]);
					}
				}


					if(i == ObjectSelected)
					{


						Gl.glRotated(60,1,0,0);
						Gl.glCallList(fileObjects);
						Gl.glTranslated(-0.3,-0.45,0);
						Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[0]);
						glPrintFileName(files.GetUnformattedName(i-fo.folderCount));
						Gl.glPopMatrix();
						Gl.glPopName();
					}else{
				

						Gl.glCallList(fileObjects);
						Gl.glTranslated(-0.3,-0.45,0);
						Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures.texture[0]);
						glPrintFileName(files.GetFormattedName(i-fo.folderCount));
						Gl.glPopMatrix();
						Gl.glPopName();
					}
			}
			Gl.glPopMatrix();
		}
			
		private static bool DrawGLScene() 
		{	
			if (select)
			{
    			startPicking();
            }

			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); // Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();
			
				
			if(showDrives)
			{  
				drawDrives();
			}

			if(!showDrives)
			{

				Gl.glPushMatrix();
				Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light0_position);
				Gl.glPushMatrix();
				drawFolders();	
				Gl.glPopMatrix();
				drawFiles();
				Gl.glPopMatrix();
			}

			if (select)
			{
				stopPicking();				
			}

			CheckUserInput();
				
			Gl.glFlush();
			return true;				
		}

		private static bool InitGL() 
		{
			if(!textures.LoadGLTextures()) 
			{                                             // Jump To Texture Loading Routine
				return false;                                                   // If Texture Didn't Load Return False
			}



			BuildFont();
			loader = new ObjectLoader("folder.fm");

			float [] spotlightCutoff = {0.5f};
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0f);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LESS);                                       // The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST); // Really Nice Perspective Calculations
				
			d = new Drive();
			drive = new DrivePositions();
			//driveCount = drive.getDriveCount();
			driveM = new DriveModel();	
			f = new Folders();
			fi = new Files();		

			ft = new FileTextureLoader();
			ft.LoadGLTextures();
			BuildLists();

			showDrives = true;//want to show the drives

			float []lightColor = {1f, 1f, 1f, 0.0f};
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, lightColor);
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light0_position);
			Gl.glEnable(Gl.GL_LIGHT0); 
			
			Gl.glEnable(Gl.GL_LIGHTING);

			return true;
		}

		static void BuildFont() 
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

		static void glPrint(string text) 
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

		static void glPrintFileName(string text) 
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

		static string getPath()
		{
			
			return path;
		}

		private static void KillGLWindow() 
		{


			if(hRC != IntPtr.Zero) 
			{                                            // Do We Have A Rendering Context?
				if(!Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero)) 
				{             // Are We Able To Release The DC and RC Contexts?
					MessageBox.Show("Release Of DC And RC Failed.", "SHUTDOWN ERROR",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				if(!Wgl.wglDeleteContext(hRC)) 
				{                                // Are We Able To Delete The RC?
					MessageBox.Show("Release Rendering Context Failed.", "SHUTDOWN ERROR",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				hRC = IntPtr.Zero;                                              // Set RC To Null
			}

			if(hDC != IntPtr.Zero) 
			{                                            // Do We Have A Device Context?
				if(form != null && !form.IsDisposed) 
				{                          // Do We Have A Window?
					if(form.Handle != IntPtr.Zero) 
					{                            // Do We Have A Window Handle?
						if(!User.ReleaseDC(form.Handle, hDC)) 
						{                 // Are We Able To Release The DC?
							MessageBox.Show("Release Device Context Failed.", "SHUTDOWN ERROR",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}

				hDC = IntPtr.Zero;                                              // Set DC To Null
			}

			if(form != null) 
			{                                                  // Do We Have A Windows Form?
				form.Hide();                                                    // Hide The Window
				form.Close();                                                   // Close The Form
				form = null;                                                    // Set form To Null
			}
		}


		private static void ReSizeGLScene(int width, int height) 
		{
			if(height == 0) 
			{                                                                   // Prevent A Divide By Zero...
				height = 1;                                                     // By Making Height Equal To One
			}
			H = height;
			W = width;
			Gl.glViewport(0, 0, width, height);                                 // Reset The Current Viewport
			Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
			Gl.glLoadIdentity();                                                // Reset The Projection Matrix
			//Glu.gluPerspective(30.0, width / (double) form.Height,0.1,400.0);		// Rest the perspective
			Glu.gluPerspective(fov, width / (double) form.Height, 2,400.0);


			Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
			Gl.glLoadIdentity();
			
		}


		
 
		private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			keys[e.KeyValue] = true;     // Key Has Been Pressed, Mark It As true
				

		}

		private void Form_KeyUp(object sender, KeyEventArgs e) 
		{
			keys[e.KeyValue] = false;                                           // Key Has Been Released, Mark It As false
		}
		private void Form_Activated(object sender, EventArgs e) 
		{
			active = true;                                                      // Program Is Active
		}

		private void Form_Closing(object sender, CancelEventArgs e) 
		{
			done = true;                                                        // Send A Quit Message
		}

		private void Form_Deactivate(object sender, EventArgs e) 
		{
			active = false;                                                     // Program Is No Longer Active
		}

		private void Form_Resize(object sender, EventArgs e) 
		{
			ReSizeGLScene(form.Width, form.Height);                             // Resize The OpenGL Window
		}



		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FileManager));
			address = new System.Windows.Forms.Label();
			this.leftPanel = new System.Windows.Forms.Panel();
			this.zoomInButton = new System.Windows.Forms.Button();
			this.upArrow = new System.Windows.Forms.Button();
			this.downArrow = new System.Windows.Forms.Button();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.zoomOutButton = new System.Windows.Forms.Button();
			this.filesUpButton = new System.Windows.Forms.Button();
			this.filesDownButton = new System.Windows.Forms.Button();
			this.BackButton = new System.Windows.Forms.Button();
			this.leftPanel.SuspendLayout();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// address
			// 
			address.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			address.Location = new System.Drawing.Point(144, 0);
			address.Name = "address";
			address.Size = new System.Drawing.Size(704, 24);
			address.TabIndex = 0;
			address.Text = "Drive View";
			address.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// leftPanel
			// 
			this.leftPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.leftPanel.Controls.Add(this.upArrow);
			this.leftPanel.Controls.Add(this.downArrow);
			this.leftPanel.Controls.Add(this.zoomInButton);
			this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftPanel.Location = new System.Drawing.Point(0, 0);
			this.leftPanel.Name = "leftPanel";
			this.leftPanel.Size = new System.Drawing.Size(32, 685);
			this.leftPanel.TabIndex = 0;
			this.leftPanel.Click += new System.EventHandler(this.leftPanel_Click);
			// 
			// zoomInButton
			// 
			this.zoomInButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomInButton.Image")));
			this.zoomInButton.Location = new System.Drawing.Point(0, 640);
			this.zoomInButton.Name = "zoomInButton";
			this.zoomInButton.Size = new System.Drawing.Size(28, 28);
			this.zoomInButton.TabIndex = 0;
			this.zoomInButton.TabStop = false;
			this.zoomInButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.zoomInButton_MouseUp);
			this.zoomInButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zoomInButton_MouseDown);
			// 
			// upArrow
			// 
			this.upArrow.Image = ((System.Drawing.Image)(resources.GetObject("upArrow.Image")));
			this.upArrow.Location = new System.Drawing.Point(0, 344);
			this.upArrow.Name = "upArrow";
			this.upArrow.Size = new System.Drawing.Size(28, 28);
			this.upArrow.TabIndex = 0;
			this.upArrow.TabStop = false;
			this.upArrow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.upArrow_MouseUp);
			this.upArrow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.upArrow_MouseDown);
			// 
			// downArrow
			// 
			this.downArrow.Image = ((System.Drawing.Image)(resources.GetObject("downArrow.Image")));
			this.downArrow.Location = new System.Drawing.Point(0, 400);
			this.downArrow.Name = "downArrow";
			this.downArrow.Size = new System.Drawing.Size(28, 28);
			this.downArrow.TabIndex = 0;
			this.downArrow.TabStop = false;
			this.downArrow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.downArrow_MouseUp);
			this.downArrow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.downArrow_MouseDown);
			// 
			// bottomPanel
			// 
			this.bottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.bottomPanel.Controls.Add(this.filesUpButton);
			this.bottomPanel.Controls.Add(this.filesDownButton);
			this.bottomPanel.Controls.Add(this.BackButton);
			this.bottomPanel.Controls.Add(this.zoomOutButton);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 685);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(1016, 32);
			this.bottomPanel.TabIndex = 0;
			// 
			// zoomOutButton
			// 
			this.zoomOutButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomOutButton.Image")));
			this.zoomOutButton.Location = new System.Drawing.Point(40, 0);
			this.zoomOutButton.Name = "zoomOutButton";
			this.zoomOutButton.Size = new System.Drawing.Size(28, 28);
			this.zoomOutButton.TabIndex = 0;
			this.zoomOutButton.TabStop = false;
			this.zoomOutButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.zoomOutButton_MouseUp);
			this.zoomOutButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zoomOutButton_MouseDown);
			// 
			// filesUpButton
			// 
			this.filesUpButton.Image = ((System.Drawing.Image)(resources.GetObject("filesUpButton.Image")));
			this.filesUpButton.Location = new System.Drawing.Point(680, 0);
			this.filesUpButton.Name = "filesUpButton";
			this.filesUpButton.Size = new System.Drawing.Size(28, 28);
			this.filesUpButton.TabIndex = 0;
			this.filesUpButton.TabStop = false;
			this.filesUpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.filesUpButton_MouseUp);
			this.filesUpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.filesUpButton_MouseDown);
			// 
			// filesDownButton
			// 
			this.filesDownButton.Image = ((System.Drawing.Image)(resources.GetObject("filesDownButton.Image")));
			this.filesDownButton.Location = new System.Drawing.Point(736, 0);
			this.filesDownButton.Name = "filesDownButton";
			this.filesDownButton.Size = new System.Drawing.Size(28, 28);
			this.filesDownButton.TabIndex = 0;
			this.filesDownButton.TabStop = false;
			this.filesDownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.filesDownButton_MouseUp);
			this.filesDownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.filesDownButton_MouseDown);
			// 
			// BackButton
			// 
			this.BackButton.Location = new System.Drawing.Point(112, 0);
			this.BackButton.Name = "BackButton";
			this.BackButton.Size = new System.Drawing.Size(80, 24);
			this.BackButton.TabIndex = 0;
			this.BackButton.TabStop = false;
			this.BackButton.Text = "Back";
			this.BackButton.Click += new System.EventHandler(this.back_Click);
			// 
			// FileManager
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 717);
			this.Controls.Add(this.leftPanel);
			this.Controls.Add(address);
			this.Controls.Add(this.bottomPanel);
			this.KeyPreview = true;
			this.Name = "FileManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FileManager_MouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FileManager_MouseUp);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_KeyUp);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.FileFileManager_MouseWheelMove);
			this.leftPanel.ResumeLayout(false);
			this.bottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}


		private static void startPicking()
		{
			int [] viewport = new int[4];	
			Gl.glSelectBuffer(512,selectBuf);
			Gl.glGetIntegerv (Gl.GL_VIEWPORT, viewport);
			Gl.glRenderMode(Gl.GL_SELECT);		
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glViewport(0, 0, W, H);
			Gl.glPushMatrix();
			Gl.glLoadIdentity();
			Glu.gluPickMatrix(mouseX,(viewport[3]-mouseY-30), 1,  1,viewport); 
			Glu.gluPerspective(fov, form.Width / (double) form.Height, 2,400.0);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glInitNames();
		}

		private static void stopPicking() 
		{
			select = false;
			int hits;
			int choose = -1;
	
			// restoring the original projection matrix
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glPopMatrix();
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glFlush();
	
			// returning to normal rendering mode
			hits = Gl.glRenderMode(Gl.GL_RENDER);
	
			// if there are hits process them
			if (hits > 0)
			{
											
					
				choose = selectBuf[3];					// Make Our Selection The First Object
				int	depth = selectBuf[1];					// Store How Far Away It Is

				for (int loop = 0; loop < hits; loop++)				// Loop Through All The Detected Hits
				{
					// If This Object Is Closer To Us Than The One We Have Selected
					if (selectBuf[loop*4+1] < depth)
					{
						choose = selectBuf[loop*4+3];			// Select The Closer Object
						depth = selectBuf[loop*4+1];			// Store How Far Away It Is
					}       
				}
				ObjectSelected = choose;

				if(showDrives)
				{
					driveSelected = true;
					fileSelected = false;
					folderSelected = false;
				}
				if(!showDrives)
				{
					//what was hit 
					//file
					if(ObjectSelected>=folderCount)
					{
						fileSelected = true;
						folderSelected = false;							
					}
					//folder
					if(ObjectSelected<folderCount)
					{			
						fileSelected = false;
						folderSelected = true;			
					}
				}			
			}
			else
			{
				//nothing selected
				//so dont want to draw selection box
				folderSelected = false;
				fileSelected = false;
				driveSelected = false;
				ObjectSelected = -1;
			}
		}
		
		private void FileFileManager_MouseWheelMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int tempMWheel= mWheel;
			mWheel =  e.Delta+mWheel;

			if(mWheel>tempMWheel)
			{
				angle = angle +4;
				mWheel = 0;
			}
			else
			{
				angle = angle -4;
				mWheel = 0;
			}
			
		
		}

		static bool contextMenu()
		{
			if((RightClick)&&(folderSelected))
			{
				cmenu.Dispose();
				cmenu  = new ContextMenu();
				cmenu.MenuItems.Add(new MenuItem("&Open", new EventHandler (OpenFolder_Clicked)));
				cmenu.MenuItems.Add(new MenuItem("&Delete", new EventHandler (DeleteFolder_Clicked)));
				cmenu.MenuItems.Add(new MenuItem("&Rename", new EventHandler (RenameFolder_Clicked)));
				cmenu.MenuItems.Add(new MenuItem("-"));
				cmenu.MenuItems.Add(new MenuItem("Properties", new EventHandler (FolderProperties_Clicked)));
				form.ContextMenu = cmenu;
			}
			if((RightClick)&&(fileSelected))
			{	
				cmenu.Dispose();
				cmenu  = new ContextMenu();
				cmenu.MenuItems.Add(new MenuItem("&Open", new EventHandler (OpenFile_Clicked)));
				cmenu.MenuItems.Add(new MenuItem("&Delete", new EventHandler (DeleteFile_Clicked)));
				cmenu.MenuItems.Add(new MenuItem("&Rename", new EventHandler (RenameFile_Clicked)));
				cmenu.MenuItems.Add(new MenuItem("-"));
				cmenu.MenuItems.Add(new MenuItem("Properties", new EventHandler (FileProperties_Clicked)));
				form.ContextMenu = cmenu;
			}

			if((!fileSelected)&&(!folderSelected)&&(!showDrives))
			{
				cmenu.Dispose();
				cmenu  = new ContextMenu();
				cmenu.MenuItems.Add(new MenuItem("&New Folder", new EventHandler (NewFolder_Clicked)));
				form.ContextMenu = cmenu;
			}
			if(showDrives)
			{
				cmenu.Dispose();
			}
			return true;
		}

		private void FileManager_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouseX= e.X;
			mouseY= e.Y;
			//cmenu.Dispose();
				
			
	
			if( e.Button.Equals(MouseButtons.Left))
			{
				clickCount = e.Clicks;
				select = true;//start picking
			}

			if( e.Button.Equals(MouseButtons.Right))
			{
				//clickCount = e.Clicks;
				select = true;
				RightClick = true;
				}
		}

		private void FileManager_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//ChangeLabel();
		}


		public void NavigateBack(string inPath)
		{
			string currentPath = inPath;
			char [] separator = {'\\'};
			string[]temp = 	currentPath.Split(separator);
			string newPath = "";
			ArrayList NavigationPaths = new ArrayList();

			for(int i = 0 ; i<temp.Length;i ++)
			{
				if(temp[i].Length>0)
				{
					NavigationPaths.Add(temp[i]+"\\");
				}
			}
		
			if(NavigationPaths.Count>1)
			{
				for(int i = 0; i<NavigationPaths.Count-1;i++)
				{		
					newPath = newPath + NavigationPaths[i];
				}
				if(NavigationPaths.Count>2)
				{
					newPath = newPath.Remove(newPath.Length-1,1);
				}
			
				path = newPath;
				scanPath();
				address.Text= getPath();
			
			}
			else
			{
				setLabel();
				drive = new DrivePositions(); //scan for drives again
				showDrives = true; //show the drives	
			}
			
		}

		static void NewFolder_Clicked(object sender, EventArgs e) 
		{
			AddFolder f = new AddFolder(path);
			f.Show();

		}
		static void OpenFolder_Clicked(object sender, EventArgs e) 
		{
			path = fo.GetFullName(ObjectSelected);//get which one was selected
			scanPath();
		}

		static void DeleteFolder_Clicked(object sender, EventArgs e) 
		{
			FolderControls FoControls = new FolderControls(path);
			if(FoControls.DeleteFolder(fo.GetFullName(ObjectSelected),fo.GetUnformattedName(ObjectSelected)))
			{
				//refresh the folder view
				scanPath();
			}
		}
		static void RenameFolder_Clicked(object sender, EventArgs e) 
		{
			FolderRename fr = new FolderRename(fo.GetFullName(ObjectSelected),fo.GetUnformattedName(ObjectSelected),path);
			fr.Show();
		}

		static void FolderProperties_Clicked(object sender, EventArgs e) 
		{
			FolderControls fc = new FolderControls(fo.GetFullName(ObjectSelected));
			fc.properties();
		}

		static void OpenFile_Clicked(object sender, EventArgs e) 
		{
			MessageBox.Show("File open","file open");
		}
		static void FileProperties_Clicked(object sender, EventArgs e) 
		{
				FileControls fc = new FileControls(path+"\\"+files.GetUnformattedName(ObjectSelected-folderCount));
				Console.WriteLine(path+"\\"+files.GetUnformattedName(ObjectSelected-folderCount));	
				fc.FileProperties();
		}
		static void RenameFile_Clicked(object sender, EventArgs e) 
		{
			FileRename fr = new FileRename(files.GetFormattedName(ObjectSelected-folderCount),files.GetUnformattedName(ObjectSelected-folderCount),path);
			fr.Show();
		}
		static void DeleteFile_Clicked(object sender, EventArgs e) 
		{
			FileControls FiControls = new FileControls(path);
			if(FiControls.DeleteFile(path+"\\"+files.GetUnformattedName(ObjectSelected-folderCount),files.GetUnformattedName(ObjectSelected-folderCount)))
			{
				//refresh the view
				scanPath();
			}
		}

		private void setLabel() 
		{
			address.Text="Drive View";
		}

		private void back_Click(object sender, System.EventArgs e)
		{
			NavigateBack(getPath());
		}

		private void upArrow_Click(object sender, System.EventArgs e)
		{
			angle = angle -0.55; 
		}

		private void leftPanel_Click(object sender, System.EventArgs e)
		{
			form.TopMost = true; 
			form.Focus();
			
		}

		private void upArrow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			rotateUp = true;
		}

		private void upArrow_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			rotateUp = false;
		}

		private void downArrow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			rotateDown = true;
		}

		private void downArrow_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			rotateDown = false;
		}

		private void zoomInButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			zoomIn = true;
		}

		private void zoomInButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			zoomIn = false;
		}

		private void zoomOutButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			zoomOut = true;
		}

		private void zoomOutButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			zoomOut = false;
		}

		private void filesUpButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			filesUp = true;
		}

		private void filesUpButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			filesUp = false;
		}

		private void filesDownButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			filesDown = true;
		}

		private void filesDownButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			filesDown = false;
		}
	}
}


