using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Ppt_Mobile
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();

        }

        private List<Point> m_newLine;

        private void picBoxUp_MouseDown(object sender, MouseEventArgs e)
        {
            m_newLine = new List<Point>(); 
        }


        /// <summary>
        /// 向当前线条中添加一个点，并将其与线条中的最后一个点连接起来
        /// </summary>
        /// <param name="x">新添加的点的横坐标</param>
        /// <param name="e">新添加的点的纵坐标</param>
        private void m_addPoint( int x, int y)
        {
            //将经过的点添加到线条

            m_newLine.Add(new Point(x, y));

            //绘制线段，连接当前线条的最后一点和新经过的这一点

            int points = m_newLine.Count;
            if (points > 1)
            {
                Graphics g = this.picBoxUp.CreateGraphics();
                Pen p = new Pen(Color.Red);
                g.DrawLine(
                    p,
                    m_newLine[points - 2].X, m_newLine[points - 2].Y,
                    m_newLine[points - 1].X, m_newLine[points - 1].Y
                    );
                g.Dispose();
                p.Dispose();
            }
        }

   

        private void picBoxUp_MouseMove(object sender, MouseEventArgs e)
        {
            m_addPoint(e.X, e.Y);
        }

        private void picBoxUp_MouseUp(object sender, MouseEventArgs e)
        {

            //将经过的点添加到当前线条列表，并连接
            m_addPoint(e.X, e.Y);
            DATATOSEND = TransferList2Str();
            startSendData();
 
        }

   
        private void picBoxUpDraw()
        {
            try
            {
                string Apppath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                string pathstr = new FileInfo(Apppath).DirectoryName;
                Bitmap bm = new Bitmap(pathstr + @"\1.jpg");
                Image myImage = Image.FromHbitmap(bm.GetHbitmap());


                //MemoryStream ms = new MemoryStream(System.Text.Encoding.Default.GetBytes(stringImage));
                //Bitmap bm = new Bitmap((Stream)ms);
                //Image myImage = Image.FromHbitmap(bm.GetHbitmap());

                this.picBoxUp.Image = myImage;
            }
            catch (System.Exception ex)
            {
                
            }
            
   
        }

        
        /*#region  LoadPictures
        
        //获取可执行文件路径及可执行文件名称
        string Apppath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        
        //获取可执行文件路径所在的文件夹
        string pathstr = new FileInfo(Apppath).DirectoryName;
        
        
        Bitmap bm = new Bitmap(pathstr + @"\1.jpg");
        Image myImage = Image.FromHbitmap(bm.GetHbitmap());
        this.pictureBox1.Image = myImage;
 


#endregion*/

        static private string getFilePath( )
        {
            string Apppath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            string pathstr = new FileInfo(Apppath).DirectoryName;
            return pathstr;
        }

    #region Bluetooth
        private BluetoothRadio radio;
        private bool receiving = false;
        private BluetoothClient client;


        // 流数据的定义
        private bool SIGN_STREAMSETUP = false;
        private System.IO.Stream stream;
        // 接收参数的定义
        private FileStream receivefilestream;
        string stringImage = "";

        private bool SIGN_FIlE_RECEIVED = false;

        static string pathstr = getFilePath();
        private string RECEIVEFILENAME = pathstr + @"\1.jpg";
        // 发送参数的定义

        private bool SUSPEND_OF_SENDFILETHREAD = false;
        
        private string DATATOSEND = "";
        // 进程的定义
        private System.Threading.Thread receiveThread;
        private System.Threading.Thread sendDataThread;

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


        private bool connect()
        {
            using (SelectBluetoothDeviceDialog bldialog = new SelectBluetoothDeviceDialog())
            {
                bldialog.ShowRemembered = false;
                if (bldialog.ShowDialog() == DialogResult.OK)
                {
                    if (bldialog.SelectedDevice == null)
                    {
                        MessageBox.Show("No device selected");
                        return false;
                    }

                    BluetoothDeviceInfo selecteddevice = bldialog.SelectedDevice;

                    if (!selecteddevice.Authenticated)
                    {
                        if (!BluetoothSecurity.PairRequest(selecteddevice.DeviceAddress, "0000"))
                        {
                            MessageBox.Show("PairRequest Error");
                            return false;
                        }
                    }

                    try
                    {
                        client = new BluetoothClient();
                        client.Connect(selecteddevice.DeviceAddress, BluetoothService.SerialPort);
                    }

                    catch
                    {
                        return false;
                    }

                    stream = client.GetStream();
                    // 标记stream已经接受对象实例化
                    SIGN_STREAMSETUP = true;
                    // 启动接收图片的进程
                    receiving = true;
                    receiveThread = new System.Threading.Thread(Receiveloop);
                    receiveThread.Start();
                    // 
                    sendDataThread = new System.Threading.Thread(SendData);
                    closeSendData();
                    sendDataThread.Start();
                    return true;
                }
                return false;
            }

        }
        static void EndWriteCallBack(IAsyncResult asyncResult)
        {
            MessageBox.Show("Error");
        }

        private void Receiveloop()
        {
            byte[] buffer;
            byte[] mark;
            while (receiving)
            {
                if (stream.CanRead)
                {
                    try
                    {
                        int streamLength;
                        mark = new byte[20];
                        int iii = 0;
                        iii = stream.Read(mark, 0, mark.Length);
                        if (iii == 0) continue;
                        int fullLength = int.Parse(System.Text.Encoding.ASCII.GetString(mark, 0, iii));

                        if (System.IO.File.Exists(RECEIVEFILENAME))
                            System.IO.File.Delete(RECEIVEFILENAME);
                        stringImage = "";
                        // 循环读取

                        FileInfo fileinfo = new FileInfo(RECEIVEFILENAME);
                        receivefilestream = fileinfo.OpenWrite();
                        BinaryWriter bw = new BinaryWriter(receivefilestream);
                        int markLength = 0;


                        do
                        {
                            buffer = new byte[1024 * 100];
                            streamLength = stream.Read(buffer, 0, buffer.Length);
                            markLength += streamLength;
                            stream.Flush();
                            //stringImage += System.Text.Encoding.ASCII.GetString(buffer, 0, streamLength);
                            bw.Write(buffer, 0, streamLength);
                            bw.Flush();
                            if (markLength > (fullLength - 100))
                                break;
                        } while (streamLength > 0);
                        receivefilestream.Close();
                        SIGN_FIlE_RECEIVED = true;
                        bw.Close();
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
        }

        private void startSendData()
        {
            SUSPEND_OF_SENDFILETHREAD = false;
        }

        private void closeSendData()
        {
            SUSPEND_OF_SENDFILETHREAD = true;
        }

        private void SendData()
        {
            while (SIGN_STREAMSETUP)
            {
                if (!SUSPEND_OF_SENDFILETHREAD)
                {
                    byte[] buffer = System.Text.Encoding.Default.GetBytes(DATATOSEND);
                    if (buffer.Length == 0)
                        closeSendData();
                    try
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.ToString());
                        break;
                    }

                    closeSendData();
#if DEBUG
                         MessageBox.Show(DATATOSEND);
#endif
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
#endregion

        private void menuItem4_Click(object sender, EventArgs e)
        {
            System.IO.File.Delete(RECEIVEFILENAME);
            Application.Exit();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (!checkBluetooth())
                MessageBox.Show("请打开您的蓝牙设备");
            if (!connect())
                MessageBox.Show("请检查PC端是否已开始服务.");
        }

        private string TransferList2Str()
        {
            string strOut = "";
            foreach(Point p in m_newLine)
            {
                strOut += p.X;
                strOut += ',';
                strOut += p.Y;
                strOut += ';';
            }

            return strOut;
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            DATATOSEND = "NextPage";
            if (picBoxUp.Image != null)
            {
                picBoxUp.Image.Dispose();
                picBoxUp.Image = null;
            }
            startSendData();

            do
            {
                if (SIGN_FIlE_RECEIVED)
                {

                    picBoxUpDraw();
                    SIGN_FIlE_RECEIVED = false;
                    break;
                }

            }
            while (true);

        }

        private void btUp_Click(object sender, EventArgs e)
        {
            DATATOSEND = "PrePage";
            if (picBoxUp.Image != null)
            {
                picBoxUp.Image.Dispose();
                picBoxUp.Image = null;
            }
            startSendData();
            do
            {
                if (SIGN_FIlE_RECEIVED)
                {

                    picBoxUpDraw();
                    SIGN_FIlE_RECEIVED = false;
                    break;
                }

            }
            while (true);

            
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            DATATOSEND = "Start";
            if (picBoxUp.Image != null)
            {
                picBoxUp.Image.Dispose();
                picBoxUp.Image = null;
            }
            startSendData();
            do
            {
                if (SIGN_FIlE_RECEIVED)
                {

                    picBoxUpDraw();
                    SIGN_FIlE_RECEIVED = false;
                    break;
                }

            }
            while (true);
        }

      

        
    }
}