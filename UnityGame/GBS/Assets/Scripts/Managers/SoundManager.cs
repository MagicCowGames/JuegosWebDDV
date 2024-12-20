using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceVoice;
    [SerializeField] private AudioSource audioSourceUI;

    [Header("Audio Clips SFX")]
    [SerializeField] private NamedAudioClip[] audioClipsSFX;

    [Header("Audio Clips Music")]
    [SerializeField] private NamedAudioClip[] audioClipsMusic;

    [Header("Audio Clips Voice")]
    [SerializeField] private NamedAudioClip[] audioClipsVoice;

    [Header("Audio Clips UI")]
    [SerializeField] private NamedAudioClip[] audioClipsUI;

    private string currentMusicName;

    #endregion

    #region Variables - Volume

    private float volumeGlobal = 1.0f; // This one controls the overall volume, which means it has to multiply its value by the other values, because the volume values are percentages (they go from 0.0f to 1.0f)
    private float volumeSFX = 1.0f;
    private float volumeMusic = 1.0f;
    private float volumeVoice = 1.0f;
    private float volumeUI = 1.0f;

    public float VolumeGlobal { get { return GetVolumeGlobal(); } set { SetVolumeGlobal(value); } }
    public float VolumeSFX { get { return GetVolumeSFX(); } set { SetVolumeSFX(value); } }
    public float VolumeMusic { get { return GetVolumeMusic(); } set { SetVolumeMusic(value); } }
    public float VolumeVoice { get { return GetVolumeVoice(); } set { SetVolumeVoice(value); } }
    public float VolumeUI { get { return GetVolumeUI(); } set { SetVolumeUI(value); } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Reset the volume values on start to 1.0f (max value) when starting the game again so that the mixer's values can be reset in the editor.
        SetVolumeGlobal(1.0f);
        SetVolumeSFX(1.0f);
        SetVolumeMusic(1.0f);
        SetVolumeVoice(1.0f);
        SetVolumeUI(1.0f);
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public AudioClip GetAudioClipSFX(string name)
    {
        return GetAudioClip(this.audioClipsSFX, name);
    }

    #endregion

    #region PublicMethods - Global

    public void SetVolumeGlobal(float volume)
    {
        this.volumeGlobal = Mathf.Clamp01(volume);
        SetMixerVolumeValue("Master", this.volumeGlobal);
    }

    public float GetVolumeGlobal()
    {
        return this.volumeGlobal;
    }

    #endregion

    #region PublicMethods - SFX / Effects

    public void PlaySoundElementSFX(Element element)
    {
        switch (element)
        {
            default:
                // Play nothing if the registered element is not supported.
                break;
            case Element.Water:
                PlaySoundSFX("element_water");
                break;
            case Element.Heal:
                PlaySoundSFX("element_heal");
                break;
            case Element.Cold:
                PlaySoundSFX("element_cold");
                break;
            case Element.Electricity:
                PlaySoundSFX("element_electricity");
                break;
            case Element.Death:
                PlaySoundSFX("element_death");
                break;
            case Element.Earth:
                PlaySoundSFX("element_rock");
                break;
            case Element.Fire:
                PlaySoundSFX("element_fire");
                break;
        }
    }

    public void PlaySoundSFX(string name)
    {
        var clip = GetAudioClip(this.audioClipsSFX, name);
        if (clip != null)
            PlaySoundSFX(clip);
    }

    public void PlaySoundSFX(AudioClip clip)
    {
        this.audioSourceSFX.PlayOneShot(clip);
        this.audioSourceSFX.pitch = Random.Range(0.6f, 1.2f); // Very shitty solution, needs to be reworked so that the results don't suck...
    }

    public void SetVolumeSFX(float volume)
    {
        this.volumeSFX = Mathf.Clamp01(volume);
        SetMixerVolumeValue("SFX", this.volumeSFX);
    }

    public float GetVolumeSFX()
    {
        return this.volumeSFX;
    }

    #endregion

    #region PublicMethods - Music

    public void PlayMusic(string name, bool loop = true)
    {
        var clip = GetAudioClip(this.audioClipsMusic, name);

        if (clip == null)
            return;

        this.audioSourceMusic.loop = loop;
        this.audioSourceMusic.clip = clip;
        this.audioSourceMusic.Play();
        this.currentMusicName = name;
    }

    public void StopMusic()
    {
        this.audioSourceMusic.Stop();
        this.currentMusicName = "";
    }

    public bool IsPlayingMusic(string name)
    {
        return name == this.currentMusicName;
    }

    public void SetVolumeMusic(float volume)
    {
        this.volumeMusic = Mathf.Clamp01(volume);
        SetMixerVolumeValue("Music", this.volumeMusic);
    }
    public float GetVolumeMusic()
    {
        return this.volumeMusic;
    }

    #endregion

    #region PublicMethods - Voice

    public void PlaySoundVoice(string name)
    {
        var clip = GetAudioClip(this.audioClipsVoice, name);
        if(clip != null)
            PlaySoundVoice(clip);
    }

    public void PlaySoundVoice(AudioClip clip)
    {
        this.audioSourceVoice.PlayOneShot(clip);
    }

    public void SetVolumeVoice(float volume)
    {
        this.volumeVoice = Mathf.Clamp01(volume);
        SetMixerVolumeValue("Voice", this.volumeVoice);
    }

    public float GetVolumeVoice()
    {
        return this.volumeVoice;
    }

    #endregion

    #region PublicMethods - UI

    public void PlaySoundUI(string name)
    {
        var clip = GetAudioClip(this.audioClipsUI, name);
        if (clip != null)
            PlaySoundUI(clip);
    }

    public void PlaySoundUI(AudioClip clip)
    {
        this.audioSourceUI.PlayOneShot(clip);
    }

    public void SetVolumeUI(float volume)
    {
        this.volumeUI = Mathf.Clamp01(volume);
        SetMixerVolumeValue("UI", this.volumeUI);
    }

    public float GetVolumeUI()
    {
        return this.volumeUI;
    }

    #endregion

    #region PrivateMethods

    private AudioClip GetAudioClip(NamedAudioClip[] clips, string name)
    {
        foreach (var clip in clips)
            if (clip.name == name)
                return clip.clip;
        return null;
    }

    // Maps the volume value from Unity's [0.0, 1.0] scale to the logarithmic db scale [-80, 0]
    private float MapVolumeValue(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1.0f); // Clamp the value cause log(0) is undefined. With the current implementation, it goes to NaN, which the mixer just treats as maximum boosting, and we don't want to rape any ears, now do we?
        return Mathf.Log10(volume) * 20.0f;
    }

    private void SetMixerVolumeValue(string name, float volume)
    {
        audioMixer.SetFloat(name, MapVolumeValue(volume));
    }

    #endregion
}
