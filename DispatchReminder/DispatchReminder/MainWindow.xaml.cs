using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DispatchReminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer t;
        Timer hourTimer = new Timer();

        public MainWindow()
        {
            InitializeComponent();

            t = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background,
                t_Tick, Dispatcher.CurrentDispatcher); t.IsEnabled = true;

            hourTimer.Enabled = false;
            hourTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            l_nextAlarm.Content = "No alarm set";
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            Action action = () => {
                Alarm alarmRef = new Alarm();
                alarmRef.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                alarmRef.Show();
            };
            Dispatcher.BeginInvoke(action);
            
            
        }

        public void t_Tick(object sender, EventArgs e)
        {
            try
            {
                setAlarm();
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        public void setAlarm() {

            string hour = "";
            string minute = "";
            string second = "";

            if (DateTime.Now.Hour < 10)
            {
                hour = "0" + DateTime.Now.Hour;
            }
            else
            {
                hour = DateTime.Now.Hour.ToString();
            }

            if (DateTime.Now.Minute < 10)
            {
                minute = "0" + DateTime.Now.Minute;
            }
            else
            {
                minute = DateTime.Now.Minute.ToString();
            }
            if (DateTime.Now.Second < 10)
            {
                second = "0" + DateTime.Now.Second;
            }
            else
            {
                second = DateTime.Now.Second.ToString();
            }

            l_Time.Content = Convert.ToString(hour + ":" + minute + ":" + second);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            int hourSet;
            bool belowTen = false;
            string minuteSet = DateTime.Now.Minute.ToString();
            string secondSet = DateTime.Now.Second.ToString();

            if (DateTime.Now.Hour < 10)
            {
                hourSet = DateTime.Now.Hour + 1;
                if (hourSet < 10)
                {
                    belowTen = true;
                }
                else {
                    belowTen = false;
                }
                
            }
            else
            {
                hourSet = DateTime.Now.Hour + 1;
            }

            if (DateTime.Now.Minute < 10)
            {
                minuteSet = "0" + DateTime.Now.Minute;
            }
            else
            {
                minuteSet = DateTime.Now.Minute.ToString();
            }
            if (DateTime.Now.Second < 10)
            {
                secondSet = "0" + DateTime.Now.Second;
            }
            else
            {
                secondSet = DateTime.Now.Second.ToString();
            }

            hourTimer.Interval = 60 * 60 * 1000;

            if (belowTen)
            {
                l_nextAlarm.Content = "Next dispatch: 0" + hourSet + ":" + minuteSet + ":" + secondSet;
            }

            else {
                l_nextAlarm.Content = "Next dispatch: " + hourSet + ":" + minuteSet + ":" + secondSet;
            }
            
            hourTimer.Enabled = true;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            l_nextAlarm.Content = "No alarm set";
            hourTimer.Enabled = false;
        }
    }
}
