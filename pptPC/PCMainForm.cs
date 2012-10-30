using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace pptPC
{
    public partial class PCMainForm : Form
    {
        public PCMainForm()
        {
            InitializeComponent();
            if(!checkBluetooth())
            {
                MessageBox.Show("请打开电脑端蓝牙设备");
            }
            Listen();
        }

        #region PrintScreen

        public bool ThumbnailCallback()
        {
            return false;
        }

        private void printSrc()
        {
            int iWidth = Screen.PrimaryScreen.Bounds.Width;
            int iHeight = Screen.PrimaryScreen.Bounds.Height;

            Image img = new Bitmap(iWidth, iHeight);

            Graphics gc = Graphics.FromImage(img);

            gc.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));

            Image.GetThumbnailImageAbort myCallback =
                new Image.GetThumbnailImageAbort(ThumbnailCallback);

            Image myThumbnail400x300 = img.GetThumbnailImage(400, 300, myCallback, IntPtr.Zero);

            myThumbnail400x300.Save(@"D:\" + 1 + ".jpg");

        }

        private void printedPicsClear()
        {
            System.IO.File.Delete(@"D:\" + 1 + ".jpg");
        }

        #endregion

        #region DrawScreen
     /*   [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]*/
        
        public struct Point2
        {
            ///LONG->int
            public int x;

            ///LONG->int
            public int y;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct HWND__
        {
            ///int
            public int unused;
        }

        public partial class NativeConstants
        {
            /// KEYEVENTF_KEYUP -> 0x0002
            public const int KEYEVENTF_KEYUP = 2;

            /// VK_DOWN -> 0x28
            public const int VK_DOWN = 40;

            /// VK_LEFT -> 0x25
            public const int VK_LEFT = 37;

            /// VK_RIGHT -> 0x27
            public const int VK_RIGHT = 39;

            /// VK_UP -> 0x26
            public const int VK_UP = 38;

            /// MOUSEEVENTF_LEFTUP -> 0x0004
            public const int MOUSEEVENTF_LEFTUP = 4;

            /// MOUSEEVENTF_LEFTDOWN -> 0x0002
            public const int MOUSEEVENTF_LEFTDOWN = 2;

            public const int VK_F5 = 116;

        }

        public partial class NativeMethods
        {
            /// VK_CONTROL -> 0x11
            public const int VK_CONTROL = 17;

            /// MOUSEEVENTF_LEFTDOWN -> 0x0002
            public const int MOUSEEVENTF_LEFTDOWN = 2;

            /// MOUSEEVENTF_LEFTUP -> 0x0004
            public const int MOUSEEVENTF_LEFTUP = 4;
            public const int VK_P = 80;
            public const int VK_A = 65;

            public const int VK_UP = 38;
            public const int VK_DOWN = 40;

            public const int VK_F5 = 116;

            /// Return Type: void
            ///bVk: BYTE->unsigned char
            ///bScan: BYTE->unsigned char
            ///dwFlags: DWORD->unsigned int
            ///dwExtraInfo: ULONG_PTR->unsigned int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "keybd_event")]
            public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

            /// Return Type: void
            ///dwFlags: DWORD->unsigned int
            ///dx: DWORD->unsigned int
            ///dy: DWORD->unsigned int
            ///dwData: DWORD->unsigned int
            ///dwExtraInfo: ULONG_PTR->unsigned int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "mouse_event")]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);

            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "Sleep")]
            public static extern void Sleep(uint dwMilliseconds);

            /// Return Type: UINT->unsigned int
            ///uCode: UINT->unsigned int
            ///uMapType: UINT->unsigned int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MapVirtualKeyW")]
            public static extern uint MapVirtualKeyW(uint uCode, uint uMapType);

            /// Return Type: HWND->HWND__*
            ///hWnd: HWND->HWND__*
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetFocus")]
            public static extern System.IntPtr SetFocus([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd);

            /// Return Type: HWND->HWND__*
            ///Point: POINT->tagPOINT
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "WindowFromPoint")]
            public static extern System.IntPtr WindowFromPoint(Point2 Point);

            /// Return Type: BOOL->int
            ///X: int
            ///Y: int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool SetCursorPos(int X, int Y);

        }

        private void Draw(string str)
        {
            NativeMethods.keybd_event(NativeMethods.VK_CONTROL,
                                       (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_CONTROL, 0),
                                       0,
                                       0);
            NativeMethods.keybd_event(NativeMethods.VK_P,
                                      (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_P, 0),
                                      0,
                                      0);
            NativeMethods.keybd_event(NativeMethods.VK_P,
                                      (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_P, 0),
                                      NativeConstants.KEYEVENTF_KEYUP,
                                      0);
            NativeMethods.keybd_event(NativeMethods.VK_CONTROL,
                                      (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_CONTROL, 0),
                                      NativeConstants.KEYEVENTF_KEYUP,
                                      0);
            str2arr(str);
            for ( int i = 0; i < listX.Count; i++ )
            {
                DDALine_all(listX[i] * 2, listY[i] * 2, listX[i + 1] * 2, listY[i + 1] * 2);
            }
        }
        
        void DDALine(int x0, int y0, int x1, int y1)
        {
            int i;
            float dx, dy, length, x, y;
            if (Math.Abs(x1 - x0) >= Math.Abs(y1 - y0))
            {
                length = Math.Abs(x1 - x0);
            }
            else
            {
                length = Math.Abs(y1 - x0);
            }

            dx = (x1 - x0) / length;
            dy = (y1 - y0) / length;

            i = 1;
            x = x0;
            y = y0;

            while (i <= length)
            {
                DrawPoint((int)x, (int)y);
                x = x + dx;
                y = y + dy;
                i++;
            }
        }
        void DrawPoint(int x, int y)
        {
            NativeMethods.SetCursorPos(x, y);
            NativeMethods.mouse_event(NativeConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            NativeMethods.mouse_event(NativeConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        void DDALine_all(int x0, int y0, int x1, int y1)
        {
            int i;
            if (x0 == x1)
            {
                //为竖线
                if (y0 <= y1)
                {
                    for (i = y0; i <= y1; i++)
                        DrawPoint(x0, i);
                }
                else
                {
                    for (i = y1; i <= y0; i++)
                        DrawPoint(x0, i);
                }

                return;
            }

            //为横线
            if (y0 == y1)
            {
                if (x0 <= x1)
                {
                    for (i = x0; i <= x1; i++)
                        DrawPoint(i, y0);
                }
                else
                {
                    for (i = x1; i <= x0; i++)
                        DrawPoint(i, y0);
                }

                return;
            }

            //为斜线
            double m = (y1 - y0) * 1.0 / (x1 - x0);
            float fTemp;

            if (Math.Abs(m) <= 1)
            {
                if (x0 < x1)
                {
                    fTemp = (float)(y0 - m);
                    for (i = x0; i <= x1; i++)
                        DrawPoint(i, (int)(fTemp += (float)m));
                }
                else
                {
                    fTemp =(float)(y1 - m);
                    for (i = x1; i <= x0; i++)
                        DrawPoint(i, (int)(fTemp += (float)m));
                }
                return;
            }

            if (y0 < y1)
            {
                fTemp = (float)(x0 - 1 / m);
                for (i = y0; i <= y1; i++)
                    DrawPoint((int)(fTemp +=(float) (1 / m)), i);
            }
            else
            {
                fTemp = (float)(x1 - 1 / m);
                for (i = y1; i <= y0; i++)
                    DrawPoint((int)(fTemp += (float)(1 / m)), i);
            }

        }
        public List<int> listX = new List<int>();
        public List<int> listY = new List<int>();
    
        public void str2arr(string str)
        {
            listX.Clear();
            listY.Clear();
            int a = 0, b = 0, c = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ';')
                    {
                        c = i;
                        for (int j = c; j > 0; j--)
                        {
                            if ( str[j] == ',')
                            {
                                b = j;
                                break;
                            }
                        }
                        cutArrfromStr(a, b, c, str);
                        a = c + 1;
                        b = c;
                    }
                }
        }

        public void cutArrfromStr(int a, int b, int c, string s)
        {
            string s1 = "", s2 = "";

            for ( int i = a; i < b; i++ )
            {
                s1 += s[i];
            }
            for ( int i = b + 1; i < c; i++ )
            {
                s2 += s[i];
            }

            int x, y;
            
            x = int.Parse(s1);
            y = int.Parse(s2);

            listX.Add(x);
            listY.Add(y);

        }

        private void NextPage()
        {
            NativeMethods.keybd_event(NativeMethods.VK_DOWN,
                (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_DOWN, 0),
                0,
                0);

            NativeMethods.keybd_event(NativeMethods.VK_DOWN,
                          (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_DOWN, 0),
                          NativeConstants.KEYEVENTF_KEYUP,
                          0);
        }

        private void PrePage()
        {
            NativeMethods.keybd_event(NativeMethods.VK_UP,
                (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_UP, 0),
                0,
                0);

            NativeMethods.keybd_event(NativeMethods.VK_UP,
                (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_UP, 0),
                NativeConstants.KEYEVENTF_KEYUP,
                0);
        }

        private void Start()
        {
            NativeMethods.keybd_event(NativeMethods.VK_F5,
                (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_F5, 0),
                0,
                0);

            NativeMethods.keybd_event(NativeMethods.VK_F5,
                (byte)NativeMethods.MapVirtualKeyW(NativeMethods.VK_F5, 0),
                NativeConstants.KEYEVENTF_KEYUP,
                0);
        }
        #endregion

        #region Bluetooth
        private BluetoothRadio radio;
        private BluetoothClient client;
        private BluetoothListener listener;

        private bool receiving = false;
        private bool listening = false;
        private bool SIGN_STREAMSETUP = false;

        private System.IO.Stream stream;
        private System.IO.FileStream sendfilestream;

        private string SENDFILENAME = @"D:\" + @"1.jpg";
        private string RECEIVEDATA = "";


        private System.Threading.Thread receiveThread;
        private System.Threading.Thread listenThread;
        private System.Threading.Thread sendfileThread;

        private bool checkBluetooth()
        {
            radio = BluetoothRadio.PrimaryRadio;
            if (radio == null)
                return false;
            if (radio.Mode == RadioMode.PowerOff)
                return false;
            radio.Mode = RadioMode.Discoverable;
            return true;
        }

        private void Listen()
        {
            if (radio == null)
                return;

            listener = new BluetoothListener(BluetoothService.SerialPort);
            listener.Start();
            listening = true;
            listenThread = new System.Threading.Thread(ListenLoop);
            listenThread.Start();
            sendfileThread = new System.Threading.Thread(SendFile);
            receiveThread = new System.Threading.Thread(ReceiveLoop);
        }

        private void ListenLoop()
        {
            while (listening)
            {
                try
                {
                    client = listener.AcceptBluetoothClient();
                    stream = client.GetStream();
                    SIGN_STREAMSETUP = true;
                    listening = false;

                    // 远程连接后，启动发送文件进程
                    

                    // 远程设备连接后，启动recieve程序
                    receiving = true;
                    receiveThread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }
            }

        }

        private void SendFile()
        {
            while (SIGN_STREAMSETUP)
            {
                sendfilestream = new FileStream(SENDFILENAME, FileMode.Open, FileAccess.Read, FileShare.None);
                byte[] buffer = new byte[sendfilestream.Length];

                byte[] bytefileLength = System.Text.Encoding.Default.GetBytes(sendfilestream.Length.ToString());
                stream.Write(bytefileLength, 0, bytefileLength.Length);
                while (true)
                {
                    try
                    {
                        int length = sendfilestream.Read(buffer, 0, buffer.Length);
                        if (length == 0)
                        {
                            sendfilestream.Close();
                            sendfilestream.Dispose();
#if DEBUG
                            MessageBox.Show("Send a file");
#endif
                            // 发送文件完毕，进程挂起。
                            sendfileThread.Suspend();
                            break;
                        }
                        stream.Write(buffer, 0, length);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }
                }
            }
        }

        private void ReceiveLoop()
        {
            byte[] buffer;
            while (receiving)
            {
                if (stream.CanRead)
                {
                    try
                    {
                        int streamLength;
                        RECEIVEDATA = "";
                        do
                        {
                            buffer = new byte[1024 * 2];
                            streamLength = stream.Read(buffer, 0, buffer.Length);
                            RECEIVEDATA += System.Text.Encoding.Default.GetString(buffer,0, streamLength);
                        } while (streamLength == 2048);
                        stream.Flush();
#if DEBUG
                        MessageBox.Show(RECEIVEDATA);
#endif
                        if (RECEIVEDATA == "NextPage")
                        {
                            NextPage();
                            NativeMethods.Sleep(500);
                            printSrc();
                            SENDFILENAME = @"D:\" + @"1.jpg"; 
                            sendfileThread.Resume();
                        }
                        else
                            if (RECEIVEDATA == "PrePage")
                            {
                                PrePage();
                                NativeMethods.Sleep(500);
                                printSrc();
                                SENDFILENAME = @"D:\" + @"1.jpg";
                                sendfileThread.Resume();
                            }
                            else
                                if (RECEIVEDATA == "Start")
                                {
                                    Start();
                                    NativeMethods.Sleep(500);
                                    printSrc();
                                    SENDFILENAME = @"D:\" + @"1.jpg";
                                    sendfileThread.Start();
                                }
                                else
                                {
                                    Draw(RECEIVEDATA);
                                }
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
        }
#endregion


    }
}
