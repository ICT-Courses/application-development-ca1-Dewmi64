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
    public partial class PredictionCardControl : UserControl
    {
        public PredictionCardControl()
        {
            InitializeComponent();
            this.BackColor = Color.SkyBlue;
            this.Margin = new Padding(10);
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        public string Semester
        {
            get => lblSemester.Text;
            set => lblSemester.Text = $"📘 Semester: {value}";
        }

        public string GPA
        {
            get => lblGPA.Text;
            set => lblGPA.Text = $"🎯 Predicted GPA: {value}";
        }

        public string OverallGPA
        {
            get => label1.Text;
            set => label1.Text = $"📊 According to this prediction, overall GPA will be: {value}";
        }


        public void SetSubjects(List<(string subject, string grade)> subjects)
        {
            tableLayoutPanelSubjects.Controls.Clear();
            tableLayoutPanelSubjects.RowStyles.Clear();
            tableLayoutPanelSubjects.RowCount = 0;

            // Set columns count to 2 (Subject, Grade)
            tableLayoutPanelSubjects.ColumnCount = 2;

            // Add header row (optional)
            tableLayoutPanelSubjects.RowCount = 1;
            tableLayoutPanelSubjects.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label headerSubject = new Label()
            {
                Text = "Subject",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkSlateBlue,
                Padding = new Padding(2)
            };
            Label headerGrade = new Label()
            {
                Text = "Grade",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkSlateBlue,
                Padding = new Padding(2)
            };

            tableLayoutPanelSubjects.Controls.Add(headerSubject, 0, 0);
            tableLayoutPanelSubjects.Controls.Add(headerGrade, 1, 0);

            int row = 1;
            foreach (var item in subjects)
            {
                tableLayoutPanelSubjects.RowCount++;
                tableLayoutPanelSubjects.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                Label lblSubject = new Label()
                {
                    Text = item.subject,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.Black,
                    Padding = new Padding(2)
                };

                Label lblGrade = new Label()
                {
                    Text = item.grade,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.Black,
                    Padding = new Padding(2)
                };

                tableLayoutPanelSubjects.Controls.Add(lblSubject, 0, row);
                tableLayoutPanelSubjects.Controls.Add(lblGrade, 1, row);

                row++;
            }
        }

        public event EventHandler DeleteClicked;

        private void button1_Click(object sender, EventArgs e) //Delete button
        {
            // Confirm delete
            DialogResult result = MessageBox.Show("Are you sure you want to delete this prediction?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DeleteClicked?.Invoke(this, EventArgs.Empty); // 🔔 Raise event to parent
            }
        }

        private void PredictionCardControl_Load(object sender, EventArgs e)
        {

            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    
}
