using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviourController : NPCBehaviourController
{
    #region Enums

    private enum BossAIState
    {
        None = 0,
        Idle,
        Combat
    }

    #endregion

    #region Variables

    private BossAIState state;
    private IUtilityAction[] actions;

    #endregion

    #region NPCBehaviour

    protected override void InitBehaviour()
    {
        this.npcController.CanDie = false;
        this.state = BossAIState.None;
        this.actions = new IUtilityAction[] {
            new ChaseAction(this.npcController),
            new AttackAction(this.npcController, new Element[]{Element.Death, Element.Fire, Element.Electricity}, Form.Beam, 1.5f, 5.2f, 0.0f, 10.0f),
            new AttackAction(this.npcController, new Element[]{Element.Fire, Element.Death, Element.Earth}, Form.Projectile, 1.0f, 3.0f, 10.1f, 20.0f),
            new AttackAction(this.npcController, new Element[]{Element.Earth, Element.Ice, Element.Ice, Element.Ice}, Form.Projectile, 1.0f, 3.0f, 20.1f, 50.0f),
            new AttackAction(this.npcController, new Element[]{Element.Death, Element.Death, Element.Fire, Element.Electricity}, Form.Beam, 5.0f, 5.2f, 50.1f, 100.0f),
        };
    }

    protected override void UpdateBehaviour(float delta)
    {
        UpdateFSM(delta);
    }

    #endregion

    #region PublicMethods

    public void StartBossBattle()
    {
        this.npcController.healthController.Heal(); // Just in case the player has done some shenanigans off screen, we restore the boss NPC's health when the battle starts...
        this.npcController.CanDie = true;
        this.state = BossAIState.Combat;
    }

    #endregion

    #region PrivateMethods - FSM

    // The Boss Behaviour is quite simple tbh... barely anything more than a fucking bool at this point tbh.
    private void UpdateFSM(float delta)
    {
        switch (this.state)
        {
            default:
            case BossAIState.None:
                this.state = BossAIState.Idle;
                break;
            case BossAIState.Idle:
                // Do Nothing...
                break;
            case BossAIState.Combat:
                // Chase the player and fight!
                this.npcController.ForwardAxis = 1.0f;
                UpdateUS(delta);
                break;
        }
    }

    #endregion

    #region PrivateMethods - US

    private void UpdateUS(float delta)
    {
        IUtilityAction chosenAction = null;
        float maxValue = 0.0f;
        foreach (var action in this.actions)
        {
            var val = action.Calculate(delta);
            // DebugManager.Instance?.Log($"{action.Name} : {val}");
            if (val > maxValue)
            {
                maxValue = val;
                chosenAction = action;
            }
        }

        foreach (var action in this.actions)
            action.Update(delta);

        chosenAction?.Execute(delta);
    }

    #endregion
}
