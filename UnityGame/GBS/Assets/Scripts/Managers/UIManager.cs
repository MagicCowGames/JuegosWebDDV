using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IManager
{
    #region Structs

    [System.Serializable]
    public struct UIControllerEntry
    {
        public string name;
        public UIController controller;

        public UIControllerEntry(string name, UIController controller)
        {
            this.name = name;
            this.controller = controller;
        }
    }

    #endregion

    #region Variables

    [Header("Specific UI Controllers")]
    [SerializeField] private InfoUIController infoUIController;
    [SerializeField] private ConsoleUIController consoleUIController;
    [SerializeField] private PauseUIController pauseUIController;
    [SerializeField] private PlayerUIController playerUIController;
    [SerializeField] private DeathUIController deathUIController;
    [SerializeField] private PopUpUIController popUpUIController;
    [SerializeField] private FinishUIController finishUIController;
    [SerializeField] private FadeUIController fadeUIController;

    [Header("Generic UI Controllers")]
    [SerializeField] private List<UIControllerEntry> uiControllers;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.popUpUIController?.Close();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public InfoUIController GetInfoUI()
    {
        return this.infoUIController;
    }

    public ConsoleUIController GetConsoleUI()
    {
        return this.consoleUIController;
    }

    public PauseUIController GetPauseUIController()
    {
        return this.pauseUIController;
    }

    public PlayerUIController GetPlayerUIController()
    {
        return this.playerUIController;
    }

    public DeathUIController GetDeathUIController()
    {
        return this.deathUIController;
    }

    public PopUpUIController GetPopUpUIController()
    {
        return this.popUpUIController;
    }

    public FinishUIController GetFinishUIController()
    {
        return this.finishUIController;
    }

    public FadeUIController GetFadeUIController()
    {
        return this.fadeUIController;
    }

    #endregion

    #region PublicMethods GenericUIControllers

    public UIController GetUIController(string name)
    {
        foreach (var x in this.uiControllers)
            if (x.name == name)
                return x.controller;
        return null;
    }

    public void AddUIController(string name, UIController controller)
    {
        this.uiControllers.Add(new UIControllerEntry(name, controller));
    }

    #endregion

    #region PrivateMethods
    #endregion

    #region IManager

    public void ResetManager()
    {
    }

    #endregion
}
