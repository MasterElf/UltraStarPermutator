using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text; // För att använda Encoding
using System.Xml.Serialization;

namespace UltraStarPermutator;

[Serializable]
public class PartModel : ObservableObject
{
    private string? _name;

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string? _filePath;

    public string? FilePath
    {
        get => _filePath;
        set => SetProperty(ref _filePath, value);
    }

    private bool _assertTrailingSpace;

    public bool AssertTrailingSpace
    {
        get => _assertTrailingSpace;
        set => SetProperty(ref _assertTrailingSpace, value);
    }

    private List<string>? _temporaryDuetNames;

    [XmlIgnore]
    public List<string>? TemporaryDuetNames
    {
        get => _temporaryDuetNames;
        set => SetProperty(ref _temporaryDuetNames, value);
    }

    [XmlIgnore] // Detta attribut ser till att MemoryStream ignoreras vid serialisering
    public MemoryStream? FileData { get; set; }

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

    #region File data fetching
    // Ny funktion för att kontrollera om FileData eller FilePath har data
    public bool HaveFileData()
    {
        return FileData != null || (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath));
    }

    // Ny funktion för att läsa data från FileData eller FilePath
    public string ReadFileData()
    {
        if (FileData != null)
        {
            FileData.Position = 0; // Återställ positionen i MemoryStream
            using StreamReader reader = new(FileData, Encoding.UTF8);
            return reader.ReadToEnd();
        }
            
        if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
        {
            return File.ReadAllText(FilePath, Encoding.UTF8);
        }

        return "Ingen data tillgänglig";
    }
    #endregion
}