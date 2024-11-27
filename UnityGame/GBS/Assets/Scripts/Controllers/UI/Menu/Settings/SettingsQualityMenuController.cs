using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsQualityMenuController : MonoBehaviour
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

    public void SetQualityVeryLow() { SetQualityInternal(0); }
    public void SetQualityLow() { SetQualityInternal(1); }
    public void SetQualityMedium() { SetQualityInternal(2); }
    public void SetQualityHigh() { SetQualityInternal(3); }
    public void SetQualityVeryHigh() { SetQualityInternal(4); }
    public void SetQualityUltra() { SetQualityInternal(5); }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods

    private void SetQualityInternal(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    #endregion
}
