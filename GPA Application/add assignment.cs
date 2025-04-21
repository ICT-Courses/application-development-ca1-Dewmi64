using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace GPA_Application
{
    public partial class add_assignment : Form
    {
       
        string connectionString = "Server=localhost;Database=assignment_storage;User ID=root;Password=Hwcs@1969;";

        public add_assignment()
        {
            InitializeComponent();
        }

        private void add_assignment_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if the required fields are filled
            if (!string.IsNullOrEmpty(richTextBox1.Text) && !string.IsNullOrEmpty(richTextBox2.Text))
            {
                string subjectName = richTextBox1.Text.Trim();
                string description = richTextBox2.Text.Trim();
                DateTime dueDate = dateTimePicker1.Value;

                // Create the SQL query to insert the data
                string query = "INSERT INTO Assignment_Storage (subject_name, description, due_date) VALUES (@subjectName, @description, @dueDate)";

                // Use ADO.NET to connect to the database and execute the insert
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@subjectName", subjectName);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@dueDate", dueDate);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Assignment saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the form after saving
                        richTextBox1.Clear();
                        richTextBox2.Clear();
                        dateTimePicker1.Value = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Assignment_managing f2 = new Assignment_managing();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpcomingAssignments f2 = new UpcomingAssignments();
            f2.Show();
        }
    }
}

