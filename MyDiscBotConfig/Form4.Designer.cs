namespace MyDiscBotConfig
{
    partial class CSVEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CSVView = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.CSVFileNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CSVView)).BeginInit();
            this.SuspendLayout();
            // 
            // CSVView
            // 
            this.CSVView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CSVView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CSVView.Location = new System.Drawing.Point(-3, 30);
            this.CSVView.Name = "CSVView";
            this.CSVView.Size = new System.Drawing.Size(803, 421);
            this.CSVView.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(720, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CSVFileNameLabel
            // 
            this.CSVFileNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CSVFileNameLabel.Location = new System.Drawing.Point(5, 4);
            this.CSVFileNameLabel.Name = "CSVFileNameLabel";
            this.CSVFileNameLabel.Size = new System.Drawing.Size(709, 23);
            this.CSVFileNameLabel.TabIndex = 2;
            this.CSVFileNameLabel.Text = "label1";
            // 
            // CSVEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CSVFileNameLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CSVView);
            this.Name = "CSVEditor";
            this.Text = "CSV Editor";
            this.Load += new System.EventHandler(this.CSVEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CSVView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView CSVView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label CSVFileNameLabel;
    }
}