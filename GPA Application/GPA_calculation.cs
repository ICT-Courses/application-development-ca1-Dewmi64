using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
using MySql.Data.MySqlClient;

namespace GPA_Application
{
    public partial class GPA_calculation : Form
    {
        public GPA_calculation()
        {
            InitializeComponent();
        }

        private DataTable excelTable;
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Files|*.xls;*.xlsx" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var conf = new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                            };

                            var dataSet = reader.AsDataSet(conf);
                            excelTable = dataSet.Tables[0];
                            dataGridView1.DataSource = excelTable;
                        }
                    }
                }
            }
        
        }

        private void GPA_calculation_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
        }
    }
    
}
