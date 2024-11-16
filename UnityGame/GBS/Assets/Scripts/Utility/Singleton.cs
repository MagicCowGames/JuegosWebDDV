using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    #region Variables
    public static T Instance { get; private set; }
    #endregion

    #region MonoBehaviour

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            // this.InitializeManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    // NOTE : This code is disabled because it is an interesting idea but it feels overengineered. It's just easier to call base.Awake() and that's it.
    /*
    #region ProtectedMethods

    protected virtual void InitializeManager()
    {
        // NOTE : This method should be overridden by classes that inherit from Singleton<T> so that they can customize their initialization on Awake
        // without having to override Awake() and call base.Awake().
    }

    #endregion
    */
}
