using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSoundMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image volumeImageMusic;
    [SerializeField] private Slider volumeSliderMusic;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.volumeSliderMusic.value = SoundManager.Instance.GetMusicVolume();
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

    // Deprecated old system
    /*
    public void DecreaseVolumeMusic()
    {
        SoundManager.Instance.SetMusicVolumeLevel(SoundManager.Instance.GetMusicVolumeLevel() - 1);
    }

    public void IncreaseVolumeMuisc()
    {
        SoundManager.Instance.SetMusicVolumeLevel(SoundManager.Instance.GetMusicVolumeLevel() + 1);
    }
    */

    public void Slider_SetVolume_Music(float volume)
    {
        SoundManager.Instance?.SetMusicVolume(volume);
    }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
