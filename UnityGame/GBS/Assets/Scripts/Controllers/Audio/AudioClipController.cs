using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple controller script to add extra functionality to audio sources in Unity.
# region Comment

// This controller script adds extra basic functionality to audio clips.
// This is some functionality that Unity should offer tbh with some OnAudioFinished event and stuff like that or whatever, considering how just adding
// running coroutine that waits for the audioclip's duration is not good enough since the length changes with pitch... but whatever, I guess we're back to the
// fucking stoneage and to polling for changes on every single fucking frame...

// This controller script will make it easier to handle audio logic in the long run.
// Unity's built in audio systems are quite barebones but pretty extensible, so at least that's a plus.

#endregion
public class AudioClipController : MonoBehaviour
{
    #region Variables

    [SerializeField] private AudioSource audioSource;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        UpdateController();
    }

    #endregion

    #region PublicMethods

    public void SetSoundData(AudioClip clip, bool loop = false, float volumeScale = 1.0f, float pitch = 1.0f)
    {
        this.audioSource.clip = clip;
        this.audioSource.loop = loop;
        this.audioSource.volume = volumeScale;
        this.audioSource.pitch = pitch;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    #endregion

    #region PrivateMethods

    private void UpdateController()
    { }

    #endregion
}
