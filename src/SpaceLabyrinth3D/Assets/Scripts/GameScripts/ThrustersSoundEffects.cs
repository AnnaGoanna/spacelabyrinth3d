using System;
using System.Collections.Generic;
using UnityEngine;

public class ThrustersSoundEffects : MonoBehaviour
{
    public enum Sound
    {
        Undefined = 0,
        FullStop = 1,
        RotateX = 2,
        RotateY = 3,
        RotateZ = 4,
        MoveX = 5,
        MoveY = 6,
        MoveZ = 7
    }

    private readonly string[] _sounds =
    {
        "Audio/RetroGamesSoundFX/FlyEngine/FlyEngine3",
        "Audio/RetroGamesSoundFX/FlyEngine/FlyEngine4",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing03",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing03",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing03",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing09",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing09",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing09",
        "Audio/RetroGamesSoundFX/GasReleasing/GasReleasing15"
    };

    private readonly Dictionary<Sound, AudioSource> _audioSources = new Dictionary<Sound, AudioSource>();

    private void Start()
    {
        foreach (Sound s in Enum.GetValues(typeof(Sound)))
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.1f;
            AudioClip clip = Resources.Load(_sounds[(int) s]) as AudioClip;
            if (clip == null) Debug.LogWarning("clip = null (" + s + ")");
            audioSource.clip = clip;
            _audioSources.Add(s, audioSource);
        }
    }

    public void PlaySound(Sound s, bool loop = true)
    {
        if (!_audioSources.ContainsKey(s)) return;
        if (!_audioSources[s].isPlaying)
        {

            _audioSources[s].loop = loop;
            _audioSources[s].Play();
        }
    }

    public void StopSound(Sound s)
    {
        if (!_audioSources.ContainsKey(s)) return;
        _audioSources[s].loop = false;
    }
}
