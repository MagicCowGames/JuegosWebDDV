using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float force;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.rigidBody.AddForce(this.transform.forward * force, ForceMode.Impulse);
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
