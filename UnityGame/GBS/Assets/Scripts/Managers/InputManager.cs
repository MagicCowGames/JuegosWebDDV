using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    #region Variables

    public Action<float> OnSetForwardAxis;
    public Action<Vector3> OnSetScreenPoint;
    public Action<Element> OnAddElement;
    public Action<Form> OnSetForm;

    public Action OnRightClickDown;
    public Action OnRightClickUp;


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

    public void SetForm(Form form)
    {
        this.OnSetForm?.Invoke(form);
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

    public void SwitchPaused()
    {
        GameUtility.SwitchPaused(); // This should send an event instead but whatever, temporary fix.
    }

    public void RightClickDown()
    {
        this.OnRightClickDown?.Invoke();
    }

    public void RightClickUp()
    {
        this.OnRightClickUp?.Invoke();
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

        // Don't send any inputs if we are pressing a button, because the UI button takes precedence over all other mouse actions. Same happens with touch inputs.
        if (IsPointerOverButton())
            return;

        SetScreenPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            SetForwardAxis(1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RightClickDown();
        }

        if (Input.GetMouseButtonUp(1))
        {
            RightClickUp();
        }
    }

    private void UpdateInputTouch()
    {
        if (Input.mousePresent)
            return;

        if (IsPointerOverButton())
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
        UIK_Forms();
        UIK_Other();
    }

    private void UIK_Elements()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            AddElement(Element.Water);

        if (Input.GetKeyDown(KeyCode.W))
            AddElement(Element.Heal);

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

    private void UIK_Forms()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetForm(Form.Projectile);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetForm(Form.Beam);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetForm(Form.Shield);

        // NOTE : Legacy input
        // TODO : Remove in the future?
        // NOTE : Removed because of beta testing in class lolololo
        /*
        if (Input.GetKeyDown(KeyCode.E))
            SetForm(Form.Shield);
        */
    }

    private void UIK_Other()
    {
        // Pause
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchPaused();

        // Console
        if (Input.GetKeyDown(KeyCode.BackQuote))
            SwitchConsole();
    }

    #endregion

    #region Private Methods - Other

    private bool IsPointerOverButton()
    {
        bool ans =
            EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject() &&
            EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>() != null
            ;
        // NOTE : GetComponent<CanvasRenderer>() would be used to determine if we're clicking ANY UI element at all. But in this case we only care about buttons.
        return ans;
    }

    #endregion
}
