public enum GameVersionType
{
    Alpha = 0,
    Beta = 1,
    Release = 2
}

// Struct used to identify a version of the game.
// NOTE : This struct can also be used to perform save data translation functions in case that the save format structure changes between
// versions by adding incremental save methods.
[System.Serializable]
public struct GameVersion
{
    #region Variables

    public int major;
    public int minor;
    public int patch;
    public GameVersionType type;
    public string name; // Specific name given to the version, reserved for special versions.

    #endregion

    #region PublicMethods

    public static bool IsEqual(GameVersion a, GameVersion b)
    {
        return
            a.type == b.type &&
            a.major == b.major &&
            a.minor == b.minor &&
            a.patch == b.patch
            ;
    }

    public static bool IsLower(GameVersion a, GameVersion b)
    {
        if ((int)a.type > (int)b.type)
            return false;

        if (a.major > b.major)
            return false;
        if (a.minor > b.minor)
            return false;
        if (a.patch > b.patch)
            return false;
        
        return true;
    }

    public static bool IsHigher(GameVersion a, GameVersion b)
    {
        return IsLower(b, a);
    }

    public static int Compare(GameVersion a, GameVersion b)
    {
        if (IsEqual(a, b))
            return 0;
        if (IsLower(a, b))
            return -1;
        if (IsHigher(a, b))
            return 1;
        return 0; // Assume that any edge cases should also point to equal versions.
    }

    #endregion
}
