using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPA_Application
{
    public partial class ViewAssignmentForm : Form
    {
        public ViewAssignmentForm(string subject, string description, string dueDate)
        {
            InitializeComponent();
            label1.Text = "Subject: " + subject;
            label2.Text = "Description: " + description;
            label3.Text = "Due Date: " + dueDate;
        }

        private void ViewAssignmentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
