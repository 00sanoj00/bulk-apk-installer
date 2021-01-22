

using MaterialSkin.Controls;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using WK.Libraries.BetterFolderBrowserNS;

namespace bulk_apk_installer
{
    public partial class Form1 : MaterialForm
    {

        StreamWriter stdin = null;
        String path = null;
        String formname = "Bulk APK Installer";
        String pathwithot = "";
        int install_count = 0;
        
        public Form1()
        {
            InitializeComponent();
            this.Text = formname;

        }

        public void installadb()
        {
            AdbServer server = new AdbServer();
            var result = server.StartServer("adb.exe", restartServerIfNewer: false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            installadb();
            loadtext();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          //                 
            
        }


        private void StartCmdProcess()
        {
            ProcessStartInfo pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "start /WAIT",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,

            };

            Process cmdProcess = new Process
            {

                StartInfo = pStartInfo,
                EnableRaisingEvents = true,
            };

            cmdProcess.Start();
            materialMultiLineTextBox1.Text = string.Empty;
            cmdProcess.BeginErrorReadLine();
            cmdProcess.BeginOutputReadLine();
            stdin = cmdProcess.StandardInput;

            cmdProcess.OutputDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        materialMultiLineTextBox1.AppendText(evt.Data + Environment.NewLine);
                        materialMultiLineTextBox1.ScrollToCaret();
                    }));
                }
            };

            cmdProcess.ErrorDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    BeginInvoke(new Action(() =>
                    {
                        //rtbStdErr.AppendText(evt.Data + Environment.NewLine);
                        //rtbStdErr.ScrollToCaret();
                    }));
                }
            };

            cmdProcess.Exited += (s, evt) =>
            {
                // cmdProcess?.Dispose();
            };
        }

        public void displaydevice()
        {
            var devices = AdbClient.Instance.GetDevices();
            foreach (var device in devices)
            {
                if (device.Model == "")
                {
                    label1.Text = "No Device has connected to this PC";
                }
                else
                {
                    label1.Text = $"The device " + device.Model + " has connected to this PC";
                }   
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string a = label1.Text;
            string b = textBox1.Text;

            if (a.Equals("No Device has connected to this PC"))
            {
                MessageBox.Show("No Device has connected to this PC");
            }else
            {
                if (b.Equals(string.Empty))
                {
                    MessageBox.Show("Please select the folder containing the apk file");
                }
                else
                {
                    string installcom = "for %e in (" + path + "*.apk) do adb install -r \"%e\"";
                    StartCmdProcess();
                    
                    try
                    {
                        stdin.Write("\u0040echo off" + Environment.NewLine);
                        stdin.Write(installcom + Environment.NewLine);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        
                    }
                    
                    
                    
                }
                
            }

            
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            install_count = 0;
            BetterFolderBrowser bfb = new BetterFolderBrowser();
            bfb.Title = "Choose any folder as the root folder...";

            if (bfb.ShowDialog() == DialogResult.OK)
                textBox1.Text = bfb.SelectedPath;

            path = bfb.SelectedPath+ "\u005C";
            pathwithot = bfb.SelectedPath;





        }

        private void materialMultiLineTextBox1_TextChanged(object sender, EventArgs e)
        {

            try
            {

                replace();

                int maxLines = materialMultiLineTextBox1.Lines.Length;
                string lastLine = materialMultiLineTextBox1.Lines[maxLines - 2];
                string lastWord = lastLine.Split(' ').Last();

                if (lastLine == "Success")
                {
                    install_count++;
                    progresstext();
                }
            }
            catch
            {

            }
            
        

        }

        private void button3_Click(object sender, EventArgs e)
        {

            

            displaydevice();
            string a = label1.Text;

            if (a == "No Device has connected to this PC")
            {
            }
            else
            {
                label1.ForeColor = System.Drawing.Color.Blue;
            }
        }
        void progresstext()
        {
            materialMultiLineTextBox1.AppendText("-------------------------------" + Environment.NewLine+ "--Build APK Installer--" + Environment.NewLine+ "--   Installed  "+install_count+" apk   --" + Environment.NewLine + "--------------------------------" + Environment.NewLine);
        }
        void loadtext()
        {
            materialMultiLineTextBox1.AppendText("..... (¯`v´¯)♥" + Environment.NewLine + ".......•.¸.•´" + Environment.NewLine + "....¸.•´" + Environment.NewLine + "... (" + Environment.NewLine + "☻/" + Environment.NewLine + "/▌♥♥" + Environment.NewLine + "/ \u005C ♥♥" + Environment.NewLine );
            materialMultiLineTextBox1.AppendText("-------------------------------------------------------------" + Environment.NewLine);
            materialMultiLineTextBox1.AppendText("Bulk APK Installer" + Environment.NewLine);
            materialMultiLineTextBox1.AppendText("By Sanoj Prabatb Jayathilaka" + Environment.NewLine);
            materialMultiLineTextBox1.AppendText("Github - github.com/00sanoj00" + Environment.NewLine);
            materialMultiLineTextBox1.AppendText("Whatsapp - +94716474696");
            materialMultiLineTextBox1.AppendText("Facebook - fb.com/sanoj.jayathilaka1" + Environment.NewLine);
            materialMultiLineTextBox1.AppendText("-------------------------------------------------------------" + Environment.NewLine);

        }
        void replace()
        {
            String Directory = System.Windows.Forms.Application.StartupPath;
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace(Directory, "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace(">", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("for %e in", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("%", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("(", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace(path, "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("*.apk)", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("do", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("adb install", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("@echo", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("off", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("-r", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("\"%e\"", "");
        }

    }
}
