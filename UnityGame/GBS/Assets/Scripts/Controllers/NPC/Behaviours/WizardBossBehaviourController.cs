using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBossBehaviourController : NPCBehaviourController
{
    #region Variables

    // NOTE : These state enums could be made to be specific for each behaviour rather than generic.

    [Header("AI States")]
    [SerializeField] private AIState_Main stateMain = AIState_Main.None;
    [SerializeField] private AIState_Wandering stateWandering = AIState_Wandering.None;
    [SerializeField] private AIState_Combat stateCombat = AIState_Combat.None;

    private float idleTime = 0.0f;
    private float wanderTime = 0.0f;

    IUtilityAction[] actions;

    #endregion

    #region NPCBehaviour

    protected override void InitBehaviour()
    {
        this.stateMain = AIState_Main.None;
        this.stateWandering = AIState_Wandering.None;
        this.stateCombat = AIState_Combat.None;

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
        // For now, just walk toward the selected target GameObject.
        if (this.npcController.Target != null)
        {
            this.stateMain = AIState_Main.Combat; // This should be set through an event only ONCE, but whatever... for now we do it this way lol...
        }

        UpdateFSM_Main(delta);
    }

    #endregion

    #region PrivateMethods - FSM

    private void UpdateFSM_Main(float delta)
    {
        switch (this.stateMain)
        {
            case AIState_Main.None:
            default:
                this.stateMain = AIState_Main.Idle;
                break;
            case AIState_Main.Idle:
                this.npcController.ForwardAxis = 0.0f;
                this.idleTime += delta;
                if (this.idleTime >= 5.0f)
                {
                    this.idleTime = 0.0f;
                    this.stateMain = AIState_Main.Wandering;
                }
                break;
            case AIState_Main.Wandering:
                this.npcController.ForwardAxis = 1.0f;
                UpdateFSM_Wandering(delta);
                break;
            case AIState_Main.Combat:
                UpdateUS(delta);
                break;
        }
    }

    private void UpdateFSM_Wandering(float delta)
    {
        switch (this.stateWandering)
        {
            case AIState_Wandering.None:
            default:
                this.stateWandering = AIState_Wandering.SelectingTarget;
                break;
            case AIState_Wandering.SelectingTarget:
                float rngX = Random.Range(-20, 20);
                float rngY = Random.Range(-20, 20);
                Vector3 vec = new Vector3(rngX, 0, rngY);
                this.npcController.NavTarget = this.transform.position + vec;
                this.stateWandering = AIState_Wandering.MovingToTarget;
                break;
            case AIState_Wandering.MovingToTarget:
                DebugManager.Instance?.DrawSphere(this.npcController.NavTarget, 2, Color.magenta);
                this.wanderTime += delta;
                float distance = Vector3.Distance(this.transform.position, this.npcController.NavTarget);
                if (wanderTime > 5.0f || distance <= this.npcController.agent.stoppingDistance)
                {
                    this.stateWandering = AIState_Wandering.ArrivedToTarget;
                    this.wanderTime = 0.0f;
                }
                break;
            case AIState_Wandering.ArrivedToTarget:
                this.stateWandering = AIState_Wandering.None;
                this.stateMain = AIState_Main.Idle;
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
