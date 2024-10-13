using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : UIController
{
    #region Variables
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

    public void Button_AddElement_Water() { Button_AddElement(Element.Water); }
    public void Button_AddElement_Heal() { Button_AddElement(Element.Heal); }
    public void Button_AddElement_Shield() { Button_AddElement(Element.Shield); }
    public void Button_AddElement_Cold() { Button_AddElement(Element.Cold); }
    public void Button_AddElement_Electricity() { Button_AddElement(Element.Electricity); }
    public void Button_AddElement_Death() { Button_AddElement(Element.Death); }
    public void Button_AddElement_Earth() { Button_AddElement(Element.Earth); }
    public void Button_AddElement_Fire() { Button_AddElement(Element.Fire); }

    #endregion

    #region PrivateMethods

    private void Button_AddElement(Element element)
    {
        InputManager.Instance?.AddElement(element);
    }

    #endregion
}
