using System;
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

// NOTE : The current implementation of the attachment logic using updates to set the transform position rather than truly attaching to a parent transform is
// used because it allows the audio source to keep living even if its parent object is destroyed, which would break audio source pooling unless further (and slower)
// logic was added to the pooling system.

#endregion
public class AudioSourceController : MonoBehaviour
{
    #region Variables

    [SerializeField] private AudioSource audioSource;

    private bool isAttached;
    private Transform target;

    public Action<AudioSourceController> OnSoundPlay;
    public Action<AudioSourceController> OnSoundStop;

    #endregion

    #region Properties

    // Component variables
    public AudioSource Source { get { return this.audioSource; } }
    public bool IsAttached { get { return this.isAttached; } set { this.isAttached = value; } }
    public Transform Target { get { return this.target; } set { this.target = value; } }

    // Audio Source Data
    public AudioClip Clip { get { return this.audioSource.clip; } set { this.audioSource.clip = value; } }
    public float Volume { get { return this.audioSource.volume; } set { this.audioSource.volume = value; } } // NOTE : Do not confuse this volume scale with the global audio mixer's volume.
    public float Pitch { get { return this.audioSource.pitch; } set { this.audioSource.pitch = value; } }
    public bool Loop { get { return this.audioSource.loop; } set { this.audioSource.loop = value; } }

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

    #region PublicMethods - Sound

    public void SetSoundData(AudioClip clip, bool loop = false, float volumeScale = 1.0f, float pitch = 1.0f)
    {
        this.audioSource.clip = clip;
        this.audioSource.loop = loop;
        this.audioSource.volume = volumeScale;
        this.audioSource.pitch = pitch;
    }

    public void PlaySound()
    {
        this.audioSource.Play();
        this.OnSoundPlay?.Invoke(this);
    }

    public void PlaySoundAt(Vector3 point)
    {
        this.audioSource.gameObject.transform.position = point;
        PlaySound();
    }

    public void StopSound()
    {
        this.audioSource.Stop();
        this.OnSoundStop?.Invoke(this);
    }

    #endregion

    #region PublicMethods - Attach

    public void Attach(Transform target)
    {
        this.target = target;
        this.isAttached = true;
    }

    public void Detach()
    {
        this.isAttached = false;
    }

    #endregion

    #region PrivateMethods

    private void UpdateController()
    {
        if (this.isAttached)
        {
            if (this.target != null)
            {
                this.gameObject.transform.position = target.position;
            }
            else
            {
                this.isAttached = false;
            }
        }

        if (this.gameObject.activeSelf && !this.audioSource.isPlaying)
            StopSound();
    }

    #endregion
}
