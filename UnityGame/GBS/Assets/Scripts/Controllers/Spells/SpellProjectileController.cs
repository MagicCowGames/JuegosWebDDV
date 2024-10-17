using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileController : SpellBaseController
{
    #region Variables

    [Header("Projectile Settings")]
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

    #region ISpell

    public override void UpdateSpellColor()
    {

    }

    #endregion
}
