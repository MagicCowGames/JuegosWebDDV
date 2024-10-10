using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float life = 12.0f; // number of seconds the bullet gets to live before being despawned by itself.

    private float elapsed;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.rigidBody.AddForce(rigidBody.transform.up * speed, ForceMode.VelocityChange);
        this.elapsed = 0.0f;
    }

    void Update()
    {
        this.elapsed += Time.deltaTime;
        if (this.elapsed >= this.life)
            Kill();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Kill()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 0.0f);
    }

    #endregion
}
