using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace BlizzadKiller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        System.DateTime currentTime = new System.DateTime();
        int beginhour = 0;
        int beginmin = 0;
        int endhour = 0;
        int endmin = 0;
        string timeDuration;
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(@"C:\\Users\\Public\\Documents\\info.txt"))
            {
                textBox.IsEnabled = false;
                this.ShowInTaskbar = false;
                this.Visibility = Visibility.Hidden;

                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += new EventHandler(timer_Click);
                timer.Start();

                FileStream aFile = new FileStream("C:\\Users\\Public\\Documents\\info.txt", FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                beginhour = Convert.ToInt32(sr.ReadLine());
                beginmin = Convert.ToInt32(sr.ReadLine());
                timeDuration = sr.ReadLine();
                sr.Close();
                
            }
          
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(@"C:\\Users\\Public\\Documents\\info.txt"))
            {
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += new EventHandler(timer_Click);
                timer.Start();
                currentTime = System.DateTime.Now;
                beginhour = currentTime.Hour;
                beginmin = currentTime.Minute;

                timeDuration = textBox.Text;
                textBox.IsEnabled = false;
                this.ShowInTaskbar = false;
                this.Visibility = Visibility.Hidden;

                //write
                FileStream aFile = new FileStream("C:\\Users\\Public\\Documents\\info.txt", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(aFile);
                sw.WriteLine($"{beginhour}");
                sw.WriteLine($"{beginmin}");
                sw.WriteLine($"{timeDuration}");
                sw.Close();

                RegRun("BlizzardKiller", true);
            }
           
        }

        private bool CheckTime()
        {
            currentTime = System.DateTime.Now;
            endhour = currentTime.Hour;
            endmin = currentTime.Minute;
            int intTime = Convert.ToInt32(timeDuration);
            int timeRun = endhour * 60 + endmin - beginhour * 60 - beginmin;
            return timeRun >= intTime;
                /*
                    this.ShowInTaskbar = true;
                    this.Visibility = Visibility.Visible;
                    timer.Stop();
                    File.Delete("C:\\Users\\Public\\Documents\\info.txt");
                    Window1 isw = new Window1();
                    isw.Title = "Warning";
                    isw.Show();*/
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {           
                Environment.Exit(0);
        }

        void timer_Click(object sender,EventArgs e)
        {
            if (!CheckTime())
            {
                Process[] gameprocess = Process.GetProcesses();
                for (int i = 0; i < gameprocess.GetLength(0); i++)
                {
                    if (gameprocess[i].ProcessName.Equals("Battle.net") ||
                        gameprocess[i].ProcessName.Equals("taskmgr")||
                        gameprocess[i].ProcessName.Equals("powershell") ||
                        gameprocess[i].ProcessName.Equals("HeroesOfTheStorm_x64") ||
                        gameprocess[i].ProcessName.Equals("Overwatch")||
                        gameprocess[i].ProcessName.Equals("Client") ||
                        gameprocess[i].ProcessName.Equals("lol.launcher_tencent") ||
                        gameprocess[i].ProcessName.Equals("League of legends") ||
                        gameprocess[i].ProcessName.Equals("LolClient")
                        )
                    {
                        gameprocess[i].Kill(); break;
                    }
                }
            }
            else
            {
                Window1 isw = new Window1();
                isw.Title = "Congradulations！";
                isw.Show();

                File.Delete("C:\\Users\\Public\\Documents\\info.txt");
                this.ShowInTaskbar = true;
                //this.Visibility = Visibility.Visible;
                timer.Stop();
                RegRun("BlizzardKiller", true);
                this.Close();          
            }
        }
        
        /*
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }*/
        //注册表
        private void RegRun(string appName, bool f)
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            bool b = false;
            foreach (string i in Run.GetValueNames())
            {
                if (i == appName)
                {
                    b = true;
                    break;
                }
            }
            try
            {
                if (f)
                {
                    Run.SetValue(appName, System.Windows.Forms.Application.ExecutablePath);
                }
                else
                {
                    Run.DeleteValue(appName);
                }
            }
            catch
            { }
            HKCU.Close();
        }
    }
}
