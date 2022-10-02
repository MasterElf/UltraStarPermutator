using System;
using System.IO;

namespace UltraStarPermutator
{
    internal static class PermutationCreator
    {
        internal static void Create(ProjectModel projectModel)
        {
            if (projectModel != null && Directory.Exists(projectModel.TagetFolder))
            {
                foreach (PartModel? part in projectModel.Parts)
                {
                    if (part != null)
                    {
                        CreatePartPermutation(part, projectModel);
                    }
                }
            }
        }

        private static void CreatePartPermutation(PartModel part, ProjectModel projectModel)
        {
            if (part != null && File.Exists(part.FilePath) && Directory.Exists(projectModel.TagetFolder) && !string.IsNullOrEmpty(projectModel.Name))
            {
                KaraokeTextFileModel karaokeTextFileModel = new KaraokeTextFileModel(File.ReadAllText(part.FilePath));

                foreach (var audio in part.AudioTracks)
                {
                    if (!string.IsNullOrEmpty(audio.FilePath))
                    {
                        // Set correct title
                        karaokeTextFileModel.SetTag(Tag.TITLE, projectModel.Name + " - " + part.Name + " - " + audio.Name);

                        // Set correct MP3 file name in text file
                        CopyAndReferenceFile(projectModel.Name + " - " + part.Name + " - " + audio.Name + ".mp3", projectModel, karaokeTextFileModel, audio.FilePath, Tag.MP3);

                        // Set correct #BACKGROUND
                        CopyAndReferenceFile(Path.GetFileName(projectModel.BackgroundFilePath), projectModel, karaokeTextFileModel, projectModel.BackgroundFilePath, Tag.BACKGROUND);

                        // Set correct #COVER
                        CopyAndReferenceFile(Path.GetFileName(projectModel.CoverFilePath), projectModel, karaokeTextFileModel, projectModel.CoverFilePath, Tag.COVER);

                        // Write text file
                        string textFileName = projectModel.Name + " - " + part.Name + " - " + audio.Name + ".txt";
                        string destinationTextFile = Path.Combine(projectModel.TagetFolder, textFileName);
                        File.WriteAllText(destinationTextFile, karaokeTextFileModel.GetText());
                    }
                }
            }
        }

        private static void CopyAndReferenceFile(string? wantedFileName, ProjectModel projectModel, KaraokeTextFileModel karaokeTextFileModel, string? sourceFilePath, Tag tag)
        {
            if (projectModel != null &&
                karaokeTextFileModel != null &&
                !string.IsNullOrEmpty(wantedFileName) &&
                !string.IsNullOrEmpty(projectModel.TagetFolder))
            {
                karaokeTextFileModel.SetTag(tag, wantedFileName);

                // Copy mp3 file
                if (File.Exists(sourceFilePath))
                {
                    string destinationAudioFile = Path.Combine(projectModel.TagetFolder, wantedFileName);
                    File.Copy(sourceFilePath, destinationAudioFile, true);
                }
            }
        }
    }
}