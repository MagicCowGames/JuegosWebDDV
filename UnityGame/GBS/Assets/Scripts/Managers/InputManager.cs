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


    public Action OnSwitchConsole;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        if (GameUtility.GetPaused()) // TODO : Fix this fucking hack, it breaks the inputs during pause screen, wtf was I thinking?
            return;
        UpdateInput();
    }

    #endregion

    #region Public Methods

    public void AddElement(Element element)
    {
        this.OnAddElement?.Invoke(element);
    }

    public void SetForwardAxis(float value)
    {
        this.OnSetForwardAxis?.Invoke(value);
    }

    public void SetScreenPoint(Vector3 point)
    {
        this.OnSetScreenPoint?.Invoke(point);
    }

    public void SwitchConsole()
    {
        this.OnSwitchConsole?.Invoke();
    }

    public void CastSpell()
    {
        this.OnCastSpell?.Invoke();
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

        if (Input.GetMouseButton(1))
        {
            CastSpell();
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
        UIK_Elements();
        UIK_Other();
    }

    private void UIK_Elements()
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

    private void UIK_Other()
    {
        // Pause
        if (Input.GetKeyDown(KeyCode.Escape))
            GameUtility.SwitchPaused();

        // Console
        if (Input.GetKeyDown(KeyCode.BackQuote))
            SwitchConsole();
    }

    #endregion
}
