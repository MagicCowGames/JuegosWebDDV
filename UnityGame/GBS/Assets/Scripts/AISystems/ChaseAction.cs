using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : IUtilityAction
{
    #region Variables

    private NPCController controller;

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
        return 0.5f;
    }

    public void Execute(float delta)
    {
        this.controller.ForwardAxis = 1.0f;
        this.controller.NavTarget = this.controller.Target.transform.position;
    }

    public void Update(float delta)
    {

    }

    #endregion
}
