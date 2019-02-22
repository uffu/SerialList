using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        { 
            string str = "";
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string port in ports)
                str += port + "\r\n";
            textBox.Dispatcher.Invoke(new Action(() => { textBox.Text = str; }));
        }

        private void OnTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = onTop.IsChecked.Value;
        }
    }
}
