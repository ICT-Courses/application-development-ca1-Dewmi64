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
        private List<(SemesterCardControl card, string semester, List<(string subject, string grade)>)> allCards =
            new List<(SemesterCardControl, string, List<(string, string)>)>();

        public view_academic_performance()
        {
            InitializeComponent();
        }



        private void view_academic_performance_Load(object sender, EventArgs e)
        {

            label10.Text = UserCredentials.Username;

            flowLayoutPanel1.Controls.Clear();
            allCards.Clear();

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";

            Dictionary<string, List<(string subject, string grade)>> data = new Dictionary<string, List<(string subject, string grade)>>();
            Dictionary<string, (string GPA, string Credits)> gpaInfo = new Dictionary<string, (string GPA, string Credits)>();

            
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT SemesterName, SubjectName, Grade, GPA, TotalNumberOfCredits FROM SemesterResults ORDER BY SemesterName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                con.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string semester = reader["SemesterName"].ToString();
                    string subject = reader["SubjectName"].ToString();
                    string grade = reader["Grade"].ToString();
                    string gpa = reader["GPA"].ToString();
                    string credits = reader["TotalNumberOfCredits"].ToString();

                    if (!data.ContainsKey(semester))
                        data[semester] = new List<(string, string)>();

                    data[semester].Add((subject, grade));
                    gpaInfo[semester] = (gpa, credits);
                }
                con.Close();
            }

            List<string> expectedSemesters = new List<string>
    {
        "1st Year 1st Semester",
        "1st Year 2nd Semester",
        "2nd Year 1st Semester",
        "2nd Year 2nd Semester",
        "3rd Year 1st Semester",
        "3rd Year 2nd Semester",
        "4th Year 1st Semester",
        "4th Year 2nd Semester"
    };

            
            foreach (var sem in expectedSemesters)
            {
                if (data.ContainsKey(sem))
                {
                  
                    var subjectList = data[sem];
                    var (gpa, credits) = gpaInfo[sem];

                    SemesterCardControl card = new SemesterCardControl
                    {
                        SemesterName = sem,
                        GPA = gpa,
                     
                    };
                    card.SetSubjects(subjectList);
                    card.OnDeleteClicked += Card_OnDeleteClicked;

                    allCards.Add((card, sem, subjectList));
                    flowLayoutPanel1.Controls.Add(card);
                }
                else
                {
                    // If data is missing, display a warning

                    Label gapLabel = new Label
                    {
                        Text = $"⚠️ No results available for {sem}",
                        AutoSize = true,
                        ForeColor = Color.DarkSlateBlue,
                        Font = new Font("Segoe UI", 10, FontStyle.Italic),
                        Margin = new Padding(10, 5, 10, 10)
                    };
                    flowLayoutPanel1.Controls.Add(gapLabel);
                }
            }

            // Latest overall GPA 
            using (MySqlConnection gpaCon = new MySqlConnection(connectionString))
            {
                string gpaQuery = "SELECT OverallGPA FROM CumulativeSemesterResults ORDER BY SemesterName DESC LIMIT 1";
                MySqlCommand gpaCmd = new MySqlCommand(gpaQuery, gpaCon);
                gpaCon.Open();
                object result = gpaCmd.ExecuteScalar();
                gpaCon.Close();

                if (result != null)
                {
                    double latestOverallGPA = Convert.ToDouble(result);
                    richTextBox1.Text = $"🎓 Your Latest Overall GPA: {latestOverallGPA:0.00}";
                }
                else
                {
                    richTextBox1.Text = "No GPA data found.";
                }
            }
        }



        private void Card_OnDeleteClicked(object sender, string semesterName)
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete all data for '{semesterName}'?",
                                          "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();

                // 1. Delete from SemesterResults
                string deleteSemesterQuery = "DELETE FROM SemesterResults WHERE SemesterName = @semester";
                using (var cmd = new MySqlCommand(deleteSemesterQuery, con))
                {
                    cmd.Parameters.AddWithValue("@semester", semesterName);
                    cmd.ExecuteNonQuery();
                }

                // 2. Delete from CumulativeSemesterResults (to keep it in sync)
                string deleteCumulativeQuery = "DELETE FROM CumulativeSemesterResults WHERE SemesterName = @semester";
                using (var cmd = new MySqlCommand(deleteCumulativeQuery, con))
                {
                    cmd.Parameters.AddWithValue("@semester", semesterName);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            //  Refresh cards and overall GPA
            view_academic_performance_Load(null, null);
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

            Weak_Area_Tracker f2 = new Weak_Area_Tracker();
            this.Hide();
            f2.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Motivation f2 = new Motivation();
            this.Hide();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GPA_calculationcs f2 = new GPA_calculationcs();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gradingscale f2 = new gradingscale();
            this.Hide();
            f2.Show();
        }
    }
}
