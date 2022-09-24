using CommunityToolkit.Mvvm.ComponentModel;

namespace UltraStarPermutator
{
    public class ProjectModel : ObservableObject
    {
        private string? sourceUltraStarTextFilePath;

        public string? SourceUltraStarTextFilePath
        {
            get => sourceUltraStarTextFilePath;
            set => SetProperty(ref sourceUltraStarTextFilePath, value);
        }
    }
}