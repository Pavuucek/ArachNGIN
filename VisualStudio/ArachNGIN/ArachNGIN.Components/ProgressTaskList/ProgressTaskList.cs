using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ArachNGIN.Components
{
	/// <summary>
	/// Implements a task-list control, derived from Panel.
	/// It displays a vertical list of tasks, with an arrow beside the current task. 
	/// When the task is complete, a green 'tick' icon is placed next to it to indicate that is is finished.
	/// To use this control, you must call Start() to begin, and NextTask() to advance to the next task.
	/// </summary>
	public class ProgressTaskList : System.Windows.Forms.Panel
	{
		private System.ComponentModel.IContainer components;
		private Label[] labels;
		private StringCollection2 tasks;
		private System.Windows.Forms.ImageList imageList1; 
		private int currentTask = 0;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <returns>class instance</returns>
		public ProgressTaskList()
		{
			InitializeComponent();
			tasks = new StringCollection2(this);
		}

		/// <summary>
		/// Adds the label controls to the panel, setting the image to render in the middle left of the label.
		/// </summary>
		public void InitLabels()
		{
			this.Controls.Clear();
			if(tasks != null && tasks.Count > 0)
			{
				// create array of labels
				labels = new Label[tasks.Count];
				int leftIndent = 3;
				int topPos = 3;
				for(int i=0; i<tasks.Count; i++)
				{
					Label l = new Label();
					l.AutoSize = true;
					l.Height = 23;
					l.Location = new Point(leftIndent, topPos);
					l.Text = "      " + tasks[i];		// preceeding spaces to leave room for image
					l.ImageAlign = ContentAlignment.MiddleLeft;
					l.TextAlign = ContentAlignment.MiddleLeft;
					l.ImageList = this.imageList1;
					topPos += 23;
					this.labels[i] = l;
					this.Controls.Add(l);
				}
			}
		}

		/// <summary>
		/// Arraylist of tasks, one per line. 
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public StringCollection2 TaskItems
		{
			get
			{
				return tasks;
			}
			set
			{
				tasks = value;
				this.InitLabels();
			}
		}

		delegate void StartDelegate();
		/// <summary>
		/// Set the icon on the first task to current/busy
		/// This method can be called synchronously or asynchronously
		/// This method can be called multiple times to reset the form.
		/// </summary>
		public void Start()
		{
			if(this.InvokeRequired)
			{
				StartDelegate del = new StartDelegate(this.Start);
				BeginInvoke(del, null);
			}
			else
			{
				currentTask = 0;
				InitLabels();
				if(labels != null && labels.Length > 0)
					this.labels[0].ImageIndex = 0;
			}
		}

		delegate void NextTaskDelegate();
		/// <summary>
		/// Set the icon on the current task to finished
		/// Set the icon on the next task to current/busy
		/// This method can be called synchronously or asynchronously
		/// </summary>
		public void NextTask()
		{
			if(this.InvokeRequired)
			{
				NextTaskDelegate del = new NextTaskDelegate(this.NextTask);
				BeginInvoke(del, null);
			}
			else
			{
				// set icon to finished
				if(currentTask < this.labels.Length)
					this.labels[currentTask].ImageIndex = 1;
				currentTask++;
				// set next task to current/busy
				if(currentTask < labels.Length)
				{
					this.ScrollControlIntoView(this.labels[currentTask]);	// make sure the label is visible. this is necessary in the case where the panel is scrolling vertically. it is nice for the user to see the current task scrolling into view automatically.
					this.labels[currentTask].ImageIndex = 0;
				}
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProgressTaskList));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(10, 10);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ProgressTaskList
			// 
			this.AutoScroll = true;
			this.Size = new System.Drawing.Size(175, 50);

		}
		#endregion
	}


	/// <summary>
	/// A helper collection class to manage items being inserted/removed to the TaskItems property.
	/// Supports modifying the collection through VS.
	/// </summary>
	public class StringCollection2 : CollectionBase
	{
		private ProgressTaskList parent;
		/// <summary>
		/// Constructor requires the  string that owns this collection
		/// </summary>
		/// <param name="parent">ProgressProgressTaskList</param>
		public StringCollection2(ProgressTaskList parent):base()
		{
			this.parent = parent;
		}

		/// <summary>
		/// Finds the string in the collection
		/// </summary>
		public string this[ int index ]  
		{
			get  
			{
				return( (string) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Returns the ProgressProgressTaskList that owns this collection
		/// </summary>
		public ProgressTaskList Parent
		{
			get 
			{
				return this.parent;
			}
		}

		
		/// <summary>
		/// Adds a string into the Collection
		/// </summary>
		/// <param name="value">The string to add</param>
		/// <returns></returns>
		public int Add(string value )  
		{		
			int result = List.Add( value );
			return result;
		}


		/// <summary>
		/// Adds an array of strings into the collection. Used by the Studio Designer generated code
		/// </summary>
		/// <param name="strings">Array of strings to add</param>
		public void AddRange(string[] strings)
		{
			// Use external to validate and add each entry
			foreach(string s in strings)
			{
				this.Add(s);
			}
		}

		/// <summary>
		/// Finds the position of the string in the colleciton
		/// </summary>
		/// <param name="value">string to find position of</param>
		/// <returns>Index of string in collection</returns>
		public int IndexOf( string value )  
		{
			return( List.IndexOf( value ) );
		}

		/// <summary>
		/// Adds a new string at a particular position in the Collection
		/// </summary>
		/// <param name="index">Position</param>
		/// <param name="value">string to be added</param>
		public void Insert( int index, string value )  
		{
			List.Insert(index, value );
		}


		/// <summary>
		/// Removes the given string from the collection
		/// </summary>
		/// <param name="value">string to remove</param>
		public void Remove( string value )  
		{
			//Remove the item
			List.Remove( value );
		}

		/// <summary>
		/// Detects if a given string is in the Collection
		/// </summary>
		/// <param name="value">string to find</param>
		/// <returns></returns>
		public bool Contains( string value )  
		{
			// If value is not of type Int16, this will return false.
			return( List.Contains( value ) );
		}


		protected override void OnInsertComplete(int index, object value)
		{
			base.OnInsertComplete (index, value);
			parent.InitLabels();
		}
	
		/// <summary>
		/// Propogates when external designers remove items from string
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		protected override void OnRemoveComplete(int index, object value)
		{
			base.OnRemoveComplete (index, value);
			parent.InitLabels();
		}
	}
}
