using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.Utilities;

namespace WinFormSetParentTest
{
    public partial class Form1 : Form
    {

        private readonly Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Interval = 300;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            var sb = new StringBuilder();

            void PrintDpiAwareness(Control control)
            {
                sb.AppendFormat("{0} DPI Awareness: {1}", control.Name, Utility.GetAwarenessFromDpiAwarenessContext(control.Handle.GetDpiAwarenessContext()));
                sb.AppendLine();
            }

            void PrintParent(Control control)
            {
                sb.AppendFormat("{0} managed parent: {1}", control.Name, control.Parent);
                sb.AppendLine();
                var pp = Utility.GetParent(control.Handle);
                sb.AppendFormat("{0} GetParent(): {1:X} ({2})", control.Name, pp, Utility.TextFromHandle(pp));
                sb.AppendLine();
            }

            sb.AppendLine("Process DPI Awareness: " + DpiAwareness.ProcessDpiAwarenessContext);
            PrintDpiAwareness(this);
            PrintDpiAwareness(textBox1);
            var oldAwareness = Utility.SetThreadDpiAwarenessContext(new IntPtr((int)DpiAwarenessContext.SystemAware));
            Trace.Assert(oldAwareness != IntPtr.Zero);
            FlowLayoutPanel panel1;
            try
            {
                panel1 = new FlowLayoutPanel
                {
                    Name = "panel1",
                    BackColor = Color.Blue,
                    Location = new Point(20, 20),
                    Size = new Size(300, 300),
                    BorderStyle = BorderStyle.FixedSingle
                };
                panel1.CreateControl();
            }
            finally
            {
                Utility.SetThreadDpiAwarenessContext(oldAwareness);
            }
            PrintDpiAwareness(panel1);
            PrintParent(panel1);
            sb.AppendLine("Set panel1 parent.");
            this.Controls.Add(panel1);
            PrintParent(panel1);
            textBox1.Text = sb.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

    }
}
