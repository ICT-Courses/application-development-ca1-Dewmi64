using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPA_Application
{
    public class Assignment_Details
    {
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public Assignment_Details(string subjectName, string description, DateTime dueDate)
        {
            SubjectName = subjectName;
            Description = description;
            DueDate = dueDate;
        }
    }
}
