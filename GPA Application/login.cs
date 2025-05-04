using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GPA_Application.create;

namespace GPA_Application
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string enteredUsername = textBox1.Text;
            string enteredPassword = textBox2.Text;

            if (string.IsNullOrEmpty(UserCredentials.Username) || string.IsNullOrEmpty(UserCredentials.Password))
            {
                MessageBox.Show("You don’t have an account yet. Please create an account.", "No Account Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Please fill in both username and password.", "Incomplete Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (enteredUsername == UserCredentials.Username && enteredPassword == UserCredentials.Password)
            {
                MessageBox.Show("Login successful!","Login Successful",MessageBoxButtons.OK,MessageBoxIcon.Information);
               
                dashbord f2 = new dashbord();
                this.Hide();
                f2.Show();
                
            }


            else
            {
                MessageBox.Show("Invalid username or password. Please try again.","login Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
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
            textBox2.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
            }

            else
            {
                textBox2.UseSystemPasswordChar = false;
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            login f2 = new login();
            this.Hide();
            f2.Show();
        }
    }
}
