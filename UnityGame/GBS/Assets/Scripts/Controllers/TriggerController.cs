using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// NOTE : The old implementation used to have a separate event handler for each type of event (on area enter,
// on area exit and on area stay).
// This is no longer the case. Triggers now only have one single type of event, and boolean checkboxes to determine
// whether the events are triggered on enter, on exit or on stay.
// If you need more than one type of trigger, you can either have 2 separate gameobjects or have them attached to the same
// gameobject with more than one trigger component.

// NOTE : For now, only the player can trigger these. We will make this configurable in the future by adding entity controller scripts which will be gotten on trigger
// and used to determine what type of entity has collided with the trigger.
public class TriggerController : MonoBehaviour
{
    #region Variables

    [Header("Trigger Properties")]
    [SerializeField] private string triggerName = "DefaultTrigger"; // TODO : Implement a system with a static list on TriggerController where all spawned triggers are added. That way, we can access them / look them up by name. This would mostly be useless unless we allow custom level creation or something like that.
    [SerializeField] private int count = 1; // Number of times the trigger can be triggered.
    [SerializeField] private bool infinite = true; // Determines if the trigger can be triggered infinite times.

    [Header("Trigger Events")]
    [SerializeField] private UnityEvent onTrigger; // This one runs when the trigger is activated through some script or another trigger. Will mostly go unused. Could also be renamed to OnTriggerActivated or something like that, but that could be confusing, idk. Maybe should add a trigger always callback that will be triggered from all of the other ones?
    [SerializeField] private bool triggerOnAreaEnter;
    [SerializeField] private bool triggerOnAreaExit;
    [SerializeField] private bool triggerOnAreaStay;

    [Header("Trigger Entities")] // The type of entities that can activate this trigger
    [SerializeField] private bool triggeredByPlayer = true;

    public UnityEvent OnTrigger { get { return this.onTrigger; } set { this.onTrigger = value; } }

    // [SerializeField] private UnityEvent OnTriggerAreaEnter; // This one runs when an entity enters the trigger's area
    // [SerializeField] private UnityEvent OnTriggerAreaExit; // This one runs when an entity exits the trigger's area
    // [SerializeField] private UnityEvent OnTriggerAreaStay; // This one runs when an entity stays on the trigger's area

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
        if (this.infinite || this.count > 0)
        {
            this.onTrigger?.Invoke();
            this.count -= 1;
        }
    }

    #endregion

    #region PrivateMethods

    // Determines if the input gameobject can activate this trigger
    // The trigger can only be activated by gameobjects that are of a certain set of "entity" types specific to the game, such as players, NPCs and whatnot.
    private bool CanTrigger(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        bool playerOk = this.triggeredByPlayer && player != null;

        // TODO : Add support for other types of entities appart from the player by adding checks for other controller components
        // eg:
        // var whatever = ...;
        // bool whateverOk = this.triggeredByWhatever && whatever != null;

        bool ans = playerOk; // || whateverOk || etc...

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
        if (this.triggerOnAreaEnter && CanTrigger(other))
            Trigger();
    }

    void OnTriggerExit(Collider other)
    {
        if(this.triggerOnAreaExit && CanTrigger(other))
            Trigger();
    }

    void OnTriggerStay(Collider other)
    {
        if(this.triggerOnAreaStay && CanTrigger(other))
            Trigger();
    }

    #endregion
}
