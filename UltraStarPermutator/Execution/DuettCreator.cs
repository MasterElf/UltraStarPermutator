using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace UltraStarPermutator
{
    internal class DuettCreator
    {
        internal static void Create(ProjectModel projectModel)
        {
            if (projectModel != null && Directory.Exists(projectModel.TagetFolder))
            {
                List<KaraokeTextFileModel> models = new List<KaraokeTextFileModel>();
                List<string> voiceNames = new List<string>();

                foreach (PartModel? part in projectModel.Parts)
                {
                    if (part != null && File.Exists(part.FilePath))
                    {
                        KaraokeTextFileModel model = ReadKaraokeTextFileModelFromFile(part.FilePath);
                        models.Add(model);
                        voiceNames.Add(part.Name ?? "Unknown");
                    }
                }

                KaraokeTextFileModel duettModel = CreateDuett(models, voiceNames);

                // Write the duett model to a text file
                string textFileName = projectModel.Name + " - Duett.txt";
                string destinationTextFile = Path.Combine(projectModel.TagetFolder, textFileName);
                File.WriteAllText(destinationTextFile, duettModel.GetText());
            }
        }

        private static KaraokeTextFileModel ReadKaraokeTextFileModelFromFile(string filePath)
        {
            return new KaraokeTextFileModel(File.ReadAllText(filePath));
        }

        public static KaraokeTextFileModel CreateDuett(List<KaraokeTextFileModel> models, List<string> voiceNames)
        {
            if (models.Count != voiceNames.Count)
            {
                throw new ArgumentException("The number of models must match the number of voice names.");
            }

            KaraokeTextFileModel duettModel = new KaraokeTextFileModel("");
            StringBuilder duettBody = new StringBuilder();

            // Choose the lowest GAP value
            double lowestGAP = double.MaxValue;
            foreach (var model in models)
            {
                if (model.Tags.TryGetValue(Tag.GAP, out string gapValue))
                {
                    gapValue = gapValue.Replace(',', '.'); // Replace comma with dot for parsing
                    if (double.TryParse(gapValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double gap))
                    {
                        if (gap < lowestGAP)
                        {
                            lowestGAP = gap;
                        }
                    }
                }
            }

            // Copy tags from the first model
            foreach (KeyValuePair<Tag, string> tag in models[0].Tags)
            {
                duettModel.SetTag(tag.Key, tag.Value);
            }

            // Update GAP tag
            duettModel.SetTag(Tag.GAP, lowestGAP.ToString(CultureInfo.InvariantCulture));

            // Combine bodies
            for (int i = 0; i < models.Count; i++)
            {
                duettBody.AppendLine($"{voiceNames[i]}:");

                foreach (var bodyRow in models[i].BodyRows)
                {
                    // Skip 'E' rows
                    if (bodyRow.NoteType == NoteType.LineBreak && bodyRow.Components.Length > 1 && bodyRow.Components[1] == "E")
                    {
                        continue;
                    }

                    // Adjust the time in the second column
                    if (bodyRow.Components.Length > 1)
                    {
                        int originalTime = int.Parse(bodyRow.Components[1]);
                        double gapDifference = lowestGAP - double.Parse(models[i].Tags[Tag.GAP].Replace(',', '.'), CultureInfo.InvariantCulture);
                        int adjustedTime = originalTime - (int)Math.Round(gapDifference);
                        bodyRow.Components[1] = adjustedTime.ToString();
                    }

                    duettBody.AppendLine(bodyRow.ToString());
                }
            }

            // Add final 'E' row
            duettBody.AppendLine("- E");

            // Set the duett body
            duettModel.SetBody(duettBody.ToString());

            return duettModel;
        }
    }
}