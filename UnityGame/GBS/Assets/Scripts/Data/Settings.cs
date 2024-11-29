using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserSettings
{
    public GraphicsSettings graphicsSettings;
    public LanguageSettings languageSettings;
    public SoundSettings soundSettings;
    public ExtraSettings extraSettings;
    public CosmeticSettings cosmeticSettings;

    public UserSettings()
    {
        this.graphicsSettings = new GraphicsSettings();
        this.languageSettings = new LanguageSettings();
        this.soundSettings = new SoundSettings();
        this.extraSettings = new ExtraSettings();
        this.cosmeticSettings = new CosmeticSettings();
    }
}

[System.Serializable]
public class GraphicsSettings
{
    public int quality;

    public GraphicsSettings()
    {
        this.quality = 3; // High
    }
}

[System.Serializable]
public class LanguageSettings
{
    public LanguageSystem.Language language;

    public LanguageSettings()
    {
        this.language = LanguageSystem.Language.English;
    }
}

[System.Serializable]
public class SoundSettings
{
    public float volumeMusic;
    public float volumeVoice;
    public float volumeEffects;

    public SoundSettings()
    {
        this.volumeMusic = 1.0f;
        this.volumeVoice = 1.0f;
        this.volumeEffects = 1.0f;
    }
}

[System.Serializable]
public class ExtraSettings
{
    public bool consoleEnabled;
    public bool displayVersion;
    public bool displayFps;

    public ExtraSettings()
    {
        this.consoleEnabled = false;
        this.displayVersion = true;
        this.displayFps = false;
    }
}

[System.Serializable]
public class CosmeticSettings
{
    [System.Serializable]
    public class Color3f
    {
        public float r;
        public float g;
        public float b;

        public Color3f(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }

    public Color3f color;
    // Other settings related to the selected outfit ID, etc...

    public CosmeticSettings()
    {
        this.color = new Color3f(0.0f, 0.0f, 0.0f);
    }
}
