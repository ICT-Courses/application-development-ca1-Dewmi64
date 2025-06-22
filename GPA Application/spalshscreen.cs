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
    public partial class spalshscreen : Form
    {
        int progressValue = 0;
        public spalshscreen()
        {
            InitializeComponent();
        }

        private void spalshscreen_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressValue += 2; 

            
            if (progressValue > 100)
                progressValue = 100;

            progressBar1.Value = progressValue;

            if (progressValue < 25)
            {
                label1.Text = "Initializing...";
            }
            else if (progressValue < 50)
            {
                label1.Text = "Starting up GPA MindBoost...";
            }
            else if (progressValue < 75)
            {
                label1.Text = "Loading Academic Data...";
            }
            else
            {
                label1.Text = "Launching GPA+MindBoost...";
            }


            // When loading completes
            if (progressValue >= 100)
            {
                timer1.Stop();     // Stop the timer
                this.Hide();       // Hide splash screen

               
                Home mainForm = new Home();
                mainForm.Show();
            }
        }
    }
}
