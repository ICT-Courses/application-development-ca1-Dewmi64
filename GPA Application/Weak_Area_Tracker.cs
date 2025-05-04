using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class Weak_Area_Tracker : Form
    {
        public Weak_Area_Tracker()
        {
            InitializeComponent();
        }

        private void Weak_Area_Tracker_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedSemester = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            List<string> weakSubjects = new List<string>();
            int totalSubjects = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = @"SELECT SubjectName, Grade, TotalNumberOfCredits, GPA
                               FROM SemesterResults
                               WHERE SemesterName = @semester";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@semester", selectedSemester);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalSubjects++;
                        string subject = reader.GetString("SubjectName");
                        string grade = reader.GetString("Grade");
                        double gpv = Convert.ToDouble(reader["GPA"]);

                        // Combined condition to identify weak areas
                        if (gpv < 2.0 || grade == "C+" || grade == "C" || grade == "C-" || grade == "D" || grade == "E")
                        {
                            weakSubjects.Add($"{subject} (Grade: {grade})");
                        }
                    }
                }
            }

            listBox1.Items.Clear();

            if (totalSubjects == 0)
            {
                richTextBox1.Text = "📌 No data available for the selected semester.\n\nPlease try again after entering subject data. 📂";
            }
            else if (weakSubjects.Count == 0)
            {
                richTextBox1.Text = "You have no weak subjects! 👍\n\nKeep up the great work and continue studying consistently! 🧠📚";
            }
            else
            {
                foreach (string subject in weakSubjects)
                {
                    listBox1.Items.Add(subject);
                }

                richTextBox1.Text = "Focus more on the following subjects:\n\n" +
                                    string.Join("\n", weakSubjects) +
                                    "\n\n✅ Dedicate 3–4 hours per week to review!\n📌 Take notes, do revisions, and stay consistent.";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear previous results
            listBox1.Items.Clear();
            richTextBox1.Clear();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.FromArgb (100,0,0,0);
        }
    }
}

