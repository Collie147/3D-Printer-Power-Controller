using System;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
namespace T84ComsInterface
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.clientModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.printerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerIPToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            this.printersToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.printerPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerPortToolStripMenuItem1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.aPIKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aPIKEYToolStripMenuItem1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.connectToPrinterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.faultDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noMotionShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShutdownMenuText = new System.Windows.Forms.ToolStripMenuItem();
            this.NoMotionTime = new System.Windows.Forms.ToolStripTextBox();
            this.motionDetectionDelaysecondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MotionDelay = new System.Windows.Forms.ToolStripTextBox();
            this.smokeDetectionShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thermalShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownTemperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShutdownTemp = new System.Windows.Forms.ToolStripTextBox();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.deviceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(385, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitWindowToolStripMenuItem,
            this.quitApplicationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitWindowToolStripMenuItem
            // 
            this.exitWindowToolStripMenuItem.Name = "exitWindowToolStripMenuItem";
            this.exitWindowToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.exitWindowToolStripMenuItem.Text = "Exit Window";
            this.exitWindowToolStripMenuItem.Click += new System.EventHandler(this.exitWindowToolStripMenuItem_Click);
            // 
            // quitApplicationToolStripMenuItem
            // 
            this.quitApplicationToolStripMenuItem.Name = "quitApplicationToolStripMenuItem";
            this.quitApplicationToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.quitApplicationToolStripMenuItem.Text = "Quit Application";
            this.quitApplicationToolStripMenuItem.Click += new System.EventHandler(this.quitApplicationToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.clientModeToolStripMenuItem,
            this.printerSettingsToolStripMenuItem,
            this.faultDetectionToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripTextBox2});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem1.Text = "Server Mode";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem3.Text = "Server Port";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox2.Text = "System.CodeDom.CodePropertyReferenceExpression";
            // 
            // clientModeToolStripMenuItem
            // 
            this.clientModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripTextBox1,
            this.toolStripSeparator2,
            this.toolStripMenuItem4,
            this.toolStripTextBox3});
            this.clientModeToolStripMenuItem.Name = "clientModeToolStripMenuItem";
            this.clientModeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.clientModeToolStripMenuItem.Text = "Client Mode";
            this.clientModeToolStripMenuItem.Click += new System.EventHandler(this.clientModeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem2.Text = "Server IP Address";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem4.Text = "Client Port";
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox3.Text = "System.CodeDom.CodePropertyReferenceExpression";
            // 
            // printerSettingsToolStripMenuItem
            // 
            this.printerSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printerAddressToolStripMenuItem,
            this.printerIPToolStripMenuItem,
            this.printersToolStripMenuItem,
            this.printerPortToolStripMenuItem,
            this.printerPortToolStripMenuItem1,
            this.toolStripSeparator3,
            this.aPIKeyToolStripMenuItem,
            this.aPIKEYToolStripMenuItem1,
            this.toolStripSeparator1,
            this.connectToPrinterToolStripMenuItem});
            this.printerSettingsToolStripMenuItem.Name = "printerSettingsToolStripMenuItem";
            this.printerSettingsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.printerSettingsToolStripMenuItem.Text = "Printer Settings";
            // 
            // printerAddressToolStripMenuItem
            // 
            this.printerAddressToolStripMenuItem.Name = "printerAddressToolStripMenuItem";
            this.printerAddressToolStripMenuItem.Size = new System.Drawing.Size(334, 22);
            this.printerAddressToolStripMenuItem.Text = "Printer Address";
            // 
            // printerIPToolStripMenuItem
            // 
            this.printerIPToolStripMenuItem.Name = "printerIPToolStripMenuItem";
            this.printerIPToolStripMenuItem.Size = new System.Drawing.Size(154, 23);
            // 
            // printersToolStripMenuItem
            // 
            this.printersToolStripMenuItem.Name = "printersToolStripMenuItem";
            this.printersToolStripMenuItem.Size = new System.Drawing.Size(331, 6);
            // 
            // printerPortToolStripMenuItem
            // 
            this.printerPortToolStripMenuItem.Name = "printerPortToolStripMenuItem";
            this.printerPortToolStripMenuItem.Size = new System.Drawing.Size(334, 22);
            this.printerPortToolStripMenuItem.Text = "Printer Port";
            // 
            // printerPortToolStripMenuItem1
            // 
            this.printerPortToolStripMenuItem1.Name = "printerPortToolStripMenuItem1";
            this.printerPortToolStripMenuItem1.Size = new System.Drawing.Size(214, 23);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(331, 6);
            // 
            // aPIKeyToolStripMenuItem
            // 
            this.aPIKeyToolStripMenuItem.Name = "aPIKeyToolStripMenuItem";
            this.aPIKeyToolStripMenuItem.Size = new System.Drawing.Size(334, 22);
            this.aPIKeyToolStripMenuItem.Text = "API Key";
            // 
            // aPIKEYToolStripMenuItem1
            // 
            this.aPIKEYToolStripMenuItem1.Name = "aPIKEYToolStripMenuItem1";
            this.aPIKEYToolStripMenuItem1.Size = new System.Drawing.Size(274, 23);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(331, 6);
            // 
            // connectToPrinterToolStripMenuItem
            // 
            this.connectToPrinterToolStripMenuItem.Name = "connectToPrinterToolStripMenuItem";
            this.connectToPrinterToolStripMenuItem.Size = new System.Drawing.Size(334, 22);
            this.connectToPrinterToolStripMenuItem.Text = "Connect to Printer";
            // 
            // faultDetectionToolStripMenuItem
            // 
            this.faultDetectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noMotionShutdownToolStripMenuItem,
            this.smokeDetectionShutdownToolStripMenuItem,
            this.thermalShutdownToolStripMenuItem});
            this.faultDetectionToolStripMenuItem.Name = "faultDetectionToolStripMenuItem";
            this.faultDetectionToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.faultDetectionToolStripMenuItem.Text = "Fault Detection";
            // 
            // noMotionShutdownToolStripMenuItem
            // 
            this.noMotionShutdownToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShutdownMenuText,
            this.NoMotionTime,
            this.motionDetectionDelaysecondsToolStripMenuItem,
            this.MotionDelay});
            this.noMotionShutdownToolStripMenuItem.Name = "noMotionShutdownToolStripMenuItem";
            this.noMotionShutdownToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.noMotionShutdownToolStripMenuItem.Text = "No Motion Shutdown";
            // 
            // ShutdownMenuText
            // 
            this.ShutdownMenuText.Name = "ShutdownMenuText";
            this.ShutdownMenuText.Size = new System.Drawing.Size(321, 22);
            this.ShutdownMenuText.Text = "Shutdown if No Motion Detected for (seconds)";
            // 
            // NoMotionTime
            // 
            this.NoMotionTime.Name = "NoMotionTime";
            this.NoMotionTime.Size = new System.Drawing.Size(100, 23);
            this.NoMotionTime.Text = "System.CodeDom.CodePropertyReferenceExpression";
            // 
            // motionDetectionDelaysecondsToolStripMenuItem
            // 
            this.motionDetectionDelaysecondsToolStripMenuItem.Name = "motionDetectionDelaysecondsToolStripMenuItem";
            this.motionDetectionDelaysecondsToolStripMenuItem.Size = new System.Drawing.Size(321, 22);
            this.motionDetectionDelaysecondsToolStripMenuItem.Text = "Motion Detection Delay (seconds)";
            // 
            // MotionDelay
            // 
            this.MotionDelay.Name = "MotionDelay";
            this.MotionDelay.Size = new System.Drawing.Size(100, 23);
            this.MotionDelay.Text = "System.CodeDom.CodePropertyReferenceExpression";
            // 
            // smokeDetectionShutdownToolStripMenuItem
            // 
            this.smokeDetectionShutdownToolStripMenuItem.Name = "smokeDetectionShutdownToolStripMenuItem";
            this.smokeDetectionShutdownToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.smokeDetectionShutdownToolStripMenuItem.Text = "Smoke Detection Shutdown";
            // 
            // thermalShutdownToolStripMenuItem
            // 
            this.thermalShutdownToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shutdownTemperatureToolStripMenuItem,
            this.ShutdownTemp});
            this.thermalShutdownToolStripMenuItem.Name = "thermalShutdownToolStripMenuItem";
            this.thermalShutdownToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.thermalShutdownToolStripMenuItem.Text = "Thermal Shutdown";
            // 
            // shutdownTemperatureToolStripMenuItem
            // 
            this.shutdownTemperatureToolStripMenuItem.Name = "shutdownTemperatureToolStripMenuItem";
            this.shutdownTemperatureToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.shutdownTemperatureToolStripMenuItem.Text = "Shutdown Temperature (°C)";
            // 
            // ShutdownTemp
            // 
            this.ShutdownTemp.Name = "ShutdownTemp";
            this.ShutdownTemp.Size = new System.Drawing.Size(100, 23);
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "Device";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(98, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(199, 106);
            this.button1.TabIndex = 1;
            this.button1.Text = "Printer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(105, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Click below to enable/disable Printer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label2.Location = new System.Drawing.Point(154, 299);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Status Indicator";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server/Client Mode";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(154, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Server/Client Status";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Temp:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(154, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Printer Name";
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Printer Idle";
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(145, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Printing Complete";
            this.label8.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 326);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "3D Printer Power";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MotionDelay_LostFocus(object sender, EventArgs e)
        {
            Form1.motionStartDelay = Convert.ToInt32(MotionDelay.Text);
            Form1.ModifyINIData("motionStartDelay", Form1.motionStartDelay.ToString());
            if (ClientMode)
                Form1.TCPConnect(serverAddr, TCPport, "motionStartDelay=" + MotionDelay);
        }

        private void NoMotionTime_LostFocus(object sender, EventArgs e)
        {
            Form1.motionShutdownTime = Convert.ToInt32(NoMotionTime.Text);
            Form1.ModifyINIData("motionShutdownTime", Form1.motionShutdownTime.ToString());
            if (ClientMode)
                Form1.TCPConnect(serverAddr, TCPport, "motionShutdownTime=" + NoMotionTime);
        }

        private void SmokeDetectionShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.smokeDetectEnabled = !Form1.smokeDetectEnabled;
            smokeDetectionShutdownToolStripMenuItem.Checked = Form1.smokeDetectEnabled;
            Form1.ModifyINIData("smokeDetectEnabled", Form1.smokeDetectEnabled.ToString());
            if (ClientMode)
                Form1.TCPConnect(serverAddr, TCPport, "smokeShutdownEnabled=" + smokeDetectEnabled);
        }

        private void NoMotionShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.motionDetectEnabled = !Form1.motionDetectEnabled;
            noMotionShutdownToolStripMenuItem.Checked = Form1.motionDetectEnabled;
            Form1.ModifyINIData("motionDetectEnabled", Form1.motionDetectEnabled.ToString());
            if (ClientMode)
                Form1.TCPConnect(serverAddr, TCPport, "motionShutdownEnabled=" + motionDetectEnabled);
        }

        private void ConnectToPrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printerMenu.Items.Clear();//clear the context menu 
            Form1.createPrinterItems();
        }

        private void PrinterPortToolStripMenuItem1_LostFocus(object sender, EventArgs e)
        {
            Form1.printerPort = printerPortToolStripMenuItem1.Text;
            Form1.ModifyINIData("printerPort", Form1.printerPort.ToString());
        }

        private void PrinterIPToolStripMenuItem_LostFocus(object sender, EventArgs e)
        {
            Form1.printerIP = printerIPToolStripMenuItem.Text;
            Form1.ModifyINIData("printerIP", Form1.printerIP.ToString());
            Debug.WriteLine("PrinterIP = " + printerIP);
        }

        private void APIKEYToolStripMenuItem1_LostFocus(object sender, EventArgs e)
        {
            Form1.apikey = aPIKEYToolStripMenuItem1.Text;
            Form1.ModifyINIData("apikey", Form1.apikey.ToString());
        }

        private void DeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form1.menu.Items.Clear();//clear the context menu 
            // Form1.CreateMenuItems();
        }
        private void ThermalShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.tempDetectEnabled = !Form1.tempDetectEnabled;
            thermalShutdownToolStripMenuItem.Checked = Form1.tempDetectEnabled;
            Form1.ModifyINIData("tempDetectEnabled", Form1.tempDetectEnabled.ToString());
            if (ClientMode)
                Form1.TCPConnect(serverAddr, TCPport, "tempShutdownEnabled=" + tempDetectEnabled);
        }

        private void ToolStripTextBox2_LostFocus(object sender, EventArgs e)
        {
            Form1.TCPport = Convert.ToInt32(toolStripTextBox2.Text);
            System.Diagnostics.Debug.WriteLine(TCPport);
            Form1.ModifyINIData("port", Form1.TCPport.ToString());
        }

        private void ToolStripTextBox3_LostFocus(object sender, System.EventArgs e)
        {
            Form1.TCPport = Convert.ToInt32(toolStripTextBox3.Text);
            System.Diagnostics.Debug.WriteLine(TCPport);
            Form1.ModifyINIData("port", Form1.TCPport.ToString());
        }

        private void ToolStripTextBox1_LostFocus(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("LostFocus");
            Form1.serverAddr = toolStripTextBox1.Text;
            System.Diagnostics.Debug.WriteLine(serverAddr);
            Form1.ModifyINIData("serverAddress", Form1.serverAddr);
        }


        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitApplicationToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clientModeToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripTextBox toolStripTextBox2;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripTextBox toolStripTextBox3;
        private Label label5;
        private ToolStripMenuItem deviceToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem printerSettingsToolStripMenuItem;
        private ToolStripMenuItem printerAddressToolStripMenuItem;
        private ToolStripTextBox printerIPToolStripMenuItem;
        private ToolStripSeparator printersToolStripMenuItem;
        private ToolStripMenuItem printerPortToolStripMenuItem;
        private ToolStripTextBox printerPortToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem connectToPrinterToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem aPIKeyToolStripMenuItem;
        private ToolStripTextBox aPIKEYToolStripMenuItem1;
        private Label label6;
        private Label label7;
        private Label label8;
        private ToolStripMenuItem faultDetectionToolStripMenuItem;
        private ToolStripMenuItem noMotionShutdownToolStripMenuItem;
        private ToolStripMenuItem ShutdownMenuText;
        private ToolStripTextBox NoMotionTime;
        private ToolStripMenuItem smokeDetectionShutdownToolStripMenuItem;
        private ToolStripMenuItem motionDetectionDelaysecondsToolStripMenuItem;
        private ToolStripTextBox MotionDelay;
        private ToolStripMenuItem thermalShutdownToolStripMenuItem;
        private ToolStripMenuItem shutdownTemperatureToolStripMenuItem;
        private ToolStripTextBox ShutdownTemp;
    }
}

