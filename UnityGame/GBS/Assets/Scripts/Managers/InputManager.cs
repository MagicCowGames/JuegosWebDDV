using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InputManager : Singleton<InputManager>
{
    #region Variables
    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        UpdateInput();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region Private Methods - UpdateInput entry point

    // Call all specific update input functions.
    // Determines the order in which the input is updated.
    // Each specific function will handle a specific input action for a specific input device.
    private void UpdateInput()
    {
        UpdateMouse();
        UpdateTouch();
    }

    #endregion

    #region Private Methods - Specific Update input functions

    private void UpdateMouse()
    { }

    private void UpdateTouch()
    { }

    #endregion
}
