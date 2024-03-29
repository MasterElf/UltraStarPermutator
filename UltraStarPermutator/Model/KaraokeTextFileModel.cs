﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UltraStarPermutator
{
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
        GAP,
        BACKGROUND
    }

    internal class KaraokeTextFileModel
    {
        Dictionary<Tag, string> tags = new Dictionary<Tag, string>();
        List<KaraokeBodyRowModel> bodyRows = new List<KaraokeBodyRowModel>();

        internal Dictionary<Tag, string> Tags { get => tags; set => tags = value; }
        internal List<KaraokeBodyRowModel> BodyRows { get => bodyRows; set => bodyRows = value; }

        public KaraokeTextFileModel(string karaokeTextFileBody, bool assertTrailingSpace)
        {
            ParseText(karaokeTextFileBody, assertTrailingSpace);
        }

        private void ParseText(string karaokeTextFileBody, bool assertTrailingSpace)
        {
            if (!string.IsNullOrEmpty(karaokeTextFileBody))
            {
                // Split into rows
                string[]? rows = karaokeTextFileBody.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                if (rows?.Length > 0)
                {
                    foreach (string row in rows)
                    {
                        if (RowIsTag(row, out Tag tag, out string content))
                        {
                            Tags[tag] = content;
                        }
                        else if (!string.IsNullOrEmpty(row))
                        {
                            KaraokeBodyRowModel bodyRow = new KaraokeBodyRowModel(row, assertTrailingSpace);
                            BodyRows.Add(bodyRow);
                        }
                    }
                }
            }
        }

        public string GetText()
        {
            StringBuilder text = new StringBuilder();

            // Write tags
            foreach (KeyValuePair<Tag, string> pair in Tags)
            {
                text.Append('#');
                text.Append(pair.Key);
                text.Append(':');
                text.Append(pair.Value);
                text.AppendLine();
            }

            // Write body
            foreach (var bodyRow in BodyRows)
            {
                text.AppendLine(bodyRow.ToString());
            }

            return text.ToString();
        }

        public void SetTag(string tagLine)
        {
            if (RowIsTag(tagLine, out Tag tag, out string content))
            {
                Tags[tag] = content;
            }
        }

        public void SetTag(Tag tag, string content)
        {
            Tags[tag] = content;
        }

        public void SetBody(string bodyText, bool assertTrailingSpace)
        {
            BodyRows.Clear();
            ParseText(bodyText, assertTrailingSpace);
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