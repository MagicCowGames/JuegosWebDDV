using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSprayController : SpellBaseController
{
    #region Variables

    [Header("Spray Settings")]
    [SerializeField] private ParticleSystem sprayParticleSystem;
    [SerializeField] private float lifeTime = 5.0f;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        this.lifeTime -= Time.deltaTime;
        if (this.lifeTime <= 0.0f)
            Destroy(this.gameObject);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion

    #region ISpell

    public override void UpdateSpellColor()
    {
        // TODO : Find some way of doing this that is not deprecated and that doesn't require me to shoot myself in the dick.
        sprayParticleSystem.startColor = GetSpellColor();
    }

    #endregion
}
