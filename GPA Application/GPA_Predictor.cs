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

        private void button1_Click(object sender, EventArgs e)
        {
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Files|*.xls;*.xlsx" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
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

                        // Accumulate total grade points and total credits for current semester
                        totalGradePoints += earnedGradePoints;
                        totalCredits += credits;

                        
                    }
                }

                con.Close();
            }

            // Calculate GPA for the selected semester
            double semesterGPA = totalCredits > 0 ? totalGradePoints / totalCredits : 0;
            richTextBox1.Text = "  Your Predicted GPA iS: " + semesterGPA.ToString("0.00");


        }

        private void GPA_Predictor_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            view_academic_performance f2 = new view_academic_performance();
            this.Hide();    
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
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

                // Update PredictedGPA for the semester
                string updateGPAQuery = "UPDATE predicted_results SET gpa = @gpa WHERE semester = @sem";
                MySqlCommand updateCmd = new MySqlCommand(updateGPAQuery, con);
                updateCmd.Parameters.AddWithValue("@gpa", predictedGPA);
                updateCmd.Parameters.AddWithValue("@sem", selectedSemester);
                updateCmd.ExecuteNonQuery();

                con.Close();
            }

            MessageBox.Show("Predicted GPA and subject data saved successfully.");
        }

    }

}
