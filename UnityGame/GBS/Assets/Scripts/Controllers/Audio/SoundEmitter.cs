using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    #region Variables

    [SerializeField] private SoundData soundData;

    #endregion

    #region MonoBehaviour

    void OnEnable()
    {
        if (this.soundData.loop)
            Play();
    }

    void OnDisable()
    {
        Stop();
    }

    #endregion

    #region PublicMethods

    public void Play()
    {
        // SoundManager.Instance?.PlaySoundSFX(this.soundData);
    }

    public void Stop()
    {

    }

    #endregion

    #region PrivateMethods
    #endregion
}
