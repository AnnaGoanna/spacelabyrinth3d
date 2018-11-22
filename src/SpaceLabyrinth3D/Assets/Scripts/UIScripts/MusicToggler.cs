using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggler : MonoBehaviour
{

    private Toggle _toggle;
    public AudioSource music;

    void Start()
    {
        _toggle = gameObject.GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(_toggle);
        });
    }

    void ToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn) music.Pause();
        else music.UnPause();
    }
}
