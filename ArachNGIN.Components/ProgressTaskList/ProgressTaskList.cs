using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ArachNGIN.Components.ProgressTaskList
{
	/// <summary>
	/// Implements a task-list control, derived from Panel.
	/// It displays a vertical list of tasks, with an arrow beside the current task. 
	/// When the task is complete, a green 'tick' icon is placed next to it to indicate that is is finished.
	/// To use this control, you must call Start() to begin, and NextTask() to advance to the next task.
	/// </summary>
	public class ProgressTaskList : Panel
	{
		private IContainer components;
		private Label[] _labels;
		private StringCollection2 _tasks;
		private ImageList _imageList1; 
		private int _currentTask;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <returns>class instance</returns>
		public ProgressTaskList()
		{
			InitializeComponent();
			_tasks = new StringCollection2(this);
		}

		/// <summary>
		/// Adds the label controls to the panel, setting the image to render in the middle left of the label.
		/// </summary>
		public void InitLabels()
		{
			Controls.Clear();
			if(_tasks != null && _tasks.Count > 0)
			{
				// create array of labels
				_labels = new Label[_tasks.Count];
				const int leftIndent = 3;
				int topPos = 3;
				for(int i=0; i<_tasks.Count; i++)
				{
					var l = new Label
					            {
					                AutoSize = true,
					                Height = 23,
					                Location = new Point(leftIndent, topPos),
					                Text = "      " + _tasks[i],
					                ImageAlign = ContentAlignment.MiddleLeft,
					                TextAlign = ContentAlignment.MiddleLeft,
					                ImageList = _imageList1
					            };
				    topPos += 23;
					_labels[i] = l;
					Controls.Add(l);
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
				return _tasks;
			}
			set
			{
				_tasks = value;
				InitLabels();
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
			if(InvokeRequired)
			{
				var del = new StartDelegate(Start);
				BeginInvoke(del, null);
			}
			else
			{
				_currentTask = 0;
				InitLabels();
				if(_labels != null && _labels.Length > 0)
					_labels[0].ImageIndex = 0;
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
			if(InvokeRequired)
			{
				var del = new NextTaskDelegate(NextTask);
				BeginInvoke(del, null);
			}
			else
			{
				// set icon to finished
				if(_currentTask < _labels.Length)
					_labels[_currentTask].ImageIndex = 1;
				_currentTask++;
				// set next task to current/busy
				if(_currentTask < _labels.Length)
				{
					ScrollControlIntoView(_labels[_currentTask]);	// make sure the label is visible. this is necessary in the case where the panel is scrolling vertically. it is nice for the user to see the current task scrolling into view automatically.
					_labels[_currentTask].ImageIndex = 0;
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
			this._imageList1 = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList1
			// 
			this._imageList1.ImageSize = new System.Drawing.Size(10, 10);
			this._imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this._imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ProgressTaskList
			// 
			this.AutoScroll = true;
			this.Size = new System.Drawing.Size(175, 50);

		}
		#endregion
	}
}
