using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellController : MonoBehaviour
{
    #region Variables

    public GameObject owner;

    [Header("Spell Components")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject spellMesh;
    [SerializeField] private GameObject spellParticle;

    [Header("Spell Info")]
    [SerializeField] public bool isProjectile;
    [SerializeField] public Color color;

    [Header("Spell Stats")]
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float life = 12.0f; // number of seconds the bullet gets to live before being despawned by itself.

    private float elapsed;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.elapsed = 0.0f;

        if (this.isProjectile)
        {
            this.rigidBody.AddForce(rigidBody.transform.up * speed, ForceMode.VelocityChange);
        }
        else
        {
            this.rigidBody.useGravity = false;
            this.rigidBody.detectCollisions = false;
        }
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
