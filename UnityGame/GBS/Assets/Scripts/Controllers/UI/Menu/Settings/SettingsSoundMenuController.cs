using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSoundMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image volumeImageMusic;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        this.volumeImageMusic.fillAmount = 0.5f;
    }

    #endregion

    #region PublicMethods

    public void DecreaseVolumeMusic()
    {
        SoundManager.Instance.SetMusicVolumeLevel(SoundManager.Instance.GetMusicVolumeLevel() - 1);
    }

    public void IncreaseVolumeMuisc()
    {
        SoundManager.Instance.SetMusicVolumeLevel(SoundManager.Instance.GetMusicVolumeLevel() + 1);
    }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
