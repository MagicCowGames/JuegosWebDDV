using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterController2 : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private GameObject owner;

    [Header("Transforms")]
    [SerializeField] private Transform spawnTransformShield;
    [SerializeField] private Transform[] spawnTransformsWalls;

    [Header("Particles")]
    [SerializeField] private ParticleSystem sprayParticle_Fire;
    [SerializeField] private ParticleSystem sprayParticle_Cold;
    [SerializeField] private ParticleSystem sprayParticle_Electric;
    [SerializeField] private LineRenderer beamLineRenderer;

    [Header("Collisions")]
    [SerializeField] private CapsuleCollider sprayCollider;
    [SerializeField] private CapsuleCollider beamCollider;

    private ElementQueue eq;

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

    public void SetElementQueue(ElementQueue queue)
    {
        this.eq = new ElementQueue(queue);
    }

    public void Cast()
    {
        // If the queue is null or the queue has no elements queued up, then we return because there is nothing else to be done.
        if (this.eq == null)
        {
            DebugManager.Instance?.Log("The ElementQueue is null!");
            return;
        }

        if (this.eq.Count <= 0)
        {
            DebugManager.Instance?.Log("The ElementQueue is empty!");
            return;
        }
        
        if (this.eq.GetElementCount(Element.Shield) > 0)
        {
            DebugManager.Instance?.Log("Shield-like spell");
            
            if (this.eq.GetElementCount(Element.Earth) > 0)
            {
                DebugManager.Instance?.Log("Shield-like spell with rock wall");
            }

            if (this.eq.GetElementCount(Element.Ice) > 0)
            {
                DebugManager.Instance?.Log("Shield-like spell with ice wall");
            }

            return;
        }

        if (this.eq.GetElementCount(Element.Earth) > 0)
        {
            DebugManager.Instance?.Log("Projectile-like spell");
            // ObjectSpawner.Spawn(this.rockSpell, this.spawnTransform);
            return;
        }

        if (this.eq.GetElementCount(Element.Ice) > 0)
        {
            DebugManager.Instance?.Log("Shard-like spell");
            return;
        }

        if (this.eq.GetElementCount(Element.Heal) > 0 || this.eq.GetElementCount(Element.Death) > 0)
        {
            DebugManager.Instance?.Log("Beam-like spell");
            return;
        }

        if (this.eq.GetElementCount(Element.Electricity) > 0)
        {
            DebugManager.Instance?.Log("Electric-like spell");
            return;
        }

        DebugManager.Instance?.Log("Spray-like spell");
        // SpawnSpraySpell();
        return;
    }

    #endregion

    #region PrivateMethods

    private void SetBeamColor(Color color)
    {
        if (this.beamLineRenderer == null)
            return;

        this.beamLineRenderer.startColor = color;
        this.beamLineRenderer.endColor = color;
    }

    private void SetBeamColor(Element[] elements)
    {
    }

    #endregion
}
