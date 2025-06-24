using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPA_Application
{
    public partial class GPA_Cal_Home : Form
    {
        public GPA_Cal_Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           gradingscale f2 = new gradingscale();
            this.Hide();
            f2.Show();
        }

        private void GPA_Cal_Home_Load(object sender, EventArgs e)
        {
          
            label10.Text = UserCredentials.Username;
        }

        

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            GPA_calculationcs f2 = new GPA_calculationcs();
            this.Hide();
            f2.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            view_academic_performance f2 = new view_academic_performance();
            this.Hide();
            f2.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            GPA_Predictor f2 = new GPA_Predictor();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
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
