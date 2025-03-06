﻿using System;
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
    public partial class add_assignment : Form
    {
        private Assignment_Storage Assignment_Storage;
        public add_assignment()
        {
            InitializeComponent();
            Assignment_Storage = new Assignment_Storage();
        }

        private void add_assignment_Load(object sender, EventArgs e)
        {
            string subjectName = richTextBox1.Text.Trim();
            string description = richTextBox2.Text.Trim();
            DateTime dueDate = dateTimePicker1.Value;

            Assignment_Details newAssignment = new Assignment_Details(subjectName, description, dueDate);
            Assignment_Storage.AddAssignment(newAssignment); // Add assignment to the list
        }
    }
}

