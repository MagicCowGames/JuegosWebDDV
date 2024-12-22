using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePoolController : ObjectPool<AudioSourceController>
{
    #region Variables
    #endregion

    #region MonoBehaviour

    void Awake()
    {
        this.OnObjectSpawn += HandleOnObjectSpawned;
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void HandleOnObjectSpawned(AudioSourceController controller)
    {
        controller.OnSoundStop += Return;
    }

    #endregion
}
