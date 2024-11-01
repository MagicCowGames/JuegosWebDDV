public enum GameVersionType
{
    Alpha = 0,
    Beta,
    Release
}

// Struct used to identify a version of the game.
// NOTE : This struct can also be used to perform save data translation functions in case that the save format structure changes between
// versions by adding incremental save methods.
// TODO : Add a set of comparator methods to determine which version is lower or higher.
[System.Serializable]
public struct GameVersion
{
    public int major;
    public int minor;
    public int patch;
    public GameVersionType type;
    public string name; // Specific name given to the version, reserved for special versions.
}
