﻿using System;
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
    public partial class GoogleHelpForm : Form
    {
        public GoogleHelpForm()
        {
            InitializeComponent();
        }

        private void Output_TextChanged(object sender, EventArgs e)
        {

        }

        private void GoogleResultsForm_Load(object sender, EventArgs e)
        {

        }

        private void SheetQuickStart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(SheetQuickStart.Text);
        }
    }
}
