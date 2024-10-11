using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IUI
{
    #region Variables

    [Header("UIController Components")]
    [SerializeField] private Canvas canvasReference;

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

    public bool UI_GetVisible()
    {
        if (!this.UI_IsValid())
            return false;
        return this.canvasReference.gameObject.activeSelf;
    }

    public void UI_SetVisible(bool visible)
    {
        if (!this.UI_IsValid())
            return;
        this.canvasReference.gameObject.SetActive(visible);
    }

    public bool UI_IsValid()
    {
        return this.canvasReference != null;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
