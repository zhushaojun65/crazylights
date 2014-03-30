using CSLightDebug.submodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSLightDebug
{
    public partial class MainDebugWin : Form
    {
        public MainDebugWin()
        {
            InitializeComponent();
        }
        Console console = new Console();
        Solution solution = new Solution();
        CSLightDebug.submodel.Attribute attribute = new CSLightDebug.submodel.Attribute();
        Watch watch = new Watch();
        private void WhatAFuck_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                Code code = new Code();
                code.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            console.Show(this.dockPanel1,WeifenLuo.WinFormsUI.Docking.DockState.DockBottom);
            solution.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            watch.Show(console.Pane,WeifenLuo.WinFormsUI.Docking.DockAlignment.Right,0.5);
            attribute.Show(solution.Pane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.5);
            CSLightDebug.Debugger.endSetLockedDebugTag();
        }
        public void SafeShow()
        {
            Action ac = () =>
            {
                this.Show();
            };
            this.Invoke(ac);
        }
        private void WhatAFuck_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

        }
         delegate void Action(); 
        public void SafeLog(string str)
        {
            Action<string> a = (s) =>
                {
                    console.Log(s,Color.Black);
                };

            this.Invoke(a,new object[]{str});
        }
        public void SafeLog_Warn(string str)
        {
            Action<string> a = (s) =>
            {
                console.Log(s, Color.Blue);
            };

            this.Invoke(a, new object[] { str });
        }
        public void SafeLog_Error(string str)
        {
            Action<string> a = (s) =>
            {
                console.Log(s, Color.Red);
            };

            this.Invoke(a, new object[] { str });
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.console.Show(dockPanel1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            //
            CSLightDebug.Debugger.beginSetLockedDebugTag(false);
            Application.ExitThread();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// 通知栏菜单exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            CSLightDebug.Debugger.beginSetLockedDebugTag(false);
            Application.ExitThread();
        }
        /// <summary>
        ///  通知栏菜单show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
