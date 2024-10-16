using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : UIController, IComponentValidator
{
    #region Variables

    [Header("Player UI Controller Components")]
    [SerializeField] private Image[] elementQueueImages;
    [SerializeField] private Image healthBar;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        // Can't update anything if the components are not valid. Just preventing the universe from exploding, don't thank me! All in a day's work...
        if (!AllComponentsAreValid())
            return;

        UpdateHealthBar(Time.deltaTime);
    }

    #endregion

    #region PublicMethods

    // NOTE : These public functions are here because Unity's inspector doesn't support enums for OnClick events for buttons, so we use this workaround instead.

    public void Button_AddElement_Water() { Button_AddElement(Element.Water); }
    public void Button_AddElement_Heal() { Button_AddElement(Element.Heal); }
    public void Button_AddElement_Shield() { Button_AddElement(Element.Shield); }
    public void Button_AddElement_Cold() { Button_AddElement(Element.Cold); }
    public void Button_AddElement_Electricity() { Button_AddElement(Element.Electricity); }
    public void Button_AddElement_Death() { Button_AddElement(Element.Death); }
    public void Button_AddElement_Earth() { Button_AddElement(Element.Earth); }
    public void Button_AddElement_Fire() { Button_AddElement(Element.Fire); }

    // TODO : Maybe rename to ElementDisplayUpdate() or something so that it's more consistent with the other function names?
    public void UpdateElementDisplay(ElementQueue queue)
    {
        // This should never happen, but if it does, we're fucked, so just return early to prevent any issues.
        if (ElementManager.Instance == null)
            return;

        // Reset the display to use empty slots
        ElementDisplayReset();

        // Early return if the input queue is null.
        // Since we reset the display first, at least we get a nice visually clean and empty queue.
        if (queue == null)
            return;

        // Set the slots to display the elements within the queue
        ElementDisplaySet(queue.Elements, queue.Count);
    }

    #endregion

    #region PrivateMethods

    private void ElementDisplayReset()
    {
        // Reset the entire queue to the default sprite / Element.None sprite
        for (int i = 0; i < this.elementQueueImages.Length; ++i)
            this.elementQueueImages[i].sprite = ElementManager.Instance.GetSprite(Element.None);
    }

    private void ElementDisplaySet(Element[] elements, int count)
    {
        // Replace the sprites for those of the elements within the queue
        for (int i = 0; i < count; ++i)
            this.elementQueueImages[i].sprite = ElementManager.Instance.GetSprite(elements[i]);
    }


    private void Button_AddElement(Element element)
    {
        InputManager.Instance?.AddElement(element);
    }

    private void UpdateHealthBar(float delta)
    {
        float healthPercentage = PlayerDataManager.Instance.GetPlayerHealth().GetPercentage();
        this.healthBar.fillAmount = Mathf.Clamp01(Mathf.Lerp(this.healthBar.fillAmount, healthPercentage, delta * 10));
    }

    #endregion

    #region IComponentValidator

    public bool AllComponentsAreValid()
    {
        return
            this.healthBar != null &&
            this.elementQueueImages != null &&
            PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.GetPlayerHealth() != null
            ;
    }

    #endregion
}
