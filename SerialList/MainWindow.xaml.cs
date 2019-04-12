using System;
using System.Windows;
using System.IO.Ports;
using System.Collections.Generic;
using Hardcodet.Wpf.TaskbarNotification;

namespace SerialList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                taskbarIcon.Visibility = Visibility.Visible;
            }
            else
                taskbarIcon.Visibility = Visibility.Hidden;

            base.OnStateChanged(e);
        }

        private List<string> known_ports = new List<string>();
        private bool isThereNewPort()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length != known_ports.Count)
                return true;
          
            foreach (string port in ports)
            {
                if (!known_ports.Contains(port))                
                    return true;                
            }
            return false;
        }

        private void pop_up()
        {
            if (WindowState != WindowState.Minimized)
                return;
            
            if (!isThereNewPort())
                return;

            known_ports.Clear();
            known_ports.AddRange(SerialPort.GetPortNames());
            string str = "";
            foreach (string port in known_ports)
                str += port + "\r\n";
            if (str.Length == 0) str = "No com port";
            taskbarIcon.ShowBalloonTip("Ports", str, BalloonIcon.Info);
            taskbarIcon.ToolTipText = str;
        }
                
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        { 
            string str = "";
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                str += port + "\r\n";
            Dispatcher.Invoke(new Action(() => { textBox.Text = str; pop_up(); }));
        }

        private void OnTop_Click(object sender, RoutedEventArgs e)
        {
            Topmost = onTop.IsChecked.Value;
        }

        private void Minimize_btn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }
    }
}
