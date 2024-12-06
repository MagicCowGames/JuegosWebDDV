using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This is a temporary solution, in the future it would be ideal to merge this with the Entity animation system I was planning, but deadlines are are
// fucking my plans in the asshole as much as humanly possible...

// TODO : Get rid of this class and try to figure out how to merge this in a cleaner way with the Entity animation system that I was making... same with the player
// animation controller... or maybe just keep the player and NPC impls as separate things, who knows what will be better in the long run!

public class NPCAnimationController : MonoBehaviour
{
    #region Variables

    [SerializeField] private NPCController npcController;
    [SerializeField] private Animator animator;

    private float animationShiftSpeed = 12.0f;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        // DebugManager.Instance?.Log($"velocityNPC = {this.npcController.Velocity.magnitude}");
        float delta = Time.deltaTime;
        UpdateAnimation(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateAnimation(float delta)
    {
        // Temporarily disabled because NPCs are destroyed instantly when they die.
        /*
        if (this.npcController.healthController.Health <= 0.0f)
        {
            this.animator.Play("Base Layer.Death");
            return;
        }
        */

        if (this.npcController.spellCaster.GetIsCasting())
        {
            ChangeAnimationValueFloat("Casting", delta * this.animationShiftSpeed, -2, 2);
        }
        else
        {
            ChangeAnimationValueFloat("Casting", delta * this.animationShiftSpeed * -1.0f, -2, 2);
        }

        if (this.npcController.Velocity.magnitude > 0.0f)
        {
            ChangeAnimationValueFloat("ForwardMovement", delta * this.animationShiftSpeed, 0, 1);
        }
        else
        {
            ChangeAnimationValueFloat("ForwardMovement", delta * this.animationShiftSpeed * -1.0f, 0, 1);
        }
    }

    private void ChangeAnimationValueFloat(string name, float amount, float min, float max)
    {
        this.animator.SetFloat(name, Mathf.Clamp(this.animator.GetFloat(name) + amount, min, max));
    }

    #endregion
}
