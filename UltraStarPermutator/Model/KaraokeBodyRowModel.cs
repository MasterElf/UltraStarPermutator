using System;
using System.Text.RegularExpressions;

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
            // Use regex to split the row into components, preserving spaces in the fifth column
            var match = Regex.Match(row, @"^(\S+)\s+(\S+)\s+(\S+)\s+(\S+)\s+(.*)$");
            if (match.Success)
            {
                Components = new string[5];
                for (int i = 1; i <= 5; i++)
                {
                    Components[i - 1] = match.Groups[i].Value;
                }
                NumberOfComponents = 5;
            }
            else
            {
                Components = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                NumberOfComponents = Components.Length;
            }

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
