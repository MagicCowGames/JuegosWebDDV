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
            // Loc Strings : Generic
            { "loc_language_name", "English" },
            { "loc_default", "Default" },
            { "loc_default_text", "Default Text"},
            
            // Loc Strings : Menu
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
            { "loc_you_are_dead", "You are Dead" },
            { "loc_retry", "Retry" },
            { "loc_store", "Store" },
            { "loc_inventory", "Inventory" },
            { "loc_register_account", "Register Account"},
            { "loc_login_account", "Log Into Account" },
            { "loc_score", "Score" },
            { "loc_scoreboard", "Scoreboard" },
            { "loc_victory", "Victory!" },
            { "loc_demo_thanks", "Thank you for playing the \"<color=#FF0000>Wizard's Death Wish</color>\" demo!\r\nStay tuned for more updates!" },

            // Loc Strings : Actions (wtf is this name?)
            { "loc_use", "Use" },
            { "loc_delete", "Delete" },

            // Loc Strings : Account
            { "loc_register", "Register" },
            { "loc_login", "Login" },
            { "loc_logout", "Log Out" },
            { "loc_delete_account", "Delete Account" },

            // Loc Strings : Errors
            { "loc_error_network_title", "Network Error" },
            { "loc_error_network_message", "A network error has occurred! Check your Internet connection." },
            { "loc_error_connection_title", "Connection Error" },
            { "loc_error_connection_message", "Could not connect to server!" },
            { "loc_error_register_title", "Registration Error" },
            { "loc_error_register_message", "Could not register account! The chosen name is already in use." },
            { "loc_error_validation_title", "Validation Error" },
            { "loc_error_validation_message_name", "The chosen name is not valid" },
            { "loc_error_validation_message_password", "The chosen password is not valid" },
            { "loc_error_login_title", "Login Error" },
            { "loc_error_login_message", "Could not log into account! The credentials are not correct." },
            { "loc_error_account_delete_title", "Validation Error" },
            { "loc_error_account_delete_message", "Could not delete the account! The credentials are not correct." },

            // Loc Strings : Windows / PopUps
            { "loc_close", "Close" },

            // Loc Strings : Test
            { "loc_test_dummy", "Test Dummy" },
            { "loc_test_dummy_description", "Test your spells on this immortal dummy" },
            { "loc_test_dummies", "Test Dummies" },
            { "loc_test_dummies_description", "Test your spells on these immortal dummies" },
            { "loc_test_dummies_mortal_description", "Test your spells on these dummies" },
            { "loc_test_healing_area", "Healing Area" },
            { "loc_test_damage_area", "Damage Area" },
            { "loc_test_pickup_coins", "Grab the Coins" },
            { "loc_test_dummy_spawn", "Step on the pressure plate to spawn a dummy" },

            // Loc Strings : Tutorial
            { "loc_tutorial_message", "Welcome to the training room! Here, aspiring wizards such as yourself can practice with their spells and learn to spell like a real wizard. Take a look around and explore to learn how to use your magic." },

            // Loc Strings : Points and Currency
            { "loc_experience", "Experience" },
            { "loc_points", "Points" },
            { "loc_money", "Money" },

            // Loc Strings : Funny
            { "loc_popmeup", "Vlad is not a vampire" },

            // Loc Strings : Store
            { "loc_buy", "Buy" },
            { "loc_inspect", "Inspect" },

            // Loc Strings : WIP
            { "loc_wip", "WIP"},
            { "loc_wip_title", "Work In Progress" },
            { "loc_wip_message", "This feature is a work in progress and is yet not finished. Stay tuned for more updates." },
            { "loc_coming_soon", "COMING SOON!" },

            // Loc Strings : Scoreboard
            { "loc_top_10", "Top 10" }
        } },
        { Language.Spanish, new Dictionary<string, string> {
            // Loc Strings : Generic
            { "loc_language_name", "Español" },
            { "loc_default", "Por defecto" },
            { "loc_default_text", "Texto por Defecto" },
            
            // Loc Strings : Menu
            { "loc_play", "Jugar" },
            { "loc_account", "Cuenta" },
            { "loc_settings", "Opciones" }, // This used to be "Configuración", my beloved </3 :( but the fucking thing was too large to fit on the UI buttons. Oh well, too bad!
            { "loc_credits", "Créditos" },
            { "loc_return", "Volver" },
            { "loc_language", "Lenguaje" },
            { "loc_setting_language", "Seleccionar Idioma" },
            { "loc_tutorial", "Tutorial" },
            { "loc_pause", "Pausar" },
            { "loc_paused", "Pausado" },
            { "loc_resume", "Reanudar" },
            { "loc_return_to_menu", "Volver al Menú" },
            { "loc_you_are_dead", "Estás Muerto" },
            { "loc_retry", "Reintentar" },
            { "loc_store", "Tienda" },
            { "loc_inventory", "Inventario" },
            { "loc_register_account", "Registrar Cuenta"},
            { "loc_login_account", "Acceder a Cuenta" },
            { "loc_score", "Puntuación" },
            { "loc_scoreboard", "Tabla de Puntuación" },
            { "loc_victory", "¡Victoria!" },
            { "loc_demo_thanks", "¡Gracias por jugar la demo de \"<color=#FF0000>Wizard's Death Wish</color>\"!\r\n¡Estad atentos para más actualizaciones!" },

            // Loc Strings : Actions (wtf is this name?)
            { "loc_use", "Usar" },
            { "loc_delete", "Borrar" },

            // Loc Strings : Account
            { "loc_register", "Registrarse" },
            { "loc_login", "Acceder" },
            { "loc_logout", "Salir" },
            { "loc_delete_account", "Eliminar Cuenta" },

            // Loc Strings : Errors
            { "loc_error_network_title", "Error de Red" },
            { "loc_error_network_message", "¡Ha ocurrido un error de red! Compruebe su conexión a Internet." },
            { "loc_error_connection_title", "Error de Conexión" },
            { "loc_error_connection_message", "¡No se pudo conectar al servidor!" },
            { "loc_error_register_title", "Error de Registro" },
            { "loc_error_register_message", "¡No se pudo registrar la cuenta! El nombre escogido ya está en uso." },
            { "loc_error_validation_title", "Error de Validación" },
            { "loc_error_validation_message_name", "El nombre escogido no es válido" },
            { "loc_error_validation_message_password", "La contraseña escogida no es válida" },
            { "loc_error_login_title", "Error de Acceso" },
            { "loc_error_login_message", "¡No se puedo acceder a la cuenta! Las credenciales no son correctas." },
            { "loc_error_account_delete_title", "Error de Validación" },
            { "loc_error_account_delete_message", "¡No se pudo eliminar la cuenta! Las credenciales no son correctas." },

            // Loc Strings : Windows / PopUps
            { "loc_close", "Cerrar" },

            // Loc Strings : Test
            { "loc_test_dummy", "Muñeco de Pruebas" },
            { "loc_test_dummy_description", "Prueba tus conjuros en este muñeco de pruebas inmortal" },
            { "loc_test_dummies", "Muñecos de Pruebas" },
            { "loc_test_dummies_description", "Prueba tus conjuros en estos muñecos de pruebas inmortales" },
            { "loc_test_dummies_mortal_description", "Prueba tus conjuros en estos muñecos de pruebas" },
            { "loc_test_healing_area", "Area de Curación" },
            { "loc_test_damage_area", "Area de Daño" },
            { "loc_test_pickup_coins", "Recolecta las Monedas" },
            { "loc_test_dummy_spawn", "Pisa la placa de presión para spawner un muñeco" },

            // Loc Strings : Tutorial
            { "loc_tutorial_message", "¡Bienvenido a la sala de entrenamiento! Aquí, los aspirantes a mago pueden practicar sus conjuros y aprender a conjurar como verdaderos hechiceros. Explora y aprende a usar tu magia." }, // This translation was kinda butchered due fact that the tutorial screen did not have enough space to display it... FUCK.

            // Loc Strings : Points and Currency
            { "loc_experience", "Experiencia" },
            { "loc_points", "Puntos" },
            { "loc_money", "Dinero" },

            // Loc Strings : Funny
            { "loc_popmeup", "Vlad no es un vampiro" },
            
            // Loc Strings : Store
            { "loc_buy", "Comprar" },
            { "loc_inspect", "Inspeccionar" },

            // Loc Strings : WIP
            { "loc_wip", "WIP"},
            { "loc_wip_title", "Trabajo en Progreso" },
            { "loc_wip_message", "Esta caracteristica está en proceso de creación y no está terminada todavía. Esté atento a más actualizaciones." },
            { "loc_coming_soon", "¡PRÓXIMAMENTE!" },

            // Loc Strings : Scoreboard
            { "loc_top_10", "Top 10" }
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
