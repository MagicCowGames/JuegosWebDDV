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
        // We could use this as the actual calculation of the FPS on every single frame, but we use a frame accumulator instead because we don't really want to
        // suffer the consequences of updating the FPS display text every single frame.
        // 1) It is more expensive to redraw the text every frame.
        // 2) It is annoying as fuck to see the framerate text change every single frame. The user wants to fucking read it, so we need to give them some time at least...
        this.UpdateFpsDisplayText((int)(1.0f / Time.unscaledDeltaTime));
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
