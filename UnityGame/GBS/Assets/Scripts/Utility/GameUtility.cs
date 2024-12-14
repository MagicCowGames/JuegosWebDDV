using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameUtility
{
    #region Pause

    private static bool isPaused = false;
    private static bool canPause = false;

    public static bool GetPaused()
    {
        return isPaused;
    }

    public static void SetPaused(bool value)
    {
        // If we try to pause but we can't pause cause we ain't allowed to do so at this moment, then just get the fuck out with an early return and get it over with.
        if (!canPause && value)
        {
            DebugManager.Instance?.Log("Cannot pause at this moment!");
            return;
        }

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

        UIManager.Instance?.GetPauseUIController()?.UI_SetVisible(isPaused);
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

    // This should be used any time the user enters a menu we don't want them to be able to pause in.
    // This will be useful if, for example, we want the escape key on those menus to mean "get out of this menu".
    // Also on the death screen, but yeah.
    // The day has come!!! LMAO Thanks myself from the past for thinking ahead of time!
    public static void SetCanPause(bool value)
    {
        canPause = value;
    }

    public static bool GetCanPause()
    {
        return canPause;
    }

    #endregion

    #region Strings and Colors

    public static string GetColorHexString(Color color)
    {
        var r = Mathf.Clamp01(color.r);
        var g = Mathf.Clamp01(color.g);
        var b = Mathf.Clamp01(color.b);

        int ri = (int)(255 * r);
        int gi = (int)(255 * g);
        int bi = (int)(255 * b);

        string rstr = $"{ri:X2}";
        string gstr = $"{gi:X2}";
        string bstr = $"{bi:X2}";

        string ans = $"#{rstr}{gstr}{bstr}";
        return ans;
    }

    public static string GetColorString(string message, Color color)
    {
        return $"<color={GetColorHexString(color)}>{message}</color>";
    }

    #endregion
}
