﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScriptTestor
{
    public partial class Form1 : Form,CSLight.ICLS_Logger
    {
        public Form1()
        {
            InitializeComponent();
        }

        void onClose()
        {
            try
            {
                Action safeclost = () =>
                    {
                        button3.Enabled = true;
                    };
                this.Invoke(safeclost);
            }catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            useDebug = false;
            Init();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            useDebug = true;
            //CSLightDebug.Debugger.OpenDebugWin(onClose); 
            
            Init();
            
        }
        public  CSLight.ICLS_Debugger CreateFormAss(string path)
        {
         string p=   System.IO.Path.GetFullPath(path);
           var ass = System.Reflection.Assembly.LoadFile(p);
          var t = ass.GetType("CSLightDebug.Debugger");
          return t.Assembly.CreateInstance(t.FullName) as CSLight.ICLS_Debugger;

        }
        bool bInit = false;
        CSLight.ICLS_Debugger debugger;

        class MoniterQuit
        {

        }
        MoniterQuit moniterQuit = new MoniterQuit();
        public void Init()
        {
            bInit = true;
            button1.Enabled = false;
            button4.Enabled = false;
            button2.Enabled = true;

            CSLight.ICLS_Logger logger = null;
            if (useDebug)
            {
                debugger = CreateFormAss("../libgen/CSLightDebuger.dll");
                debugger.Init(this, moniterQuit);
                logger = debugger;
            }
            else
            {
                logger = this;
            }
            scriptEnv = new CSLight.CLS_Environment(logger);
            //start timer
            timer1.Enabled = true;
            timer1.Interval = 1000;
            scriptEnv.ExecuteCode(this.codeInFolder.getCode("code\\init.cls.txt"));

        }

        CSLight.CLS_Environment scriptEnv;
        bool useDebug = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            codeInFolder =new CodeColl();

            foreach(var f in codeInFolder.listCode)
            {
                TreeNode node = new TreeNode(f);
                treeView1.Nodes.Add(node);
            }
            
        }
        public class CodeColl : CSLight.ICLS_CodeCollection
        {
            public CodeColl()
            {
                this.rootPath = Application.StartupPath;
                var varf=System.IO.Directory.GetFiles("code", "*.cls.txt", System.IO.SearchOption.AllDirectories);
                files = new List<string>(varf);
            }
            public string rootPath
            {
                get;
                private set;
            }
            List<string> files = new List<string>();
            public IList<string> listCode
            {
                get { return files.AsReadOnly(); }
            }

            public string getCode(string name)
            {
                return System.IO.File.ReadAllText(name);
            }

            public bool SaveCode(string name, string code)
            {
                return false;
            }

            public bool newCode(string name)
            {
                return false;
            }
        }
        CodeColl codeInFolder;

        public void Log(string str)
        {
            _Log(str);

        }

        public void Log_Warn(string str)
        {
            _Log("<W>" + str);
        }

        public void Log_Error(string str)
        {
            _Log("<E>" + str);
        }
        void _Log(String str)
        {
            var now =DateTime.Now;
            this.listBox1.Items.Insert(0, now.ToString("hhmmss") + ":"+ str);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.textBox1.Text=    codeInFolder.getCode(e.Node.Text);
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bInit)
            {
                scriptEnv.ExecuteCode(this.codeInFolder.getCode("code\\update.cls.txt"));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scriptEnv.ExecuteCode(this.codeInFolder.getCode("code\\once.cls.txt"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            debugger.Show();
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            moniterQuit = null;
            GC.Collect();
        }

    }
}
