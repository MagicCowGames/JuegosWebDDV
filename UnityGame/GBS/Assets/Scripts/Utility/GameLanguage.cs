using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// NOTE : If it were up to me, I would personally store the language data within some JSON files on the game's files
// and just load the dictionaries for the Language Packs from there, but sadly I have no fucking clue how to make
// Unity include and then read config files in the build for WebGL when it would be as trivial as including a simple
// JS script file with a JSON object for the language data like back in Burning Village...
// but yeah, it is what it is... it kind of kills modding support, but it's not like it even matters for this game tbh
public static class LanguageSystem
{
    #region Variables

    private static string currentLanguage { get; set; } = "english";

    private static Dictionary<string, Dictionary<string, string>> languageData = new Dictionary<string, Dictionary<string, string>> {
        {"english", new Dictionary<string, string> {
            { "loc_play", "Play"},
            { "loc_account", "Account" },
            { "loc_settings", "Settings" },
            { "loc_credits", "Credits" }
        } }
    };

    #endregion

    #region PublicMethods

    public static string GetLocalizedString(string language, string locString)
    {
        if(languageData != null)
            if(languageData.ContainsKey(language))
                if (languageData[language].ContainsKey(locString))
                    return languageData[language][locString];
        return $"LOC[\"{language}\"][\"{locString}\"] NOT FOUND";
    }

    public static string GetLocalizedString(string locString)
    {
        return GetLocalizedString(currentLanguage, locString);
    }

    public static void SetLanguage(string language)
    {
        currentLanguage = language;
    }

    public static string GetLanguage()
    {
        return currentLanguage;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
