﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScriptTestor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CSLightDebug.DebugWPF.OpenDebugWin(onClose);
        }
        void onClose()
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            CSLightDebug.Debug.OpenDebugWin(onClose);
        }
    }
}
