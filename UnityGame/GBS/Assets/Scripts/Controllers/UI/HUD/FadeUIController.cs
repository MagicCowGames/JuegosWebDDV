using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// NOTE : The Fade UI supports custom images and colors for the fading UI.
// The system is inspired by the GTA connected mod's fading mechanism.
public class FadeUIController : UIController
{
    #region Variables

    [Header("Menu PopUp Controller")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image backgroundImage;

    private float targetOpacity;
    private float lerpSpeed;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    void Update()
    {
        float delta = Time.unscaledDeltaTime;
        UpdateFade(delta);
    }

    #endregion

    #region PublicMethods - Fade In / Out

    public void Fade(float targetOpacity, float duration = 1.5f)
    {
        this.targetOpacity = Mathf.Clamp01(targetOpacity);
        float difference = targetOpacity - this.canvasGroup.alpha;
        this.lerpSpeed = difference / duration;
    }

    public void FadeIn(float duration = 1.5f)
    {
        Fade(0.0f, duration);
    }

    public void FadeOut(float duration = 1.5f)
    {
        Fade(1.0f, duration); // Bender es genial, Fundido a negro.
    }

    #endregion

    #region PublicMethods - Image

    public void SetColor(Color color)
    {
        this.backgroundImage.color = color;
    }

    public void SetImage(Sprite image)
    {
        this.backgroundImage.sprite = image;
    }

    public void SetOpacity(float opacity)
    {
        this.canvasGroup.alpha = opacity;
    }

    #endregion

    #region PrivateMethods

    private void Init()
    {
        this.targetOpacity = 0.0f;
        this.lerpSpeed = 0.0f;
    }

    private void UpdateFade(float delta)
    {
        this.canvasGroup.alpha = Mathf.Clamp01(this.canvasGroup.alpha + (delta * this.lerpSpeed)); // No lerping, just constant speed change to keep transition durations consistent with framerate.
    }

    #endregion
}
