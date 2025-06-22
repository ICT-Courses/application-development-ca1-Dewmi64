namespace GPA_Application
{
    partial class PredictionCardControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSemester = new System.Windows.Forms.Label();
            this.lblGPA = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanelSubjects = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Font = new System.Drawing.Font("Myanmar Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSemester.Location = new System.Drawing.Point(14, 30);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(68, 36);
            this.lblSemester.TabIndex = 1;
            this.lblSemester.Text = "label1";
            // 
            // lblGPA
            // 
            this.lblGPA.AutoSize = true;
            this.lblGPA.Font = new System.Drawing.Font("Myanmar Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPA.Location = new System.Drawing.Point(14, 67);
            this.lblGPA.Name = "lblGPA";
            this.lblGPA.Size = new System.Drawing.Size(68, 36);
            this.lblGPA.TabIndex = 2;
            this.lblGPA.Text = "label2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(626, 477);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanelSubjects
            // 
            this.tableLayoutPanelSubjects.AutoScroll = true;
            this.tableLayoutPanelSubjects.ColumnCount = 2;
            this.tableLayoutPanelSubjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSubjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.tableLayoutPanelSubjects.Font = new System.Drawing.Font("Myanmar Text", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelSubjects.Location = new System.Drawing.Point(20, 116);
            this.tableLayoutPanelSubjects.Name = "tableLayoutPanelSubjects";
            this.tableLayoutPanelSubjects.RowCount = 1;
            this.tableLayoutPanelSubjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSubjects.Size = new System.Drawing.Size(702, 340);
            this.tableLayoutPanelSubjects.TabIndex = 4;
            this.tableLayoutPanelSubjects.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // PredictionCardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelSubjects);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblGPA);
            this.Controls.Add(this.lblSemester);
            this.Name = "PredictionCardControl";
            this.Size = new System.Drawing.Size(752, 519);
            this.Load += new System.EventHandler(this.PredictionCardControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.Label lblGPA;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSubjects;
    }
}
