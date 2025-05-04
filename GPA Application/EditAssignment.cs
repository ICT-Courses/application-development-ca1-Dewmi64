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
    public partial class EditAssignment : Form
    {
        public Assignment_Details UpdatedAssignment { get; private set; }
        public EditAssignment(Assignment_Details original)
        {
            InitializeComponent();

            richTextBox1.Text = original.SubjectName;
            richTextBox2.Text = original.Description;
            dateTimePicker1.Value = original.DueDate;

            // Assign initial data to be used for comparison or reference if needed
            UpdatedAssignment = new Assignment_Details(original.SubjectName, original.Description, original.DueDate);


        }

        private void EditAssignment_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatedAssignment.SubjectName = richTextBox1.Text;
            UpdatedAssignment.Description = richTextBox2.Text;
            UpdatedAssignment.DueDate = dateTimePicker1.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
