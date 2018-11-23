using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    private readonly string[] _songs =
    {
        "Audio/Absolute Space And Sci-Fi Vol.1 - Sample Pack - Voltz Supreme/Relaxing Planet - Pads Only",
        "Audio/Absolute Space And Sci-Fi Vol.1 - Sample Pack - Voltz Supreme/Stars - Sparse",
        "Audio/Absolute Space And Sci-Fi Vol.1 - Sample Pack - Voltz Supreme/Space Drift - Poly Focus Intense"
    };

    private readonly string _defaultSong =
        "Audio/Classical/Johann Strauss II - The Blue Danube";

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.volume = 0.4f;
    }

    public void ChangeSoundTrack(int levelNumber, bool useDefault = false)
    {
        string songName;
        if (useDefault) songName = _defaultSong;
        else songName = _songs[levelNumber];

        if (_audioSource.isPlaying) _audioSource.Stop();
        _audioSource.clip = Resources.Load(songName) as AudioClip;
        if (_audioSource.clip != null)
        {
            _audioSource.Play();
        }
        else
        {
            Debug.Log("Attempted to play missing audio clip by name: " + songName);
        }
    }
}
