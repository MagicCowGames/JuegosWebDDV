using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameUtility
{
    #region Pause

    private static bool isPaused = false;

    public static bool GetPaused()
    {
        return isPaused;
    }

    public static void SetPaused(bool value)
    {
        isPaused = value;

        float timeScale;
        string str;

        if (isPaused)
        {
            timeScale = 0.0f;
            str = "Pause";
        }
        else
        {
            timeScale = 1.0f;
            str = "Resume";
        }

        DebugManager.Instance?.Log(str);

        // UIManager.Instance?.GetPauseUI().SetVisible(isPaused);
        // UIManager.Instance?.GetPlayerUI().SetVisible(!isPaused);

        Time.timeScale = timeScale;
    }

    public static void SwitchPaused()
    {
        SetPaused(!GetPaused());
    }

    public static void Pause()
    {
        SetPaused(true);
    }

    public static void Resume()
    {
        SetPaused(false);
    }

    #endregion
}
