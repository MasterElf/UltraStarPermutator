using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace UltraStarPermutator
{
    /// <summary>
    /// Interaction logic for PartControl.xaml
    /// </summary>
    public partial class PartControl : UserControl
    {
        public PartControl()
        {
            InitializeComponent();
        }

        private void AddAudioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is PartModel partModel)
            {
                partModel.AudioTracks.Add(new AudioModel());
            }
        }

        private void FindPartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is PartModel partModel)
            {
                // Open load dialogue
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "UltraStar|*.txt";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.InitialDirectory = Path.GetDirectoryName(partModel.FilePath);

                if (openFileDialog.ShowDialog() == true)
                {
                    // Load model from file
                    partModel.FilePath = openFileDialog.FileName;
                }
            }
        }
    }
}