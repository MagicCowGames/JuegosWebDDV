using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonUIController : MonoBehaviour
{
    #region Variables

    [Header("Components - Button")]
    [SerializeField] private Button button;
    [SerializeField] private Image buttonShadow;

    [Header("Components - Text")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text textShadow;

    [Header("Settings - Button")]
    [SerializeField] private UnityEvent OnClick;
    [SerializeField] private bool buttonHasShadow;

    [Header("Settings - Text")]
    [SerializeField] private string localizationString;
    [SerializeField] private bool textHasShadow;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
