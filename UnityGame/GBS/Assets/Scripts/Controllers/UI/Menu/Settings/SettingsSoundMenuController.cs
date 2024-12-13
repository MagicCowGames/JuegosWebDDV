using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSoundMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Slider volumeSliderGlobal;
    [SerializeField] private Slider volumeSliderSFX;
    [SerializeField] private Slider volumeSliderMusic;
    [SerializeField] private Slider volumeSliderVoice;
    [SerializeField] private Slider volumeSliderUI;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.volumeSliderGlobal.value = SoundManager.Instance.VolumeGlobal;
        this.volumeSliderSFX.value = SoundManager.Instance.VolumeSFX;
        this.volumeSliderMusic.value = SoundManager.Instance.VolumeMusic;
        this.volumeSliderVoice.value = SoundManager.Instance.VolumeVoice;
        this.volumeSliderUI.value = SoundManager.Instance.VolumeUI;
    }

    void Update()
    {

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

    public void Slider_SetVolume_Global(float volume)
    {
        SoundManager.Instance.VolumeGlobal = volume;
    }

    public void Slider_SetVolume_SFX(float volume)
    {
        SoundManager.Instance.VolumeSFX = volume;
    }

    public void Slider_SetVolume_Music(float volume)
    {
        SoundManager.Instance.VolumeMusic = volume;
    }

    public void Slider_SetVolume_Voice(float volume)
    {
        SoundManager.Instance.VolumeVoice = volume;
    }

    public void Slider_SetVolume_UI(float volume)
    {
        SoundManager.Instance.VolumeUI = volume;
    }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
