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
    public partial class gradingscale_for_predictor : Form
    {
        string connectionString = "server=localhost;user=root;database=gradingscale_for_predictor;password=Hwcs@1969;";

        public gradingscale_for_predictor()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gradingscale_for_predictor_Load(object sender, EventArgs e)
        {
            LoadGrading_Scale();
        }

        private void LoadGrading_Scale()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("Grade", "Grade");
            dataGridView1.Columns.Add("Marks_level", "Marks_level");
            dataGridView1.Columns.Add("GPV", "GPV");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Grade, Marks_level, GPV FROM Grading_scale ORDER BY Marks_level DESC";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                            reader["Grade"].ToString(),
                            reader["Marks_level"].ToString(),
                            reader["GPV"].ToString()
                        );
                    }
                }
                else
                {
                    reader.Close(); // Important: Close reader before insert

                    // Default grading scale
                    string[,] gradingScale = new string[,]
                    {
                        { "A+", "85", "4.0" },
                        { "A", "75", "4.0" },
                        { "A-", "70", "3.7" },
                        { "B+", "65", "3.3" },
                        { "B", "60", "3.0" },
                        { "C+", "55", "2.7" },
                        { "C", "50", "2.3" },
                        { "D", "40", "1.0" },
                        { "F", "0", "0.0" }
                    };

                    for (int i = 0; i < gradingScale.GetLength(0); i++)
                    {
                        string grade = gradingScale[i, 0];
                        int marks = int.Parse(gradingScale[i, 1]);
                        double gpv = double.Parse(gradingScale[i, 2]);

                        // Add to DataGridView
                        dataGridView1.Rows.Add(grade, marks, gpv);

                        // Insert into DB
                        string insertQuery = "INSERT INTO Grading_scale (Grade, Marks_level, GPV) VALUES (@grade, @marks, @gpv)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@grade", grade);
                        insertCmd.Parameters.AddWithValue("@marks", marks);
                        insertCmd.Parameters.AddWithValue("@gpv", gpv);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    string Grade = row.Cells[0].Value?.ToString();
                    int Marks_level = int.Parse(row.Cells[1].Value.ToString());
                    double GPV = double.Parse(row.Cells[2].Value.ToString());

                    // Check if grade exists
                    string checkQuery = "SELECT COUNT(*) FROM Grading_Scale WHERE Grade = @grade";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@grade", Grade);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // UPDATE existing record
                        string updateQuery = "UPDATE Grading_Scale SET Marks_level = @Marks_level, GPV = @gpv WHERE Grade = @grade";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@grade", Grade);
                        updateCmd.Parameters.AddWithValue("@Marks_level", Marks_level);
                        updateCmd.Parameters.AddWithValue("@gpv", GPV);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // INSERT new
                        string insertQuery = "INSERT INTO Grading_Scale (Grade, Marks_level, GPV) VALUES (@grade, @Marks_level, @GPV)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@grade", Grade);
                        insertCmd.Parameters.AddWithValue("@Marks_level", Marks_level);
                        insertCmd.Parameters.AddWithValue("@GPV", GPV);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Grading scale updated successfully!");
            }
        }
    }
}

