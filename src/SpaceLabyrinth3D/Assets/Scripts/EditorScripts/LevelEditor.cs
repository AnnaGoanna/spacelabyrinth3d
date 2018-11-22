using System;
using System.Text;
using SharedScripts;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    private Level level;
    private Vector3 cursorPosition;

    private readonly Color _unselectedSegmentColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);
    private readonly Color _selectedSegmentColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);

    private void Start()
    {
        level = gameObject.GetComponent<Level>();
        level.ClearAndFillWithEmptySegments();
        SetCursor(Vector3.zero);
        Debug.Log("Cursor position reset.");
    }

    private void Update()
    {
        /* Read input only if no dialog open */
        if (FindObjectOfType<ModalFilePicker>() != null) return;
        if (GameObject.Find("InfoPanel") != null) return;


        /* Direction related actions */
        /* moving cursor and rotating segment */
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (IsKey(KeyCode.W) || IsKey(KeyCode.UpArrow)) dir = Vector3.right;
            else if (IsKey(KeyCode.S) || IsKey(KeyCode.DownArrow)) dir = Vector3.left;
            else if (IsKey(KeyCode.A) || IsKey(KeyCode.LeftArrow)) dir = Vector3.down;
            else if (IsKey(KeyCode.D) || IsKey(KeyCode.RightArrow)) dir = Vector3.up;
            else if (IsKey(KeyCode.Q) || IsKey(KeyCode.KeypadPlus)) dir = Vector3.forward;
            else if (IsKey(KeyCode.E) || IsKey(KeyCode.KeypadMinus)) dir = Vector3.back;
        }
        else
        {
            if (IsKey(KeyCode.W) || IsKey(KeyCode.UpArrow)) dir = Vector3.up;
            else if (IsKey(KeyCode.S) || IsKey(KeyCode.DownArrow)) dir = Vector3.down;
            else if (IsKey(KeyCode.A) || IsKey(KeyCode.LeftArrow)) dir = Vector3.left;
            else if (IsKey(KeyCode.D) || IsKey(KeyCode.RightArrow)) dir = Vector3.right;
            else if (IsKey(KeyCode.Q) || IsKey(KeyCode.KeypadPlus)) dir = Vector3.forward;
            else if (IsKey(KeyCode.E) || IsKey(KeyCode.KeypadMinus)) dir = Vector3.back;
        }

        // TODO change directions according to view OR: ! add a rotating compass rose

        if (dir != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftControl)) RotateSegmentAtCursor(dir);
            else MoveCursorBy(dir);
        }


        /* Numbers related actions */
        /* placing segment */
        int num = -1;
        if (IsKey(KeyCode.Alpha0) || IsKey(KeyCode.Keypad0)) num = 0;
        else if (IsKey(KeyCode.Alpha1) || IsKey(KeyCode.Keypad1)) num = 1;
        else if (IsKey(KeyCode.Alpha2) || IsKey(KeyCode.Keypad2)) num = 2;
        else if (IsKey(KeyCode.Alpha3) || IsKey(KeyCode.Keypad3)) num = 3;
        else if (IsKey(KeyCode.Alpha4) || IsKey(KeyCode.Keypad4)) num = 4;
        else if (IsKey(KeyCode.Alpha5) || IsKey(KeyCode.Keypad5)) num = 5;
        else if (IsKey(KeyCode.Alpha6) || IsKey(KeyCode.Keypad6)) num = 6;
        else if (IsKey(KeyCode.Alpha7) || IsKey(KeyCode.Keypad7)) num = 7;
        else if (IsKey(KeyCode.Alpha8) || IsKey(KeyCode.Keypad8)) num = 8;
        else if (IsKey(KeyCode.Alpha9) || IsKey(KeyCode.Keypad9)) num = 9;

        if (num != -1) PlaceSegmentAtCursor(num);
    }

    private bool IsKey(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }


    public void OnSaveOptionClick()
    {
        var errors = level.Validate();

        if (errors.Count != 0)
        {
            ModalFilePicker.InstantiateMessageDialog("Level contains errors: " + Level.BuildErrorMessage(errors), "OK", null);
        }
        else
        {
            ModalFilePicker.InstantiateModalDialog("SaveFileDialog", "Choose a file to save as", "Save", true, SaveLevel);
        }
    }

    public void OnLoadOptionClick()
    {
        ModalFilePicker.InstantiateModalDialog("LoadFileDialog", "Choose a file to load", "Load", false, LoadLevel);
    }

    private void SaveLevel(string fileName = null)
    {
        if (string.IsNullOrEmpty(fileName)) return;
        level.SaveLevel(fileName);
    }

    private void LoadLevel(string fileName = null)
    {
        if (string.IsNullOrEmpty(fileName)) return;
        level.LoadLevel(fileName, true);
        SetCursor(Vector3.zero);
    }


    public void OnTagChanged(int tagId)
    {
        GetSegmentAtCursor().SegmentTag = (Segment.Tag)tagId;
    }


    public void MoveCursorBy(Vector3 dir)
    {
        Vector3 newPosition = cursorPosition + dir;

        if ((newPosition.x < 0 || newPosition.x >= Level.SegmentsInLine)
            || (newPosition.y < 0 || newPosition.y >= Level.SegmentsInLine)
            || (newPosition.z < 0 || newPosition.z >= Level.SegmentsInLine))
        {
            newPosition = cursorPosition;
        }

        if (newPosition != cursorPosition) MoveCursorTo(newPosition);
    }

    private void MoveCursorTo(Vector3 newPosition)
    {
        HideCursor();
        cursorPosition = newPosition;
        ShowCursor();
    }

    public Vector3 GetCursor()
    {
        return cursorPosition;
    }

    private void SetCursor(Vector3 newPosition)
    {
        cursorPosition = newPosition;
        ShowCursor();
    }

    private void HideCursor()
    {
        level.UpdateSegmentColor(cursorPosition, _unselectedSegmentColor);
    }

    private void ShowCursor()
    {
        level.UpdateSegmentColor(cursorPosition, _selectedSegmentColor);
    }


    public Segment GetSegmentAtCursor()
    {
        return level.GetSegment(cursorPosition);
    }

    public void PlaceSegmentAtCursor(int num)
    {
        level.PlaceSegment(LevelData.SegmentModelDefaultIds[num], cursorPosition);
        ShowCursor();
    }

    public void RotateSegmentAtCursor(Vector3 dir)
    {
        level.GetSegment(cursorPosition).Rotate(dir);
    }

}
