using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace UltraStarPermutator
{
    [Serializable]
    public class ProjectModel : ObservableObject
    {
        private string? name;

        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        [XmlIgnore]
        public ObservableCollection<PartModel> Parts { get; private set; } = new ObservableCollection<PartModel>();

        #region Parts serialization
        public PartModel[] SerializedParts
        {
            get
            {
                var partsArray = new PartModel[Parts.Count];
                Parts.CopyTo(partsArray, 0);
                return partsArray;
            }
            set { Parts = new ObservableCollection<PartModel>(value); }
        }
        #endregion
    }
}