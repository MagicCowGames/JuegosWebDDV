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
    #region Enums

    public enum Language
    {
        English = 0,
        Spanish,
        COUNT
    }

    #endregion

    #region Variables

    private static Language currentLanguage = Language.English;

    // TODO : Rework the dictionary systems to use plain arrays instead since the language is now an enum and not a string, so the lookup would be O(1) anyways...
    // All the dict is doing now is actually harming performance and memory usage lol...

    // NOTE : We could change these strings to be enums too...

    private static Dictionary<Language, Dictionary<string, string>> languageData = new Dictionary<Language, Dictionary<string, string>> {
        { Language.English, new Dictionary<string, string> {
            { "loc_language_name", "English" },
            { "loc_play", "Play"},
            { "loc_account", "Account" },
            { "loc_settings", "Settings" },
            { "loc_credits", "Credits" },
            { "loc_return", "Return" },
            { "loc_language", "Language" },
            { "loc_setting_language", "Choose Language" },
            { "loc_tutorial", "Tutorial" },
            { "loc_pause", "Pause" },
            { "loc_paused", "Paused" },
            { "loc_resume", "Resume" },
            { "loc_return_to_menu", "Return to Menu" },


            { "loc_test_dummy", "Test Dummy" },
            { "loc_test_dummy_description", "Test your spells on this immortal dummy" },
            { "loc_test_dummies", "Test Dummies" },
            { "loc_test_dummies_description", "Test your spells on these immortal dummies" },
            { "loc_test_dummies_mortal_description", "Test your spells on these dummies" },
            { "loc_test_healing_area", "Healing Area" },
            { "loc_test_damage_area", "Damage Area" }
        } },
        { Language.Spanish, new Dictionary<string, string> {
            { "loc_language_name", "Espa�ol" },
            { "loc_play", "Jugar" },
            { "loc_account", "Cuenta" },
            { "loc_settings", "Opciones" }, // This used to be "Configuraci�n", my beloved </3 :( but the fucking thing was too large to fit on the UI buttons. Oh well, too bad!
            { "loc_credits", "Cr�ditos" },
            { "loc_return", "Volver" },
            { "loc_language", "Lenguaje" },
            { "loc_setting_language", "Seleccionar Idioma" },
            { "loc_tutorial", "Tutorial" },
            { "loc_pause", "Pausar" },
            { "loc_paused", "Pausado" },
            { "loc_resume", "Reanudar" },
            { "loc_return_to_menu", "Volver al Men�" },


            { "loc_test_dummy", "Mu�eco de Pruebas" },
            { "loc_test_dummy_description", "Prueba tus conjuros en este mu�eco de pruebas inmortal" },
            { "loc_test_dummies", "Mu�ecos de Pruebas" },
            { "loc_test_dummies_description", "Prueba tus conjuros en estos mu�ecos de pruebas inmortales" },
            { "loc_test_dummies_mortal_description", "Prueba tus conjuros en estos mu�ecos de pruebas" },
            { "loc_test_healing_area", "Area de Curaci�n" },
            { "loc_test_damage_area", "Area de Da�o" }
        } }
    };

    private static Dictionary<Language, string[]> languageStrings = new Dictionary<Language, string[]> {
        { Language.English, new string[] { "0", "english", "en", "eng" } },
        { Language.Spanish, new string[] { "1", "spanish", "es", "esp" } }
    };

    #endregion

    #region PublicMethods

    public static string GetLocalizedString(Language language, string locString)
    {
        // This if ladder is ugly, and I know how to fix it, but it's not even going to make performance better or worse so I can't be assed right now to clean this shit.
        if(languageData != null && locString != null)
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
        if (language != null)
            foreach (var lang in languageStrings)
                if (lang.Value.Contains(language))
                {
                    currentLanguage = lang.Key;
                    return;
                }
        currentLanguage = Language.English; // Set the language to English by default if the string could not be found.
        // NOTE : Maybe we should just not change the current language if the language is not found? Or offer an external bool func to return if the str is valid or not.
    }

    public static void SetLanguage(Language language)
    {
        currentLanguage = language;
    }

    // This used to be a proper language wrap around function, but the wrap around code did not work correctly because the modulo operator does not work
    // the same way in Unity's version of C# as it does in base .NET or C or C++, and I can't be assed to figure out why Unity does things different
    // for the fucking modulo operator, so yeah. It is what it is.
    public static void SetLanguage(int language)
    {
        if (language < 0)
            language = (int)Language.COUNT - 1;
        if (language >= (int)Language.COUNT)
            language = 0;
        currentLanguage = (Language)language;
    }

    public static Language GetLanguage()
    {
        return currentLanguage;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
