using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPA_Application
{
    public class Assignment_Storage
    {
        public List<Assignment_Details> Assignments { get; private set; }

        public Assignment_Storage()
        {
            Assignments = new List<Assignment_Details>();
        }

        public void AddAssignment(Assignment_Details assignment)
        {
            Assignments.Add(assignment);
        }
    }
}
