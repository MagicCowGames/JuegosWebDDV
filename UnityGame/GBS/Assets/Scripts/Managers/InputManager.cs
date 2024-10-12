using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : Singleton<InputManager>
{
    #region Variables

    public Action<float> OnSetForwardAxis;
    public Action<Vector3> OnSetScreenPoint;
    public Action<Element> OnAddElement;

    public Action/*<CastType>*/ OnCastSpell;

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

    #region Private Methods

    private void AddElement(Element element)
    {
        this.OnAddElement?.Invoke(element);
    }

    private void SetForwardAxis(float value)
    {
        this.OnSetForwardAxis?.Invoke(value);
    }

    private void SetScreenPoint(Vector3 point)
    {
        this.OnSetScreenPoint?.Invoke(point);
    }

    #endregion

    #region Private Methods - Update Input

    // NOTE : UI Inputs such as buttons are handled on the UI itself, by calling Input manager functions,
    // so there will only be update functions for input devices that require it.
    private void UpdateInput()
    {
        UpdateInputReset();
        UpdateInputMouse();
        UpdateInputKeyboard();
        UpdateInputKeyboard2();
        UpdateInputTouch();
    }

    private void UpdateInputReset()
    {
        SetForwardAxis(0);
    }

    private void UpdateInputMouse()
    {
        if (!Input.mousePresent)
            return;

        SetScreenPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            SetForwardAxis(1);
        }
    }

    private void UpdateInputTouch()
    {
        if (Input.mousePresent)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var pos = touch.position;

            SetForwardAxis(1);
            SetScreenPoint(pos);
        }
    }

    private void UpdateInputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            AddElement(Element.Water);

        if (Input.GetKeyDown(KeyCode.W))
            AddElement(Element.Heal);

        if (Input.GetKeyDown(KeyCode.E))
            AddElement(Element.Shield);

        if (Input.GetKeyDown(KeyCode.R))
            AddElement(Element.Cold);

        if (Input.GetKeyDown(KeyCode.A))
            AddElement(Element.Electricity);

        if (Input.GetKeyDown(KeyCode.S))
            AddElement(Element.Death);

        if (Input.GetKeyDown(KeyCode.D))
            AddElement(Element.Earth);

        if (Input.GetKeyDown(KeyCode.F))
            AddElement(Element.Fire);
    }

    private void UpdateInputKeyboard2()
    {
        // Other inputs:
        if (Input.GetKeyDown(KeyCode.Escape))
            Debug.Log("show menu lol");

        if (Input.GetKeyDown(KeyCode.Return))
            Debug.Log("lol");
    }

    #endregion
}
