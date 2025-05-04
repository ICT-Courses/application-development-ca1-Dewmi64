using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace GPA_Application
{
    public partial class view_academic_performance : Form
    {
        private FlowLayoutPanel panelContainer;
        public view_academic_performance()
        {
            InitializeComponent();
            InitializePanel();
        }

        private void InitializePanel()
        {
            panelContainer = new FlowLayoutPanel();
            panelContainer.Location = new Point(10, 50);
            panelContainer.Size = new Size(980, 654); // Adjust width to fit two cards
            panelContainer.FlowDirection = FlowDirection.LeftToRight; // Horizontal layout
            panelContainer.WrapContents = true; // Enable wrapping to next row
            panelContainer.AutoScroll = true; // Allow vertical scrolling
            panelContainer.Padding = new Padding(10);
            panelContainer.BackColor = Color.SkyBlue;
            this.Controls.Add(panelContainer);
        }



        private void view_academic_performance_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                // Corrected SQL Query:  The issue was the unnecessary line break and whitespace before the FROM clause.
                string query = "SELECT SemesterName, SubjectName, Grade FROM SemesterResults ORDER BY SemesterName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, HashSet<(string SubjectName, string Grade)>> semesterData =
                    new Dictionary<string, HashSet<(string, string)>>();
                while (reader.Read())
                {
                    string semesterName = reader["SemesterName"].ToString();
                    string subjectName = reader["SubjectName"].ToString();
                    string grade = reader["Grade"].ToString();
                    if (!semesterData.ContainsKey(semesterName))
                    {
                        semesterData[semesterName] = new HashSet<(string, string)>();
                    }
                    semesterData[semesterName].Add((subjectName, grade));
                }
                con.Close();
                foreach (var semester in semesterData)
                {
                    Panel cardPanel = CreateSemesterCard(semester.Key, semester.Value.ToList());
                    panelContainer.Controls.Add(cardPanel);
                }
            }
        }

        private Panel CreateSemesterCard(string semesterName, List<(string SubjectName, string Grade)> subjects)
        {
            Panel cardPanel = new Panel();
            cardPanel.Width = 350;
            cardPanel.AutoSize = true;
            cardPanel.BackColor = Color.WhiteSmoke;
            cardPanel.BorderStyle = BorderStyle.FixedSingle;
            cardPanel.Margin = new Padding(10);
            Label semesterTitleLabel = new Label();
            semesterTitleLabel.Text = semesterName;
            semesterTitleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            semesterTitleLabel.AutoSize = true;
            semesterTitleLabel.Location = new Point(10, 10);
            cardPanel.Controls.Add(semesterTitleLabel);
            string gpa = "", totalCredits = "";
            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT GPA, TotalNumberOfCredits FROM SemesterResults WHERE SemesterName = @semester LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@semester", semesterName);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    gpa = reader["GPA"].ToString();
                    totalCredits = reader["TotalNumberOfCredits"].ToString();
                }
            }
            int yOffset = 40;
            Label gpaLabel = new Label();
            gpaLabel.Text = "GPA: " + gpa;
            gpaLabel.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            gpaLabel.AutoSize = true;
            gpaLabel.Location = new Point(10, yOffset);
            cardPanel.Controls.Add(gpaLabel);
            yOffset += 25;
            Label creditLabel = new Label();
            creditLabel.Text = "Total Credits: " + totalCredits;
            creditLabel.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            creditLabel.AutoSize = true;
            creditLabel.Location = new Point(10, yOffset);
            cardPanel.Controls.Add(creditLabel);
            yOffset += 30;
            foreach (var subject in subjects)
            {
                Label subjectLabel = new Label();
                subjectLabel.Text = subject.SubjectName;
                subjectLabel.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                subjectLabel.AutoSize = true;
                subjectLabel.Location = new Point(15, yOffset);
                cardPanel.Controls.Add(subjectLabel);
                Label gradeLabel = new Label();
                gradeLabel.Text = subject.Grade;
                gradeLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                gradeLabel.AutoSize = true;
                gradeLabel.Location = new Point(250, yOffset);
                cardPanel.Controls.Add(gradeLabel);
                yOffset += 30;
            }
            // Add delete label at the bottom of the card
            Label deleteLabel = new Label();
            deleteLabel.Text = "  🗑";
            deleteLabel.ForeColor = Color.Red;
            deleteLabel.AutoSize = true;
            deleteLabel.Cursor = Cursors.Hand;
            deleteLabel.Location = new Point(cardPanel.Width - 80, yOffset);
            deleteLabel.Tag = semesterName; // use this to identify which semester to delete
            deleteLabel.Click += DeleteLabel_Click;
            cardPanel.Controls.Add(deleteLabel);
            return cardPanel;
        }

        private void DeleteLabel_Click(object sender, EventArgs e)
        {
            Label deleteLabel = sender as Label;
            string semesterToDelete = deleteLabel.Tag.ToString();
            DialogResult result = MessageBox.Show(
              $"Are you sure you want to delete all records for '{semesterToDelete}'?",
              "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    string deleteQuery = "DELETE FROM SemesterResults WHERE SemesterName = @semester";
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, con);
                    cmd.Parameters.AddWithValue("@semester", semesterToDelete);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                // Refresh UI
                panelContainer.Controls.Clear();
                view_academic_performance_Load(null, null);
            }
        }
    }
}