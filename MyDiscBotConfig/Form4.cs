using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace MyDiscBotConfig
{
    public partial class CSVDataRow 
    {
        public string name { get; set; }
        public int gatherNum { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int farm { get; set; }
        public bool fuel { get; set; }
        public bool lumber { get; set; }
        public bool iron { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }
        public bool sunday { get; set; }
        public bool equalize { get; set; }
        public bool ignoreOthers { get; set; }
        public int skipAfterMarchFail { get; set; }

    }
    public partial class CSVEditor : Form
    {
        private Form DiscBotForm;
        public CSVEditor(Form form)
        {
            DiscBotForm = form;
            InitializeComponent();
        }

        private void CSVEditor_Load(object sender, EventArgs e)
        {
            string csv_file_path = DiscBotForm.Controls["ConfigurationTabs"].Controls["GoogleSheet"].Controls["gatherCSV"].Text;
            CSVFileNameLabel.Text = csv_file_path;
            DataTable csvData = GetDataTableFromCSVFile(csv_file_path);
            Console.WriteLine("Rows count:" + csvData.Rows.Count);
            CSVView.DataSource = csvData;
        }
        private static DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    Type t = typeof(bool);

                    foreach (string column in colFields)
                    {
                        if (column == "name")
                        {
                            t = typeof(string);
                        }
                        else if (column == "gatherNum" | column == "cfg.x" | column == "cfg.y" | column == "cfg.skipAfterMarchFail")
                        {
                            t = typeof(int);
                        }
                        else
                        {
                            t = typeof(bool);
                        }
                        DataColumn datacolumn = new DataColumn(column, t);
                        datacolumn.AllowDBNull = true;
                        // csvData.Columns.Add(datacolumn);
                        csvData.Columns.Add(column, t);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == null)
                            {
                                fieldData[i] = "";
                            }
                        }

                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return csvData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save the CSV?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                saveCSVFile(CSVFileNameLabel.Text);
            }
        }

        private void saveCSVFile(string fileName)
        {
            string csvText = "";
            FileStream CSVOutFile = File.Open(CSVFileNameLabel.Text, FileMode.Truncate, FileAccess.Write);
            StreamWriter CSVWriter = new StreamWriter(CSVOutFile);
            foreach (DataGridViewColumn column in CSVView.Columns)
            {
                csvText += column.HeaderText;
                if (column.Index < CSVView.Columns.Count - 1)
                {
                    csvText += ",";
                }
            }
            CSVWriter.WriteLine(csvText);
            foreach (DataGridViewRow row in CSVView.Rows)
            {
                csvText = "";
                if (row.Index < CSVView.Rows.Count - 1)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        csvText += cell.Value;
                        if (cell.ColumnIndex < CSVView.Columns.Count - 1)
                        {
                            csvText += ",";
                        }
                    }
                    CSVWriter.WriteLine(csvText);
                }
            }
            CSVWriter.Flush();
            CSVWriter.Close();
            CSVOutFile.Close();
        }
    }
}
