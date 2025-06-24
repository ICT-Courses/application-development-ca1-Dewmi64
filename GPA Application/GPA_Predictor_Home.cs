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
    public partial class GPA_Predictor_Home : Form
    {
        public GPA_Predictor_Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gradingscale f2 = new gradingscale();
            this.Hide();
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dashbord f2 = new dashbord();
            this.Hide();
            f2.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            GPA_Predictor f2 = new GPA_Predictor();
            this.Hide();
            f2.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            view_predictions f2 = new view_predictions();
            this.Hide();
            f2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Weak_Area_Tracker f2 = new Weak_Area_Tracker();
            this.Hide();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Motivation f2 = new Motivation();   
            this.Hide();
            f2.Show();
        }
    }
}
