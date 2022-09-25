using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace UltraStarPermutator
{
    /// <summary>
    /// Interaction logic for AudioControl.xaml
    /// </summary>
    public partial class AudioControl : UserControl
    {
        public AudioControl()
        {
            InitializeComponent();
        }

        private void FindAudioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is AudioModel audioModel)
            {
                // Open load dialogue
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "MP3 audio|*.mp3";
                openFileDialog.DefaultExt = "mp3";
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.InitialDirectory = Path.GetDirectoryName( audioModel.FilePath);

                if (openFileDialog.ShowDialog() == true)
                {
                    // Load model from file
                    audioModel.FilePath = openFileDialog.FileName;
                }
            }
        }
    }
}