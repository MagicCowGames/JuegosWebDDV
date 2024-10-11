using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
    #region Variables
    [SerializeField] private bool debugEnabled = false;
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

    public void Log(string str)
    {
        Debug.Log($"[Debug] : {str}");
    }

    public void SetDebugEnabled(bool value)
    {
        this.debugEnabled = value;
    }

    public bool GetDebugEnabled()
    {
        return this.debugEnabled;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
