namespace MyDiscBotConfig
{
    partial class GoogleHelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoogleHelpForm));
            this.SheetQuickStart = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SheetQuickStart
            // 
            this.SheetQuickStart.AutoSize = true;
            this.SheetQuickStart.Location = new System.Drawing.Point(12, 138);
            this.SheetQuickStart.Name = "SheetQuickStart";
            this.SheetQuickStart.Size = new System.Drawing.Size(295, 13);
            this.SheetQuickStart.TabIndex = 0;
            this.SheetQuickStart.TabStop = true;
            this.SheetQuickStart.Text = "https://developers.google.com/sheets/api/quickstart/nodejs";
            this.SheetQuickStart.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SheetQuickStart_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(455, 130);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // GoogleHelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 160);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.SheetQuickStart);
            this.Name = "GoogleHelpForm";
            this.ShowIcon = false;
            this.Text = "GoogleSheetCSV";
            this.Load += new System.EventHandler(this.GoogleResultsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel SheetQuickStart;
        private System.Windows.Forms.TextBox textBox1;
    }
}