using UnityEngine;

public enum SoundType
{
    SFX = 0,
    Music,
    Voice,
    UI
}

public enum SoundPlayType
{
    Global = 0,
    Local
}

[System.Serializable]
public struct SoundData
{
    public AudioClip clip;
    public bool loop;
    public float volume;
    public float pitch;
    public SoundType type;

    public SoundData(AudioClip clip, SoundType type = SoundType.SFX, bool loop = false, float volume = 1.0f, float pitch = 1.0f)
    {
        this.clip = clip;
        this.loop = loop;
        this.volume = volume;
        this.pitch = pitch;
        this.type = type;
    }
}