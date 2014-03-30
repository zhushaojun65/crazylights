using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSLightDebug
{
    public partial class WhatAFuck : Form
    {
        public WhatAFuck()
        {
            InitializeComponent();
        }
        Console console = new Console();
        private void WhatAFuck_Load(object sender, EventArgs e)
        {
            console.Show(this.dockPanel1);
            CSLightDebug.Debugger.endSetLockedDebugTag();
        }

        private void WhatAFuck_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //Application.ExitThread();
            CSLightDebug.Debugger.beginSetLockedDebugTag(false);
        }
         delegate void Action(string str); 
        public void SafeLog(string str)
        {
            Action a = (s) =>
                {
                    console.Log(s,Color.Black);
                };

            this.Invoke(a,new object[]{str});
        }
        public void SafeLog_Warn(string str)
        {
            Action a = (s) =>
            {
                console.Log(s, Color.Blue);
            };

            this.Invoke(a, new object[] { str });
        }
        public void SafeLog_Error(string str)
        {
            Action a = (s) =>
            {
                console.Log(s, Color.Red);
            };

            this.Invoke(a, new object[] { str });
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.console.Show(dockPanel1);
        }
    }
}
