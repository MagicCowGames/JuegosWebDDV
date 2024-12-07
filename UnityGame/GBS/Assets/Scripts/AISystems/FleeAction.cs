using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeAction : IUtilityAction
{
    #region Properties

    public string Name { get; set; } = "FleeAction";

    #endregion

    #region Variables

    private NPCController controller;
    private float fleeTime = 0.0f;
    // private float fleeTimeMin = 0.0f;
    private float fleeTimeMax = 3.5f;

    private float fleeTimeCooldown = 5.0f;
    private float fleeTimeRest = 0.0f;

    #endregion

    #region Constructor

    public FleeAction(NPCController controller)
    {
        this.controller = controller;
    }

    #endregion

    #region PublicMethods

    public float Calculate(float delta)
    {
        if (this.controller.Target == null)
            return 0.0f;

        if (this.fleeTime >= this.fleeTimeMax)
        {
            return 0.0f;
        }
        else
        {
            var min = this.controller.healthController.HealthMin;
            var max = this.controller.healthController.HealthMax;
            var val = this.controller.healthController.Health;
            return GameMath.MapValue(val, min, max, 1.0f, 0.0f); // This shit's kinda hacky if you ask me! lol...
        }
    }

    public void Execute(float delta)
    {
        // Vector that goes from target to self
        Vector3 vec = this.controller.transform.position - this.controller.Target.transform.position;
        
        // Director vector (normalized)
        Vector3 dir = vec.normalized;

        // Calculate a point in that direction and move to it
        this.controller.ForwardAxis = 1.0f;
        this.controller.NavTarget = this.controller.transform.position + dir * 10.0f;

        // Spawn a wall for protection before fleeing
        if(!this.controller.isFleeing)
            this.controller.Attack(new Element[] { Element.Earth, Element.Ice }, Form.Shield, 0.2f);

        // Update flee time
        this.fleeTime += delta;
        this.fleeTimeRest = 0.0f;
        this.controller.isFleeing = true;
    }

    public void Update(float delta)
    {
        this.fleeTimeRest += delta;
        if (this.fleeTimeRest >= this.fleeTimeCooldown)
        {
            this.fleeTime = 0.0f;
            this.controller.isFleeing = false;
        }
    }

    #endregion
}
