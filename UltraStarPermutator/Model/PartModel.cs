using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace UltraStarPermutator
{
    [Serializable]
    public class PartModel : ObservableObject
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

        [XmlIgnore]
        public ObservableCollection<AudioModel> AudioTracks { get; private set; } = new ObservableCollection<AudioModel>();

        #region Parts serialization
        public AudioModel[] SerializedAudioTracks
        {
            get
            {
                var partsArray = new AudioModel[AudioTracks.Count];
                AudioTracks.CopyTo(partsArray, 0);
                return partsArray;
            }
            set { AudioTracks = new ObservableCollection<AudioModel>(value); }
        }
        #endregion
    }
}