using System;
using System.Windows.Forms;

namespace GPA_Application
{
    public partial class SuccessSplash : Form
    {
        int progressValue = 0;

        public SuccessSplash()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void SuccessSplash_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            timer1.Start();
        }

       
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            progressValue += 5;

            if (progressValue > 100)
                progressValue = 100;

            progressBar1.Value = progressValue;

            if (progressValue < 40)
            {
                label1.Text = "Creating your account...";
            }
            else if (progressValue < 80)
            {
                label1.Text = "Securing your data...";
            }
            else
            {
                label1.Text = "Finalizing setup...";
            }

            if (progressValue >= 100)
            {
                timer1.Stop();
                label1.Text = "✅ Account Created Successfully!";

                // Optional short delay
                Timer delayTimer = new Timer();
                delayTimer.Interval = 500;
                delayTimer.Tick += (s, args) =>
                {
                    delayTimer.Stop();
                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    login loginForm = new login();
                    this.Hide();
                    loginForm.Show();
                };
                delayTimer.Start();

            }
        }
    }
}
