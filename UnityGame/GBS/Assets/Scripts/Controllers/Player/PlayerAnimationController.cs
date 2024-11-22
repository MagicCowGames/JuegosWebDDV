using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Variables

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private float forwardMovement;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateAnimation(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {
        this.forwardMovement = 0.0f;
    }

    private void UpdateAnimation(float delta)
    {

    }

    #endregion
}
