using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Chronos
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private DispatcherTimer timeDisplayTimer;
        private DispatcherTimer timerDisplayTimer;
        private DispatcherTimer stopwatchDisplayTimer;
        private DateTime currentTimeTime;
        private DateTime currentTimerTime;
        private DateTime currentStopwatchTime;
        private DateTime timerStart;
        private TimeSpan timerEnd;
        private DateTime stopwatchStart;
        private TimeSpan stopwatchOffset = new TimeSpan(0);

        private List<DateTime> notedTimeStamps = new List<DateTime>();

        public MainWindow()
        {
            InitializeComponent();
            timeDisplayTimer = new System.Windows.Threading.DispatcherTimer();
            timerDisplayTimer = new System.Windows.Threading.DispatcherTimer();
            stopwatchDisplayTimer = new System.Windows.Threading.DispatcherTimer();
            timeDisplayTimer.Tick += new EventHandler(timeDisplayTimer_Tick);
            timerDisplayTimer.Tick += new EventHandler(timerDisplayTimer_Tick);
            stopwatchDisplayTimer.Tick += new EventHandler(stopwatchDisplayTimer_Tick);
            timeDisplayTimer.Interval = new TimeSpan(1000);
            timerDisplayTimer.Interval = new TimeSpan(100);
            stopwatchDisplayTimer.Interval = new TimeSpan(100);
            timeDisplayTimer.Start();
        }

        private void timeDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            DateTime d = DateTime.Now;

            currentTimeTime = d;
            timeDisplayHH.Content = d.ToString("HH");
            timeDisplaymm.Content = d.ToString("mm");
            timeDisplayss.Content = d.ToString("ss");

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void timerDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            DateTime d = DateTime.Now;
            //if (d.TotalMilliseconds <= 0)
            //{
            //    stopwatchDisplayTimer.Stop();
            //    stopwatchStart = new DateTime(0);
            //    d = new TimeSpan(0);
            //}
            currentTimerTime = d;
            timerDisplayHH.Text = d.ToString("HH");
            timerDisplaymm.Text = d.ToString("mm");
            timerDisplayss.Text = d.ToString("ss");

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void stopwatchDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            TimeSpan d = DateTime.Now - stopwatchStart + stopwatchOffset;
            currentStopwatchTime = new DateTime(0) + d;
            stopwatchDisplayHH.Content = d.ToString(@"hh");
            stopwatchDisplaymm.Content = d.ToString(@"mm");
            stopwatchDisplayss.Content = d.ToString(@"ss");

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (timerDisplayTimer.IsEnabled)
            {
                timerDisplayTimer.Stop();
                startTimerButton.Content = "▶";
            }
            else
            {
                timerDisplayTimer.Start();
                startTimerButton.Content = "❚❚";
            }
        }

        private void startStopwatchButton_Click(object sender, RoutedEventArgs e)
        {

            if (stopwatchDisplayTimer.IsEnabled)
            {
                stopwatchDisplayTimer.Stop();
                stopwatchOffset = DateTime.Now - stopwatchStart + stopwatchOffset;
                startStopwatchButton.Content = "▶";
            }
            else
            {
                stopwatchStart = DateTime.Now;
                stopwatchDisplayTimer.Start();
                startStopwatchButton.Content = "❚❚";
            }
        }

        private void stopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timerDisplayTimer.Stop();
            startTimerButton.Content = "▶";
            timerStart = new DateTime(0);
            timerDisplayHH.Text = "00";
            timerDisplaymm.Text = "00";
            timerDisplayss.Text = "00";
        }

        private void stopStopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatchDisplayTimer.Stop();
            startStopwatchButton.Content = "▶";
            stopwatchStart = new DateTime(0);
            stopwatchOffset = new TimeSpan(0);
            stopwatchDisplayHH.Content = "00";
            stopwatchDisplaymm.Content = "00";
            stopwatchDisplayss.Content = "00";
        }

        private void notateTimeButton_Click(object sender, RoutedEventArgs e)
        {
            notedTimeStamps.Add(currentTimeTime);
        }

        private void notateTimerButton_Click(object sender, RoutedEventArgs e)
        {
            notedTimeStamps.Add(currentTimerTime);
        }

        private void notateStopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            notedTimeStamps.Add(currentStopwatchTime);
        }

        // ❚❚
    }
}
