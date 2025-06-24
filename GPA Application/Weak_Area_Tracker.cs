using System;
using System.Collections.Generic;
using System.Drawing;
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
            label10.Text = UserCredentials.Username;

            ButtonCornerStyler.ApplyRoundedCorners(button1, 30);
            

            // Hide UI elements initially
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Visible = false;

            lblStatus.Visible = false;

            // Optionally load semesters into comboBox1 here
            // comboBox1.Items.Add("Semester 1");
            // comboBox1.Items.Add("Semester 2");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedSemester = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            // Show progress bar and analyzing message
            progressBar1.Visible = true;
            flowLayoutPanel1.Visible = false;
            lblStatus.Text = "🔎 Analyzing, please wait...";
            lblStatus.Visible = true;
            button1.Enabled = false;

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            List<(string subject, string grade, string tip)> weakSubjects = new List<(string, string, string)>();
            int totalSubjects = 0;

            Dictionary<string, string> gradeTips = new Dictionary<string, string>()
    {
        { "C+", "Try to review your notes regularly and clarify doubts." },
        { "C", "Focus on fundamentals and practice more problems." },
        { "C-", "Increase study hours and ask for help if needed." },
        { "D", "Attend extra tutorials and revise key concepts often." },
        { "E", "Consider consulting your teacher and dedicate more time to study." }
    };

            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = @"SELECT SubjectName, Grade, GPA
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

                                if (gpv < 2.0 || grade == "C+" || grade == "C" || grade == "C-" || grade == "D" || grade == "E")
                                {
                                    string tip = gradeTips.ContainsKey(grade) ? gradeTips[grade] : "Keep practicing regularly!";
                                    weakSubjects.Add((subject, grade, tip));
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                lblStatus.Visible = false;
                progressBar1.Visible = false;
                button1.Enabled = true;
                MessageBox.Show("Error loading data: " + ex.Message);
                return;
            }

            // Clear previous cards
            flowLayoutPanel1.Controls.Clear();

            if (totalSubjects == 0)
            {
                lblStatus.Visible = false;
                progressBar1.Visible = false;
                button1.Enabled = true;

                MessageBox.Show("📌 No data available for the selected semester.\nPlease enter subject data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (weakSubjects.Count == 0)
            {
                WeakSubjectCard noWeakCard = new WeakSubjectCard();
                noWeakCard.SetData("🎉 No Weak Subjects!", " Excellent Performance!", "You're doing great! Keep it up!");
                Color.FromArgb(255, 230, 255, 230);
                flowLayoutPanel1.Controls.Add(noWeakCard);
                flowLayoutPanel1.Visible = true;

                lblStatus.Visible = false;
                progressBar1.Visible = false;
                button1.Enabled = true;
                return;
            }

            // Add cards and show results
            foreach (var ws in weakSubjects)
            {
                WeakSubjectCard card = new WeakSubjectCard();
                card.SetData(ws.subject, ws.grade, ws.tip);
                flowLayoutPanel1.Controls.Add(card);
            }

            lblStatus.Visible = false;
            progressBar1.Visible = false;
            flowLayoutPanel1.Visible = true;
            button1.Enabled = true;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Visible = false;
            lblStatus.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Motivation f2 = new Motivation();
            this.Hide();
            f2.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
