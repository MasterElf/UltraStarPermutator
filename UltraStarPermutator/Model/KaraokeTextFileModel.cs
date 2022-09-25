using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UltraStarPermutator
{
    //#ARTIST:Entertainmen
    //#TITLE:Let it snow
    //#LANGUAGE:English
    //#YEAR:2022
    //#GENRE:Barbershop
    //#CREATOR:Mats Elfving
    //#EDITION:1.0
    //#MP3:Let it snow - Karaoke.mp3
    //#COVER:Let it snow - Karaoke.jpg
    //#VIDEO:Let it snow - Karaoke.mp4
    //#BPM:400
    //#GAP:10664,44

    internal enum Tag
    {
        Unknown,
        ARTIST,
        TITLE,
        LANGUAGE,
        YEAR,
        GENRE,
        CREATOR,
        EDITION,
        MP3,
        COVER,
        VIDEO,
        BPM,
        GAP
    }

    internal class KaraokeTextFileModel
    {
        Dictionary<Tag, string> tags = new Dictionary<Tag, string>();
        StringBuilder body = new StringBuilder();

        public KaraokeTextFileModel(string karaokeTextFileBody)
        {
            if (!string.IsNullOrEmpty(karaokeTextFileBody))
            {
                // Split into rows
                string[]? rows = karaokeTextFileBody.Split(new char[] { '\n', '\r' });

                if (rows?.Length > 0)
                {
                    foreach (string row in rows)
                    {
                        if (RowIsTag(row, out Tag tag, out string content))
                        {
                            tags[tag] = content;
                        }
                        else if (!string.IsNullOrEmpty(row))
                        {
                            body.AppendLine(row);
                        }
                    }
                }
            }
        }

        public string GetText()
        {
            StringBuilder text = new StringBuilder();

            // Write tags
            foreach (KeyValuePair<Tag, string> pair in tags)
            {
                text.Append('#');
                text.Append(pair.Key);
                text.Append(':');
                text.Append(pair.Value);
                text.AppendLine();
            }

            // Write body
            text.Append(body.ToString());

            return text.ToString();
        }

        public void SetTag(Tag tag, string content)
        {
            tags[tag] = content;
        }

        private bool RowIsTag(string row, out Tag tag, out string content)
        {
            bool rowIsTag = false;

            tag = Tag.Unknown;
            content = default;

            if (!string.IsNullOrEmpty(row))
            {
                string pattern = @"\#(.*)\:(.*)";

                Regex r = new Regex(pattern, RegexOptions.CultureInvariant);

                Match m = r.Match(row);
                int matchCount = 0;

                if (m.Success)
                {
                    if (m.Groups.Count == 3)
                    {
                        if (Enum.TryParse(typeof(Tag), m.Groups[1].Value, out object? foundObject) && foundObject is Tag foundTag)
                        {
                            tag = foundTag;
                            content = m.Groups[2].Value;
                            rowIsTag = true;
                        }
                    }
                }
            }

            return rowIsTag;
        }
    }
}