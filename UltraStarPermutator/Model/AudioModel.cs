using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace UltraStarPermutator
{
    [Serializable]
    public class AudioModel : ObservableObject
    {
        private string? name;

        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string? filePath;

        public string? FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }
    }
}