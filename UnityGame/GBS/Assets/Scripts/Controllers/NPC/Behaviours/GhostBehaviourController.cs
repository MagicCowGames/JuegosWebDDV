using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviourController : NPCBehaviourController
{
    #region Variables

    // NOTE : These state enums could be made to be specific for each behaviour rather than generic.

    [Header("AI States")]
    [SerializeField] private AIState_Main stateMain = AIState_Main.None;
    [SerializeField] private AIState_Wandering stateWandering = AIState_Wandering.None;
    [SerializeField] private AIState_Combat stateCombat = AIState_Combat.None;

    private float idleTime = 0.0f;
    private float wanderTime = 0.0f;

    private float retreatTime = 0.0f;

    IUtilityAction[] actions;

    #endregion

    #region NPCBehaviour

    protected override void InitBehaviour()
    {
        this.actions = new IUtilityAction[] {
            new ChaseAction(this.npcController),
            new FleeAction(this.npcController),
            new AttackAction(this.npcController, new Element[]{Element.Fire, Element.Fire, Element.Earth}, Form.Projectile, 1.5f, 3.5f, 15.0f, 20.0f),
            // new AttackAction(this, new Element[]{Element.Fire, Element.Death, Element.Death}, Form.Beam, 3.0f, 10.5f, 21.0f, 40.0f),
            new AttackAction(this.npcController, new Element[]{Element.Fire, Element.Fire}, Form.Projectile, 0.1f, 1.5f, 0.0f, 3.0f)
        };
    }

    protected override void UpdateBehaviour(float delta)
    {
        // For now, just walk toward the selected target GameObject.
        if (this.npcController.Target != null)
        {
            this.stateMain = AIState_Main.Combat; // This should be set through an event only ONCE, but whatever... for now we do it this way lol...
        }
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

    private void UpdateFSM_Combat(float delta)
    {
        this.stateCombat = AIState_Combat.Chasing;

        float distanceToTarget = Vector3.Distance(this.transform.position, this.npcController.Target.transform.position);
        if (distanceToTarget <= 20.0f)
        {
            this.stateCombat = AIState_Combat.Fighting;
        }

        if (this.npcController.healthController.Health <= 10.0f && this.retreatTime < 5.0f)
        {
            this.stateCombat = AIState_Combat.Retreating;
        }

        switch (this.stateCombat)
        {
            case AIState_Combat.None:
            default:
                this.stateCombat = AIState_Combat.Chasing;
                break;
            case AIState_Combat.Chasing:
                this.npcController.ForwardAxis = 1.0f;
                this.npcController.NavTarget = this.npcController.Target.transform.position;

                break;
            case AIState_Combat.Fighting:
                // TODO : Implement more complex fighting logic with a custom FSM with distances based on whether this NPC can perform
                // ranged attacks or not, distance to player, and other stuff like that, etc...
                break;
            case AIState_Combat.Retreating:
                if (this.retreatTime >= 5.0f)
                {
                    this.retreatTime = 0.0f;
                }
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
