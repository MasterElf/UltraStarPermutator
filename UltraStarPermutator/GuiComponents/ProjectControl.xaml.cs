using Microsoft.Win32;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace UltraStarPermutator
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        public ProjectControl()
        {
            InitializeComponent();
        }

        private void AddPartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ProjectModel projectModel)
            {
                projectModel.Parts.Add(new PartModel());
            }
        }

        private void SetTargetFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ProjectModel projectModel)
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = projectModel.TagetFolder;
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    projectModel.TagetFolder = dialog.FileName;
                    //MessageBox.Show(You selected: +dialog.FileName);
                }
            }
        }
    }
}