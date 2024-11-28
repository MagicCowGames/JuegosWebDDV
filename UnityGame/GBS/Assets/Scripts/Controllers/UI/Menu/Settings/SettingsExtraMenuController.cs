using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsExtraMenuController : MonoBehaviour
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

    public void SetDebugEnabled(bool enabled)
    { }

    public void SetConsoleEnabled(bool enabled)
    { }

    public void SetVersionEnabled(bool enabled)
    { }

    public void SetFpsEnabled(bool enabled)
    { }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
