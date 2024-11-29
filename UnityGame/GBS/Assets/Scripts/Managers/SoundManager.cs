using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : I would LOVE to have a MusicManager class, but this class will handle all things sound related. Fuck the names, focus on functionality.
public class SoundManager : SingletonPersistent<SoundManager>
{
    #region Structs

    [System.Serializable]
    public struct MusicTrack
    {
        public string name;
        public AudioClip clip;
        public MusicTrack(string name = default, AudioClip clip = null)
        {
            this.name = name;
            this.clip = clip;
        }
    }

    #endregion

    #region Variables

    [Header("Components")]
    [SerializeField] private AudioSource musicSource; // Audio source for music.
    [SerializeField] private AudioSource uiSource; // Audio source for UI related sounds.

    // NOTE : More complex music system ahead.
    // One sound will be the one currently playing, the other will be used for fading into the next music track.
    // For now, this is disabled for simplicity.
    // [SerializeField] private AudioSource audioSourceMusic1;
    // [SerializeField] private AudioSource audioSourceMusic2;

    [Header("Volume")]
    [SerializeField] private float effectsVolume;
    [SerializeField] private float musicVolume;
    [SerializeField] private float voiceVolume;

    [Header("Music Tracks")] // The audio clips used for music.
    [SerializeField] private MusicTrack[] musicTracks;

    [Header("UI Audio Clips")]
    [SerializeField] private AudioClip clickClip;

    private string currentMusicName;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods - Music

    public void PlayMusic(string name, bool loop = true)
    {
        foreach (var track in this.musicTracks)
        {
            if (track.name == name)
            {
                this.musicSource.loop = loop;
                this.musicSource.clip = track.clip;
                this.musicSource.Play();
                this.currentMusicName = track.name;
                return;
            }
        }
    }

    public void StopMusic()
    {
        this.musicSource.Stop();
        this.currentMusicName = "";
    }

    public bool IsPlayingMusic(string name)
    {
        return name == this.currentMusicName;
    }

    #endregion

    #region PublicMethods - UI

    public void PlayClickSound()
    {
        this.uiSource.PlayOneShot(this.clickClip);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
