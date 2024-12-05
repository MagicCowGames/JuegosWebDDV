using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationController : MonoBehaviour
{
    #region Variables

    private EntityController controller;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.controller = GetComponent<EntityController>();
    }

    void Update()
    {
        if (this.controller == null)
            return;
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
