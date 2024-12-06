using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Merge logic with the regular attack action and just make it configurable with an enum or something.
public class AttackAction : IUtilityAction
{
    #region Properties

    public string Name { get; set; } = "AttackAction";

    #endregion

    #region Variables

    private NPCController controller;
    private float attackCooldown = 2.0f;
    private float timeSinceLastAttack = 0.0f;
    private float attackDistance = 10.0f;
    private Element[] attackElements;
    private Form attackForm;
    private float attackDuration;

    #endregion

    #region Constructor

    public AttackAction(NPCController controller, Element[] elements, Form form, float castTime, float cooldown, float distance)
    {
        this.controller = controller;
        this.attackCooldown = cooldown;
        this.attackDistance = distance;
        this.attackElements = elements;
        this.attackForm = form;
        this.attackDuration = castTime;
    }

    #endregion

    #region PublicMethods

    public float Calculate(float delta)
    {
        if (this.controller.Target == null)
            return 0.0f;

        var distance = this.controller.DistanceToTarget;

        if (distance > this.attackDistance || this.timeSinceLastAttack <= this.attackCooldown || this.controller.spellCaster.GetIsCasting())
        {
            return 0.0f;
        }
        else
        {
            return 0.6f;
        }
    }

    public void Execute(float delta)
    {
        this.controller.ForwardAxis = 0.0f;
        this.controller.Attack(this.attackElements, this.attackForm, this.attackDuration);
        this.timeSinceLastAttack = 0.0f;
    }

    public void Update(float delta)
    {
        this.timeSinceLastAttack += delta;
    }

    #endregion
}
