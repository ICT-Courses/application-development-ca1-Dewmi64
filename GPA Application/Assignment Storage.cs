using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPA_Application
{
    public class Assignment_Storage
    {
        internal static IEnumerable<object> assignments;

        public static List<Assignment_Details> Assignments { get; private set; } = new List<Assignment_Details>();

        public static void AddAssignment(Assignment_Details assignment)
        {
            Assignments.Add(assignment);
        }
    }


}
