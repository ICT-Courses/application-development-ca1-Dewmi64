using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPA_Application
{

    public class AssignmentDatabaseHelper
    {
        private string connectionString = "server=localhost;uid=root;pwd=Hwcs@1969;database=assignment_storage;";

        public void UpdateAssignment(Assignment_Details oldAssignment, Assignment_Details updated)
        {
            string query = @"UPDATE Assignment_Storage 
                         SET subject_name = @newSubject, 
                             description = @newDescription, 
                             due_date = @newDueDate 
                         WHERE subject_name = @oldSubject AND 
                               description = @oldDescription AND 
                               due_date = @oldDueDate";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@newSubject", updated.SubjectName);
                    cmd.Parameters.AddWithValue("@newDescription", updated.Description);
                    cmd.Parameters.AddWithValue("@newDueDate", updated.DueDate);
                    cmd.Parameters.AddWithValue("@oldSubject", oldAssignment.SubjectName);
                    cmd.Parameters.AddWithValue("@oldDescription", oldAssignment.Description);
                    cmd.Parameters.AddWithValue("@oldDueDate", oldAssignment.DueDate);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating assignment: " + ex.Message);
            }
        }
    }

}
