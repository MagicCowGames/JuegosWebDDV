using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    #region Variables

    [Header("Stat Components")]
    [SerializeField] public HealthController healthController; // TODO : Maybe move most of the health controller logic to just be within the entity controller?

    [Header("Entity Components")]
    [SerializeField] public EntityMovementController movementController;
    [SerializeField] public EntityAnimationController animationController;

    #endregion

    #region Events

    // TODO : Implement

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
