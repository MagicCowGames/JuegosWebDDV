using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : IUtilityAction
{
    #region Variables

    private NPCController controller;
    private float chaseTime;
    private float maxChaseTime;


    #endregion

    #region Constructor

    public ChaseAction(NPCController controller)
    {
        this.controller = controller;
    }

    #endregion

    #region PublicMethods

    public float Calculate(float delta)
    {
        return GameMath.Sigmoid();
    }

    public void Execute(float delta)
    {
        this.controller.NavTarget = this.controller.Target.transform.position;
        this.chaseTime += delta;
    }

    public void Reset()
    {
        this.chaseTime = 0.0f;
    }

    #endregion
}
