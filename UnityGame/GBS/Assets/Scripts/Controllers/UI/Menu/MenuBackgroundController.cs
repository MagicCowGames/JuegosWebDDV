using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// NOTE : This script could be used for all types of menus and UI elements with a background image, so maybe it would make sense to just rename
// it to BackgroundUIController or something like that.

[ExecuteInEditMode]
public class MenuBackgroundController : UIController
{
    #region Variables

    [Header("Background Menu Settings")]
    [SerializeField] Image backgroundImageNormal;
    [SerializeField] Image backgroundImageBlurry;
    [SerializeField] bool isBlurry;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        UpdateBackgroundImage();
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        UpdateBackgroundImage();
    }

    #endregion

    #region PublicMethods

    public void SetBlurry(bool blurry)
    {
        UpdateBackgroundImage();
        this.isBlurry = blurry;
    }

    public bool GetBlurry()
    {
        return this.isBlurry;
    }

    #endregion

    #region PrivateMethods

    private void UpdateBackgroundImage()
    {
        if (this.backgroundImageNormal == null || this.backgroundImageBlurry == null)
            return;
        this.backgroundImageNormal.gameObject.SetActive(!this.isBlurry);
        this.backgroundImageBlurry.gameObject.SetActive(this.isBlurry);
    }
    
    #endregion
}
