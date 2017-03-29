using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using IniParser;
using IniParser.Model;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using T84ComsInterface.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace T84ComsInterface
{

    public partial class Form1 : Form
    {
        private static bool printerItemsCreated = false;
        private static bool newPrint = true;
        private static bool isPrinting = false;
        private static bool isMoving = false;
        private static bool isSmoking = false;
        private static bool isThermal = false;
        private static bool updateFaultStatus = false;
        private static bool motionTimerDelayComplete = false;
        private static bool motionDetectEnabled = Convert.ToBoolean(ReadINIData("motionDetectEnabled"));
        private static bool smokeDetectEnabled = Convert.ToBoolean(ReadINIData("smokeDetectEnabled"));
        private static bool tempDetectEnabled = Convert.ToBoolean(ReadINIData("tempDetectEnabled"));
        private static int motionStartDelay = Convert.ToInt32(ReadINIData("motionStartDelay"));
        private static int motionShutdownTime = Convert.ToInt32(ReadINIData("motionShutdownTime"));
        private static int shutdownTemp = Convert.ToInt32(ReadINIData("shutdownTemp"));
        private static string printJob = string.Empty;
        private static string printerPort = ReadINIData("printerPort");
        private static string printerIP = ReadINIData("printerIP");
        private static string apikey = ReadINIData("apikey");
        private static int printerNumSelected = Convert.ToInt32(ReadINIData("printerNum"));
        private static IEnumerable<string> printerNames;
        private static IEnumerable<string> printerSlugs;
        private static string printerNameSelected = ReadINIData("printerName");
        private static string printerSlugSelected = ReadINIData("printerSlug");
        private static string printCompleteString = string.Empty;
        private static decimal printComplete;
        private static bool ServerMode = Convert.ToBoolean(ReadINIData("serverMode"));
        private static bool ClientMode = Convert.ToBoolean(ReadINIData("clientMode"));
        private static TcpListener server = null;
        private static bool ListenToTCP = false;
        private static bool getPrinterStatus = false;
        private static bool TCPConnected = false;
        private static float temp = 0;
        private static Int32 TCPport = Convert.ToInt32(ReadINIData("port"));
        private static IPAddress localAddr = IPAddress.Parse(getLocalIPAddress());
        private static String serverAddr = ReadINIData("serverAddress");
        private static Thread TCPListenThread = new Thread(ListenTCP);
        private static Thread printerStatusThread = new Thread(checkPrinterStatus);
        private static SerialPort mySerialPort;
        private static string SPortVid_Pid = string.Empty;             //VID and PID string for the Com port in manual selection mode
        private static Int16 PortInstruction = 0;
        NotifyIcon T84ApplicationIcon;
        Icon T84IconOn;
        Icon T84IconOff;
        Icon T84Icon0;
        Icon T84Icon1;
        Icon T84Icon2;
        Icon T84Icon2x;
        Icon T84Icon2On;
        Icon T84Icon2Off;
        private static bool isAttached = false;
        private static bool isConnected = false;
        private static bool manualIsAttached = false;              // boolean to denote whether the manual device is attached to the computer
        private static string portSelected = string.Empty;         // string to store the selected port in Manual mode
        private static bool AutomaticPortSelect = true;            // boolean to denote whether the program has been set to Automatic connection mode
        private static bool ManualPortSelect = false;           //boolean to denote whether the program has been set to manual connection mode
                                                                //boolean to denote whether the default device is connected and port opened
        private System.Windows.Forms.Timer connectionTimer1 = new System.Windows.Forms.Timer(); //creates a timer to periodically check if the T32 device has been connected (once connected this stops)
        private System.Windows.Forms.Timer connectionTimer2 = new System.Windows.Forms.Timer(); //creates a timer to poll the devices status
        private System.Windows.Forms.Timer noMotionTimer = new System.Windows.Forms.Timer(); // creates a timer when no motion is detected.
        private static System.Windows.Forms.Timer noMotionTimerDelay = new System.Windows.Forms.Timer(); // creates a timer to delay before starting the no motion detection timer
        public static bool switchEnabled = false;
        // private const int VendorID = 0x16d0;//0x03EB;//0x16d0;
        // private const int ProductID = 0x087e;//0x206A;//0x087e;
        private static string VID = Convert.ToString(ReadINIData("VendorID"));                     //set the device VID, this will be the default one to connect to (sparkfun pro micro)
        private static string PID = Convert.ToString(ReadINIData("ProductID"));                     //set the device PID, this will be the default one to connect to (sparkfun pro micro)        
        private static string devID = Convert.ToString(ReadINIData("DeviceID"));//string.Empty;             // this is a device ID string to contain the exact device ID
        private static string Vid_Pid = "VID_" + VID + "&PID_" + PID;
        public static ContextMenuStrip deviceMenu = new ContextMenuStrip(); //create a menu strip for the devices 
        public static ContextMenuStrip printerMenu = new ContextMenuStrip(); //create a menu strip for the printers

        private static bool NonTaskbarClose = false;
        public Form1()
        {
            DoubleBuffered = true;
            InitializeComponent();

            //ToolStripMenuItems
            this.toolStripMenuItem1.Checked = Form1.ServerMode;
            this.toolStripTextBox2.Text = Form1.TCPport.ToString();
            this.toolStripTextBox2.LostFocus += ToolStripTextBox2_LostFocus;
            this.clientModeToolStripMenuItem.Checked = Form1.ClientMode;
            this.toolStripTextBox1.Text = Form1.serverAddr;
            this.toolStripTextBox1.LostFocus += ToolStripTextBox1_LostFocus;
            this.toolStripTextBox3.Text = Form1.TCPport.ToString();
            this.toolStripTextBox3.LostFocus += ToolStripTextBox3_LostFocus;
            this.printerIPToolStripMenuItem.Text = Form1.printerIP;
            this.printerIPToolStripMenuItem.LostFocus += PrinterIPToolStripMenuItem_LostFocus;
            this.printerPortToolStripMenuItem1.Text = Form1.printerPort;
            this.printerPortToolStripMenuItem1.LostFocus += PrinterPortToolStripMenuItem1_LostFocus;
            this.aPIKEYToolStripMenuItem1.Text = Form1.apikey;
            this.aPIKEYToolStripMenuItem1.LostFocus += APIKEYToolStripMenuItem1_LostFocus;
            this.noMotionShutdownToolStripMenuItem.Checked = Form1.motionDetectEnabled;
            this.noMotionShutdownToolStripMenuItem.Click += NoMotionShutdownToolStripMenuItem_Click;
            this.NoMotionTime.Text = Form1.motionShutdownTime.ToString();
            this.NoMotionTime.LostFocus += NoMotionTime_LostFocus;
            this.MotionDelay.Text = Form1.motionStartDelay.ToString();
            this.MotionDelay.LostFocus += MotionDelay_LostFocus;
            this.smokeDetectionShutdownToolStripMenuItem.Checked = Form1.smokeDetectEnabled;
            this.smokeDetectionShutdownToolStripMenuItem.Click += SmokeDetectionShutdownToolStripMenuItem_Click;
            this.deviceToolStripMenuItem.DropDown = Form1.deviceMenu;
            this.thermalShutdownToolStripMenuItem.Checked = Form1.tempDetectEnabled;
            this.thermalShutdownToolStripMenuItem.Click += ThermalShutdownToolStripMenuItem_Click;
            this.ShutdownTemp.Text = Form1.shutdownTemp.ToString();
            this.ShutdownTemp.LostFocus += ShutdownTemp_LostFocus;

            T84IconOn = new Icon("_3dPrinterIconOn.ico");
            T84IconOff = new Icon("_3dPrinterIconOff.ico");
            T84Icon0 = new Icon("_3dPrinterIcon0.ico");
            T84Icon1 = new Icon("_3dPrinterIcon1.ico");
            T84Icon2 = new Icon("_3dPrinterIcon2.ico");
            T84Icon2x = new Icon("_3dPrinterIcon2x.ico");
            T84Icon2On = new Icon("_3dPrinterIcon2On.ico");
            T84Icon2Off = new Icon("_3dPrinterIcon2Off.ico");
            T84ApplicationIcon = new NotifyIcon();
            T84ApplicationIcon.Icon = T84Icon1;
            T84ApplicationIcon.Visible = true;
            T84ApplicationIcon.BalloonTipIcon = ToolTipIcon.Info;
            T84ApplicationIcon.BalloonTipText = "Printer Controller";
            T84ApplicationIcon.BalloonTipTitle = "Printer Controller";
            this.Icon = T84Icon2;

            MenuItem quitMenuItem = new MenuItem("Quit");
            MenuItem controlPrinterMenuItem = new MenuItem("Printer Power Control");
            MenuItem aboutMenuItem = new MenuItem("About");
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(controlPrinterMenuItem);
            contextMenu.MenuItems.Add(aboutMenuItem);
            contextMenu.MenuItems.Add(quitMenuItem);
            T84ApplicationIcon.ContextMenu = contextMenu;
            T84ApplicationIcon.MouseUp += T84ApplicationIcon_MouseUp;
            T84ApplicationIcon.DoubleClick += T84ApplicationIcon_DoubleClick;
            controlPrinterMenuItem.Click += ControlPrinterMenuItem_Click;
            aboutMenuItem.Click += AboutMenuItem_Click;
            quitMenuItem.Click += QuitMenuItem_Click;

            this.WindowState = FormWindowState.Minimized;  //start minimized
            this.ShowInTaskbar = false; // dont show icon on the taskbar
            this.Hide(); //Hide

            this.FormBorderStyle = FormBorderStyle.FixedDialog;//gets rid of the standard border (with scroll bar etc.)
            //fixes the form1 size
            this.MaximizeBox = false; //disables maximize
            //this.MinimizeBox = false; //disables minimize 

            this.StartPosition = FormStartPosition.CenterParent;//opens the box in the middle of the parent window

            connectionTimer1.Interval = 1000; //sets the connectionTimer to "tick" every 500milliseconds 
            connectionTimer1.Tick += connectionTimer1_Tick; //event manager for each "tick"
            connectionTimer1.Start();
            setupFromIni();
            UsbDeviceNotifier.RegisterUsbDeviceNotification(this.Handle); // handle a notifier for usb devices
            checkDevice();                                      // check if the device is already attached (handler only looks after devices added or removed)
            CreateMenuItems();
            //createPrinterItems();
            this.FormClosing += Form1_FormClosing;
             
        }

        private void ShutdownTemp_LostFocus(object sender, EventArgs e)
        {
            Form1.shutdownTemp = Convert.ToInt32(ShutdownTemp.Text);
            Form1.ModifyINIData("shutdownTemp", Form1.shutdownTemp.ToString());
            if (ClientMode)
                Form1.TCPConnect(serverAddr, TCPport, "shutdownTemp=" + ShutdownTemp.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && (NonTaskbarClose == false)) //if closed from menu close (signalled by NonTaskBarClose Boolean)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Hide();
            }
            else
            {

            }
        }
        private static void createPrinterItems()    
        {
            ToolStripMenuItem item;
            ToolStripSeparator sep;          
            int printerNum = 0;
            try
            {
                foreach (var printerName in printerNames)
                {
                    item = new ToolStripMenuItem();
                    item.Text = printerName;
                    if (printerName == printerNameSelected)
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                    item.Click += new EventHandler((sender, e) => Selected_Printer(sender, e, printerName, printerNum)); //add a new event handler when clicking that item to run the function Selected_Serial with the port 
                                                                                                                         //item.Image = 
                    printerMenu.Items.Add(item);          //add the item to the contextmenu menu
                    printerNum++;
                }
                sep = new ToolStripSeparator();   // instantiate a new toolstripseparator
                printerMenu.Items.Add(sep);              // at the separator to the contextmenu
                item = new ToolStripMenuItem();
                item.Text = "Refresh";
                item.Click += refresh_Click2;
                printerMenu.Items.Add(item);
            }
            catch { }

        }
        private static void refresh_Click2(object sender, EventArgs e)
        {
            printerMenu.Items.Clear();
            createPrinterItems();
        }
        public static void Selected_Printer(object sender, EventArgs e, string selected_printer, int selected_printerNum)
        {
            printerNameSelected = selected_printer;
            printerNumSelected = selected_printerNum;
            printerSlugSelected = printerSlugs.ElementAt(printerNumSelected-1);
            ModifyINIData("printerSlug", printerSlugSelected);
            ModifyINIData("printerName", printerNameSelected);
            ModifyINIData("printerNum", printerNumSelected.ToString());
            printerMenu.Items.Clear();
            createPrinterItems();
        }
        private static void CreateMenuItems()
        {
            ToolStripMenuItem item;             // create an instance of a toolStripMenuItem called item
            ToolStripSeparator sep;             // create an instance of a toolStripMenuSeparator called sep
            String AutoText = String.Empty;
            item = new ToolStripMenuItem();     // instantiate item as a new toolstripmenu item
            if ((AutomaticPortSelect) && (mySerialPort != null))
            {
                Debug.WriteLine("AutoMaticModePortName:" + mySerialPort.PortName);
                String AutoDeviceName = getDeviceName(mySerialPort.PortName.ToString());//change the menu item to the device name
                AutoText = "Automatic | " + AutoDeviceName + " (" + mySerialPort.PortName.ToString() + ")";
                Debug.WriteLine("AutoText:" + AutoText);
            }
            else
                AutoText = "Automatic Mode";
            item.Text = AutoText;       // give it the text Automatic Mode
            if (AutomaticPortSelect)
                item.Checked = true;            // if the boolean is set, check the item
            else
                item.Checked = false;           // else set it as false
            item.Click += Item_Click;           // add a handler for clicking the menu item
            deviceMenu.Items.Add(item);               // add it to the menu

            item = new ToolStripMenuItem();     // instantiate item as a new toolstripmenuitem
            item.Text = "Serial Ports";         // set items text to "serial ports"
            deviceMenu.Items.Add(item);               // add item to the contextmenu menu

            string[] ports = SerialPort.GetPortNames(); // set up a string array called ports and set it to the list of port names

            foreach (string port in ports) // for each string in the array
            {
                item = new ToolStripMenuItem();// instantiate a new toolstripmenuitem
                string deviceName = getDeviceName(port);            // set the device's name to the function (passing the port name)
                item.Text = port + " | " + deviceName;              // set the text to the string, which is the port name   
                if (port == portSelected)
                    item.Checked = true;                            // if the port is the port that was selected, mark it as checked
                else
                    item.Checked = false;
                item.Click += new EventHandler((sender, e) => Selected_Serial(sender, e, port)); //add a new event handler when clicking that item to run the function Selected_Serial with the port
                item.Image = Resources.Serial; //set the image for the item as the Serial image from the resources section
                deviceMenu.Items.Add(item);          //add the item to the contextmenu menu
            }
            sep = new ToolStripSeparator();   // instantiate a new toolstripseparator
            deviceMenu.Items.Add(sep);              // at the separator to the contextmenu
            item = new ToolStripMenuItem();   // instantiate a new toolstripmenuitem
            item.Text = "Refresh";            // set the text as refresh
            item.Click += refresh_Click;
            deviceMenu.Items.Add(item);             // add the item to the menu

        }
        private static void refresh_Click(object sender, EventArgs e)
        {
            deviceMenu.Items.Clear();//clear the context menu 
            CreateMenuItems();
        }
        private static void refresh_Menu()
        {
            Debug.WriteLine("Refreshing Menu");
            deviceMenu.Items.Clear();//clear the context menu 
            CreateMenuItems();
        }
        private static void Item_Click(object sender, EventArgs e)// automatic port selection handler
        {

            ManualPortSelect = false; // mark manualPortSelect as false
            AutomaticPortSelect = true;//mark automaticportselect as true
            portSelected = string.Empty;//set the portSelected as blank
            try
            {
                if (mySerialPort.IsOpen)//try close the port first
                {
                    mySerialPort.Close();
                }
                if (manualIsAttached)
                {
                    isAttached = true;
                    manualIsAttached = false;
                    Vid_Pid = SPortVid_Pid;//save the vid and pid from the manual selection

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error closing mySerialPort " + ex);
            }
            refresh_Menu();
        }
        private void connectionTimer1_Tick(object sender, EventArgs e)
        {
            if (!ClientMode)
            {
                if (isAttached)
                    isConnected = ConnectToDevice();
                if (manualIsAttached)
                {
                    isConnected = true;
                }
                if (isConnected)
                {
                    // isAttached = true;
                    try
                    {
                        if (mySerialPort.IsOpen == false)
                        {
                            mySerialPort.Open();
                        }
                        if (PortInstruction > 0)
                        {
                            if (PortInstruction == 1)
                            {
                                mySerialPort.Write("1");
                                PortInstruction = 0;
                            }
                            else if (PortInstruction == 2)
                            {
                                mySerialPort.Write("0");
                                PortInstruction = 0;
                            }
                        }
                        mySerialPort.Write("i");
                        //Debug.WriteLine(mySerialPort.ReadLine().ToString());
                        string on_off = mySerialPort.ReadLine();
                        System.Diagnostics.Debug.WriteLine("ON Off Value = " + on_off);
                        if (on_off.StartsWith("0"))
                        {
                            switchEnabled = false;
                            System.Diagnostics.Debug.WriteLine(switchEnabled);
                        }
                        if (on_off.StartsWith("1"))
                        {
                            switchEnabled = true;
                            System.Diagnostics.Debug.WriteLine(switchEnabled);
                        }
                        mySerialPort.Write("t");
                        string tempString = mySerialPort.ReadLine();
                        if (tempString.StartsWith("T"))
                        {
                            int tempIndex = tempString.IndexOf("=") + 1;
                            tempString = tempString.Substring(tempIndex);
                            temp = float.Parse(tempString);
                            Debug.WriteLine("Temp Received = " + temp);
                            if ((tempDetectEnabled) && (temp > shutdownTemp))
                            {
                                shutdownPrinterError("Over Temperature Detected");
                            }
                        }
                        mySerialPort.Write("m");
                        string motionString = mySerialPort.ReadLine();
                        if (motionString.StartsWith("M"))
                        {
                            int motionIndex = motionString.IndexOf("=") + 1;
                            motionString = motionString.Substring(motionIndex, 1);
                            Debug.WriteLine("Motion Received = " + motionString);
                            if (motionString == "1")
                                isMoving = true;
                            else if (motionString == "0")
                                isMoving = false;
                        }
                        mySerialPort.Write("s");
                        string smokeString = mySerialPort.ReadLine();
                        if (smokeString.StartsWith("S"))
                        {
                            int smokeIndex = smokeString.IndexOf("=") + 1;
                            smokeString = smokeString.Substring(smokeIndex, 1);
                            Debug.WriteLine("Smoke Received = " + smokeString);
                            if (smokeString == "1")
                            {
                                isSmoking = true;
                                if (smokeDetectEnabled)
                                {
                                    shutdownPrinterError("Smoke Detected");
                                }
                            }
                            else if (smokeString == "0")
                                isSmoking = false;
                        }
                        if ((!isMoving) && (motionDetectEnabled) && (isPrinting) && (motionTimerDelayComplete))
                        {
                            noMotionTimer.Interval = motionShutdownTime;
                            noMotionTimer.Tick += NoMotionTimer_Tick;
                            noMotionTimer.Start();
                        }
                        else
                        {
                            if (noMotionTimer.Enabled == true)
                            {
                                noMotionTimer.Stop();
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        System.Diagnostics.Debug.Write("Error opening Port " + e1);
                        try
                        {
                            mySerialPort.Close();
                        }
                        catch
                        {
                            System.Diagnostics.Debug.Write("Error closing Port");
                        }
                    }
                }
                else
                {
                    isAttached = false;
                    //manualIsAttached = false;
                }
            }
            if (ClientMode)
            {
                string AttachedString = TCPConnect(serverAddr, TCPport, "attach");
                if (AttachedString == string.Empty)
                {
                    TCPConnected = false;
                    connectionTimer1.Interval = 3000;
                }
                else if (AttachedString != string.Empty)
                {
                    TCPConnected = true;
                    connectionTimer1.Interval = 500;
                }
                if (AttachedString == "True")
                {
                    isAttached = true;
                    string on_off = TCPConnect(serverAddr, 3323, "info");
                    if (on_off.StartsWith("False"))
                    {
                        switchEnabled = false;
                        //System.Diagnostics.Debug.WriteLine(switchEnabled);
                    }
                    if (on_off.StartsWith("True"))
                    {
                        switchEnabled = true;
                        // System.Diagnostics.Debug.WriteLine(switchEnabled);
                    }
                    string tempString = TCPConnect(serverAddr, TCPport, "temp");
                    Debug.WriteLine(tempString);
                    if ((tempString != string.Empty) || (tempString != null) || (tempString != ""))
                    {
                        float.TryParse(tempString, out temp);
                    }
                    string printerNumSelectedString = TCPConnect(serverAddr, TCPport, "printerNum");
                    
                    string isPrintingString = TCPConnect(serverAddr, TCPport, "isPrinting");
                    string printCompleteString = TCPConnect(serverAddr, TCPport, "printComplete");
                    string printName = TCPConnect(serverAddr, TCPport, "printerName");
                    if (printName != "NA")
                        printerNameSelected = printName;
                    string printJob1 = TCPConnect(serverAddr, TCPport, "printJob");
                    if (printJob1 != "NA")
                        printJob = printJob1;
                    Debug.WriteLine("isPrintingString = " + isPrintingString);
                    try
                    {
                        printerNumSelected = Convert.ToInt32(printerNumSelectedString);
                        isPrinting = Convert.ToBoolean(isPrintingString);
                        printComplete = Convert.ToDecimal(printCompleteString);
                    }
                    catch { }
                    
                }
                else
                {
                    isAttached = false;
                    connectionTimer1.Interval = 3000;
                }
            }

            updateLabel();
        }

        private void NoMotionTimer_Tick(object sender, EventArgs e)
        {
            shutdownPrinterError("No Motion Detected");
        }

        private void updateLabel()
        {
            MethodInvoker mi2 = delegate ()
            {
                if ((ServerMode == false) && (ClientMode == false))
                {
                    label3.Text = "";
                }
                else if (ServerMode == true)
                {
                    label3.Text = "Server Mode";
                    if (TCPConnected)
                    {
                        label4.Text = "Client Connected"; Debug.WriteLine(temp.ToString());
                    }
                    else
                    {
                        label4.Text = "Client unavailable";
                    }
                }
                else if (ClientMode == true)
                {
                    label3.Text = "Client Mode";
                    if (TCPConnected)
                    {
                        label4.Text = "Server Connected";
                    }
                    else
                    {
                        label4.Text = "Server unavailable";
                    }
                }

                if ((isAttached) || (manualIsAttached))
                {
                    label2.Text = "Controller Connected";
                    label2.BackColor = Color.Green;
                    label2.ForeColor = Color.WhiteSmoke;
                    button1.Enabled = true;
                    if (switchEnabled)
                    {
                        button1.Text = "ON";
                        button1.BackColor = Color.Green;
                        button1.ForeColor = Color.GhostWhite;
                        T84ApplicationIcon.Icon = T84IconOn;
                        this.Icon = T84Icon2On;
                    }
                    else
                    {
                        button1.Text = "OFF";
                        button1.BackColor = Color.Red;
                        button1.ForeColor = Color.Gray;
                        T84ApplicationIcon.Icon = T84IconOff;
                        this.Icon = T84Icon2Off;
                    }
                    string tempC = temp + " °C";
                    label5.Text = tempC;
                }
                else if ((isAttached == false) && (manualIsAttached == false))
                {
                    label2.Text = "Controller Not Connected!";
                    label2.BackColor = Color.Red;
                    label2.ForeColor = Color.Black;
                    button1.Enabled = false;
                    T84ApplicationIcon.Icon = T84Icon0;
                    this.Icon = T84Icon2x;
                }
                if (printerNumSelected > 0)
                {
                    label6.Text = printerNameSelected;
                    label6.Visible = true;
                    label6.Update();
                    label7.Visible = true;
                }
                else
                {
                    label6.Visible = false;
                    label6.Update();
                    label7.Visible = false;
                }
                if (isPrinting)
                {
                    label7.Text = "Printing: "+printJob;
                    label7.Update();
                    label8.Text = decimal.Round(printComplete, 2) + "% Completed";
                    label8.Visible = true;
                    label8.Update();
                }
                else
                {
                    label7.Text = "Printer Idle";
                    label7.Update();
                    label8.Text = "Print Completed";
                    label8.Visible = false;
                    label8.Update();
                }
                if (updateFaultStatus)
                {
                    this.MotionDelay.Text = motionStartDelay.ToString();
                    this.NoMotionTime.Text = motionShutdownTime.ToString();
                    this.ShutdownTemp.Text = shutdownTemp.ToString();
                    this.noMotionShutdownToolStripMenuItem.Checked = motionDetectEnabled;
                    this.smokeDetectionShutdownToolStripMenuItem.Checked = smokeDetectEnabled;
                    this.thermalShutdownToolStripMenuItem.Checked = tempDetectEnabled;
                    updateFaultStatus = false;
                }
                label2.Update();
            };
            this.Invoke(mi2);
        }
        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            connectionTimer1.Stop();
            ListenToTCP = false;
            ServerMode = false;
            ClientMode = false;
            getPrinterStatus = false;
            NonTaskbarClose = true; //declare this as a close that was initiated from actually selecting Quit
            if (TCPListenThread.ThreadState != System.Threading.ThreadState.Unstarted)
            {
                TCPListenThread.Interrupt();
            }
            if (printerStatusThread.ThreadState != System.Threading.ThreadState.Unstarted)
            {
                printerStatusThread.Interrupt();
            }
            T84ApplicationIcon.Dispose();
            T84Icon1.Dispose();
            T84Icon2.Dispose();
            T84Icon0.Dispose();
            T84Icon2x.Dispose();
            T84Icon2On.Dispose();
            T84Icon2Off.Dispose();
            T84IconOn.Dispose();
            T84IconOff.Dispose();

            //TCPListenThread.Abort();
            //printerStatusThread.Abort();
            

            try
            {
                mySerialPort.Close();
            }
            catch
            {
                System.Diagnostics.Debug.Write("Error closing Port");
            }


            this.Dispose();
            this.Close();
        }
        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1(); //create a new version of the AboutBox form (used as a template)
            aboutBox.Show(); //display it
        }
        private void ControlPrinterMenuItem_Click(object sender, EventArgs e)
        {
            refresh_Menu();
            this.Visible = true; // make the form visuble
            this.WindowState = FormWindowState.Normal; //open it
            this.ShowInTaskbar = true; // show icon in the taskbar
        }
        private void T84ApplicationIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //if its a left click
            {
                refresh_Menu();
                //MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic); // invoke a call to show the menu
                //mi.Invoke(T84ApplicationIcon, null);
                this.Visible = true; // make the form visuble
                this.WindowState = FormWindowState.Normal; //open it
                this.ShowInTaskbar = true; // show icon in the taskbaron();
                this.Activate();
            }
        }
        private void T84ApplicationIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true; // make the form visuble
            this.WindowState = FormWindowState.Normal; //open it
            this.ShowInTaskbar = true; // show icon in the taskbaron();
        }
        private void quitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectionTimer1.Stop();
            ListenToTCP = false;
            ServerMode = false;
            ClientMode = false;
            getPrinterStatus = false;
            NonTaskbarClose = true; //declare this as a close that was initiated from actually selecting Quit
            if (TCPListenThread.ThreadState != System.Threading.ThreadState.Unstarted)
            {
                TCPListenThread.Interrupt();
                server.Stop();
            }
            if (printerStatusThread.ThreadState != System.Threading.ThreadState.Unstarted)
            {
                printerStatusThread.Interrupt();
                //server.Stop();
            }

            T84ApplicationIcon.Dispose();  //Dispose of the icon so it's not left waiting after the application is closed
            T84Icon1.Dispose(); //dispose of the icon
            T84Icon2.Dispose(); //dispose of the icon
            T84Icon0.Dispose();
            T84Icon2x.Dispose();
            T84Icon2On.Dispose();
            T84Icon2Off.Dispose();
            T84IconOn.Dispose();
            T84IconOff.Dispose();

            //TCPListenThread.Abort();
            //printerStatusThread.Abort();

            

            try
            {
                mySerialPort.Close();
            }
            catch
            {
                System.Diagnostics.Debug.Write("Error closing Port");
            }
            this.Dispose();
            this.Close(); // close the form
        }
        private void exitWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((isAttached) || (manualIsAttached))
            {
                if (!ClientMode)
                {

                    if (mySerialPort.IsOpen)
                    {
                        try
                        {
                            if (switchEnabled)
                            {
                                mySerialPort.Write("0");
                            }
                            else
                            {
                                mySerialPort.Write("1");
                            }
                        }
                        catch
                        {
                            //System.Diagnostics.Debug.WriteLine("Error");
                        }
                    }
                    else
                    {
                        try
                        {
                            Debug.WriteLine("Port closed: " + mySerialPort.IsOpen);
                            mySerialPort.Open();
                            mySerialPort.Write(switchEnabled.ToString());
                        }
                        catch { }

                    }
                }
                else if (ClientMode)
                {
                    String switchEnabledString = string.Empty;
                    if (switchEnabled)
                    {
                        switchEnabledString = TCPConnect(serverAddr, 3323, "Off");
                    }
                    else
                    {
                        switchEnabledString = TCPConnect(serverAddr, 3323, "On");
                    }

                }
            }
        }
        public static void ListenTCP()
        {
            startTCPLoop:
            try
            {
                server = new TcpListener(localAddr, TCPport);
                server.Server.ReceiveTimeout = 3000;
                server.Server.SendTimeout = 3000;
                if (server.Server.IsBound != true)
                {
                    server.Start();
                }

                Byte[] bytes = new Byte[256];
                String data = null;
                String dataReturn = null;

                while (ListenToTCP)
                {
                    try
                    {
                        while (!server.Pending())
                        {
                            Thread.Sleep(10);
                            continue;
                        }
                        TcpClient client = server.AcceptTcpClient();
                        System.Diagnostics.Debug.WriteLine("TCP CONNECTED");
                        if (client.Connected)
                        {
                            TCPConnected = true;
                        }
                        else
                        {
                            TCPConnected = false;
                        }
                        data = null;
                        dataReturn = null;
                        NetworkStream stream = client.GetStream();
                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            System.Diagnostics.Debug.WriteLine("Received: "+ data);
                            //data = data.ToUpper();
                            if (data == "attach")
                            {
                                dataReturn = isAttached.ToString();
                            }
                            else if (data == "info")
                            {
                                dataReturn = switchEnabled.ToString();
                                System.Diagnostics.Debug.WriteLine(switchEnabled.ToString());
                            }
                            else if (data == "On")
                            {
                                PortInstruction = 1;
                                dataReturn = "On";
                            }
                            else if (data == "Off")
                            {
                                PortInstruction = 2;
                                dataReturn = "Off";
                            }
                            else if (data == "temp")
                            {
                                string tempReturn = temp.ToString();
                                Debug.WriteLine("tempReturn = "+tempReturn);
                                //dataReturn = temp.ToString();
                                dataReturn = tempReturn;
                            }
                            else if (data == "printerName")
                            {
                                if ((printerNameSelected != string.Empty) && (printerNameSelected != null))
                                    dataReturn = printerNameSelected;
                                else
                                    dataReturn = "NA";
                            }
                            else if (data == "isPrinting")
                            {
                                dataReturn = isPrinting.ToString();
                            }
                            else if (data == "printJob")
                            {
                                if ((printJob != string.Empty) && (printJob != null))
                                    dataReturn = printJob;
                                else
                                    dataReturn = "NA";
                            }
                            else if (data == "printComplete")
                            {
                                if ((printCompleteString != string.Empty) && (printCompleteString != null))
                                    dataReturn = printCompleteString;
                                else
                                    dataReturn = "NA";
                            }
                            else if (data == "printerNum")
                            {
                                dataReturn = printerNumSelected.ToString();
                            }
                            else if (data.StartsWith("shutdownTemp"))
                            {
                                int shutdownTempIndex = data.IndexOf("=") + 1;
                                string shutdownTempString = data.Substring(shutdownTempIndex);
                                Debug.WriteLine("Client data received = " + shutdownTempString);
                                shutdownTemp = Convert.ToInt32(shutdownTempString);
                                updateFaultStatus = true;
                            }
                            else if (data.StartsWith("motionStartDelay"))
                            {
                                int motionStartDelayIndex = data.IndexOf("=") + 1;
                                string motionStartDelayString = data.Substring(motionStartDelayIndex);
                                Debug.WriteLine("Client data received = " + motionStartDelayString);
                                motionStartDelay = Convert.ToInt32(motionStartDelayString);
                                updateFaultStatus = true;
                            }
                            else if (data.StartsWith("motionShutdownTime"))
                            {
                                int motionShutdownTimeIndex = data.IndexOf("=") + 1;
                                string motionShutdownTimeString = data.Substring(motionShutdownTimeIndex);
                                Debug.WriteLine("Client data received = " + motionShutdownTimeString);
                                motionShutdownTime = Convert.ToInt32(motionShutdownTimeString);
                                updateFaultStatus = true;
                            }
                            else if (data.StartsWith("motionShutdownEnabled"))
                            {
                                int motionShutdownEnabledIndex = data.IndexOf("=") + 1;
                                String motionShutdownEnabledString = data.Substring(motionShutdownEnabledIndex);
                                Debug.WriteLine("Client data received = " + motionShutdownEnabledString);
                                motionDetectEnabled = Convert.ToBoolean(motionShutdownEnabledString);
                                updateFaultStatus = true;
                            }
                            else if (data.StartsWith("smokeShutdownEnabled"))
                            {
                                int smokeShutdownEnabledIndex = data.IndexOf("=") + 1;
                                String smokeShutdownEnabledString = data.Substring(smokeShutdownEnabledIndex);
                                Debug.WriteLine("Client data received = " + smokeShutdownEnabledString);
                                smokeDetectEnabled = Convert.ToBoolean(smokeShutdownEnabledString);
                                updateFaultStatus = true;
                            }
                            else if (data.StartsWith("tempShutdownEnabled"))
                            {
                                int tempShutdownEnabledIndex = data.IndexOf("=") + 1;
                                String tempShutdownEnabledString = data.Substring(tempShutdownEnabledIndex);
                                Debug.WriteLine("Client data received = " + tempShutdownEnabledString);
                                tempDetectEnabled = Convert.ToBoolean(tempShutdownEnabledString);
                                updateFaultStatus = true;
                            }
                            else if (data == "Disconnect")
                            {
                                client.Close();
                                break;
                            }
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(dataReturn);
                            stream.Write(msg, 0, msg.Length);
                            System.Diagnostics.Debug.WriteLine("Sent: " + data);
                        }
                        client.Close();
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR ON TCP CONNECTION");
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                if (ServerMode != true)
                {
                    ListenToTCP = false;
                    server.Stop();
                    Thread.Sleep(Timeout.Infinite);
                }
                else if (ServerMode)
                {
                    if (NonTaskbarClose)
                    {
                        ListenToTCP = false;
                    }
                    else
                    {
                        ListenToTCP = true;
                        goto startTCPLoop;
                    }
                }
            }
        }
        public static void Selected_Serial(object sender, EventArgs e, string selected_port)
        {
            manualIsAttached = true;//if the manual port device is attached
            SPortVid_Pid = getVidPid(selected_port); // save the device ID to a string using the function passing the port name
            Debug.WriteLine("SPortVidPid=" + SPortVid_Pid);
            devID = getDeviceID(selected_port);
            Int32 indexOfVID = SPortVid_Pid.IndexOf("_") + 1;
            Int32 indexOfPID = SPortVid_Pid.IndexOf("_", indexOfVID) + 1;
            Debug.WriteLine("SPortVid_Pid:" + SPortVid_Pid);
            Debug.WriteLine("SPortVid_Pid.Substring:" + SPortVid_Pid.Substring(indexOfVID));
            try
            {
                VID = SPortVid_Pid.Substring(indexOfVID, 4);
                PID = SPortVid_Pid.Substring(indexOfPID, 4);
                ModifyINIData("VendorID", VID.ToString());
                ModifyINIData("ProductID", PID.ToString());
            }
            catch (Exception f)
            {
                Debug.WriteLine("Exception setting VidPid in Selected_Serial():" + f);
            }
            ModifyINIData("DeviceID", devID.ToString());
            try
            {
                if (mySerialPort.IsOpen)// if the port is already open, close it
                {
                    mySerialPort.Close();
                }
            }
            catch { }
            if (AutomaticPortSelect)
            {
                AutomaticPortSelect = false;                                    //set the boolean to false this is checked when sending data to the device

            }
            if (!ManualPortSelect)
            {
                ManualPortSelect = true; // set the boolean to true, this is checked when sending data to the device
            }
            portSelected = selected_port;//save the port to a string
            Console.WriteLine("Selected port: " + selected_port);   // write the string to the console
            mySerialPort = new SerialPort(selected_port, 9600, Parity.None, 8, StopBits.One);// create a new port instance with values in the argument
            mySerialPort.ReadTimeout = 500;//set the timeouts
            mySerialPort.WriteTimeout = 500;
            mySerialPort.DtrEnable = true;
            mySerialPort.RtsEnable = true;
            try
            {
                if (!mySerialPort.IsOpen)//if the port created is not open
                {
                    mySerialPort.Open();//open it
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error opening mySerialPort " + ex);
            }
            refresh_Menu();
        }
        void Selected_Serial(string selected_port)//an overload of the function above, called when the manual device is reattached
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    mySerialPort.Close();
                }
            }
            catch { }
            if (AutomaticPortSelect)
            {
                AutomaticPortSelect = false;                                    //set the boolean to false this is checked when sending data to the device
            }
            if (!ManualPortSelect)
            {
                ManualPortSelect = true; // set the boolean to true, this is checked when sending data to the device
            }
            portSelected = selected_port;
            Console.WriteLine("Selected port: " + selected_port);   // write the string to the console
            mySerialPort = new SerialPort(selected_port, 9600, Parity.None, 8, StopBits.One);// create a new port instance with values in the argument
            mySerialPort.ReadTimeout = 500;//set the timeouts
            mySerialPort.WriteTimeout = 500;
            mySerialPort.DtrEnable = true;
            mySerialPort.RtsEnable = true;
            manualIsAttached = true;
            try
            {
                if (!mySerialPort.IsOpen)//if the port created is not open
                {
                    mySerialPort.Open();//open it
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error opening mySerialPort " + ex);
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //var TCPListenThread = new Thread(ListenTCP);
            if (ServerMode)
            {
                toolStripMenuItem1.Checked = false;
                ServerMode = false;
                ModifyINIData("serverMode", ServerMode.ToString());
                clientModeToolStripMenuItem.Enabled = true;
                //printerSettingsToolStripMenuItem.Enabled = false;
                TCPListenThread.Interrupt();
                printerStatusThread.Interrupt();
            }
            else
            {
                toolStripMenuItem1.Checked = true;
                ServerMode = true;
                ModifyINIData("serverMode", ServerMode.ToString());
                ListenToTCP = true;
                clientModeToolStripMenuItem.Enabled = false;
                printerSettingsToolStripMenuItem.Enabled = true;
                System.Diagnostics.Debug.WriteLine(TCPListenThread.ThreadState);
                if (TCPListenThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    TCPListenThread.Interrupt();
                }
                else if (TCPListenThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    TCPListenThread.Start();
                }
                else
                {
                    TCPListenThread.Interrupt();
                }
                if (printerStatusThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    printerStatusThread.Interrupt();
                }
                else if (printerStatusThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    printerStatusThread.Start();
                }
                else
                {
                    printerStatusThread.Interrupt();
                }
            }
        }
        private void clientModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClientMode)
            {
                clientModeToolStripMenuItem.Checked = false;
                toolStripMenuItem1.Enabled = true;
                ClientMode = false;
                ModifyINIData("clientMode", ClientMode.ToString());
                printerSettingsToolStripMenuItem.Enabled = true;
            }
            else
            {
                clientModeToolStripMenuItem.Checked = true;
                toolStripMenuItem1.Enabled = false;
                ClientMode = true;
                ModifyINIData("clientMode", ClientMode.ToString());
                printerSettingsToolStripMenuItem.Enabled = false;
            }
        }
        private static void shutdownPrinterError(string error)
        {
            String ErrorOutput = DateTime.Now.ToString() + " - Print error : "+error+". Print Finished @  " + printCompleteString + "% Complete";
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            using (System.IO.StreamWriter errorlog =
                new System.IO.StreamWriter(path + "\\ErrorLog.txt", true))
            {
                errorlog.WriteLine(ErrorOutput);
            }
            mySerialPort.Write("0");
        }
        static string TCPConnect(String server, Int32 port, String message)
        {
            String MessageReturn = String.Empty;
            try
            {
                TcpClient client = new TcpClient(server, port);
                client.ReceiveTimeout = 100;
                client.SendTimeout = 100;
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                System.Diagnostics.Debug.WriteLine("Sent:" + message);
                data = new Byte[256];
                if (message != "Disconnect")
                {
                    String responseData = String.Empty;
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    System.Diagnostics.Debug.WriteLine("Received: "+ responseData);
                    MessageReturn = responseData;
                }
                stream.Close();
                client.Close();
                if (message == "Disconnect")
                {
                    Thread.Sleep(500);
                    stream.Dispose();
                }
            }
            catch (ArgumentNullException e)
            {
                System.Diagnostics.Debug.WriteLine("ArgumentNullException: "+ e.ToString());
            }
            catch (SocketException e)
            {
                System.Diagnostics.Debug.WriteLine("SocketException:" + e.ToString());
            }
            catch { }
            return MessageReturn;
        }
        static string getLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = String.Empty;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            System.Diagnostics.Debug.WriteLine("Local IP: " + localIP);
            return localIP;
        }
        private bool ConnectToDevice()//function to connect to the default device
        {
            //Debug.WriteLine("Attempting to Find Device");
            string[] portNames = SerialPort.GetPortNames(); //set a string array to the names of the ports
            string sInstanceName = string.Empty; // set an empty string to assign to the instance name of the serial port
            string sPortName = string.Empty;     // set an empty string to assign to the serial port name
            bool bFound = false;                // set a boolean to assign if the default port has been found
            for (int y = 0; y < portNames.Length; y++) // for every port that's available (a foreach would have also done here)
            {
                try //set a try to catch any exceptions accessing the management object searcher or opening the ports (if another program or instance of this program is running and is using that port it will cause an error)
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");//create a new ManagementObjectSearcher and instantiate it to the value results from the search function
                    foreach (ManagementObject queryObj in searcher.Get()) // for each result from the searcher above
                    {
                        sInstanceName = queryObj["PNPDeviceID"].ToString().ToUpper(); //set sInstanceName to the resulting instance name
                        if (devID != string.Empty)
                        {
                            //Debug.WriteLine("Checking DEV-ID");
                            if ((sInstanceName.IndexOf(Vid_Pid) > -1) && (sInstanceName.IndexOf(devID) > -1))//if the string Vid_Pid is present in the string
                            {
                                if ((isConnected == false) && (bFound == false))// if not already connected
                                {
                                    sPortName = queryObj["DeviceID"].ToString();// set the sPortName to the portname in the query
                                    mySerialPort = new SerialPort(sPortName, 9600, Parity.None, 8, StopBits.One);// create a new port instance with values in the argument
                                    mySerialPort.ReadTimeout = 500;//set the timeouts
                                    mySerialPort.WriteTimeout = 500;
                                    mySerialPort.DtrEnable = true;
                                    mySerialPort.RtsEnable = true;
                                    try
                                    {
                                        mySerialPort.Open();
                                        Debug.WriteLine(mySerialPort.PortName);
                                    }
                                    catch
                                    {
                                        Debug.WriteLine("Couldnt Open Serial Port");
                                    }
                                }
                                bFound = true;//set the boolean as true
                                Debug.WriteLine("DeviceFound1");
                            }
                            else // if the vid and pid are not found
                            {
                                //  bFound = false; // set the boolean as false
                            }
                        }
                        else
                        {
                            if (sInstanceName.IndexOf(Vid_Pid) > -1) //if the string Vid_Pid is present in the string
                            {
                                if (isConnected == false) // if not already connected
                                {
                                    sPortName = queryObj["PortName"].ToString();// set the sPortName to the portname in the query
                                    mySerialPort = new SerialPort(sPortName, 9600, Parity.None, 8, StopBits.One);// create a new port instance with values in the argument
                                    mySerialPort.ReadTimeout = 500;//set the timeouts
                                    mySerialPort.WriteTimeout = 500;
                                    mySerialPort.DtrEnable = true;
                                    mySerialPort.RtsEnable = true;
                                    //Debug.WriteLine("MySerial:" + mySerialPort.PortName);
                                }
                                bFound = true;//set the boolean as true
                                Debug.WriteLine("DeviceFound2");
                            }
                            else // if the vid and pid are not found
                            {
                                //  bFound = false; // set the boolean as false
                            }
                        }
                    }
                }
                catch (ManagementException e)
                {
                    System.Diagnostics.Debug.WriteLine("An error occurred while querying for WMI data: " + e.Message); //catch exceptions and output the error
                }
            }
            if (bFound) //if the boolean above is true
            {
                return true; // self explanitory
            }
            else
            {
                return false;
            }
        }
        private static string getDeviceName(string port)
        {
            if (port == "COM1")// if the port is COM1 set the name to System port (as it generally is, might need to review this)
            {
                return "System Port";
            }
            string deviceID = string.Empty;
            string deviceName = string.Empty;
            string sInstanceName = string.Empty; // set an empty string to assign to the instance name of the serial port
            string sPortName = string.Empty;     // set an empty string to assign to the serial port name
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\cimv2", "SELECT * FROM Win32_SerialPort");//create a new ManagementObjectSearcher and instantiate it to the value results from the search function
            try
            {
                foreach (ManagementObject queryObj in searcher.Get()) // for each result from the searcher above
                {
                    sPortName = queryObj["DeviceID"].ToString(); //set sInstanceName to the resulting instance name
                    Debug.WriteLine("Checking if port " + port + " matches " + sPortName);
                    if (sPortName == port)
                    {
                        deviceName = queryObj["Description"].ToString();
                    }
                }
                if (deviceName == string.Empty)
                    deviceName = "(Name Not Available)";
            }
            catch
            {
                deviceName = "(Name Not Available)";
            }
            return deviceName;
        }
        private static string getVidPid(string port)
        {
            if (port == "COM1")// if the port is COM1 set the name to System port (as it generally is, might need to review this)
            {
                return "VID_0000&PID_0000";
            }
            string sPortDeviceID = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort WHERE DeviceID = '" + port + "'");
            foreach (ManagementObject QueryObj in searcher.Get())
            {
                sPortDeviceID = QueryObj["PNPDeviceID"].ToString();//save the device id to a string
                Debug.WriteLine("SPortDeviceID from getVidPid:" + sPortDeviceID);
                Int32 indexOfVIDPID = sPortDeviceID.IndexOf("VID");//get the index position of "VID" in the string
                sPortDeviceID = sPortDeviceID.Substring(indexOfVIDPID, 17);// get a substring of VID and PID numbers
                System.Diagnostics.Debug.WriteLine("GetDeviceName - sPortDeviceID=" + sPortDeviceID);
            }
            return sPortDeviceID;
        }
        private static string getDeviceID(string port)
        {
            if (port == "COM1")// if the port is COM1 set the name to System port (as it generally is, might need to review this)
            {
                return "SystemPort";
            }
            string deviceID = string.Empty;
            string sPortDeviceID = string.Empty;
            string sInstanceName = string.Empty; // set an empty string to assign to the instance name of the serial port
            string sPortName = string.Empty;     // set an empty string to assign to the serial port name
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort WHERE DeviceID = '" + port + "'");
            foreach (ManagementObject QueryObj in searcher.Get())
            {
                deviceID = QueryObj["PNPDeviceID"].ToString();//save the device id to a string
                Int32 indexOfVIDPID = deviceID.IndexOf("VID");//get the index position of "VID" in the string
                Int32 indexOfDevID = deviceID.IndexOf("\\", indexOfVIDPID);
                Int32 indexOfDevIDEnd = deviceID.IndexOf("_", indexOfDevID);
                Debug.WriteLine("deviceID:" + deviceID);
                sPortDeviceID = deviceID.Substring(indexOfDevID + 1);
                Debug.WriteLine("sPortDeviceID:" + sPortDeviceID);
            }
            return sPortDeviceID;
        }
        public void Usb_DeviceRemoved(string deviceNameID)
        {
            if (deviceNameID.IndexOf(Vid_Pid) > -1)
            {
                Debug.WriteLine(Vid_Pid);
                System.Diagnostics.Debug.WriteLine("Default Device Removed");
                isConnected = false;
                isAttached = false;
                if (AutomaticPortSelect)
                {
                    try
                    {
                        mySerialPort.Dispose();
                        mySerialPort.Close();//try close the port

                    }
                    catch { }//catch any errors but dont bother outputting them
                }
            }
            if (deviceNameID.IndexOf(SPortVid_Pid) > -1)
            {
                System.Diagnostics.Debug.WriteLine("Manual Port Device Removed");
                manualIsAttached = false;

                if (ManualPortSelect)
                {
                    try
                    {
                        mySerialPort.Dispose();
                        mySerialPort.Close();//try close the port

                    }
                    catch { }//catch any errors but dont bother outputting them
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Device:" + deviceNameID);
                System.Diagnostics.Debug.WriteLine("device:" + SPortVid_Pid);
            }

        }
        public void Usb_DeviceAdded(string deviceNameID)
        {
            if (deviceNameID.IndexOf(Vid_Pid) > -1)
            {
                System.Diagnostics.Debug.WriteLine("Default Device Attached");
                System.Threading.Thread.Sleep(1000);//wait a second for the device's com port to come online
                isAttached = true;//set the automatic device as attached
            }
            if (deviceNameID.IndexOf(SPortVid_Pid) > -1)
            {
                System.Diagnostics.Debug.WriteLine("Manual Port Device Attached");
                System.Threading.Thread.Sleep(1000);//wait a second for the device's com port to come online
                if (ManualPortSelect == true)//if its in manual mode
                    Selected_Serial(portSelected);//re assign the com port and open it
            }
        }
        protected override void WndProc(ref Message m)//function to handle the USB device notifications
        {
            base.WndProc(ref m);
            //System.Diagnostics.Debug.WriteLine(m.ToString());
            if (m.Msg == UsbDeviceNotifier.WmDevicechange)
            {
                // System.Diagnostics.Debug.WriteLine(m.ToString());
                switch ((int)m.WParam)
                {
                    case UsbDeviceNotifier.DbtDeviceremovecomplete:
                        DEV_BROADCAST_DEVICEINTERFACE hdrOut = (DEV_BROADCAST_DEVICEINTERFACE)m.GetLParam(typeof(DEV_BROADCAST_DEVICEINTERFACE));
                        // System.Diagnostics.Debug.WriteLine("HDROut:" + hdrOut.dbcc_name);
                        Usb_DeviceRemoved(hdrOut.dbcc_name); // this is where you do your magic
                        break;
                    case UsbDeviceNotifier.DbtDevicearrival:
                        DEV_BROADCAST_DEVICEINTERFACE hdrIn = (DEV_BROADCAST_DEVICEINTERFACE)m.GetLParam(typeof(DEV_BROADCAST_DEVICEINTERFACE));
                        //System.Diagnostics.Debug.WriteLine("HDRIn:" + hdrIn.dbcc_name);
                        Usb_DeviceAdded(hdrIn.dbcc_name); // this is where you do your magic
                        break;
                }
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]//sets the layout structure for the function below
        internal struct DEV_BROADCAST_DEVICEINTERFACE
        {
            // Data size.
            public int dbcc_size;
            // Device type.
            public int dbcc_devicetype;
            // Reserved data.
            public int dbcc_reserved;
            // Class GUID.
            public Guid dbcc_classguid;
            // Device name data.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]//manage the data in the next line
            public string dbcc_name;
        }
        public void checkDevice()//function to check if the default device is already attached once the program has started
        {
            string sInstanceName = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");//create a new ManagementObjectSearcher and instantiate it to the value results from the search function
            foreach (ManagementObject queryObj in searcher.Get()) // for each result from the searcher above
            {
                //string sPortName = queryObj["DeviceID"].ToString(); //set sPortName to the port name
                sInstanceName = queryObj["PNPDeviceID"].ToString().ToUpper(); //set sInstanceName to the DeviceID
                if (sInstanceName.IndexOf(Vid_Pid) > -1)
                {
                    if (sInstanceName.IndexOf(devID) > -1)
                        isAttached = true;
                    else
                        isAttached = true;
                }
            }
        }
        public static void CreateINIData()//create INI data file data
        {
            var data = new IniData();
            IniData createData = new IniData();
            FileIniDataParser iniParser = new FileIniDataParser();

            createData.Sections.AddSection("PrinterPowerConfig");
            createData.Sections.GetSectionData("PrinterPowerConfig").LeadingComments.Add("This is the configuration file for the Application");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("serverAddress", "127.0.0.1");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("port", "3323");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("serverMode", "false");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("clientMode", "false");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("VendorID", "0000");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("ProductID", "0000");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("DeviceID", "0");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("printerIP", "127.0.0.1");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("printerPort", "3344");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("apikey", "c0000000-0000-0000-0000-000000000000");//c43686f3-867c-4c08-88b7-aaaaaaaaaaaa
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("printerSlug", "");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("printerName", "");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("printerNum", "0");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("motionDetectEnabled", "false");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("smokeDetectEnabled", "false");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("tempDetectEnabled", "false");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("motionStartDelay", "300");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("motionShutdownTime", "60");
            createData.Sections.GetSectionData("PrinterPowerConfig").Keys.AddKey("shutdownTemp", "80");

            iniParser.WriteFile("Config.ini", createData);
        }
        public static void ModifyINIData(String name, String value) // Modify INI data file data
        {
            int RetryTimes = 0;
            RetryIniModify:
            if (File.Exists("Config.ini"))
            {
                FileIniDataParser iniParser = new FileIniDataParser();
                IniData modifiedData = iniParser.ReadFile("Config.ini");
                modifiedData["PrinterPowerConfig"][name] = value;
                iniParser.WriteFile("Config.ini", modifiedData);
            }
            else
            {
                if (RetryTimes == 0)
                {
                    CreateINIData();
                    RetryTimes = 1;
                    goto RetryIniModify;
                }
            }
            //return modifiedData;
        }
        public static String ReadINIData(String name) //Read INI data
        {
            string readIniData = null;

            int RetryTimes = 0;
            RetryIniRead:
            if (File.Exists("Config.ini"))
            {
                FileIniDataParser iniParser = new FileIniDataParser();
                IniData readData = iniParser.ReadFile("Config.ini");
                readIniData = readData["PrinterPowerConfig"][name];
            }
            else
            {
                if (RetryTimes == 0)
                {
                    CreateINIData();
                    RetryTimes = 1;
                    goto RetryIniRead;
                }

            }
            return readIniData;

        }
        public void setupFromIni()
        {
            if (ServerMode)
            {
                clientModeToolStripMenuItem.Enabled = false;
                clientModeToolStripMenuItem.Checked = false;
                toolStripMenuItem1.Enabled = true;
                ListenToTCP = true;
                TCPListenThread.Start();

            }
            else if (ClientMode)
            {
                toolStripMenuItem1.Enabled = false;
                toolStripMenuItem1.Checked = false;
                clientModeToolStripMenuItem.Enabled = true;
                printerSettingsToolStripMenuItem.Enabled = false;
                //printerStatusThread.Interrupt();

            }
            if (!ClientMode)
            {
                printerStatusThread.Start();
                getPrinterStatus = true;
                printerSettingsToolStripMenuItem.Enabled = true;
            }
            label2.Update();
        }
        private static void checkPrinterStatus()
        {
            startPrinterStatusLoop:
            try
            {
                while (getPrinterStatus)
                {
                    JObject JSONObject1 = new JObject();
                    JObject JSONObject2 = new JObject();
                    string json1 = string.Empty;
                    string json2 = string.Empty;
                    string selectedPrinter;
                    string selectedSlug;
                    try
                    {
                        using (WebClient wc1 = new WebClient())
                        {
                            
                            string getString = "http://" + printerIP + ":" + printerPort + "/printer/info";
                            json1 = wc1.DownloadString(getString);
                            
                            Debug.WriteLine(json1);
                            string jsonString = json1.ToString();
                            if (jsonString.IndexOf("Authorization required") > -1)
                            {
                                getString = "http://" + printerIP + ":" + printerPort + "/printer/info/" + "?apikey=" + apikey;
                                Debug.WriteLine(getString);
                                json1 = wc1.DownloadString(getString);
                                Debug.WriteLine(json1);
                            }
                            wc1.Dispose();
                        }
                        JSONObject1 = JObject.Parse(json1);
                        JArray printers = (JArray)JSONObject1["printers"];
                        printerNames = from printer in JSONObject1["printers"] select (string)printer["name"];
                        printerSlugs = from slug in JSONObject1["printers"] select (string)slug["slug"];
                        foreach (var printerName in printerNames)
                        {
                            Debug.WriteLine("PrinterName: " + printerName);
                        }
                        if (!printerItemsCreated)
                        {
                            createPrinterItems();
                            printerItemsCreated = true;
                        }
                        if (printerNumSelected > 0)
                        {
                            selectedPrinter = printerNames.ElementAt(printerNumSelected - 1);
                            selectedSlug = printerSlugs.ElementAt(printerNumSelected - 1);
                            using (WebClient wc2 = new WebClient())
                            {
                                json2 = wc2.DownloadString("http://" + printerIP + ":" + printerPort + "/printer/list/" + selectedSlug + "?apikey=" + apikey);
                            }
                            JSONObject2 = JObject.Parse(json2);
                            Console.WriteLine(json2);
                            try
                            {
                                printCompleteString = (string)JSONObject2["data"][0]["done"];
                            }
                            catch { }
                            printJob = (string)JSONObject2["data"][0]["job"];
                            if ((printCompleteString != string.Empty) && (printCompleteString != null))
                            {
                                isPrinting = true;
                                printComplete = Convert.ToDecimal(printCompleteString);
                                Debug.WriteLine("Printer is Printing: " + printComplete + "% of " + printJob);
                                if (newPrint)
                                {
                                    noMotionTimerDelay.Interval = motionStartDelay;
                                    noMotionTimerDelay.Tick += NoMotionTimerDelay_Tick;
                                    noMotionTimerDelay.Start();
                                    newPrint = false;
                                }
                            }
                            else
                            {
                                isPrinting = false;
                                Debug.WriteLine("Printer is not Printing: Printjob = " + printJob);
                                if (newPrint == false)
                                {
                                    newPrint = true;
                                    motionTimerDelayComplete = false;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("http://" + printerIP + ":" + printerPort + "/printer/info");
                        
                        Debug.WriteLine("Exception getting printer info: " + e);
                    }
                    
                    Thread.Sleep(5000);
                }
            
            }
            catch (ThreadInterruptedException)
            {
                if (Form1.ClientMode)
                {
                    getPrinterStatus = false;
                    Thread.Sleep(Timeout.Infinite);
                }
                else if (!Form1.ClientMode)
                {
                    if (NonTaskbarClose)
                    {
                        getPrinterStatus = false;
                    }
                    else
                    {
                        getPrinterStatus = true;
                        goto startPrinterStatusLoop;
                    }
                }
            }
        }

        private static void NoMotionTimerDelay_Tick(object sender, EventArgs e)
        {

            motionTimerDelayComplete = true;
            noMotionTimerDelay.Stop();
        }
    }
}
