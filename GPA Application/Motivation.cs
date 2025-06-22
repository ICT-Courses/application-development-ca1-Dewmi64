using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class Motivation : Form
    {
        public Motivation()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "⏳ Loading...";
            label1.Enabled = false;
            lbl1.Text = ""; // clear previous text

            await ShowRandomMotivationalQuote();

            await Task.Delay(500); // optional UI pause
            button1.Text = "🔄 Show Another Quote";
            button1.Enabled = true;
        }

        private async Task ShowRandomMotivationalQuote()
        {
            string connectionString = "server=localhost;uid=root;pwd=Hwcs@1969;database=quotes_db;";
            string query = "SELECT quote FROM motivational_quotes ORDER BY RAND() LIMIT 1";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    string quote = cmd.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(quote))
                    {
                        panel1.BackColor = Color.FromArgb(100, 0, 0, 0); // semi-transparent dark panel
                        panel1.Visible = true;

                        await TypeTextAsync(lbl1, $"“{quote}”");

                        label1.Enabled = true;
                    }
                    else
                    {
                        lbl1.Text = "No quote found.";
                        panel1.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private async Task TypeTextAsync(Label label, string text, int delay = 30)
        {
            label.Text = "";
            foreach (char c in text)
            {
                label.Text += c;
                await Task.Delay(delay);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lbl1.Text))
            {
                Clipboard.SetText(lbl1.Text);
                MessageBox.Show("Quote copied to clipboard! 📋", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Motivation_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            label1.Enabled = false;

            // Word-wrap & auto-resize setup
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(panel1.Width - 20, 0); // wrap inside panel
            lbl1.TextAlign = ContentAlignment.TopLeft;
            lbl1.Padding = new Padding(10);

            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lbl1.Text))
            {
                Clipboard.SetText(lbl1.Text);
                MessageBox.Show("Quote copied to clipboard! 📋", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
