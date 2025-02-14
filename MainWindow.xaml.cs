using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace TempCleanerLite
{
    public partial class MainWindow : Window
    {
        private Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            StartBackgroundAnimation();
            OverallProgressBar.IsIndeterminate = true;
        }

        private void StartBackgroundAnimation()
        {
            // Position the circles randomly
            PositionCircle(Circle1);
            PositionCircle(Circle2);
            PositionCircle(Circle3);

            // Animate Circle1
            AnimateCircle(Circle1Brush, Colors.Blue, Colors.Purple, TimeSpan.FromSeconds(5));
            AnimateOpacity(Circle1, TimeSpan.FromSeconds(5));
            AnimateMovement(Circle1, TimeSpan.FromSeconds(10));

            // Animate Circle2
            AnimateCircle(Circle2Brush, Colors.Red, Colors.Orange, TimeSpan.FromSeconds(7));
            AnimateOpacity(Circle2, TimeSpan.FromSeconds(7));
            AnimateMovement(Circle2, TimeSpan.FromSeconds(12));

            // Animate Circle3
            AnimateCircle(Circle3Brush, Colors.Green, Colors.Yellow, TimeSpan.FromSeconds(9));
            AnimateOpacity(Circle3, TimeSpan.FromSeconds(9));
            AnimateMovement(Circle3, TimeSpan.FromSeconds(14));
        }

        private void PositionCircle(Ellipse circle)
        {
            // Randomly position the circle within the window
            double x = _random.NextDouble() * 400 - 100;
            double y = _random.NextDouble() * 300 - 100;

            Canvas.SetLeft(circle, x);
            Canvas.SetTop(circle, y);
        }

        private void AnimateCircle(SolidColorBrush brush, Color fromColor, Color toColor, TimeSpan duration)
        {
            // Color animation
            ColorAnimation colorAnimation = new ColorAnimation
            {
                From = fromColor,
                To = toColor,
                Duration = duration,
                AutoReverse = true, // Reverse the animation
                RepeatBehavior = RepeatBehavior.Forever // Loop forever
            };

            // Apply the animation to the brush's color
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private void AnimateOpacity(Ellipse circle, TimeSpan duration)
        {
            // Opacity animation
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0.5,
                To = 0,
                Duration = duration,
                AutoReverse = true, // Reverse the animation
                RepeatBehavior = RepeatBehavior.Forever // Loop forever
            };

            // Apply the animation to the circle's opacity
            circle.BeginAnimation(Ellipse.OpacityProperty, opacityAnimation);
        }

        private void AnimateMovement(Ellipse circle, TimeSpan duration)
        {
            // Randomly generate new target positions
            double newX = _random.NextDouble() * 400 - 50;
            double newY = _random.NextDouble() * 400 - 50;

            // Animate the circle's position
            DoubleAnimation xAnimation = new DoubleAnimation
            {
                To = newX,
                Duration = duration,
                AutoReverse = true, // Reverse the animation
                RepeatBehavior = RepeatBehavior.Forever // Loop forever
            };

            DoubleAnimation yAnimation = new DoubleAnimation
            {
                To = newY,
                Duration = duration,
                AutoReverse = true, // Reverse the animation
                RepeatBehavior = RepeatBehavior.Forever // Loop forever
            };

            // Apply the animations to the circle's position
            circle.BeginAnimation(Canvas.LeftProperty, xAnimation);
            circle.BeginAnimation(Canvas.TopProperty, yAnimation);
        }

        private async void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            CleanButton.IsEnabled = false; // Disable the button during cleanup
            StatusText.Text = "Cleaning...";
            OverallProgressBar.Value = 0;
            CurrentFolderProgressBar.Value = 0;
            OverallProgressText.Text = "0%";
            CurrentFolderProgressText.Text = "0%";

            try
            {
                // Run the cleanup process asynchronously
                await CleanTempFoldersAsync();

                StatusText.Text = "Cleaned!";
            }
            catch (Exception)
            {
            }
            finally
            {
                CleanButton.IsEnabled = true; // Re-enable the button after cleanup
            }
        }

        private async Task CleanTempFoldersAsync()
        {
            OverallProgressBar.IsIndeterminate = true;
            // Get the list of temporary folders based on the checkbox state
            List<string> tempFolders = GetTempFolders();

            int totalFolders = tempFolders.Count;
            int foldersProcessed = 0;

            // Clean each folder
            foreach (var folder in tempFolders)
            {
                OverallProgressBar.IsIndeterminate = true;

                if (!Directory.Exists(folder))
                {
                    foldersProcessed++;
                    UpdateOverallProgress(foldersProcessed, totalFolders);
                    continue;
                }

                StatusText.Text += $"\nCleaning...";

                // Get files and directories in the current folder
                string[] files = Directory.GetFiles(folder);
                string[] directories = Directory.GetDirectories(folder);

                int totalElements = files.Length + directories.Length;
                int elementsProcessed = 0;
                OverallProgressBar.IsIndeterminate = false;

                // Delete files in the current folder
                foreach (string file in files)
                {
                    try
                    {
                        await Task.Run(() => File.Delete(file));
                        elementsProcessed++;
                        UpdateCurrentFolderProgress(elementsProcessed, totalElements);
                        UpdateInfo(elementsProcessed, totalElements, foldersProcessed, totalFolders);
                    }
                    catch (Exception)
                    {
                    }
                }

                // Delete directories in the current folder
                foreach (string dir in directories)
                {
                    try
                    {
                        await Task.Run(() => Directory.Delete(dir, true));
                        elementsProcessed++;
                        UpdateCurrentFolderProgress(elementsProcessed, totalElements);
                        UpdateInfo(elementsProcessed, totalElements, foldersProcessed, totalFolders);
                    }
                    catch (Exception)
                    {
                    }
                }

                OverallProgressBar.IsIndeterminate = true;

                CurrentFolderProgressBar.Value = 100;
                CurrentFolderProgressText.Text = "100%";

                foldersProcessed++;
                UpdateInfo(elementsProcessed, totalElements, foldersProcessed, totalFolders);
                UpdateOverallProgress(foldersProcessed, totalFolders);
            }

            OverallProgressBar.Value = 100;
            OverallProgressText.Text = "100%";
            UpdateInfo(0, 0, totalFolders, totalFolders);
            OverallProgressBar.IsIndeterminate = true;
        }

        private List<string> GetTempFolders()
        {
            List<string> tempFolders = new List<string>();

            if (CleanAllTempFoldersCheckBox.IsChecked == true)
            {
                // Clean all temporary folders
                tempFolders = new List<string>
                {
                    Path.GetTempPath(), // User's TEMP folder
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp") // System TEMP folder (alternative)
                };
            }
            else
            {
                // Clean only the local user's TEMP folder
                tempFolders.Add(Path.GetTempPath());
            }

            // Remove duplicates and invalid paths
            tempFolders = tempFolders
                .Where(folder => !string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                .Distinct()
                .ToList();

            return tempFolders;
        }

        private void UpdateOverallProgress(int foldersProcessed, int totalFolders)
        {
            double progress = (double)foldersProcessed / totalFolders * 100;
            OverallProgressBar.Value = progress;
            OverallProgressText.Text = $"{progress:0}%";
            StatusText.Text = $"Cleaning items...";
        }

        private void UpdateCurrentFolderProgress(int elementsProcessed, int totalElements)
        {
            double progress = (double)elementsProcessed / totalElements * 100;
            CurrentFolderProgressBar.Value = progress;
            CurrentFolderProgressText.Text = $"{progress:0}%";
            StatusText.Text = $"Cleaning items...";
        }

        private void UpdateInfo(int itemsProcessed, int itemsTotal, int foldersProcessed, int totalFolders)
        {
            InfoText.Text = $"{foldersProcessed}/{totalFolders} folders, {itemsProcessed}/{itemsTotal} items";
        }

        // Custom Top Bar Functionality
        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Allow the window to be dragged
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Minimize the window
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }
    }
}