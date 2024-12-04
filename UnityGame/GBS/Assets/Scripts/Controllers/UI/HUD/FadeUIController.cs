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

    void Start()
    {
        Init();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateFade(delta);
    }

    #endregion

    #region PublicMethods - Fade In / Out

    public void Fade(float targetOpacity, float duration = 1.5f)
    {
        this.targetOpacity = Mathf.Clamp01(targetOpacity);
        float difference = Mathf.Abs(targetOpacity - this.canvasGroup.alpha);
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

    private void SetColor(Color color)
    {
        this.backgroundImage.color = color;
    }

    private void SetImage(Sprite image)
    {
        this.backgroundImage.sprite = image;
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
        this.canvasGroup.alpha = Mathf.Lerp(this.canvasGroup.alpha, this.targetOpacity, delta * this.lerpSpeed);
    }

    #endregion
}
