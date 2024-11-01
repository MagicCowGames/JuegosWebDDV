using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// NOTE : For now, only the player can trigger these. We will make this configurable in the future by adding entity controller scripts which will be gotten on trigger
// and used to determine what type of entity has collided with the trigger.
public class TriggerController : MonoBehaviour
{
    #region Variables

    // [SerializeField] private string triggerName;
    [SerializeField] private UnityEvent OnTrigger; // This one runs when the trigger is activated through some script or another trigger. Will mostly go unused. Could also be renamed to OnTriggerActivated or something like that, but that could be confusing, idk. Maybe should add a trigger always callback that will be triggered from all of the other ones?
    [SerializeField] private UnityEvent OnTriggerAreaEnter; // This one runs when an entity enters the trigger's area
    [SerializeField] private UnityEvent OnTriggerAreaExit; // This one runs when an entity exits the trigger's area
    [SerializeField] private UnityEvent OnTriggerAreaStay; // This one runs when an entity stays on the trigger's area

    // TODO : Add bools or bitmask to determine what type of entities can activate this trigger.

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

    public void Trigger()
    {
        this.OnTrigger?.Invoke();
    }

    #endregion

    #region PrivateMethods

    // Determines if the input gameobject can activate this trigger
    // The trigger can only be activated by gameobjects that are of a certain set of "entity" types specific to the game, such as players, NPCs and whatnot.
    private bool CanTrigger(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        // TODO : Add support for other types of entities appart from the player by adding checks for other controller components
        bool ans =
            player != null // || whatever != null || etc...
            ;
        return ans;
    }

    private bool CanTrigger(Collider collider)
    {
        return CanTrigger(collider.gameObject);
    }

    #endregion

    #region Collisions

    void OnTriggerEnter(Collider other)
    {
        if(CanTrigger(other))
            OnTriggerAreaEnter?.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if(CanTrigger(other))
            OnTriggerAreaExit?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if(CanTrigger(other))
            OnTriggerAreaStay?.Invoke();
    }

    #endregion
}
