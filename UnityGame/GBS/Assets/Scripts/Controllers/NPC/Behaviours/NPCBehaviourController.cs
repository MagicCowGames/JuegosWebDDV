using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the logic for specific behaviours for NPCs.
// This will, for example, implement the state machines for specific AIs and whatnot.
// NOTE : This is the base class from which you should inherit if you want to implement behaviours for NPCs.
// NOTE : Each NPC should have one single behaviour, mixing multiple behaviours is bound to fuck shit up, so that's why we limit it.
// NOTE : Maybe add support for multiple NPCBehaviourControllers? That way, we could mix some GenericWanderbehaviour, GenericChaseBehaviour, GenericWarriorBehaviour, GenericWizardBehaviour, etc...
[DisallowMultipleComponent]
[RequireComponent(typeof(NPCController))]
public class NPCBehaviourController : MonoBehaviour
{
    #region Variables

    protected NPCController npcController;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.npcController = GetComponent<NPCController>();
        // if(this.npcController != null) // Since we're using require component, this check ain't needed anymore.
        InitBehaviour();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        // if(this.npcController != null)
        UpdateBehaviour(delta);
    }

    #endregion

    #region PublicMethods

    // NOTE : This method should only be called on very specific situations. Obviously, calling it constantly won't break anything if the behaviour impls
    // are written correctly, but it should be noted that this is mostly used for stuff like CmdLeaveMeAlone() and some cinematic events.
    public void ResetBehaviour()
    {
        // Reset the behaviour to the initial state
        InitBehaviour();
    }

    #endregion

    #region ProtectedMethods

    protected virtual void InitBehaviour()
    {
        // Set the initial state of the behaviour
    }

    protected virtual void UpdateBehaviour(float delta)
    {
        // Set the current state of the behaviour
    }

    #endregion

    #region PrivateMethods
    #endregion
}

// NOTE : The nomenclature used in most NPC Behaviour controllers will be the following:
// FSM : Finite State Machine
// US  : Utility System
// BT  : Behaviour Tree
