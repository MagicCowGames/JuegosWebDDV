using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUIController : UIController
{
    #region Variables

    [Header("Info UI Components")]
    [SerializeField] private TMP_Text fpsText;

    [Header("Info UI Configuration")]
    [SerializeField] private float fpsUpdateTime = 0.4f;

    private float accumulatedTime = 0.0f;
    private int accumulatedFrames = 0;

    private float fps = 0.0f;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Rough initial estimate of the FPS on the first frame of the game.
        this.UpdateFpsDisplayText((int)(1.0f / Time.deltaTime));
    }

    void Update()
    {
        this.UpdateFps(Time.unscaledDeltaTime);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateFps(float delta)
    {
        this.accumulatedFrames += 1;
        this.accumulatedTime += delta;

        if (this.accumulatedTime < this.fpsUpdateTime)
            return;

        this.fps = this.accumulatedFrames / this.accumulatedTime;
        this.UpdateFpsDisplayText((int)fps);

        this.accumulatedFrames = 0;
        this.accumulatedTime = 0.0f;
    }

    private void UpdateFpsDisplayText(int fps)
    {
        if (this.fpsText == null)
            return;
        this.fpsText.text = $"{fps} FPS";
    }

    #endregion
}
