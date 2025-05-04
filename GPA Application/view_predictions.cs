using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class view_predictions : Form
    {
        private FlowLayoutPanel predictionsPanelContainer;

        public view_predictions()
        {
            ;
            InitializePredictionsPanel();
        }

        private void InitializePredictionsPanel()
        {
            predictionsPanelContainer = new FlowLayoutPanel();
            predictionsPanelContainer.Location = new Point(10, 50);
            predictionsPanelContainer.AutoSize = true;
            predictionsPanelContainer.FlowDirection = FlowDirection.LeftToRight;
            predictionsPanelContainer.WrapContents = true;
            this.Controls.Add(predictionsPanelContainer);
        }

        private void view_predictions_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = @"
                    SELECT semester, subject, grade, gpa
                    FROM predicted_results
                    ORDER BY semester";

                MySqlCommand cmd = new MySqlCommand(query, con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                Dictionary<string, List<(string Subject, string Grade, string GPA)>> predictionsBySemester =
                    new Dictionary<string, List<(string, string, string)>>();

                while (reader.Read())
                {
                    string semesterName = reader["semester"].ToString();
                    string subjectName = reader["subject"].ToString();
                    string predictedGrade = reader["grade"].ToString();
                    string predictedGPA = reader["gpa"].ToString();

                    if (!predictionsBySemester.ContainsKey(semesterName))
                    {
                        predictionsBySemester[semesterName] = new List<(string, string, string)>();
                    }
                    predictionsBySemester[semesterName].Add((subjectName, predictedGrade, predictedGPA));
                }

                con.Close();

                // If no predictions found, show default card
                if (predictionsBySemester.Count == 0)
                {
                    Panel defaultCard = CreateDefaultCard();
                    predictionsPanelContainer.Controls.Add(defaultCard);
                    return;
                }

                foreach (var semesterPrediction in predictionsBySemester)
                {
                    Panel predictionCard = CreatePredictionCard(semesterPrediction.Key, semesterPrediction.Value);
                    predictionsPanelContainer.Controls.Add(predictionCard);
                }
            }
        }

        private Panel CreatePredictionCard(string semesterName, List<(string Subject, string Grade, string GPA)> predictions)
        {
            Panel cardPanel = new Panel
            {
                Width = 300,
                AutoSize = true,
                BackColor = Color.LightCyan,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };

            Label semesterTitleLabel = new Label
            {
                Text = $"📘 Semester: {semesterName}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            cardPanel.Controls.Add(semesterTitleLabel);

            string gpa = predictions.First().GPA;
            Label gpaLabel = new Label
            {
                Text = $"🎯 Predicted GPA: {gpa}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, semesterTitleLabel.Bottom + 10)
            };
            cardPanel.Controls.Add(gpaLabel);

            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Location = new Point(10, gpaLabel.Bottom + 10)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // Header row
            table.Controls.Add(new Label
            {
                Text = "Subject",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            }, 0, 0);

            table.Controls.Add(new Label
            {
                Text = "Grade",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            }, 1, 0);

            // Subject rows
            int row = 1;
            foreach (var prediction in predictions)
            {
                table.Controls.Add(new Label
                {
                    Text = prediction.Subject,
                    Font = new Font("Segoe UI", 11),
                    AutoSize = true
                }, 0, row);

                table.Controls.Add(new Label
                {
                    Text = prediction.Grade,
                    Font = new Font("Segoe UI", 11),
                    AutoSize = true
                }, 1, row);

                row++;
            }

            cardPanel.Controls.Add(table);

            Label deleteLabel = new Label
            {
                Text = "🗑 Delete",
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9, FontStyle.Underline),
                AutoSize = true,
                Cursor = Cursors.Hand,
                Tag = semesterName,
                Location = new Point(cardPanel.Width - 80, table.Bottom + 10)
            };
            deleteLabel.Click += DeleteLabel_Click;
            cardPanel.Controls.Add(deleteLabel);

            return cardPanel;
        }

        private Panel CreateDefaultCard()
        {
            Panel defaultPanel = new Panel
            {
                Width = 300,
                Height = 100,
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };

            Label messageLabel = new Label
            {
                Text = "📭 No predictions available.",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(10, 30),
                ForeColor = Color.DimGray
            };

            defaultPanel.Controls.Add(messageLabel);
            return defaultPanel;
        }

        private void DeleteLabel_Click(object sender, EventArgs e)
        {
            Label deleteLabel = sender as Label;
            string semesterToDelete = deleteLabel.Tag.ToString();

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete all predictions for semester '{semesterToDelete}'?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost;user=root;database=grading_scale_database;password=Hwcs@1969;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string deleteQuery = "DELETE FROM predicted_results WHERE semester = @semester";
                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@semester", semesterToDelete);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the view
                predictionsPanelContainer.Controls.Clear();
                view_predictions_Load(null, null);
            }
        }
    }
}
