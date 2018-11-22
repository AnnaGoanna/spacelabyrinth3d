using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharedScripts;
using UnityEngine;

public class LevelLoader
{
    private class LevelEntry
    {
        public readonly string Id;
        public readonly Vector3 Position;
        public readonly Segment.Representation Representation;
        public Segment.Tag Tag { get; private set; }

        private LevelEntry(string id, Vector3 position, Segment.Representation representation, Segment.Tag tag)
        {
            Id = id;
            Position = position;
            Representation = representation;
            Tag = tag;
        }

        public static LevelEntry Parse(string line)
        {
            var fields = line.Split(':');

            var coordinates = fields[0].Split(',');
            var position = new Vector3(
                Convert.ToSingle(coordinates[0]),
                Convert.ToSingle(coordinates[1]),
                Convert.ToSingle(coordinates[2]));

            string id = fields[1].ToUpper();

            Segment.Tag tag;
            if (fields.Length >= 3)
            {
                tag = (Segment.Tag) Enum.Parse(typeof(Segment.Tag), fields[2], true);
            }
            else
            {
                tag = Segment.Tag.None;
            }
            Debug.LogWarning(id);
            var representation = LevelData.SegmentIdMapping[id];
            var levelEntry = new LevelEntry(id, position, representation, tag);
            return levelEntry;
        }
    }

    private readonly Level _level;

    public LevelLoader(Level level)
    {
        _level = level;
    }

    public static void SaveLevel(string fileName, Dictionary<Vector3, GameObject> levelData)
    {
        string fileData = "";

        foreach (var levelEntry in levelData)
        {
            var segment = levelEntry.Value.GetComponent<Segment>();

            if (!segment.Id.Equals(""))
            {
                var position = levelEntry.Key;
                fileData += string.Format("{0},{1},{2}:{3}{4}", position.x, position.y, position.z, segment,
                    Environment.NewLine);
            }
        }

        Debug.Log(string.Format("Saving to file: {0} with data:{2}{1}{2}", fileName, fileData, Environment.NewLine));

        using (var streamWriter = File.CreateText(fileName))
        {
            streamWriter.Write(fileData);
        }
    }

    public void LoadLevel(string fileName)
    {
        using (var streamReader = File.OpenText(fileName))
        {
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var levelEntry = LevelEntry.Parse(line);
                var position = levelEntry.Position;

                var exit = _level.PlaceSegment(levelEntry.Id, position,
                    levelEntry.Representation.Rotation, levelEntry.Tag);

                if (levelEntry.Tag == Segment.Tag.Start)
                {
                    Debug.Log("Found start tag at position " + position);

                    var validPassageId = levelEntry.Id.Last().ToString();
                    var passageRotation = LevelData.SegmentIdMapping[validPassageId].Rotation;
                    var validSegmentDirection = Quaternion.Euler(passageRotation) * Vector3.right;
                    var adjustedStartPosition = position + validSegmentDirection * 0.25f;

                    _level.StartPosition = adjustedStartPosition;
                    _level.StartingDirection = validSegmentDirection;
                } else if (levelEntry.Tag == Segment.Tag.Exit)
                {
                    Debug.Log("Adding exit at position " + position);
                    _level.Exits.Add(exit);
                }
            }
        }
    }
}
