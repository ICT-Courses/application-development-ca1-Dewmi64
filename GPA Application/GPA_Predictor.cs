using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class GPA_Predictor : Form
    {
        public GPA_Predictor()
        {
            InitializeComponent();
        }

        
        
             private DataTable excelTable;

        private void button1_Click(object sender, EventArgs e) // Excel file upload function
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Files|*.xls;*.xlsx" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Display file name in textBox1
                    textBox1.Text = Path.GetFileName(ofd.FileName); // Only file name (no path)
                                                                    // If you want full path: textBox1.Text = ofd.FileName;

                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var conf = new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                            };

                            var dataSet = reader.AsDataSet(conf);
                            excelTable = dataSet.Tables[0];
                        }
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e) //prediction button
        {
            string selectedSemester = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            if (excelTable == null || excelTable.Rows.Count == 0)
            {
                MessageBox.Show("Please upload an Excel file first.");
                return;
            }

            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
            double latestOverallGPA = 0;
            double totalCompletedCredits = 0;

            

            // Step 1: Get latest overall GPA and total completed credits
            using (MySqlConnection gpaCon = new MySqlConnection(connectionString))
            {
                string gpaQuery = "SELECT OverallGPA, TotalCreditsUpToThisSemester FROM CumulativeSemesterResults ORDER BY SemesterName DESC LIMIT 1";
                MySqlCommand gpaCmd = new MySqlCommand(gpaQuery, gpaCon);
                gpaCon.Open();
                using (var reader = gpaCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        latestOverallGPA = Convert.ToDouble(reader["OverallGPA"]);
                        totalCompletedCredits = Convert.ToDouble(reader["TotalCreditsUpToThisSemester"]);
                    }
                }
                gpaCon.Close();
            }


            // Step 2: Calculate predicted GPA for selected semester
            double predictedGradePoints = 0;
            double predictedCredits = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                foreach (DataRow row in excelTable.Rows)
                {
                    string grade = row["Grade"].ToString();
                    double credits = Convert.ToDouble(row["No of Credits"]);

                    string gpvQuery = "SELECT GPV FROM Grading_scale WHERE Grade = @grade";
                    MySqlCommand gpvCmd = new MySqlCommand(gpvQuery, con);
                    gpvCmd.Parameters.AddWithValue("@grade", grade);

                    object result = gpvCmd.ExecuteScalar();
                    if (result != null)
                    {
                        double gpv = Convert.ToDouble(result);
                        predictedGradePoints += gpv * credits;
                        predictedCredits += credits;
                    }
                }
                con.Close();
            }

            double predictedSemesterGPA = predictedCredits > 0 ? predictedGradePoints / predictedCredits : 0;

            // Step 3: Calculate estimated new overall GPA
            double newTotalGradePoints = (latestOverallGPA * totalCompletedCredits) + predictedGradePoints;
            double newTotalCredits = totalCompletedCredits + predictedCredits;
            double estimatedNewOverallGPA = newTotalCredits > 0 ? newTotalGradePoints / newTotalCredits : 0;

            // Step 4: Display result in richTextBox2
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Current Overall GPA: {latestOverallGPA:0.00}");
            sb.AppendLine($"Predicted GPA for {selectedSemester}: {predictedSemesterGPA:0.00}");
            sb.AppendLine($"If you achieving your prediction your Overall GPA: {estimatedNewOverallGPA:0.00}");

            if (estimatedNewOverallGPA > latestOverallGPA)
                sb.AppendLine("Your prediction may improve your GPA. Work hard to your goal 💪");
            else if (estimatedNewOverallGPA < latestOverallGPA)
                sb.AppendLine("Your prediction may reduce your GPA. Try to focus more and push harder.");
            else
                sb.AppendLine("Your GPA will likely remain the same. Consistency matters!");

            richTextBox1.Text = sb.ToString();

            

        }

        private void GPA_Predictor_Load(object sender, EventArgs e)
        {

            ButtonCornerStyler.ApplyRoundedCorners(button2, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button3, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button4, 30);
            

            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            

            label10.Text = UserCredentials.Username;
        }

        private void button4_Click(object sender, EventArgs e) //view predictions button
        {
            view_predictions f2 = new view_predictions();
            this.Hide();    
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e) //save button
        {
            string selectedSemester = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedSemester))
            {
                MessageBox.Show("Please select a semester.");
                return;
            }

            if (excelTable == null || excelTable.Rows.Count == 0)
            {
                MessageBox.Show("Please upload an Excel file first.");
                return;
            }

            double totalGradePoints = 0;
            double totalCredits = 0;
            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                foreach (DataRow row in excelTable.Rows)
                {
                    string subjectName = row["Subject"].ToString();
                    string grade = row["Grade"].ToString();
                    double credits = Convert.ToDouble(row["No of Credits"]);

                    string gpvQuery = "SELECT GPV FROM Grading_scale WHERE Grade = @grade";
                    MySqlCommand gpvCmd = new MySqlCommand(gpvQuery, con);
                    gpvCmd.Parameters.AddWithValue("@grade", grade);

                    object result = gpvCmd.ExecuteScalar();
                    if (result != null)
                    {
                        double gpv = Convert.ToDouble(result);
                        double earnedGradePoints = gpv * credits;

                        totalGradePoints += earnedGradePoints;
                        totalCredits += credits;

                        // Insert subject details into PredictedResults
                        string insertQuery = "INSERT INTO predicted_results (semester, subject, grade) VALUES (@sem, @sub, @grade)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, con);
                        insertCmd.Parameters.AddWithValue("@sem", selectedSemester);
                        insertCmd.Parameters.AddWithValue("@sub", subjectName);
                        insertCmd.Parameters.AddWithValue("@grade", grade);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                // Calculate GPA
                double predictedGPA = totalCredits > 0 ? totalGradePoints / totalCredits : 0;

                using (MySqlConnection con2 = new MySqlConnection(connectionString))
                {
                    con2.Open();
                    string insertOverallQuery = @"
        INSERT INTO predicted_results (semester, subject, grade, gpa, is_overall_prediction)
        VALUES (@sem, 'Overall Prediction', '-', @gpa, TRUE)";

                    MySqlCommand insertOverallCmd = new MySqlCommand(insertOverallQuery, con2);
                    insertOverallCmd.Parameters.AddWithValue("@sem", selectedSemester);
                    insertOverallCmd.Parameters.AddWithValue("@gpa", predictedGPA);
                    insertOverallCmd.ExecuteNonQuery();
                    con2.Close();
                }
            }

            MessageBox.Show("Predicted GPA and subject data saved successfully.");
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            view_predictions f2 = new view_predictions();
            this.Hide();
            f2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Weak_Area_Tracker f2 = new Weak_Area_Tracker();
            this.Hide();
            f2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Motivation f2 = new Motivation();
            this.Hide();
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            GPA_Predictor_Home f2 = new GPA_Predictor_Home();
            this.Hide();
            f2.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}
