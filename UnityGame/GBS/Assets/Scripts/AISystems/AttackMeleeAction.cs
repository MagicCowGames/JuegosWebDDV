using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Merge logic with the regular attack action and just make it configurable with an enum or something.
public class AttackMeleeAction : IUtilityAction
{
    #region Variables

    private NPCController controller;
    private float attackCooldown = 5.0f;
    private float timeSinceLastAttack = 0.0f;
    private float attackDistance = 5.0f;

    #endregion

    #region Constructor

    public AttackMeleeAction(NPCController controller)
    {
        this.controller = controller;
    }

    #endregion

    #region PublicMethods

    public float Calculate(float delta)
    {
        if (this.controller.Target == null)
            return 0.0f;

        var distance = this.controller.DistanceToTarget;

        if (distance > this.attackDistance || this.timeSinceLastAttack <= this.attackCooldown)
        {
            return 0.0f;
        }
        else
        {
            return 1.0f;
        }
    }

    public void Execute(float delta)
    {
        this.controller.ForwardAxis = 0.0f;
        this.controller.AttackMelee();
        this.timeSinceLastAttack = 0.0f;
    }

    public void Update(float delta)
    {
        this.attackCooldown -= delta;
    }

    #endregion
}
