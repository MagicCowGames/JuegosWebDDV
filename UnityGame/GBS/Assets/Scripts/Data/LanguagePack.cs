using System.Collections.Generic;

public struct LanguagePack
{
    // public string Name { get; set; } // language name
    // public int Version { get; set; } // language version
    public Dictionary<string, string> Entries { get; set; } // individual entries within this language
}
