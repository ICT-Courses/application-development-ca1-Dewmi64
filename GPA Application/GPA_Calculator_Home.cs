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
    public partial class GPA_Calculator_Home : Form
    {
        public GPA_Calculator_Home()
        {
            InitializeComponent();
        }

        private void GPA_Calculator_Home_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            gradingscale f2 = new gradingscale();
            f2.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            view_academic_performance f2 = new view_academic_performance();
            f2.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Weak_Area_Tracker f2 = new Weak_Area_Tracker();
            f2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gradingscale f2 = new gradingscale();
            this.Hide();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            view_academic_performance f2 = new view_academic_performance();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
