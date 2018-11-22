using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMuteToggler : MonoBehaviour
{
    private Toggle _toggle;
    public List<AudioSource> sounds;

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
        if (toggle.isOn) foreach (AudioSource s in sounds) s.mute = true;
        else foreach (AudioSource s in sounds) s.mute = false;
    }
}
