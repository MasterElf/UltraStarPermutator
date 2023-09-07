using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace UltraStarPermutator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProjectModel? projectModel = new ProjectModel();

        string lastProjectFileName;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = projectModel;
        }

        private void LoadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Open load dialogue
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "UltraStar permutator|*.usp";
            openFileDialog.DefaultExt = "usp";
            openFileDialog.CheckPathExists = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.InitialDirectory = Path.GetDirectoryName(lastProjectFileName);

            if (openFileDialog.ShowDialog() == true)
            {
                // Load model from file
                projectModel = StorageHelper.LoadFromFile(openFileDialog.FileName);
                DataContext = projectModel;
                lastProjectFileName = openFileDialog.FileName;
            }
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (projectModel != null)
            {
                // Open save dialogue
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "UltraStar permutator|*.usp";
                saveFileDialog.DefaultExt = "usp";
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.FileName = lastProjectFileName;
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(lastProjectFileName);

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Save model to file
                    StorageHelper.SaveToFile(projectModel!, saveFileDialog.FileName);
                    lastProjectFileName = saveFileDialog.FileName;
                }
            }
        }

        private void ExecuteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (projectModel != null)
            {
                PermutationCreator.Create(projectModel);
            }
        }

        private void CreateMixesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (projectModel != null)
            {
                PermutationCreator.Create(projectModel);
            }
        }
    }
}