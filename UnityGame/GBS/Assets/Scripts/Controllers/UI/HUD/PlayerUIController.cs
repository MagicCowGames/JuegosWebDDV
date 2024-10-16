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

        // Updating this stuff every single frame is not ideal, and some of this used to be updated through events, but this is much less of a hassle,
        // It doesn't even have a performance impact and at least we make sure that there are no stupid edge cases caused by late night brain-fart coding.
        // Yes, sad, I know, but it is what it is.
        // Also, fuck coroutines.
        UpdateHealthBar(Time.deltaTime);
        UpdateElementDisplay();
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

    #endregion

    #region PrivateMethods

    private void UpdateElementDisplay()
    {
        var queue = PlayerDataManager.Instance.GetPlayerElementQueue();
        ElementDisplayReset();
        ElementDisplaySet(queue.Elements, queue.Count);
    }

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

    // NOTE : It's better to check for null for all required components once every frame rather than multiple times, which is why
    // the null checking code from each update function has been removed.
    // For example, keeping each update method's own null checking would lead to needing to check for PlayerDataManager's instance to be null
    // more than once every frame, and that's just dumb.
    // In short : we just check for all of the needs of all update methods in this function right here and call it a day. If any of them fails, we bail.
    // The only downside is that now we can't partially update UI by allowing some parts to update while bailing on others. Now we either update all or fuck off.
    public bool AllComponentsAreValid()
    {
        return
            this.healthBar != null &&
            this.elementQueueImages != null &&
            ElementManager.Instance != null && // This should never happen, but if it does, we're fucked, so we should just return early to prevent any issues.
            PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.GetPlayerHealth() != null &&
            PlayerDataManager.Instance.GetPlayerElementQueue() != null
            ;
    }

    #endregion
}
