using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UI_ScoreEntryController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text displayTextShadow;
    [SerializeField] private TMP_Text displayText;

    [Header("Config")]
    [SerializeField] private bool isDark;

    private static Color darkColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    private static Color lightColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        if (this.isDark)
        {
            SetBackgroundDark();
        }
        else
        {
            SetBackgroundLight();
        }
    }

    #endregion

    #region PublicMethods

    public void SetBackgroundDark()
    {
        this.isDark = true;
        this.backgroundImage.color = darkColor;
    }

    public void SetBackgroundLight()
    {
        this.isDark = false;
        this.backgroundImage.color = lightColor;
    }

    public void SetData(int position, string name, long score)
    {
        string str = $"[{position}] : {name} : {score} pts";
        this.displayTextShadow.text = str;
        this.displayText.text = str;
    }

    public void SetData(int position, string name, string score)
    {
        string str = $"[{position}] : {name} : {score} pts";
        this.displayTextShadow.text = str;
        this.displayText.text = str;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
