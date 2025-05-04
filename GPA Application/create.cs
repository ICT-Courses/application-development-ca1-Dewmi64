using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GPA_Application

{
    public partial class create : Form
    {
        public create()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textbox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please fill in all fields before creating an account.","Incomplete Input",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            
            }
            else if (textbox2.Text != textBox3.Text)
            {
                MessageBox.Show(" Confirm Passwords do not match.","Password Mismatch",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                UserCredentials.Username = textBox1.Text;
                UserCredentials.Password = textbox2.Text;


               

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

               


                login f2 = new login();
                this.Hide();
                f2.Show();
            }
        }
        

        private void label2_Click(object sender, EventArgs e)
        {
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Home f2 = new Home();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textbox2.Clear();
        }

        private void create_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textbox2.Text, @"^[A-Za-z]{4}[^A-Za-z0-9]{1}$"))
            {
                errorProvider1.SetError(textbox2, " ");
            }
            else
            {
                errorProvider1.SetError(textbox2, "Must contain exactly four letters and one symbol");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void show_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.UseSystemPasswordChar = true;
            }

            else
            {
                textBox3.UseSystemPasswordChar = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            create f2 = new create();
            this.Hide();
            f2.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
