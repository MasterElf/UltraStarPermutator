using CommunityToolkit.Mvvm.ComponentModel;

namespace UltraStarPermutator
{
    public class AudioModel : ObservableObject
    {
        private string? filePath;

        public string? FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }

        private string? name;

        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
    }
}