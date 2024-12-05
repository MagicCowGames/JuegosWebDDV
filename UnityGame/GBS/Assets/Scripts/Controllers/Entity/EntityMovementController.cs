using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementController : MonoBehaviour
{
    #region Variables

    [SerializeField] private CharacterController characterController;

    #endregion

    #region Variables - Private

    private Vector3 gravityVector;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {
        this.gravityVector = new Vector3(0.0f, -9.8f, 0.0f);
    }

    #endregion
}
