using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileController : SpellBaseController
{
    #region Variables

    [Header("Projectile Settings")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float force;
    [SerializeField] private ParticleSystem elementParticles;

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

    #region ISpell

    public override void UpdateSpellColor()
    {
        // TODO : Find non-deprecated alternative to do this.
        this.elementParticles.startColor = this.spellColor;
    }

    #endregion

    #region Collisions

    private void OnCollisionEnter(Collision collision)
    {


        // TODO : Change this logic when we implement spell pooling.
        Destroy(this.gameObject);
    }

    #endregion
}
