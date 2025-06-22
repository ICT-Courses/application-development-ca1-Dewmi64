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
    public partial class Edit_Assignment : Form
    {
        public string UpdatedSubject => textBox1.Text;
        public string UpdatedDescription => textBox2.Text;
        public DateTime UpdatedDueDate => dateTimePicker1.Value;

        public Edit_Assignment(string subject, string description, DateTime dueDate)
        {
            InitializeComponent();

            // Set initial values
            textBox1.Text = subject;
            textBox2.Text = description;
            dateTimePicker1.Value = dueDate;
        }

        private void Edit_Assignment_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e) // Cancel button
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) // Save button
        {
            // Simple validation
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            
        }
    }
}
