using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class UpcomingAssignments : Form
    {
        public UpcomingAssignments()
        {
            InitializeComponent();
        }

        private void UpcomingAssignments_Load(object sender, EventArgs e)
        {
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown; // Vertical layout
            flowLayoutPanel1.WrapContents = false; // Prevent side wrapping
            flowLayoutPanel1.AutoScroll = true;

            // Load assignments from the database
            List<Assignment_Details> assignments = LoadAssignmentsFromDatabase();

            // Debugging: Print assignment count
            Console.WriteLine($"Assignments Count: {assignments.Count}");

            // If no assignments exist, show a "No upcoming assignments" message
            if (assignments == null || assignments.Count == 0)
            {
                Label noAssignmentsLabel = new Label();
                noAssignmentsLabel.Text = "No upcoming assignments.";
                noAssignmentsLabel.Font = new Font("Segoe UI", 12, FontStyle.Italic);
                noAssignmentsLabel.ForeColor = Color.Gray;
                noAssignmentsLabel.AutoSize = true;
                flowLayoutPanel1.Controls.Add(noAssignmentsLabel);
                return;
            }

            // Loop through assignments and display each in the flow layout panel
            foreach (var assignment in assignments)
            {
                if (assignment.DueDate >= DateTime.Today &&
                    !string.IsNullOrEmpty(assignment.SubjectName) &&
                    !string.IsNullOrEmpty(assignment.Description))
                {
                    // Create a panel for each assignment
                    Panel notePanel = new Panel
                    {
                        Width = 450, // Wider panel
                        Height = 160,
                        BackColor = Color.SkyBlue, // Default color set to SkyBlue
                        Padding = new Padding(10),
                        Margin = new Padding(10),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    // Subject label
                    Label lblSubject = new Label
                    {
                        Text = "Subject Name: " + assignment.SubjectName,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        AutoSize = true
                    };

                    // Due date label
                    Label lblDate = new Label
                    {
                        Text = "Due Date: " + assignment.DueDate.ToString("yyyy-MM-dd"),
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.DarkSlateGray,
                        AutoSize = true
                    };

                    // Description label
                    Label lblDescription = new Label
                    {
                        Text = "Assignment description: " + assignment.Description,
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        MaximumSize = new Size(430, 0), // text wrapping
                        AutoSize = true,
                        Margin = new Padding(0, 5, 0, 5)
                    };

                    // FlowLayoutPanel to hold the labels
                    FlowLayoutPanel labelFlow = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.TopDown,
                        Dock = DockStyle.Fill,
                        WrapContents = false,
                        AutoScroll = true
                    };

                    // Button to mark assignment as done
                    Button btnEdit = new Button
                    {
                        Text = "Edit",
                        Size = new Size(120, 35),
                        BackColor = Color.Orange,
                        Font = new Font("Segoe UI", 9, FontStyle.Regular),
                        Cursor = Cursors.Hand,
                        Margin = new Padding(0, 5, 0, 0), // Adding margin to separate buttons
                    };

                    // Button to edit the assignment
                     Button btnMarkDone = new Button
                     {
                        Text = "Mark as Done",
                        Size = new Size(120, 35),
                        BackColor = Color.LightGreen,
                        Font = new Font("Segoe UI", 9, FontStyle.Regular),
                        Cursor = Cursors.Hand,
                        Margin = new Padding(10, 5, 0, 0), // Increased margin between the buttons
                    };

                    // Create a separate FlowLayoutPanel to hold the buttons and align them side by side
                    FlowLayoutPanel buttonFlow = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight, // Align buttons horizontally
                        AutoSize = true,
                        Margin = new Padding(0, 5, 0, 0)
                    };

                    // Add buttons to the button flow panel
                    buttonFlow.Controls.Add(btnMarkDone);
                    buttonFlow.Controls.Add(btnEdit);

                    // Add labels to the label flow panel
                    labelFlow.Controls.Add(lblSubject);
                    labelFlow.Controls.Add(lblDescription);
                    labelFlow.Controls.Add(lblDate);
                    labelFlow.Controls.Add(buttonFlow); // Add the button panel to the label flow panel

                    // Add the flow panel to the note panel
                    notePanel.Controls.Add(labelFlow);

                    // Add the note panel to the FlowLayoutPanel
                    flowLayoutPanel1.Controls.Add(notePanel);
                }
            }
        }

        private List<Assignment_Details> LoadAssignmentsFromDatabase()
        {
            List<Assignment_Details> assignments = new List<Assignment_Details>();

            string connectionString = "server=localhost;uid=root;pwd=Hwcs@1969;database=assignment_storage;";
            string query = "SELECT subject_name, description, due_date FROM Assignment_Storage WHERE due_date >= @today ORDER BY due_date ASC";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@today", DateTime.Today);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string subjectName = reader.GetString("subject_name");
                        string description = reader.GetString("description");
                        DateTime dueDate = reader.GetDateTime("due_date");

                        assignments.Add(new Assignment_Details(subjectName, description, dueDate));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading assignments: " + ex.Message);
            }

            return assignments;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // You can handle painting logic here if needed
        }
    }
}


