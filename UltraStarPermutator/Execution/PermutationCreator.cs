using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UltraStarPermutator
{
    internal static class PermutationCreator
    {
        internal static void Create(ProjectModel? projectModel)
        {
            if (projectModel != null && Directory.Exists(projectModel.TagetFolder))
            {
                ProjectModel? modelToPermutate = projectModel;

                if (projectModel.CreateDuets)
                {
                    // Clone source to modelToPermutate
                    modelToPermutate = Serializer.DeepCopyWithXml(projectModel);

                    // Create duettes in modelToPermutate
                    CreateDuets(projectModel, modelToPermutate);
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

        private static void CreatePartPermutation(PartModel part, ProjectModel? projectModel)
        {
            if (part != null && part.HaveFileData() && Directory.Exists(projectModel.TagetFolder) &&
                !string.IsNullOrEmpty(projectModel.Name)) // Använder HaveFileData
            {
                string fileContent = part.ReadFileData(); // Använder ReadFileData
                KaraokeTextFileModel karaokeTextFileModel =
                    new KaraokeTextFileModel(fileContent, part.AssertTrailingSpace);

                foreach (var audio in part.AudioTracks)
                {
                    if (!string.IsNullOrEmpty(audio.FilePath))
                    {
                        // Set correct title
                        karaokeTextFileModel.SetTag(Tag.TITLE,
                            projectModel.Name + " - " + part.Name + " - " + audio.Name);

                        // Set correct MP3 file name in text file
                        CopyAndReferenceFile(projectModel.Name + " - " + part.Name + " - " + audio.Name + ".mp3",
                            projectModel, karaokeTextFileModel, audio.FilePath, Tag.MP3);

                        // Set correct #BACKGROUND
                        AddTextAndSaveImage(projectModel.Name + " - " + part.Name + " - " + audio.Name + ".png",
                            projectModel, karaokeTextFileModel, projectModel.BackgroundFilePath, Tag.BACKGROUND, 16.0/9, "P1", "P2");
                        //CopyAndReferenceFile(Path.GetFileName(projectModel.BackgroundFilePath), projectModel, karaokeTextFileModel, projectModel.BackgroundFilePath, Tag.BACKGROUND);

                        // Set correct #COVER
                        CopyAndReferenceFile(Path.GetFileName(projectModel.CoverFilePath), projectModel,
                            karaokeTextFileModel, projectModel.CoverFilePath, Tag.COVER);

                        // Write text file
                        string textFileName = projectModel.Name + " - " + part.Name + " - " + audio.Name + ".txt";
                        string destinationTextFile = Path.Combine(projectModel.TagetFolder, textFileName);
                        File.WriteAllText(destinationTextFile, karaokeTextFileModel.GetText());
                    }
                }
            }
        }

        private static void CreateDuets(ProjectModel? source, ProjectModel? destination)
        {
            var partModels = source.Parts.ToArray();

            for (int i = 0; i < partModels.Length; i++)
            {
                for (int j = i + 1; j < partModels.Length; j++)
                {
                    PartModel part1 = partModels[i];
                    PartModel part2 = partModels[j];

                    if (part1.HaveFileData() && part2.HaveFileData())
                    {
                        // Read file data into KaraokeTextFileModels
                        KaraokeTextFileModel model1 =
                            new KaraokeTextFileModel(part1.ReadFileData(), part1.AssertTrailingSpace);
                        KaraokeTextFileModel model2 =
                            new KaraokeTextFileModel(part2.ReadFileData(), part2.AssertTrailingSpace);

                        // Create the duet model
                        List<KaraokeTextFileModel> models = new List<KaraokeTextFileModel> { model1, model2 };
                        //List<string> voiceNames = new List<string> { part1.Name ?? "Unknown", part2.Name ?? "Unknown" };
                        List<string> voiceNames = new List<string> { "P1", "P2" };
                        KaraokeTextFileModel duetModel = DuettCreator.CreateDuett(models, voiceNames);

                        // Serialize the duet model to a string
                        string duetText = duetModel.GetText();

                        // Convert the string to a MemoryStream
                        MemoryStream duetStream = new MemoryStream(Encoding.UTF8.GetBytes(duetText));

                        // Create a new PartModel for the duet
                        PartModel duetPart = new PartModel
                        {
                            Name = part1.Name + " & " + part2.Name,
                            FileData = duetStream
                        };

                        // Copy AudioTracks from part1 to duetPart
                        foreach (var audioTrack in part1.AudioTracks)
                        {
                            duetPart.AudioTracks.Add(audioTrack);
                        }

                        // Add the new PartModel to the destination ProjectModel
                        destination.Parts.Add(duetPart);
                    }
                }
            }
        }

        private static void CopyAndReferenceFile(string? wantedFileName, ProjectModel? projectModel,
            KaraokeTextFileModel karaokeTextFileModel, string? sourceFilePath, Tag tag)
        {
            if (projectModel != null &&
                karaokeTextFileModel != null &&
                !string.IsNullOrEmpty(wantedFileName) &&
                !string.IsNullOrEmpty(projectModel.TagetFolder))
            {
                karaokeTextFileModel.SetTag(tag, wantedFileName);

                if (File.Exists(sourceFilePath))
                {
                    string destinationAudioFile = Path.Combine(projectModel.TagetFolder, wantedFileName);
                    File.Copy(sourceFilePath, destinationAudioFile, true);
                }
            }
        }

        /// <summary>
        /// Adds text to an image and saves it to a new file.
        /// </summary>
        /// <param name="wantedFileName">The name of the new image file.</param>
        /// <param name="projectModel">The project model containing target folder information.</param>
        /// <param name="karaokeTextFileModel">The karaoke text file model for setting tags.</param>
        /// <param name="sourceFilePath">The source file path of the image.</param>
        /// <param name="tag">The tag to set in the karaoke text file model.</param>
        /// <param name="aspectRatio">The aspect ratio to use for cropping the image. Format is width/height.</param>
        /// <param name="topLeftText">The text to display at the top-left corner of the image.</param>
        /// <param name="bottomCenterText">The text to display at the bottom-center of the image.</param>
        private static void AddTextAndSaveImage(string? wantedFileName, ProjectModel? projectModel, KaraokeTextFileModel karaokeTextFileModel, string? sourceFilePath, Tag tag, double aspectRatio, string topLeftText, string bottomCenterText)
        {
            if (projectModel != null &&
                karaokeTextFileModel != null &&
                !string.IsNullOrEmpty(wantedFileName) &&
                !string.IsNullOrEmpty(projectModel.TagetFolder))
            {
                karaokeTextFileModel.SetTag(tag, wantedFileName);

                if (File.Exists(sourceFilePath))
                {
                    string destinationImageFile = Path.Combine(projectModel.TagetFolder, wantedFileName);

                    using (SKBitmap srcBitmap = SKBitmap.Decode(sourceFilePath))
                    {
                        // Calculate the dimensions for cropping based on the aspect ratio
                        int newWidth = srcBitmap.Width;
                        int newHeight = (int)(newWidth / aspectRatio);

                        if (newHeight > srcBitmap.Height)
                        {
                            newHeight = srcBitmap.Height;
                            newWidth = (int)(newHeight * aspectRatio);
                        }

                        // Create a new cropped bitmap
                        SKRectI cropRect = new SKRectI(0, 0, newWidth, newHeight);
                        SKBitmap croppedBitmap = new SKBitmap(cropRect.Width, cropRect.Height);
                        srcBitmap.ExtractSubset(croppedBitmap, cropRect);

                        using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                        {
                            SKPaint paint = new SKPaint
                            {
                                Color = SKColors.White,
                                IsAntialias = true,
                                Style = SKPaintStyle.Fill,
                                TextAlign = SKTextAlign.Left,
                                TextSize = 240
                            };

                            // Draw outline
                            SKPaint outlinePaint = new SKPaint
                            {
                                Color = SKColors.Black,
                                IsAntialias = true,
                                Style = SKPaintStyle.Stroke,
                                StrokeWidth = 4,
                                TextAlign = SKTextAlign.Left,
                                TextSize = 240
                            };

                            // Draw shadow
                            SKPaint shadowPaint = new SKPaint
                            {
                                Color = SKColors.Gray,
                                IsAntialias = true,
                                Style = SKPaintStyle.Fill,
                                TextAlign = SKTextAlign.Left,
                                TextSize = 240
                            };

                            float xPos = 10;

                            // Draw top-left text
                            canvas.DrawText(topLeftText, xPos, 250 + 5, shadowPaint);
                            canvas.DrawText(topLeftText, xPos, 250, outlinePaint);
                            canvas.DrawText(topLeftText, xPos, 250, paint);

                            // Draw bottom-center text
                            //float x = croppedBitmap.Width / 2;
                            float y = croppedBitmap.Height - 100;

                            paint.TextAlign = SKTextAlign.Center;
                            outlinePaint.TextAlign = SKTextAlign.Center;

                            canvas.DrawText(bottomCenterText, xPos, y + 5, shadowPaint);
                            canvas.DrawText(bottomCenterText, xPos, y, outlinePaint);
                            canvas.DrawText(bottomCenterText, xPos, y, paint);
                        }

                        using (SKFileWStream outStream = new SKFileWStream(destinationImageFile))
                        {
                            SKPixmap.Encode(outStream, croppedBitmap, SKEncodedImageFormat.Png, 100);
                        }
                    }
                }
            }
        }
    }
}
