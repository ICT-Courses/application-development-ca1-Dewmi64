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
    public partial class gradingscale : Form
    {
        string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
        public gradingscale()
        {
            InitializeComponent();
        }

        private void Gradingscale_Load(object sender, EventArgs e)
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

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["Grade"].ToString(),
                        reader["Marks_level"].ToString(),
                        reader["GPV"].ToString()
                    );
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
        private void gradingscale_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Grade";
            dataGridView1.Columns[1].Name = "Marks_level";
            dataGridView1.Columns[2].Name = "GPV";

            // Predefined grading scale
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
                dataGridView1.Rows.Add(
                    gradingScale[i, 0],
                    gradingScale[i, 1],
                    gradingScale[i, 2]
                );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string Grade = row.Cells[0].Value?.ToString();
                        int Marks_level = int.Parse(row.Cells[1].Value.ToString());
                        double GPV = double.Parse(row.Cells[2].Value.ToString());

                        // First check if this grade already exists
                        string checkQuery = "SELECT COUNT(*) FROM Grading_scale WHERE Grade = @grade";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@grade", Grade);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // UPDATE existing
                            string updateQuery = "UPDATE grading_scale SET Marks_level = @Marks_level, GPV = @gpv WHERE Grade = @grade";
                            MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                            updateCmd.Parameters.AddWithValue("@grade", Grade);
                            updateCmd.Parameters.AddWithValue("@Marks_level", Marks_level);
                            updateCmd.Parameters.AddWithValue("@GPV", GPV);
                            updateCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // INSERT new
                            string insertQuery = "INSERT INTO Grading_scale (Grade, Marks_level, GPV) VALUES (@grade, @Marks_level, @GPV)";
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

        