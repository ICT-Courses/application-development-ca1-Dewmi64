﻿using System;
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

        }

        private void button4_Click(object sender, EventArgs e)
        {
            add_assignment f2 = new add_assignment();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
