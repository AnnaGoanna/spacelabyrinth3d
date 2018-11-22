using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagDropdownMenu : MonoBehaviour {

    public GameObject levelObject;
    private LevelEditor editor;
    private Vector3 currentSegment;
    private Dropdown menu;

    // TODO add variable for parent and add options for some tags

	// Use this for initialization
	void Start () {
        List<string> tags = new List<string>();
        tags.AddRange(Enum.GetNames(typeof(Segment.Tag)));

        menu = gameObject.GetComponent<Dropdown>();
        menu.ClearOptions();
        menu.AddOptions(tags);

        menu.RefreshShownValue();

        editor = levelObject.GetComponent<LevelEditor>();
        currentSegment = editor.GetCursor();
	}

	// Update is called once per frame AFTER everything else has initialized
	void LateUpdate () {
        if (currentSegment != editor.GetCursor())
        {
            Segment.Tag currentTag = editor.GetSegmentAtCursor().SegmentTag;
            string currentOption = menu.options[menu.value].text;

            if (!currentOption.Equals(currentTag.ToString()))
            {
                menu.value = (int)currentTag;
            }

            currentSegment = editor.GetCursor();
        }
	}
}
