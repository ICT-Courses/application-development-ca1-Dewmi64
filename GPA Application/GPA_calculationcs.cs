using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
using MySql.Data.MySqlClient;



namespace GPA_Application
{
    public partial class GPA_calculationcs : Form
    {
        public GPA_calculationcs()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (excelTable == null || excelTable.Rows.Count == 0)
            {
                MessageBox.Show("No data to save.");
                return;
            }

            string selectedSemester = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            double totalCredits = 0;

            foreach (DataRow row in excelTable.Rows)
            {
                totalCredits += Convert.ToDouble(row["No of Credits"]);
            }

            double semesterGPA = 0;
            if (richTextBox1.Text.Contains(":"))
            {
                string[] parts = richTextBox1.Text.Split(':');
                if (parts.Length > 1)
                {
                    double.TryParse(parts[1], out semesterGPA);
                }
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // Total Earned Grade Points tracker
                double totalEarnedGradePoints = 0;

                foreach (DataRow row in excelTable.Rows)
                {
                    string subjectName = row["Subject"].ToString();
                    string grade = row["Grade"].ToString();
                    double credits = Convert.ToDouble(row["No of Credits"]);

                    string gpvQuery = "SELECT GPV FROM Grading_scale WHERE Grade = @grade";
                    MySqlCommand gpvCmd = new MySqlCommand(gpvQuery, con);
                    gpvCmd.Parameters.AddWithValue("@grade", grade);
                    object gpvResult = gpvCmd.ExecuteScalar();
                    double gpv = gpvResult != null ? Convert.ToDouble(gpvResult) : 0;

                    double earnedGradePoints = gpv * credits;
                    totalEarnedGradePoints += earnedGradePoints;

                    string insertQuery = @"INSERT INTO SemesterResults
                (SemesterName, SubjectName, Grade, TotalNumberOfCredits, GPA, TotalEarnedGradePoints)
                 VALUES
                (@semester, @subject, @grade, @credits, @gpa, @earnedGP)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@semester", selectedSemester);
                        cmd.Parameters.AddWithValue("@subject", subjectName);
                        cmd.Parameters.AddWithValue("@grade", grade);
                        cmd.Parameters.AddWithValue("@credits", credits);
                        cmd.Parameters.AddWithValue("@gpa", semesterGPA);
                        cmd.Parameters.AddWithValue("@earnedGP", earnedGradePoints);
                        cmd.ExecuteNonQuery();
                    }
                }

                // ✅ Insert cumulative summary logic here
                double cumulativeCredits = 0;
                double cumulativeEarnedGP = 0;

                // Step 1: Get sum of all previous semesters
                string sumQuery = @"SELECT 
                                IFNULL(SUM(TotalNumberOfCredits), 0), 
                                IFNULL(SUM(TotalEarnedGradePoints), 0) 
                            FROM SemesterResults 
                            WHERE SemesterName != @semester";

                using (MySqlCommand sumCmd = new MySqlCommand(sumQuery, con))
                {
                    sumCmd.Parameters.AddWithValue("@semester", selectedSemester);
                    using (MySqlDataReader reader = sumCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cumulativeCredits = Convert.ToDouble(reader[0]) + totalCredits;
                            cumulativeEarnedGP = Convert.ToDouble(reader[1]) + totalEarnedGradePoints;
                        }
                    }
                }

                // Calculate Overall GPA
                double overallGPA = cumulativeCredits > 0 ? cumulativeEarnedGP / cumulativeCredits : 0;

                // Insert or Update to cumulative table
                string insertCumulativeQuery = @"INSERT INTO CumulativeSemesterResults 
            (SemesterName, TotalCreditsUpToThisSemester, TotalEarnedGradePointsUpToThisSemester, OverallGPA) 
            VALUES (@semester, @totalCredits, @totalEarnedGP, @overallGPA)
            ON DUPLICATE KEY UPDATE 
                TotalCreditsUpToThisSemester = @totalCredits,
                TotalEarnedGradePointsUpToThisSemester = @totalEarnedGP,
                OverallGPA = @overallGPA";

                using (MySqlCommand cmd = new MySqlCommand(insertCumulativeQuery, con))
                {
                    cmd.Parameters.AddWithValue("@semester", selectedSemester);
                    cmd.Parameters.AddWithValue("@totalCredits", cumulativeCredits);
                    cmd.Parameters.AddWithValue("@totalEarnedGP", cumulativeEarnedGP);
                    cmd.Parameters.AddWithValue("@overallGPA", overallGPA);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            MessageBox.Show("Data saved successfully, including cumulative GPA!");
        
        }
            
        private DataTable excelTable;


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Files|*.xls;*.xlsx" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Show selected file name
                    textBox1.Text = Path.GetFileName(ofd.FileName);
                    labelStatus.Text = "Uploading...";
                    labelStatus.ForeColor = Color.White;

                    try
                    {
                        // Read the selected Excel file
                        using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                            {
                                var conf = new ExcelDataSetConfiguration
                                {
                                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                                    {
                                        UseHeaderRow = true
                                    }
                                };

                                var dataSet = reader.AsDataSet(conf);
                                excelTable = dataSet.Tables[0];

                                // Success message
                                labelStatus.Text = "✅ File uploaded successfully!";
                                labelStatus.ForeColor = Color.LightGreen;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        labelStatus.Text = "❌ Error: " + ex.Message;
                        labelStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    textBox1.Text = "No file selected";
                    labelStatus.Text = "Upload cancelled.";
                    labelStatus.ForeColor = Color.Gray;
                }
            }
        }





        private void GPA_calculationcs_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            

            ButtonCornerStyler.ApplyRoundedCorners(button2, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button3, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button4, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button6, 30);

            label10.Text = UserCredentials.Username;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedSemester = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            double totalGradePoints = 0;  // Total grade points for current semester
            double totalCredits = 0;       // Total credits for current semester

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                foreach (DataRow row in excelTable.Rows)
                {
                    string grade = row["Grade"].ToString();
                    double credits = Convert.ToDouble(row["No of Credits"]);

                    string query = "SELECT GPV FROM Grading_scale WHERE Grade = @grade";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@grade", grade);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        double gpv = Convert.ToDouble(result);
                        double earnedGradePoints = gpv * credits;

                        totalGradePoints += earnedGradePoints;
                        totalCredits += credits;
                    }
                }

                con.Close();
            }

            // Calculate GPA for the selected semester
            double semesterGPA = totalCredits > 0 ? totalGradePoints / totalCredits : 0;
            richTextBox1.Text = "  Your GPA IS: " + semesterGPA.ToString("0.00");


        }

        private void button4_Click(object sender, EventArgs e)
      
        {
            string selectedSemester = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            double overallGPA = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = @"SELECT OverallGPA 
                         FROM CumulativeSemesterResults 
                         WHERE SemesterName = @semester";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@semester", selectedSemester);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        overallGPA = Convert.ToDouble(result);
                        richTextBox2.Text = "  Your Overall GPA IS: " + overallGPA.ToString("0.00");
                    }
                    else
                    {
                        richTextBox2.Text = "  No GPA data found for the selected semester.";
                    }
                }

                con.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            view_academic_performance f2 = new view_academic_performance();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            GPA_Cal_Home f2 = new GPA_Cal_Home();
            this.Hide();
            f2.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            GPA_Predictor f2 = new GPA_Predictor();
            this.Hide();
            f2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            view_predictions f2 = new view_predictions();
            this.Hide();
            f2.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


