using Synchronizer.Library;
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

namespace Synchronizer.Wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StackPanel ConsoleOutputStackPanel { get; private set; }
        public TextBlock ConsoleOutputTextBox { get; private set; }

        private StepsCollection steps;


        public MainWindow()
        {
            InitializeComponent();
            InitializeElements();
            InitializeSteps();
            InitializeEventHandlers();
            DisplayStepsListBox();
        }

        private void AddNewStep()
        {

        }

        private void DisplayStepsListBox()
        {
            StepsListBox.ItemsSource = steps.Steps;
        }

        private void InitializeSteps()
        {
            steps = new StepsCollection(true, 100);
        }

        private void InitializeEventHandlers()
        {
            BtnSynchronize.Click += RunSynchronizer;
        }

        public void InitializeElements()
        {
            MyMainWindow.ResizeMode = ResizeMode.NoResize;

            ConsoleOutputStackPanel = new StackPanel();
            ConsoleOutputStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            ConsoleOutputStackPanel.VerticalAlignment = VerticalAlignment.Top;

            ConsoleOutputTextBox = new TextBlock();
            ConsoleOutputTextBox.TextWrapping = TextWrapping.Wrap;
            ConsoleOutputTextBox.Margin = new Thickness(0, 0, 0, 20);
            ConsoleOutputTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;

            ConsoleOutputStackPanel.Children.Add(ConsoleOutputTextBox);
            ConsoleOutputStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            ConsoleOutputScrollViewer.Content = ConsoleOutputStackPanel;
        }

        private async void RunSynchronizer(object sender, RoutedEventArgs e)
        {
            BtnSynchronize.IsEnabled = false;
            var scriptResult = await Task.Run(() =>
            {
                var script = Script.BuildScript(steps);
                var scriptRunner = new ScriptRunner();

                var result = scriptRunner.RunScript(script);
                return result;
            });
            ConsoleOutputTextBox.Text = scriptResult;
            BtnSynchronize.IsEnabled = true;
        }
    }
}
