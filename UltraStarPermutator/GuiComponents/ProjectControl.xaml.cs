﻿using Microsoft.Win32;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

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

        private void SetBackgroundFilePathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ProjectModel projectModel)
            {
                // Open load dialogue
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Jpeg|*.jpg|Png|*.png";
                openFileDialog.DefaultExt = "jpg";
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.InitialDirectory = Path.GetDirectoryName(projectModel.BackgroundFilePath);

                if (openFileDialog.ShowDialog() == true)
                {
                    // Load model from file
                    projectModel.BackgroundFilePath = openFileDialog.FileName;
                }
            }
        }

        private void SetCoverFilePathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ProjectModel projectModel)
            {
                // Open load dialogue
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Jpeg|*.jpg|Png|*.png";
                openFileDialog.DefaultExt = "jpg";
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.InitialDirectory = Path.GetDirectoryName(projectModel.CoverFilePath);

                if (openFileDialog.ShowDialog() == true)
                {
                    // Load model from file
                    projectModel.CoverFilePath = openFileDialog.FileName;
                }
            }
        }
    }
}