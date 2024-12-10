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
        float delta = Time.deltaTime;
        float speed = 10.0f;
        float sdt = delta * speed;

        this.volumeImageMusic.fillAmount = Mathf.Lerp(this.volumeImageMusic.fillAmount, SoundManager.Instance.GetMusicVolume(), sdt);
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
