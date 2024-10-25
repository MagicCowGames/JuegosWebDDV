using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// NOTE : If it were up to me, I would personally store the language data within some JSON files on the game's files
// and just load the dictionaries for the Language Packs from there, but sadly I have no fucking clue how to make
// Unity include and then read config files in the build for WebGL when it would be as trivial as including a simple
// JS script file with a JSON object for the language data like back in Burning Village...
// but yeah, it is what it is... it kind of kills modding support, but it's not like it even matters for this game tbh
public static class LanguageSystem // TODO : Rename this fucking class bruh
{
    #region Variables

    private static string currentLanguage { get; set; } = "english";

    private static Dictionary<string, Dictionary<string, string>> languageData = new Dictionary<string, Dictionary<string, string>> {
        {"english", new Dictionary<string, string> {
            { "loc_play", "Play"},
            { "loc_account", "Account" },
            { "loc_settings", "Settings" },
            { "loc_credits", "Credits" },
            { "loc_return", "Return" }
        } },
        { "spanish", new Dictionary<string, string> {
            { "loc_play", "Jugar" },
            { "loc_account", "Cuenta" },
            { "loc_settings", "Configuración" },
            { "loc_credits", "Créditos" },
            { "loc_return", "Volver" }
        } }
    };

    #endregion

    #region PublicMethods

    public static string GetLocalizedString(string language, string locString)
    {
        // This if ladder is ugly, and I know how to fix it, but it's not even going to make performance better or worse so I can't be assed right now to clean this shit.
        if(languageData != null && language != null && locString != null)
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

    // NOTE : Maybe we should hard code this list or just build it on start up to prevent having to construct this data every single time...
    // For now, it will suffice, but this makes me sad :(
    // FUCKING DEADLINES
    // Or maybe we can just hack around this problem by making it so that whatever component requires getting this value gets it on start or on validate.
    // That would kind of add support for adding new languages during runtime, which we don't really need, but whatever... it is what it is!
    public static string[] GetAllLanguages()
    {
        string[] ans = languageData.Keys.ToArray();
        return ans;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
