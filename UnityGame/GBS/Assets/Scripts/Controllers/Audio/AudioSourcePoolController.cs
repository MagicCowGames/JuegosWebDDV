using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePoolController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject prefab;
    [SerializeField] private int startingCount = 10;
    [SerializeField] private int maxCount = 100;

    private List<AudioSource> audioSources;
    private int activeCount;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.audioSources = new List<AudioSource>();
        for (int i = 0; i < startingCount; ++i)
        {
            SpawnAudioSource();
        }
        this.activeCount = 0;
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    // TODO : Maybe implement some kind of SoundSettings class or struct and use that as a parameter to contain the requested clip, volume multiplier, pitch, etc...
    public void PlaySound(AudioClip clip)
    {
        // TODO : Implement
    }

    #endregion

    #region PrivateMethods

    private AudioSource SpawnAudioSource()
    {
        var obj = ObjectSpawner.Spawn(prefab, this.transform);
        obj.transform.parent = this.transform;
        var audioSource = obj.GetComponent<AudioSource>();
        this.audioSources.Add(audioSource);
        audioSource.gameObject.SetActive(false);
        return audioSource;
    }

    // TODO : Refactor to push the inactive audio sources to the end of the list so that we can just use an "activeCount" to instantly get the correct index.
    private AudioSource GetAudioSource()
    {
        // var source = this.audioSources

        foreach (var source in this.audioSources)
        {
            if (!source.gameObject.activeSelf)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }

        if (this.audioSources.Count < this.maxCount)
            return SpawnAudioSource();

        return null;
    }

    private void ReturnAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(false);
    }

    #endregion
}
