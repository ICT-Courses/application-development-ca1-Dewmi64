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
    public partial class dashbord : Form
    {
        public dashbord()
        {
            InitializeComponent();
        }

        private void dashbord_Load(object sender, EventArgs e)
        {
            label3.Text = UserCredentials.Username;

            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);

            //applied round button corners
            ButtonCornerStyler.ApplyRoundedCorners(button1, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button2, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button3, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button4, 30);
            ButtonCornerStyler.ApplyRoundedCorners(button5, 30);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Assignment_Deadline_Tracker f2 = new Assignment_Deadline_Tracker();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Weak_Area_Tracker f2 = new Weak_Area_Tracker();
            this.Hide();
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Motivation f2 = new Motivation();
            this.Hide();
            f2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GPA_Cal_Home f2 = new GPA_Cal_Home();   
            this.Hide();
            f2.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GPA_Predictor_Home f2 = new GPA_Predictor_Home();
            this.Hide();
            f2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            gradingscale f2 = new gradingscale();
            this.Hide();
            f2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            view_academic_performance f2 = new view_academic_performance();
            this.Hide();
            f2.Show();
        }

        private void button10_Click(object sender, EventArgs e)
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
