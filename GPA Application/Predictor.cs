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
    public partial class Predictor : Form
    {
        public Predictor()
        {
            InitializeComponent();
        }


        private DataTable excelTable;
        private void button1_Click(object sender, EventArgs e)
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
                            dataGridView1.DataSource = excelTable;
                        }
                    }
                }
            }
        }

        private void Predictor_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            double totalGradePoints = 0;
            double totalCredits = 0;

            string connectionString = "server=localhost;user=root;database=gradingscale_for_predictor;password=Hwcs@1969;";

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
                        totalGradePoints += gpv * credits;
                        totalCredits += credits;
                    }
                }

                con.Close();
            }

            double gpa = totalCredits > 0 ? totalGradePoints / totalCredits : 0;
            richTextBox1.Text = "YOUR PREDICTED GPA IS: " + gpa.ToString("0.00");

        }
    }
}
