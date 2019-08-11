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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections;

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
            InitializeStepsCollection();
            InitializeEventHandlers();
            DisplayStepsListBox();
        }

        private void DisplayStepsListBox()
        {
            StepsListBox.ItemsSource = null;
            StepsListBox.ItemsSource = steps.Steps;
        }

        private void InitializeStepsCollection()
        {
            //steps = new StepsCollection(true, 3); // mock
            steps = new StepsCollection();
        }

        private void InitializeEventHandlers()
        {
            this.Loaded += DisableEditMode;
            BtnSynchronize.Click += RunSynchronizer;
            BtnLoadSource.Click += LoadSource;
            BtnLoadDestination.Click += LoadDestination;
            StepsListBox.SelectionChanged += DisplaySelectedItem;
            BtnAddStep.Click += PrepareFormForNewStep;
            BtnAddStep.Click += EnableEditMode;
            BtnEdit.Click += EnableEditMode;
            BtnCancel.Click += DisableEditMode;
            BtnSaveChanges.Click += SaveChanges;
            BtnSaveChanges.Click += PrepareFormForNewStep;
            BtnSaveChanges.Click += DisableEditMode;
            BtnDeleteStep.Click += DeleteStep;
            BtnDeleteStep.Click += DisableEditMode;
        }

        private void DeleteStep(object sender, RoutedEventArgs e)
        {
            var selectedStep = StepsListBox.SelectedItem as Step;
            if(selectedStep != null)
            {
                steps.Steps.Remove(selectedStep);
            }
            DisplayStepsListBox();
        }

        private void PrepareFormForNewStep(object sender, RoutedEventArgs e)
        {
            StepsListBox.UnselectAll();
            SourceTextBox.Text = "";
            SourceNameTextBox.Text = "";
            DestinationTextBox.Text = "";
            DestinationNameTextBox.Text = "";
            IsEnabledRadio.IsChecked = true;
            IsDisabledRadio.IsChecked = false;
            BtnEdit.IsEnabled = false;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            var newStep = new Step()
            {
                SourceDirectory = SourceTextBox.Text,
                SourceName = SourceNameTextBox.Text,
                DestinationDirectory = DestinationTextBox.Text,
                DestinationName = DestinationNameTextBox.Text,
                IsEnabled = (bool)IsEnabledRadio.IsChecked
            };

            var selectedStep = StepsListBox.SelectedItem as Step;
            if (selectedStep != null)
            {
                selectedStep.SourceDirectory = newStep.SourceDirectory;
                selectedStep.SourceName = newStep.SourceName;
                selectedStep.DestinationDirectory = newStep.DestinationDirectory;
                selectedStep.DestinationName = newStep.DestinationName;
                selectedStep.IsEnabled = newStep.IsEnabled;
            }
            else
            {
                steps.AddNew(newStep);
            }
            
            DisplayStepsListBox();
        }

        private void EnableEditMode(object sender, RoutedEventArgs e)
        {
            foreach(var button in GetButtonsForEditMode())
            {
                button.IsEnabled = true;
            }

            foreach(var textBox in GetTextBoxesForEditMode())
            {
                textBox.IsEnabled = true;
            }

            IsEnabledRadio.IsEnabled = true;
            IsDisabledRadio.IsEnabled = true;
            BtnEdit.IsEnabled = false;
            BtnAddStep.IsEnabled = false;
        }

        private void DisableEditMode(object sender, RoutedEventArgs e)
        {
            foreach (var button in GetButtonsForEditMode())
            {
                button.IsEnabled = false;
            }

            foreach (var textBox in GetTextBoxesForEditMode())
            {
                textBox.IsEnabled = false;
            }

            IsEnabledRadio.IsEnabled = false;
            IsDisabledRadio.IsEnabled = false;
            BtnEdit.IsEnabled = true;
            BtnAddStep.IsEnabled = true;
        }

        private void DisplaySelectedItem(object sender, RoutedEventArgs e)
        {
            var selectedStep = StepsListBox.SelectedItem as Step;
            if (selectedStep is null)
            {
                return;
            }
            BtnEdit.IsEnabled = true;

            SourceTextBox.Text = selectedStep.SourceDirectory;
            SourceNameTextBox.Text = selectedStep.SourceName;
            DestinationTextBox.Text = selectedStep.DestinationDirectory;
            DestinationNameTextBox.Text = selectedStep.DestinationName;
            if (selectedStep.IsEnabled)
            {
                IsEnabledRadio.IsChecked = true;
            }
            else
            {
                IsDisabledRadio.IsChecked = true;
            }
        }

        private void LoadSource(object sender, RoutedEventArgs e)
        {
            var result = LoadPath();
            SourceTextBox.Text = result;
        }

        private void LoadDestination(object sender, RoutedEventArgs e)
        {
            var result = LoadPath();
            DestinationTextBox.Text = result;
        }

        public void InitializeElements()
        {
            MyMainWindow.ResizeMode = ResizeMode.NoResize;
            StepsListBox.SelectionMode = SelectionMode.Single;

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
            DisableAllButtons();
            var scriptResult = await Task.Run(() =>
            {
                var script = Script.BuildScript(steps);
                var scriptRunner = new ScriptRunner();

                var result = scriptRunner.RunScript(script);
                return result;
            });
            ConsoleOutputTextBox.Text = scriptResult;
            EnableAllButtons();
        }

        private string LoadPath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.EnsureReadOnly = true;
            dialog.AllowNonFileSystemItems = false;
            dialog.Multiselect = false;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            else
            {
                return "Error :(";
            }
        }

        private IEnumerable<Button> GetButtonsForEditMode()
        {
            var list = new List<Button>();
            list.Add(BtnCancel);
            list.Add(BtnSaveChanges);
            list.Add(BtnLoadSource);
            list.Add(BtnLoadDestination);
            list.Add(BtnDeleteStep);

            return list;
        }

        private IEnumerable<TextBox> GetTextBoxesForEditMode()
        {
            var list = new List<TextBox>();
            list.Add(SourceTextBox);
            list.Add(SourceNameTextBox);
            list.Add(DestinationTextBox);
            list.Add(DestinationNameTextBox);

            return list;
        }

        private void DisableAllButtons()
        {
            List<Button> bunchOfButtons = new List<Button>();
            GetLogicalChildCollection(this, bunchOfButtons);
            foreach (var button in bunchOfButtons)
            {
                button.IsEnabled = false;
            }
        }

        private void EnableAllButtons()
        {
            List<Button> bunchOfButtons = new List<Button>();
            GetLogicalChildCollection(this, bunchOfButtons);
            foreach (var button in bunchOfButtons)
            {
                button.IsEnabled = true;
            }
        }

        private void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }
    }
}
