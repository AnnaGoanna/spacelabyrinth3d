using System;
using System.Collections;
using SharedScripts;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class Segment : MonoBehaviour
{
    public enum Tag
    {
        None,
        Start,
        Exit,
        // not implemented yet:
        /*Checkpoint,
        Trap, // (0)
        Enemy, // (0)
        Door, // (v)
        Lever, // (x,y,z)
        Obstacle, // (w,h,d;x,y,z)
        Movingobstacle // (v,s,-||-)*/
    }

    private readonly Dictionary<string, Dictionary<Vector3, string>> _neigboringWallsMapping =
        new Dictionary<string, Dictionary<Vector3, string>>
        {
            {"L", new Dictionary<Vector3, string>
            {
                {Vector3.up,      "B"},
                {Vector3.down,    "F"},
                {Vector3.forward, "D"},
                {Vector3.back,    "U"}
            }},
            {"R", new Dictionary<Vector3, string>
            {
                {Vector3.up,      "F"},
                {Vector3.down,    "B"},
                {Vector3.forward, "U"},
                {Vector3.back,    "D"}
            }},
            {"U", new Dictionary<Vector3, string>
            {
                {Vector3.forward, "L"},
                {Vector3.back,    "R"},
                {Vector3.left,    "F"},
                {Vector3.right,   "B"}
            }},
            {"D", new Dictionary<Vector3, string>
            {
                {Vector3.forward, "R"},
                {Vector3.back,    "L"},
                {Vector3.left,    "B"},
                {Vector3.right,   "F"}
            }},
            {"F", new Dictionary<Vector3, string>
            {
                {Vector3.up,      "L"},
                {Vector3.down,    "R"},
                {Vector3.left,    "D"},
                {Vector3.right,   "U"}
            }},
            {"B", new Dictionary<Vector3, string>
            {
                {Vector3.up,      "R"},
                {Vector3.down,    "L"},
                {Vector3.left,    "U"},
                {Vector3.right,   "D"}
            }},
        };


    public string Id;
    private Tag _segmentTag = Tag.None;

    public Tag SegmentTag
    {
        get { return _segmentTag; }
        set { _segmentTag = value; }
    }
    // TODO add variable for additional information

    public void Rotate(Vector3 dir)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            Debug.Log("Id before rotation: " + Id);
            Debug.Log("Applying rotation: " + dir);
        }
        RotateIdMultiways(dir);
        SortId();
        RotateModel(dir);

        if (!string.IsNullOrEmpty(Id))
        {
            Debug.Log("Id after rotation: " + Id);
        }
    }

    private void SortId()
    {
        List<char> sortedId = new List<char>(Id.ToCharArray());
        sortedId.Sort((lhs, rhs) =>
        {
            var d = new Dictionary<char, int>();

            string idd = LevelData.SegmentModelDefaultIds[LevelData.SegmentModelDefaultIds.Length - 1];
            for (int i = 0; i < idd.Length; i++)
            {
                d.Add(idd[i], i);
            }

            return d[lhs] - d[rhs];
        });

        Id = new string(sortedId.ToArray());
    }

    private void RotateIdMultiways(Vector3 dir)
    {
        var sb = new StringBuilder();
        foreach (char c in Id)
        {
            string newC = _neigboringWallsMapping[c.ToString()].TryGetValue(dir, out newC)? newC: c.ToString();
            sb.Append(newC);
        }

        Id = sb.ToString();
    }

    internal void RotateModel(Vector3 dir)
    {
        transform.Rotate(dir * 90, Space.World);
    }

    public override string ToString()
    {
        var output = Id;
        if (SegmentTag != Tag.None)
        {
            output += ":" + SegmentTag;
        }
        return output;
    }

    public class Representation
    {
        public readonly int Type;
        public Vector3 Rotation;

        public Representation(int type, Vector3 rotation)
        {
            Type = type;
            Rotation = rotation;
        }
    }
}
