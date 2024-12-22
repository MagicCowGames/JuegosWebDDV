using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePoolController : ObjectPool<AudioSourceController>
{
    #region Variables
    #endregion

    #region MonoBehaviour

    void Start()
    {

    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public new AudioSourceController Get()
    {
        var ans = base.Get();
        if (ans == null)
            return null;

        // And so the hack starts... fucking Unity I swear to God.
        ans.OnSoundStop = null;
        ans.OnSoundStop += (audioSource) => { Return(audioSource); };
        
        return ans;
    }

    #endregion

    #region PrivateMethods


    #endregion
}
