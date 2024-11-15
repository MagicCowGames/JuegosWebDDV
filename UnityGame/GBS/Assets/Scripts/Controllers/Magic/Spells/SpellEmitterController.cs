using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEmitterController : MonoBehaviour
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
        if (!b)
        {
            this.elementParticle.Stop();
        }
        else
        if (this.elementParticle.isStopped) // If we just checked for b, then setting the particle to Play() while it is already playing would reset the particle animation.
        {
            this.elementParticle.Play();
            return;
        }
    }

    public bool GetEmitterPlaying()
    {
        return !this.elementParticle.isStopped;
    }

    public void SetEmitterElement(Element element)
    {
        Color color = MagicManager.Instance.GetElementColor(element); // NOTE : The element does not get stored within this class, it just changes the color and calls it a day... this may need to be changed in the future.
        this.elementParticle.startColor = color; // TODO : Find non-deprecated alternative to do this.
    }

    #endregion

    #region PrivateMethods
    #endregion
}
