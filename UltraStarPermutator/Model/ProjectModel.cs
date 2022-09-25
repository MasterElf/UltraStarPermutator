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
        [XmlIgnore]
        public ObservableCollection<PartModel> Parts { get; private set; } = new ObservableCollection<PartModel>();

        #region Parts serialization
        public List<PartModel> SerializedParts
        {
            get { return new List<PartModel>(Parts); }
            set { Parts = new ObservableCollection<PartModel>(value); }
        }
        #endregion
    }
}