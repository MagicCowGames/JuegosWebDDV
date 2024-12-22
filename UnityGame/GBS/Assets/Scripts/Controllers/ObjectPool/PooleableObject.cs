using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooleableObject<T> : MonoBehaviour where T : Component
{
    #region Variables

    public T Object { get; private set; }

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
    #endregion

    #region PrivateMethods
    #endregion
}
