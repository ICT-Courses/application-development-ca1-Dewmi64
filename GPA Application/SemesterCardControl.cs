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
    public partial class SemesterCardControl : UserControl
    {
        public SemesterCardControl()
        {
            InitializeComponent();
        }

        private void SemesterCardControl_Load(object sender, EventArgs e)
        {
            ButtonCornerStyler.ApplyRoundedCorners(btnDelete, 30);
            
        }

        public string SemesterName
        {
            get => lblSemester.Text;
            set => lblSemester.Text = value;
        }

        public string GPA
        {
            get => lblGPA.Text;
            set => lblGPA.Text = "GPA: " + value;
        }

       

        public void SetSubjects(List<(string subject, string grade)> subjects)
        {
            // Clear existing rows
            dataGridViewSubjects.Rows.Clear();
            dataGridViewSubjects.Columns.Clear();

            // Add Columns
            DataGridViewTextBoxColumn subjectColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Subject",
                HeaderText = "Subject",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            DataGridViewTextBoxColumn gradeColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Grade",
                HeaderText = "Grade",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            dataGridViewSubjects.Columns.Add(subjectColumn);
            dataGridViewSubjects.Columns.Add(gradeColumn);

            // Add Rows
            foreach (var item in subjects)
            {
                dataGridViewSubjects.Rows.Add(item.subject, item.grade);
            }

            // Optional: Disable sorting
            foreach (DataGridViewColumn col in dataGridViewSubjects.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }






        public event EventHandler<string> OnDeleteClicked;
        private void btnDelete_Click(object sender, EventArgs e)
        {
            OnDeleteClicked?.Invoke(this, SemesterName);
        }
    }
}
