using System;
using System.Collections.Generic;
using System.Windows;
using System.Media;
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
        private DateTime stopwatchStart;
        private TimeSpan timerOffset = new TimeSpan(0);
        private TimeSpan stopwatchOffset = new TimeSpan(0);
        private TimeSpan timerEnd;
        SoundPlayer notifyPlayer = new SoundPlayer("Sounds/notification.wav");

        private List<DateTime> notedTimeStamps = new List<DateTime>();

        // TODO: start/pause on timer breaks it

        public MainWindow()
        {
            InitializeComponent();
            if (!SetupTimers())
            {
                Environment.Exit(1001);
            }
            notifyPlayer.LoadAsync();
        }

        private bool SetupTimers()
        {
            try
            {
                timeDisplayTimer =      new DispatcherTimer();
                timerDisplayTimer =     new DispatcherTimer();
                stopwatchDisplayTimer = new DispatcherTimer();
                timeDisplayTimer.Tick +=      new EventHandler(timeDisplayTimer_Tick);
                timerDisplayTimer.Tick +=     new EventHandler(timerDisplayTimer_Tick);
                stopwatchDisplayTimer.Tick += new EventHandler(stopwatchDisplayTimer_Tick);
                timeDisplayTimer.Interval =      new TimeSpan(1000000);
                timerDisplayTimer.Interval =     new TimeSpan(500000);
                stopwatchDisplayTimer.Interval = new TimeSpan(500000);
                timeDisplayTimer.Start();
            }
            catch (Exception e) {
                MessageBox.Show(e.Message + "\nStopping...");
                return false;
            }
            return true;
        }

        private void timeDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            DateTime d = DateTime.Now;

            currentTimeTime = d;
            timeDisplayHH.Content = d.ToString("HH");
            timeDisplaymm.Content = d.ToString("mm");
            timeDisplayss.Content = d.ToString("ss");

        }

        private void timerDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            TimeSpan d = timerEnd - (DateTime.Now - timerStart + timerOffset);
            if (new TimeSpan(0) <= d)
            {
                currentTimerTime = new DateTime(0) + d;
                timerDisplayhh.Text = d.ToString(@"hh");
                timerDisplaymm.Text = d.ToString(@"mm");
                timerDisplayss.Text = d.ToString(@"ss");
            }
            else
            {
                notifyPlayer.Play();
                stopTimer();
            }

        }

        private void stopwatchDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Calculate time ticked (Now - when the stopwatch started + time on pause)
            TimeSpan d = DateTime.Now - stopwatchStart + stopwatchOffset;
            // Convert timespan to datetime
            currentStopwatchTime = new DateTime(0) + d;
            // Show new data
            stopwatchDisplayHH.Content = d.ToString(@"hh");
            stopwatchDisplaymm.Content = d.ToString(@"mm");
            stopwatchDisplayss.Content = d.ToString(@"ss");

        }

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            startTimer();
        }

        private void startStopwatchButton_Click(object sender, RoutedEventArgs e)
        {

            if (stopwatchDisplayTimer.IsEnabled)
            {
                stopwatchDisplayTimer.Stop();
                stopwatchOffset = DateTime.Now - stopwatchStart + stopwatchOffset; // += maybe?
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
            stopTimer();
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

        private void timerDisplay_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (currentTimerTime != new DateTime(0))
            //    timerDisplayTimer.Stop();
        }

        private void timerDisplay_LostFocus(object sender, RoutedEventArgs e)
        {
            //if(currentTimerTime != new DateTime(0))
            //    timerDisplayTimer.Start();
        }

        private void startTimer()
        {
            if (timerDisplayTimer.IsEnabled)
            {
                timerDisplayTimer.Stop();
                timerOffset = DateTime.Now - timerStart + timerOffset; // += maybe?
                startTimerButton.Content = "▶";
            }
            else
            {
                timerEnd = getTimerEnd();
                timerStart = DateTime.Now;
                timerDisplayTimer.Start();
                startTimerButton.Content = "❚❚";
            }
        }

        private void stopTimer()
        {
            timerDisplayTimer.Stop();
            startTimerButton.Content = "▶";
            timerStart = new DateTime(0);
            timerOffset = new TimeSpan(0);
            timerEnd = new TimeSpan(0);
            timerDisplayhh.Text = "00";
            timerDisplaymm.Text = "00";
            timerDisplayss.Text = "00";
        }

        private TimeSpan getTimerEnd()
        {
            return TimeSpan.FromSeconds(Convert.ToInt32(timerDisplayss.Text) + Convert.ToInt32(timerDisplaymm.Text) * 60 + Convert.ToInt32(timerDisplayhh.Text) * 3600);
        }

        // ❚❚
    }
}
