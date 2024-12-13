using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : I would LOVE to have a MusicManager class, but this class will handle all things sound related. Fuck the names, focus on functionality.
// I would also love it if I had named this AudioManager instead, but I don't want to bother renaming shit now, so fuck it...

// NOTE : We could have used the audio mixer system but I've found it to be quite a hassle to work with and it doesn't bring much to the table for what I'm trying
// to do, which is quite simple, so I'd rather manage things by myself for now.

public class SoundManager : SingletonPersistent<SoundManager>
{
    #region Structs

    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
        public NamedAudioClip(string name = default, AudioClip clip = null)
        {
            this.name = name;
            this.clip = clip;
        }
    }

    #endregion

    #region Variables

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // Audio source for music.
    [SerializeField] private AudioSource uiSource; // Audio source for UI related sounds.
    [SerializeField] private AudioSource audioSourceSFX;

    // NOTE : More complex music system ahead.
    // One sound will be the one currently playing, the other will be used for fading into the next music track.
    // For now, this is disabled for simplicity.
    // [SerializeField] private AudioSource audioSourceMusic1;
    // [SerializeField] private AudioSource audioSourceMusic2;

    // TODO : Rework volume vars system to be less fucked up...
    [Header("Volume")]
    [SerializeField] private float effectsVolume;
    [SerializeField] private float musicVolume;
    [SerializeField] private float voiceVolume;

    [Header("Music Audio Clips")] // The audio clips used for music.
    [SerializeField] private NamedAudioClip[] musicTracks;

    [Header("UI Audio Clips")]
    [SerializeField] private AudioClip clickClip;

    [Header("Voice Audio Clips")]
    [SerializeField] private NamedAudioClip[] voiceAudioClips;

    [Header("Ambient Audio Clips")]
    [SerializeField] private NamedAudioClip[] ambientAudioClips;

    private readonly int minVolumeLevel = 0;
    private readonly int maxVolumeLevel = 10;

    private string currentMusicName;
    private int currentMusicVolumeLevel;


    #endregion

    #region Variables - Volume

    private float volumeGlobal = 1.0f; // This one controls the overall volume, which means it has to multiply its value by the other values, because the volume values are percentages (they go from 0.0f to 1.0f)
    private float volumeSFX = 1.0f;
    private float volumeMusic = 1.0f;
    private float volumeVoice = 1.0f;

    // public float VolumeGlobal { get { return GetVolumeGlobal(); } set { SetVolumeGlobal(value); } }

    public float VolumeSFX { get { return GetVolumeSFX(); } set { SetVolumeSFX(value); } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.currentMusicVolumeLevel = this.maxVolumeLevel;
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods - SFX / Effects

    public void PlaySoundSFX(AudioClip clip)
    {
        this.audioSourceSFX.PlayOneShot(clip);
    }

    public void SetVolumeSFX(float volume)
    {
        this.volumeSFX = Mathf.Clamp01(volume);
        this.audioSourceSFX.volume = this.volumeSFX * this.volumeGlobal;
    }

    public float GetVolumeSFX()
    {
        return this.volumeSFX;
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

    // Int volume level functions
    public void SetMusicVolumeLevel(int volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, this.minVolumeLevel, this.maxVolumeLevel);
        this.currentMusicVolumeLevel = volumeLevel;
        float volume = (float)(((float)volumeLevel) / ((float)this.maxVolumeLevel));
        SetMusicVolume(volume);
    }
    public int GetMusicVolumeLevel()
    {
        return this.currentMusicVolumeLevel;
    }

    // Float volume functions
    public void SetMusicVolume(float volume)
    {
        this.musicSource.volume = Mathf.Clamp01(volume);
    }
    public float GetMusicVolume()
    {
        return this.musicSource.volume;
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
