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
    private Element[] attackElements;
    private Form attackForm;
    private float attackDuration;

    private float attackDistanceMin;
    private float attackDistanceMax;

    #endregion

    #region Constructor

    public AttackAction(NPCController controller, Element[] elements, Form form, float castTime, float cooldown, float distanceMin, float distanceMax)
    {
        this.controller = controller;
        this.attackCooldown = cooldown;
        this.attackElements = elements;
        this.attackForm = form;
        this.attackDuration = castTime;

        this.attackDistanceMin = distanceMin;
        this.attackDistanceMax = distanceMax;
    }

    #endregion

    #region PublicMethods

    public float Calculate(float delta)
    {
        if (this.controller.Target == null || this.controller.isFleeing || this.controller.spellCaster.GetIsCasting() || this.timeSinceLastAttack <= this.attackCooldown)
            return 0.0f;

        var distance = this.controller.DistanceToTarget;

        return distance >= this.attackDistanceMin && distance <= this.attackDistanceMax ? 1.0f : 0.0f;

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
