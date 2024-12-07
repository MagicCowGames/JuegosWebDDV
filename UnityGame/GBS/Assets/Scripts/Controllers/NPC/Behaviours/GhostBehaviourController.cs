using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviourController : NPCBehaviourController
{
    #region NPCBehaviour

    protected override void InitBehaviour()
    {

    }

    protected override void UpdateBehaviour(float delta)
    {

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
                this.forwardAxis = 0.0f;
                this.idleTime += delta;
                if (this.idleTime >= 5.0f)
                {
                    this.idleTime = 0.0f;
                    this.stateMain = AIState_Main.Wandering;
                }
                break;
            case AIState_Main.Wandering:
                this.forwardAxis = 1.0f;
                UpdateFSM_Wandering(delta);
                break;
            case AIState_Main.Combat:
                UpdateUtilitySystem(delta);
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
                this.NavTarget = this.transform.position + vec;
                this.stateWandering = AIState_Wandering.MovingToTarget;
                break;
            case AIState_Wandering.MovingToTarget:
                DebugManager.Instance?.DrawSphere(this.NavTarget, 2, Color.magenta);
                this.wanderTime += delta;
                float distance = Vector3.Distance(this.transform.position, this.NavTarget);
                if (wanderTime > 5.0f || distance <= this.agent.stoppingDistance)
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

        float distanceToTarget = Vector3.Distance(this.transform.position, this.Target.transform.position);
        if (distanceToTarget <= 20.0f)
        {
            this.stateCombat = AIState_Combat.Fighting;
        }

        if (this.healthController.Health <= 10.0f && this.retreatTime < 5.0f)
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
                this.forwardAxis = 1.0f;
                this.NavTarget = this.Target.transform.position;

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
