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
    public partial class Motivation : Form
    {
        string connectionString = "server=localhost;user=root;password=Hwcs@1969;database=quotes_DB;";
        Random rand = new Random();


        public Motivation()
        {
            InitializeComponent();
        }


        

        private void Motivation_Load(object sender, EventArgs e)
        {
            LoadRandomQuote();
        }


        private void LoadRandomQuote()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string countQuery = "SELECT COUNT(*) FROM Motivational_Quotes";
                    MySqlCommand countCmd = new MySqlCommand(countQuery, conn);
                    int count = Convert.ToInt32(countCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        int randomId = rand.Next(1, count + 1);
                        string quoteQuery = "SELECT quote FROM Motivational_Quotes WHERE id = @id";
                        MySqlCommand quoteCmd = new MySqlCommand(quoteQuery, conn);
                        quoteCmd.Parameters.AddWithValue("@id", randomId);

                        string quote = quoteCmd.ExecuteScalar()?.ToString();
                        richTextBox1.Text = quote;

                        
                    }
                    else
                    {
                        richTextBox1.Text = "No motivations found in the database.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadRandomQuote();
        }
    }
}
