using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CSLightDebug
{
    public partial class Console :DockContent
    {
        public Console()
        {
            InitializeComponent();
        }

        private void Console_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Console_Load(object sender, EventArgs e)
        {

        }
        public void Log(string str,Color color)
        {
            TreeNode node =new TreeNode();
            node.Text=str;
            node.ForeColor =color;
            this.treeView1.Nodes.Add(node);
        }
    }
}
