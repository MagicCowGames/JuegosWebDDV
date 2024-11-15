using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This spell exists only for visual purposes. It is so that it can reuse the SpellBaseController's spell color logic.
// The damage code is literally going to go unused, and it would be wiser to just refactor the color calculation side of the code to the Magic Manager, but
// whatever, this quick hack works for now.
public class SpellEmitterController : SpellBaseController
{
    #region Variables

    [Header("Emitter Settings")]
    [SerializeField] private ParticleSystem elementParticle;

    #endregion

    #region MonoBehaviour

    void Start()
    {

    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void SetEmitterPlaying(bool b)
    {
        if (b)
        {
            this.elementParticle.Play();
        }
        else
        {
            this.elementParticle.Stop();
        }
    }

    public bool GetEmitterPlaying()
    {
        return this.elementParticle.isPlaying;
    }

    #endregion

    #region PrivateMethods
    #endregion

    #region ISpell

    public override void UpdateSpellColor()
    {
        // TODO : Find non-deprecated alternative to do this.
        this.elementParticle.startColor = this.spellColor;
    }

    #endregion
}
