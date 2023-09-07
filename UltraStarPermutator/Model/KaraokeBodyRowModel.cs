using System;

namespace UltraStarPermutator
{
    internal enum NoteType
    {
        Regular,
        Golden,
        Freestyle,
        LineBreak,
        Unknown
    }

    internal class KaraokeBodyRowModel
    {
        public NoteType NoteType { get; set; }
        public int NumberOfComponents { get; set; }
        public string[] Components { get; set; }

        public KaraokeBodyRowModel(string row)
        {
            Components = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            NumberOfComponents = Components.Length;

            if (NumberOfComponents > 0)
            {
                NoteType = Components[0] switch
                {
                    ":" => NoteType.Regular,
                    "*" => NoteType.Golden,
                    "F" => NoteType.Freestyle,
                    "-" => NoteType.LineBreak,
                    _ => NoteType.Unknown
                };
            }
        }

        public override string ToString()
        {
            return string.Join(' ', Components);
        }
    }
}
