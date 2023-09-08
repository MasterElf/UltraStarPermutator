using System;
using System.Collections.Generic;
using System.Text;

namespace UltraStarPermutator
{
    internal class DuettCreator
    {
        public static KaraokeTextFileModel CreateDuett(List<KaraokeTextFileModel> models, List<string> voiceNames)
        {
            if (models == null || models.Count == 0 || voiceNames.Count != models.Count)
            {
                throw new ArgumentException("Invalid input models or voice names.");
            }

            // Create a new KaraokeTextFileModel to hold the duet
            KaraokeTextFileModel duetModel = new KaraokeTextFileModel("");

            // Copy tags from the first model
            string firstModelText = models[0].GetText();
            string[] firstModelLines = firstModelText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in firstModelLines)
            {
                if (line.StartsWith("#"))
                {
                    duetModel.SetTag(line);
                }
            }

            // Combine the body parts
            StringBuilder duetBody = new StringBuilder();
            for (int i = 0; i < models.Count; i++)
            {
                duetBody.AppendLine($"{voiceNames[i]}:"); // Add the name of the voice
                string modelText = models[i].GetText();
                string[] modelLines = modelText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in modelLines)
                {
                    if (!line.StartsWith("#"))
                    {
                        duetBody.AppendLine(line);
                    }
                }
            }

            // Add the "E" tag at the end
            duetBody.AppendLine("E");

            // Set the body part to the duet model
            duetModel.SetBody(duetBody.ToString());

            return duetModel;
        }
    }
}