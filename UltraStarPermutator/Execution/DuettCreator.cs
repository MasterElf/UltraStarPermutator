using System;
using System.Collections.Generic;
using System.Text;

namespace UltraStarPermutator
{
    internal class DuettCreator
    {
        public static KaraokeTextFileModel CreateDuett(List<KaraokeTextFileModel> models, List<string> voiceNames)
        {
            if (models.Count != voiceNames.Count)
            {
                throw new ArgumentException("The number of models must match the number of voice names.");
            }

            KaraokeTextFileModel duettModel = new KaraokeTextFileModel("");
            StringBuilder duettBody = new StringBuilder();

            // Choose the lowest GAP value
            int lowestGAP = int.MaxValue;
            foreach (var model in models)
            {
                if (model.Tags.TryGetValue(Tag.GAP, out string gapValue))
                {
                    int gap = int.Parse(gapValue);
                    if (gap < lowestGAP)
                    {
                        lowestGAP = gap;
                    }
                }
            }

            // Copy tags from the first model
            foreach (KeyValuePair<Tag, string> tag in models[0].Tags)
            {
                duettModel.SetTag($"#{tag.Key}:{tag.Value}");
            }

            // Update GAP tag
            duettModel.SetTag($"#GAP:{lowestGAP}");

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
                        int adjustedTime = originalTime - (lowestGAP - int.Parse(models[i].Tags[Tag.GAP]));
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