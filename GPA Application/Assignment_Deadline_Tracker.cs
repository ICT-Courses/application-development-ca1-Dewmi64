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
    public partial class Assignment_Deadline_Tracker : Form
    {
        public Assignment_Deadline_Tracker()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_assignment f2 = new add_assignment();
            this.Hide();
            f2.Show();
        }
    }
}
