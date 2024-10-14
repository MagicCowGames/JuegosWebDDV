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

    [Header("Spell Config")]
    [SerializeField] private SpellType spellType;

    [Header("Spell Stats")]
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float speed = 100.0f;

    [Header("Spell Lifetime")]
    [SerializeField] private float duration = 10.0f; // Amount of time until the spell's effects are disabled and it's visual appearance is hidden.
    [SerializeField] private float life = 12.0f; // Amount of time until the spell's entity is actually killed / returned to the pool. This value should be higher than duration.

    private float elapsed;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.elapsed = 0.0f;

        if (this.spellType == SpellType.Projectile)
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

        if (this.elapsed >= this.duration)
            DisableSpell();
        
        if (this.elapsed >= this.life)
            KillSpell();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void KillSpell()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 0.0f);
    }

    private void DisableSpell()
    {

    }

    #endregion
}
