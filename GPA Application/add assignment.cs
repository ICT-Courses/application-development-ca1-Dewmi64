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
    public partial class add_assignment : Form
    {
        private Assignment_Storage Assignment_Storage;
        public add_assignment()
        {
            InitializeComponent();
            Assignment_Storage = new Assignment_Storage();
        }

        private void add_assignment_Load(object sender, EventArgs e)
        {
            string subjectName = richTextBox1.Text.Trim();
            string description = richTextBox2.Text.Trim();
            DateTime dueDate = dateTimePicker1.Value;


            Assignment_Details newAssignment = new Assignment_Details(subjectName, description, dueDate);
            Assignment_Storage.AddAssignment(newAssignment); // Add assignment to the list

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            
            if (!string.IsNullOrEmpty(richTextBox1.Text) &&

                !string.IsNullOrEmpty(richTextBox2.Text)) 
            {
                
                MessageBox.Show("Assignment saved Successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

                richTextBox1.Clear();
                richTextBox2.Clear();
                dateTimePicker1.Value = DateTime.Now;
            }
            else
            {
                
                MessageBox.Show("Please fill in all fields","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            f2.Show();
        }
    }
}
