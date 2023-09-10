﻿using System.IO;
using System.Linq;

namespace UltraStarPermutator
{
    internal static class PermutationCreator
    {
        internal static void Create(ProjectModel projectModel)
        {
            if (projectModel != null && Directory.Exists(projectModel.TagetFolder))
            {
                ProjectModel modelToPermutate = projectModel;

                if (projectModel.CreateDuettes)
                {
                    // Clone source to modelToPermutate
                    modelToPermutate = Serializer.DeepCopyWithXml(projectModel);

                    // Create duettes in modelToPermutate
                    CreateDuettes(projectModel, modelToPermutate);
                }

                foreach (PartModel? part in modelToPermutate.Parts)
                {
                    if (part != null && part.HaveFileData()) // Använder HaveFileData
                    {
                        CreatePartPermutation(part, modelToPermutate);
                    }
                }
            }
        }

        private static void CreatePartPermutation(PartModel part, ProjectModel projectModel)
        {
            if (part != null && part.HaveFileData() && Directory.Exists(projectModel.TagetFolder) && !string.IsNullOrEmpty(projectModel.Name)) // Använder HaveFileData
            {
                string fileContent = part.ReadFileData(); // Använder ReadFileData
                KaraokeTextFileModel karaokeTextFileModel = new KaraokeTextFileModel(fileContent);

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

        private static void CreateDuettes(ProjectModel source, ProjectModel destination)
        {
            var partModels = source.Parts.ToArray();

            for (int i = 0; i < partModels.Length; i++)
            {
                for (int j = i + 1; j < partModels.Length; j++)
                {
                    // Create a new ProjectModel for the duet
                    ProjectModel duetProjectModel = Serializer.DeepCopyWithXml(source);

                    // Clear existing parts and add only the two parts for the duet
                    duetProjectModel.Parts.Clear();
                    duetProjectModel.Parts.Add(partModels[i]);
                    duetProjectModel.Parts.Add(partModels[j]);

                    // Create the duet
                    DuettCreator.Create(duetProjectModel);
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
