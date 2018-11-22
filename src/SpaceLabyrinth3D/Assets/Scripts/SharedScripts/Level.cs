using System.Collections.Generic;
using System.Linq;
using SharedScripts;
using UnityEngine;
using System.Text;

public class Level : MonoBehaviour
{
    private static readonly float _materialScale = 0.75f;

    private readonly Dictionary<Vector3, GameObject> _levelData = new Dictionary<Vector3, GameObject>();
    private LevelLoader _levelLoader;
    private Vector3 _startPosition;

    public const int SegmentsInLine = 10;

    public Material Material { private get; set; }
    public float Scale { private get; set; }

    public Vector3 StartPosition
    {
        get { return _startPosition * Scale; }
        set { _startPosition = value; }
    }

    public Vector3 StartingDirection { get; set; }

    public readonly List<GameObject> Exits = new List<GameObject>();

    private void Awake()
    {
        _levelLoader = new LevelLoader(this);
        Material = (Material)Resources.Load("Materials/TransparentSegmentMaterial", typeof(Material));
        //Material = (Material)Resources.Load("Textures/Metal Floor (Rust Low)/Material/Metal_floor (Rust Low)", typeof(Material));
        Scale = 1.0f;
    }

    public void ClearAndFillWithEmptySegments()
    {
        for (int i = 0; i < SegmentsInLine; i++)
            for (int j = 0; j < SegmentsInLine; j++)
                for (int k = 0; k < SegmentsInLine; k++)
                    PlaceSegment(LevelData.SegmentModelDefaultIds[0], new Vector3(i, j, k));

        Debug.Log("Level cleared and filled with empty segments.");
    }

    public void SaveLevel(string fileName)
    {
        LevelLoader.SaveLevel(fileName, _levelData);
    }

    public void LoadLevel(string fileName, bool fullLoad = false)
    {
        foreach (var segment in _levelData) DestroyObject(segment.Value);

        Exits.Clear();
        _levelData.Clear();

        if (fullLoad) ClearAndFillWithEmptySegments();
        _levelLoader.LoadLevel(fileName);
    }


    public Segment GetSegment(Vector3 position)
    {
        return _levelData[position].GetComponent<Segment>();
    }

    public void UpdateSegmentColor(Vector3 position, Color color)
    {
        GameObject segment = _levelData[position];
        if (segment == null) return;
        segment.GetComponent<MeshRenderer>().material.color = color;
    }

    // TODO a texture wich has only edges visible would be nice
    public GameObject PlaceSegment(string id, Vector3 position, Vector3 rotation = default(Vector3), Segment.Tag segmentTag = Segment.Tag.None)
    {
        int type = LevelData.SegmentIdMapping[id].Type;
        string modelName = LevelData.SegmentModelNames[type];

        GameObject segmentPrefab = (GameObject)Resources.Load("Models/Segments/" + modelName, typeof(GameObject));
        GameObject curObj = Instantiate(segmentPrefab, transform);

        // TODO: we could use here same format as in file - for consistency -> yes, we should
        curObj.name = string.Format("[{0}][{1}][{2}]:{3}", position.x, position.y, position.z, modelName);

        Transform curTrans = curObj.transform;
        curTrans.localScale = Vector3.one * Scale * 50;
        curTrans.localPosition = position * Scale;
        curTrans.localRotation = Quaternion.identity;

        MeshRenderer curMeshRend = curObj.GetComponent<MeshRenderer>();
        curMeshRend.material = Material;
        curMeshRend.material.mainTextureScale = Vector2.one * Scale * _materialScale;

        Segment curSegm = curObj.AddComponent<Segment>();
        curSegm.Id = id;
        curSegm.SegmentTag = segmentTag;
        curSegm.RotateModel(rotation / 90);

        curObj.AddComponent<MeshCollider>();

        if (_levelData.ContainsKey(position))
        {
            if (_levelData[position].GetComponent<Segment>().Id != "")
            {
                Debug.LogWarning("Duplicate entry at position: " + position);
            }
            DestroyObject(_levelData[position]);
        }
        _levelData[position] = curObj;

        return curObj;
    }


    public static string BuildErrorMessage(List<string> errors)
    {
        var sb = new StringBuilder();
        foreach (string error in errors)
        {
            sb.Append(error + ", ");
        }
        sb.Remove(sb.Length - 3, 2);
        return sb.ToString();
    }

    public List<string> Validate()
    {
        bool startTagFound = false;
        bool exitTagFound = false;

        var errors = new List<string>();

        foreach (var levelData in _levelData)
        {
            var position = levelData.Value.transform.localPosition / Scale;
            var segment = levelData.Value.GetComponent<Segment>();

            errors.AddRange(ValidatePassages(segment, position));

            var segmentTag = segment.SegmentTag;

            if (segmentTag == Segment.Tag.Start)
            {
                AddErrorIf(startTagFound, errors, "Multiple start tags found.");

                startTagFound = true;
            }
            else if (segmentTag == Segment.Tag.Exit)
            {
                exitTagFound = true;
            }
        }

        AddErrorIf(!startTagFound, errors, "No start tag found.");
        AddErrorIf(!exitTagFound, errors, "No exit tag found.");

        return errors;
    }

    private static void AddErrorIf(bool condition, ICollection<string> errors, string errorMessage)
    {
        if (condition)
        {
            errors.Add(errorMessage);
            Debug.LogError(errorMessage);
        }
    }

    private List<string> ValidatePassages(Segment segment, Vector3 position)
    {
        var errors = new List<string>();
        var neighborPositions = new[]
            {Vector3.left, Vector3.right, Vector3.up, Vector3.down, Vector3.forward, Vector3.back};
        var requiredWalls = new[] { "L", "R", "U", "D", "B", "F"};
        var requiredNeighborWalls = new[] {"R", "L", "D", "U", "F", "B"};

        for (int i = 0; i < neighborPositions.Length; i++)
        {
            var neighborPosition = position + neighborPositions[i];
            if (!_levelData.ContainsKey(neighborPosition))
            {
                continue;
            }

            var neighborSegment = _levelData[neighborPosition].GetComponent<Segment>();

            string wall = requiredWalls[i];
            string neighborWall = requiredNeighborWalls[i];
            string errorMessage = "Segment " + position + " has unconnected passage " + wall + ", neighbor "
                                  + neighborPosition + " doesn't contain entrance in wall "
                                  + neighborWall;

            AddErrorIf(neighborSegment.Id != ""
                       && segment.Id.Contains(wall)
                       && !neighborSegment.Id.Contains(neighborWall),
                errors,
                errorMessage);
        }

        return errors;
    }
}
