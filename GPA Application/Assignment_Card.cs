using System;
using System.Drawing;
using System.Windows.Forms;

namespace GPA_Application
{
    public partial class Assignment_Card : UserControl
    {
        public Assignment_Card()
        {
            InitializeComponent();
            this.Load += Assignment_Card_Load;

            button1.Click += button1_Click;  // e.g. Mark Done
            button2.Click += button2_Click;  // View
            button3.Click += button3_Click;  // Edit
            button4.Click += button4_Click;  // Delete
        }

        public string SubjectName
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        public string Description
        {
            get => label2.Text;
            set => label2.Text = value;
        }

        public string DueDate
        {
            get => label3.Text;
            set => label3.Text = value;
        }


        // Declare events for buttons
        public event EventHandler MarkDoneClicked;
        public event EventHandler ViewClicked;
        public event EventHandler EditClicked;
        public event EventHandler DeleteClicked;

        private void Assignment_Card_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.LightSkyBlue;
            
        }


        public void MarkAsDone()
        {
            this.BackColor = Color.FromArgb(200, 255, 200); // light green
            button1.Text = "Done & Dusted ✅";
            button1.Enabled = false;

            //  Make labels or other components look muted 
            button2.ForeColor = Color.DarkGreen;
            button3.ForeColor = Color.DarkGreen;
            button4.ForeColor = Color.DarkGreen;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MarkDoneClicked?.Invoke(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewClicked?.Invoke(this, EventArgs.Empty);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditClicked?.Invoke(this, EventArgs.Empty);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
