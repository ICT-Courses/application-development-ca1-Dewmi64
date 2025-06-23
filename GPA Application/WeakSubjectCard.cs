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
    public partial class WeakSubjectCard : UserControl
    {
        public WeakSubjectCard()
        {
            InitializeComponent();
        }

        public void SetData(string subject, string grade, string tip)
        {
            label1.Text = $"📘 {subject}";
            label2.Text = $"Grade: {grade}";
            label3.Text = $"Tip: {tip}";
        }

        private void WeakSubjectCard_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
