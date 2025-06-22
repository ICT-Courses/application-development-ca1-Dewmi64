using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GPA_Application
{
    public partial class Upcoming_Assignments : Form
    {
        string connectionString = "Server=localhost;Database=assignment_storage;User ID=root;Password=Hwcs@1969;";

        public Upcoming_Assignments()
        {
            InitializeComponent();

            
        }

        private void Upcoming_Assignments_Load(object sender, EventArgs e)
        {
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            flowLayoutPanel1.Controls.Clear();

            string query = "SELECT subject_name, description, due_date FROM Assignment_Storage ORDER BY due_date ASC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string subject = reader.GetString("subject_name");
                        string desc = reader.GetString("description");
                        DateTime dueDate = reader.GetDateTime("due_date");

                        Assignment_Card card = new Assignment_Card();
                        card.SubjectName = subject;
                        card.Description = desc;
                        card.DueDate = dueDate.ToShortDateString();

                        // Store identity data in Tag (as anonymous object)
                        card.Tag = new { subject, desc, dueDate };

                        // Attach event handlers
                        card.MarkDoneClicked += Card_MarkDoneClicked;
                        card.ViewClicked += Card_ViewClicked;
                        card.EditClicked += Card_EditClicked;
                        card.DeleteClicked += Card_DeleteClicked;

                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading assignments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ----- Event Handlers -----

        private void Card_MarkDoneClicked(object sender, EventArgs e)
        {
            if (sender is Assignment_Card card)
            {
                DialogResult result = MessageBox.Show(
                    $"Marked '{card.SubjectName}' as Done ✅",
                    "Done",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {
                    card.MarkAsDone(); 
                }
            }
        }





        private void Card_ViewClicked(object sender, EventArgs e)
        {
         
            if (sender is Assignment_Card card)
            {
                ViewAssignmentForm viewForm = new ViewAssignmentForm(
                    card.SubjectName,
                    card.Description,
                    card.DueDate
                );

                viewForm.StartPosition = FormStartPosition.CenterParent;
                viewForm.ShowDialog(); // Modal popup
            }
        }



        private void Card_EditClicked(object sender, EventArgs e)
        {
            if (sender is Assignment_Card card)
            {
                var original = (dynamic)card.Tag;

                Edit_Assignment editForm = new Edit_Assignment(
                    original.subject,
                    original.desc,
                    original.dueDate
                );

                editForm.StartPosition = FormStartPosition.CenterParent;

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string updatedSubject = editForm.UpdatedSubject;
                    string updatedDescription = editForm.UpdatedDescription;
                    DateTime updatedDueDate = editForm.UpdatedDueDate;

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        string updateQuery = @"
                    UPDATE Assignment_Storage 
                    SET subject_name = @newSubject, description = @newDescription, due_date = @newDueDate 
                    WHERE subject_name = @oldSubject AND description = @oldDescription AND due_date = @oldDueDate";

                        MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@newSubject", updatedSubject);
                        cmd.Parameters.AddWithValue("@newDescription", updatedDescription);
                        cmd.Parameters.AddWithValue("@newDueDate", updatedDueDate);

                        cmd.Parameters.AddWithValue("@oldSubject", original.subject);
                        cmd.Parameters.AddWithValue("@oldDescription", original.desc);
                        cmd.Parameters.AddWithValue("@oldDueDate", original.dueDate);

                        try
                        {
                            conn.Open();
                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                MessageBox.Show("Assignment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAssignments(); // Refresh list
                            }
                            else
                            {
                                MessageBox.Show("Assignment update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        private void Card_DeleteClicked(object sender, EventArgs e)
        {
            if (sender is Assignment_Card card)
            {
                var assignment = (dynamic)card.Tag;

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete '{assignment.subject}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        string deleteQuery = @"
                    DELETE FROM Assignment_Storage 
                    WHERE subject_name = @subject AND description = @desc AND due_date = @dueDate";

                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@subject", assignment.subject);
                        cmd.Parameters.AddWithValue("@desc", assignment.desc);
                        cmd.Parameters.AddWithValue("@dueDate", assignment.dueDate);

                        try
                        {
                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Assignment deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAssignments(); // Refresh the list
                            }
                            else
                            {
                                MessageBox.Show("Assignment not found or already deleted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting assignment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is Assignment_Card card)
                {
                    bool match = card.SubjectName.ToLower().Contains(searchText) ||
                                 card.Description.ToLower().Contains(searchText);

                    card.Visible = match;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Assignment_Deadline_Tracker f2 = new Assignment_Deadline_Tracker();
            this.Hide();
            f2.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


