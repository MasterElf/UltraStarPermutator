using System;
using System.IO;

namespace UltraStarPermutator
{
    internal static class StorageHelper
    {
        internal static void SaveToFile(ProjectModel projectModel, string filePath)
        {
            if (projectModel != null && !string.IsNullOrEmpty(filePath))
            {
                // Get path
                string? path = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(path))
                {
                    // Assert that path exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Create serialized version of projectModel
                    byte[]? serialized = Serializer.Serialize(projectModel);

                    // Overwrite (possible existing) file
                    if (serialized?.Length > 0)
                    {
                        File.WriteAllBytes(filePath, serialized);
                    }
                }
            }
        }

        internal static ProjectModel? LoadFromFile(string fileName)
        {
            ProjectModel? projectModel = null;

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                var fileBytes = File.ReadAllBytes(fileName);

                projectModel = Serializer.Deserialize(fileBytes);
            }

            return projectModel;
        }
    }
}