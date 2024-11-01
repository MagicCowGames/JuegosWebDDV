public enum GameVersionType
{
    Alpha = 0,
    Beta,
    Release
}

[System.Serializable]
public struct GameVersion
{
    public int major;
    public int minor;
    public int patch;
    public GameVersionType type;
    public string name; // Specific name given to the version, reserved for special versions.
}
