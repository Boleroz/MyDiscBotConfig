using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDiscBotConfig
{
    public partial class HourlyConfigForm : Form
    {
        public HourlyConfigForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                ConfigDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
