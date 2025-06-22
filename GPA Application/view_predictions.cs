using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class view_predictions : Form
    {
        string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";

        
        private List<(PredictionCardControl card, string semester, DateTime timestamp, List<(string subject, string grade)> subjects)> allCards
            = new List<(PredictionCardControl, string, DateTime, List<(string, string)>)>();

        public view_predictions()
        {
            InitializeComponent();

            
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private void view_predictions_Load(object sender, EventArgs e)
        {
            LoadAllPredictionCards();
        }

        private void LoadAllPredictionCards()
        {
            flowLayoutPanel1.Controls.Clear();
            allCards.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string semQuery = "SELECT DISTINCT semester, upload_timestamp FROM predicted_results ORDER BY semester, upload_timestamp DESC";
                MySqlCommand semCmd = new MySqlCommand(semQuery, conn);

                var semesterTimestampList = new List<(string semester, DateTime timestamp)>();

                using (MySqlDataReader semReader = semCmd.ExecuteReader())
                {
                    while (semReader.Read())
                    {
                        string semester = semReader.GetString("semester");
                        DateTime timestamp = semReader.GetDateTime("upload_timestamp");

                        semesterTimestampList.Add((semester, timestamp));
                    }
                }

                foreach (var (semester, timestamp) in semesterTimestampList)
                {
                    double gpa = GetGPA(conn, semester, timestamp);
                    var subjects = GetSubjects(conn, semester, timestamp);

                    PredictionCardControl card = new PredictionCardControl
                    {
                        Semester = $"{semester} ({timestamp:yyyy-MM-dd HH:mm})",
                        GPA = gpa.ToString("0.00")
                    };

                    card.SetSubjects(subjects);

                    // Store for searching/filtering
                    allCards.Add((card, semester, timestamp, subjects));

                    flowLayoutPanel1.Controls.Add(card);
                }
            }
        }

        private double GetGPA(MySqlConnection conn, string semester, DateTime timestamp)
        {
            string gpaQuery = "SELECT gpa FROM predicted_results WHERE semester = @sem AND upload_timestamp = @ts LIMIT 1";
            MySqlCommand cmd = new MySqlCommand(gpaQuery, conn);
            cmd.Parameters.AddWithValue("@sem", semester);
            cmd.Parameters.AddWithValue("@ts", timestamp);

            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToDouble(result) : 0.0;
        }

        private List<(string subject, string grade)> GetSubjects(MySqlConnection conn, string semester, DateTime timestamp)
        {
            var list = new List<(string, string)>();

            string subjectQuery = "SELECT subject, grade FROM predicted_results WHERE semester = @sem AND upload_timestamp = @ts";
            MySqlCommand cmd = new MySqlCommand(subjectQuery, conn);
            cmd.Parameters.AddWithValue("@sem", semester);
            cmd.Parameters.AddWithValue("@ts", timestamp);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string subject = reader.GetString("subject");
                    string grade = reader.GetString("grade");
                    list.Add((subject, grade));
                }
            }
            return list;
        }

        // Search TextChanged event handler
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            flowLayoutPanel1.Controls.Clear();

            foreach (var (card, semester, timestamp, subjects) in allCards)
            {
                bool matchSemester = semester.ToLower().Contains(searchTerm);

                if (string.IsNullOrEmpty(searchTerm) || matchSemester)
                {
                    flowLayoutPanel1.Controls.Add(card);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GPA_Predictor f2 = new GPA_Predictor();
            this.Hide();
            f2.Show();
        }
    }
}



