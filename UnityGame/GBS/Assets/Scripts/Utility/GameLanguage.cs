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
            { "loc_next", "Next" },
            { "loc_previous", "Previous" },
            
            // Loc Strings : Menu
            { "loc_play", "Play"},
            { "loc_play_campaign", "Play Campaign" },
            { "loc_play_procedural", "Play Procedural" },
            { "loc_account", "Account" },
            { "loc_credits", "Credits" },
            { "loc_return", "Return" },
            { "loc_language", "Language" },
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

            // Loc Strings : Settings
            { "loc_settings", "Settings" },
            { "loc_settings_language", "Language" },
            { "loc_settings_choose_language", "Choose Language" },
            { "loc_settings_enable_fps", "Display FPS" },
            { "loc_settings_enable_version", "Display Version" },
            { "loc_settings_enable_console", "Enable Console" },
            { "loc_settings_quality", "Quality" },
            { "loc_settings_quality_0", "Very Low" },
            { "loc_settings_quality_1", "Low" },
            { "loc_settings_quality_2", "Medium" },
            { "loc_settings_quality_3", "High" },
            { "loc_settings_quality_4", "Very High" },
            { "loc_settings_quality_5", "Ultra" },
            { "loc_settings_extra", "Extra" },
            { "loc_settings_sound", "Sound" },
            { "loc_settings_volume_global", "Global Volume" },
            { "loc_settings_volume_sfx", "SFX Volume" },
            { "loc_settings_volume_music", "Music Volume" },
            { "loc_settings_volume_voice", "Voice Volume" },
            { "loc_settings_volume_ui", "UI Volume" },

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
            { "loc_warning_title", "Warning" },
            { "loc_warning_play", "You are about to play without logging into an account. Your progress will not be saved." },

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
            { "loc_test_ghost_spawn", "Step on the pressure plate to spawn a ghost" },

            // Loc Strings : Tutorial Level
            { "loc_tutorial_message_0", "Welcome to the training room! Here, aspiring wizards such as yourself can practice with their spells and learn to spell like a real wizard. Take a look around and explore to learn how to use your magic." },
            { "loc_tutorial_message_1", "Explore the area ahead. There are plenty of things to do. When you are done training, pause and go to the menu. Beware of evil ghosts!" },
            
            // Loc Strings : Tutorial Menu
            { "loc_tutorial_menu", "Read Tutorial" },
            { "loc_tutorial_map", "Play Tutorial" },

            // Loc Strings : Tutorial Pages
            { "loc_tut_pages_0", "Controls" },
            { "loc_tut_pages_1", "The mouse is used to move and cast spells.\r\nThe mouse pointer determines the aiming direction." },
            { "loc_tut_pages_2", "RMB\r\nMovement" },
            { "loc_tut_pages_3", "LMB\r\nCast Spell" },
            { "loc_tut_pages_4", "Elements" },
            { "loc_tut_pages_5", "The elements determine the effects of the spell.\r\nA spell can combine multiple elements." },
            { "loc_tut_pages_6", "Q : Water" },
            { "loc_tut_pages_7", "F : Fire" },
            { "loc_tut_pages_8", "R : Cold" },
            { "loc_tut_pages_9", "A : Electricity" },
            { "loc_tut_pages_10", "W : Healing" },
            { "loc_tut_pages_11", "D : Earth" },
            { "loc_tut_pages_12", "S : Death" },
            { "loc_tut_pages_13", "Forms" },
            { "loc_tut_pages_14", "The form determines the physical manifestation of the spell" },
            { "loc_tut_pages_15", "1 : Projectile" },
            { "loc_tut_pages_16", "2 : Beam" },
            { "loc_tut_pages_17", "3 : Shield" },
            { "loc_tut_pages_18", "Element Queue" },
            { "loc_tut_pages_19", "The element queue is used to create spells. First choose the form. Then combine the elements." },
            { "loc_tut_pages_20", "Form" },
            { "loc_tut_pages_21", "Elements" },
            { "loc_tut_pages_22", "Spell" },
            { "loc_tut_pages_23", "Element Queue" },
            { "loc_tut_pages_24", "You can combine up to 5 elements.\r\nEach element has a different effect." },
            { "loc_tut_pages_25", "Empty queue" },
            { "loc_tut_pages_26", "Filled queue" },
            { "loc_tut_pages_27", "Combinations" },
            { "loc_tut_pages_28", "There are subelements that can be obtained by combining other elements." },
            { "loc_tut_pages_29", "Steam" },
            { "loc_tut_pages_30", "Ice" },
            { "loc_tut_pages_31", "Opposites" },
            { "loc_tut_pages_32", "Opposite elements cancel eachother out." },
            { "loc_tut_pages_33", "Examples" },

            // Loc Strings : Points and Currency
            { "loc_experience", "Experience" },
            { "loc_points", "Points" },
            { "loc_money", "Money" },

            // Loc Strings : Funny
            { "loc_popmeup", "Vlad is not a vampire" },

            // Loc Strings : Store
            { "loc_buy", "Buy" },
            { "loc_inspect", "Inspect" },
            { "loc_buy_items", "Buy Items" },
            { "loc_buy_currency", "Buy Coins" },
            { "loc_transaction", "Transaction" },
            { "loc_transaction_success", "Transaction Successfully Processed!" },
            { "loc_transaction_success_msg", "The transaction has been successfully processed. Enjoy your purchase!" },

            // Loc Strings : WIP
            { "loc_wip", "WIP"},
            { "loc_wip_title", "Work In Progress" },
            { "loc_wip_message", "This feature is a work in progress and is yet not finished. Stay tuned for more updates." },
            { "loc_coming_soon", "COMING SOON!" },

            // Loc Strings : Scoreboard
            { "loc_top_10", "Top 10" },

            // Loc Strings : Entity Names
            { "loc_name_wizard", "Wizard" },
            { "loc_name_wizard_unknown", "Unknown Wizard" },

            // Loc Strings : Dialogues
            { "loc_dialogue_0_0", "A pile of most interesting arcane texts!" },
            { "loc_dialogue_0_1", "Too bad I don't really care about any of this." },
            { "loc_dialogue_1_0", "They think they can keep me imprissoned in this tower? Amateurs... They don't know my true power!" },
            { "loc_dialogue_2_0", "So you are trying to escape..." },
            { "loc_dialogue_2_1", "¡You will not make it out of here alive!" },
            { "loc_dialogue_3_0", "No... it cannot be..." },
            { "loc_dialogue_3_1", "You will not be able to escape. The Tower is powerful. The dungeon, it shift as you go and there is nothing you can do to escape the wrath of The Order!" },
            { "loc_dialogue_3_2", "..." },
            { "loc_dialogue_3_3", "We'll see about that. There is no dungeon I cannot crack." },
            { "loc_dialogue_3_4", "There is nothing you can do to stop me from escaping..." },
            { "loc_dialogue_4_0", "The dungeons of The Tower change their physical manifestation constantly due to the flow of magical energies..." },
            { "loc_dialogue_4_1", "Every time I cross a portal I could end up anywhere within The Tower... I'll have to be careful." },
        } },
        { Language.Spanish, new Dictionary<string, string> {
            // Loc Strings : Generic
            { "loc_language_name", "Español" },
            { "loc_default", "Por defecto" },
            { "loc_default_text", "Texto por Defecto" },
            { "loc_next", "Siguiente" },
            { "loc_previous", "Anterior" },
            
            // Loc Strings : Menu
            { "loc_play", "Jugar" },
            { "loc_play_campaign", "Jugar Campaña" },
            { "loc_play_procedural", "Jugar Procedural" },
            { "loc_account", "Cuenta" },
            { "loc_credits", "Créditos" },
            { "loc_return", "Volver" },
            { "loc_language", "Lenguaje" },
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

            // Loc Strings : Settings
            { "loc_settings", "Opciones" }, // This used to be "Configuración", my beloved </3 :( but the fucking thing was too large to fit on the UI buttons. Oh well, too bad!
            { "loc_settings_language", "Idioma" },
            { "loc_Settings_choose_language", "Seleccionar Idioma" },
            { "loc_settings_enable_fps", "Mostrar FPS" },
            { "loc_settings_enable_version", "Mostrar Versión" },
            { "loc_settings_enable_console", "Habilitar Consola" },
            { "loc_settings_quality", "Calidad" },
            { "loc_settings_quality_0", "Muy Bajo" },
            { "loc_settings_quality_1", "Bajo" },
            { "loc_settings_quality_2", "Medio" },
            { "loc_settings_quality_3", "Alto" },
            { "loc_settings_quality_4", "Muy Alto" },
            { "loc_settings_quality_5", "Ultra" },
            { "loc_settings_extra", "Extra" },
            { "loc_settings_sound", "Sonido" },
            { "loc_settings_volume_global", "Volumen Global" },
            { "loc_settings_volume_sfx", "Volumen de Efectos" },
            { "loc_settings_volume_music", "Volumen de Música" },
            { "loc_settings_volume_voice", "Volumen de Voz" },
            { "loc_settings_volume_ui", "Volumen de Interfaz" },

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
            { "loc_warning_title", "Alerta" },
            { "loc_warning_play", "Estás a punto de jugar una partida sin acceder a una cuenta. Tu progreso no será guardado." },

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
            { "loc_test_ghost_spawn", "Pisa la placa de presión para spawner un fantasma" },

            // Loc Strings : Tutorial Level
            { "loc_tutorial_message_0", "¡Bienvenido a la sala de entrenamiento! Aquí, los aspirantes a mago pueden practicar sus conjuros y aprender a conjurar como verdaderos hechiceros. Explora y aprende a usar tu magia." }, // This translation was kinda butchered due fact that the tutorial screen did not have enough space to display it... FUCK.
            { "loc_tutorial_message_1", "Explora el área. Hay muchas cosas por hacer. Cuando hayas terminado de entrenar, pausa y vuelve al menú. ¡Cuidado con los fantasmas malignos!" },
            
            // Loc Strings : Tutorial Menu
            { "loc_tutorial_menu", "Leer Tutorial" },
            { "loc_tutorial_map", "Jugar Tutorial" },
            
            // Loc Strings : Tutorial Pages
            { "loc_tut_pages_0", "Controles" },
            { "loc_tut_pages_1", "El ratón es utilizado para desplazarse y lanzar conjuros.\r\nEl puntero determina la dirección en la que mira el jugador." },
            { "loc_tut_pages_2", "RMB\r\nDesplazamiento" },
            { "loc_tut_pages_3", "LMB\r\nLanzar Conjuro" },
            { "loc_tut_pages_4", "Elementos" },
            { "loc_tut_pages_5", "Los elementos determinan los efectos del conjuro.\r\nUn conjuro puede combinar múltiples elementos." },
            { "loc_tut_pages_6", "Q : Agua" },
            { "loc_tut_pages_7", "F : Fuego" },
            { "loc_tut_pages_8", "R : Frío" },
            { "loc_tut_pages_9", "A : Electricidad" },
            { "loc_tut_pages_10", "W : Curación" },
            { "loc_tut_pages_11", "D : Tierra" },
            { "loc_tut_pages_12", "S : Muerte" },
            { "loc_tut_pages_13", "Formas" },
            { "loc_tut_pages_14", "La forma determina la manifestación física del conjuro" },
            { "loc_tut_pages_15", "1 : Proyectil" },
            { "loc_tut_pages_16", "2 : Rayo" },
            { "loc_tut_pages_17", "3 : Escudo" },
            { "loc_tut_pages_18", "Caja de Hechizos" },
            { "loc_tut_pages_19", "La caja de hechizos sirve para crear conjuros. Primero se selecciona la forma. Luego se combinan elementos." },
            { "loc_tut_pages_20", "Forma" },
            { "loc_tut_pages_21", "Elementos" },
            { "loc_tut_pages_22", "Conjuro" },
            { "loc_tut_pages_23", "Caja de Hechizos" },
            { "loc_tut_pages_24", "Puedes combinar hasta 5 elementos.\r\nCada elemento tiene un efecto diferente." },
            { "loc_tut_pages_25", "Caja vacía" },
            { "loc_tut_pages_26", "Caja llena" },
            { "loc_tut_pages_27", "Combinaciones" },
            { "loc_tut_pages_28", "Existen subelementos que pueden ser creados al combinar otros elementos." },
            { "loc_tut_pages_29", "Vapor" },
            { "loc_tut_pages_30", "Hielo" },
            { "loc_tut_pages_31", "Opuestos" },
            { "loc_tut_pages_32", "Los elementos opuestos se cancelan." },
            { "loc_tut_pages_33", "Ejemplos" },

            // Loc Strings : Points and Currency
            { "loc_experience", "Experiencia" },
            { "loc_points", "Puntos" },
            { "loc_money", "Dinero" },

            // Loc Strings : Funny
            { "loc_popmeup", "Vlad no es un vampiro" },
            
            // Loc Strings : Store
            { "loc_buy", "Comprar" },
            { "loc_inspect", "Revisar" }, // FUCK ME The OG did not fit within the button's size :(
            { "loc_buy_items", "Comprar Objetos" },
            { "loc_buy_currency", "Comprar Monedas" },
            { "loc_transaction", "Transacción" },
            { "loc_transaction_success", "¡Transacción Procesada Exitosamente!" },
            { "loc_transaction_success_msg", "La transacción ha sido procesoda exitosamente. ¡Disfrute de su compra!" },

            // Loc Strings : WIP
            { "loc_wip", "WIP"},
            { "loc_wip_title", "Trabajo en Progreso" },
            { "loc_wip_message", "Esta caracteristica está en proceso de creación y no está terminada todavía. Esté atento a más actualizaciones." },
            { "loc_coming_soon", "¡PRÓXIMAMENTE!" },

            // Loc Strings : Scoreboard
            { "loc_top_10", "Top 10" },

            // Loc Strings : Entity Names
            { "loc_name_wizard", "Mago" },
            { "loc_name_wizard_unknown", "Mago Desconocido" },

            // Loc Strings : Dialogues
            { "loc_dialogue_0_0", "¡Una pila de textos arcanos extremadamente interesantes!" },
            { "loc_dialogue_0_1", "Es una pena que no me importe absolutamente nada de esto." },
            { "loc_dialogue_1_0", "¿Se creen que pueden mantenerme encerrado en esta torre? Amateurs... ¡No conocen mi poder!" },
            { "loc_dialogue_2_0", "Así que estás intentando escapar..." },
            { "loc_dialogue_2_1", "¡No lograrás salir de aquí con vida!" },
            { "loc_dialogue_3_0", "No... no puede ser..." },
            { "loc_dialogue_3_1", "No serás capaz de escapar. La Torre es poderosa. La mazmorra, se modifica a medida que avanzas ¡y no hay nada que puedas hacer para escapar de la ira de La Orden!" },
            { "loc_dialogue_3_2", "..." },
            { "loc_dialogue_3_3", "Ya veremos. No hay mazmorra que se me resista." },
            { "loc_dialogue_3_4", "No hay nada que puedas hacer para evitar que escape..." },
            { "loc_dialogue_4_0", "Las mazmorras de La Torre cambian su manifestación física de forma constante debido al flujo de la magia..." },
            { "loc_dialogue_4_1", "Cada vez que cruce un portal podría acabar en cualquier parte de La Torre... tendré que tener cuidado." },
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

    public static Language GetLanguageFromString(string language)
    {
        if (language != null)
        {
            foreach (var lang in languageStrings)
            {
                if (lang.Value.Contains(language))
                {
                    return lang.Key;
                }
            }
        }
        return Language.English; // Defaults to returning English in case the language is not found.
    }

    #endregion

    #region PrivateMethods
    #endregion
}
