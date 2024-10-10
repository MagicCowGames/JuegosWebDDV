using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float speed = 100.0f;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.rigidBody.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
    }

    void Update()
    {
        // this.rigidBody.velocity = this.rigidBody.transform.up * this.speed;
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
