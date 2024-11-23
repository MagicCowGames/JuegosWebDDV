using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
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

    public LanguageSystem.Language language;
    public Color3f color;
    public float volume;

    public Settings()
    {
        this.language = LanguageSystem.Language.English;
        this.color = new Color3f(1.0f, 0.0f, 0.0f);
        this.volume = 1.0f;
    }
}
