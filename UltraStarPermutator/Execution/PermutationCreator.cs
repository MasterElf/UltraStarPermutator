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
                        CreatePartPermutation(part, projectModel.TagetFolder, projectModel.Name);
                    }
                }
            }
        }

        private static void CreatePartPermutation(PartModel part, string destinationFolder, string? projectName)
        {
            if (part != null && File.Exists(part.FilePath) && Directory.Exists(destinationFolder) && !string.IsNullOrEmpty(projectName))
            {
                KaraokeTextFileModel karaokeTextFileModel = new KaraokeTextFileModel(File.ReadAllText(part.FilePath));

                foreach (var audio in part.AudioTracks)
                {
                    if (!string.IsNullOrEmpty(audio.FilePath))
                    {
                        // Set correct title
                        karaokeTextFileModel.SetTag(Tag.TITLE, projectName + " - " + part.Name + " - " + audio.Name);

                        // Set correct MP3 file name in text file
                        string audioFileName = projectName + " - " + part.Name + " - " + audio.Name + ".mp3";
                        karaokeTextFileModel.SetTag(Tag.MP3, audioFileName);

                        // Copy mp3 file
                        if (File.Exists(audio.FilePath))
                        {
                            string destinationAudioFile = Path.Combine(destinationFolder, audioFileName);
                            File.Copy(audio.FilePath, destinationAudioFile, true);
                        }

                        // Write text file
                        string textFileName = projectName + " - " + part.Name + " - " + audio.Name + ".txt";
                        string destinationTextFile = Path.Combine(destinationFolder, textFileName);
                        File.WriteAllText(destinationTextFile, karaokeTextFileModel.GetText());
                    }
                }
            }
        }
    }
}