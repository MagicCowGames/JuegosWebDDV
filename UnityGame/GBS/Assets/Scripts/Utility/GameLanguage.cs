using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : If it were up to me, I would personally store the language data within some JSON files on the game's files
// and just load the dictionaries for the Language Packs from there, but sadly I have no fucking clue how to make
// Unity include and then read config files in the build for WebGL when it would be as trivial as including a simple
// JS script file with a JSON object for the language data like back in Burning Village...
// but yeah, it is what it is... it kind of kills modding support, but it's not like it even matters for this game tbh
public static class LanguageSystem
{
    #region Variables
    
    public static string DefaultString { get; private set; }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
