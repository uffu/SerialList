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

        private string old_ports_str = "initial";
        private void pop_up(string str)
        {
            if (WindowState != WindowState.Minimized)
                return;

            if (old_ports_str.Equals(str))
                return;

            old_ports_str = str;

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
            str = str.TrimEnd();
            Dispatcher.Invoke(new Action(() => { textBox.Text = str; pop_up(str); }));
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
