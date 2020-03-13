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
    public partial class TokenInstructions : Form
    {
        public TokenInstructions()
        {
            InitializeComponent();
            textBox1.Select(0, 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void TokenInstructions_Load(object sender, EventArgs e)
        {

        }
    }
}
