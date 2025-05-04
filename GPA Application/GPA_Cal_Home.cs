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
            Weak_Area_Tracker f2 = new Weak_Area_Tracker();
            this.Hide();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gradingscale f2 = new gradingscale();
            this.Hide();
            f2.Show();
        }
    }
}
