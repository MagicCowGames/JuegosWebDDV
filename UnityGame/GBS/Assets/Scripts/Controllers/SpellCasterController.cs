using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform spawnTransform;

    // TODO : Implement this system in a somewhat clean way...?
    // [SerializeField] private GameObject parentTarget; // The target entity that owns this spell caster. This is the entity to which the self-cast spells must be applied to.

    [SerializeField] private GameObject rockSpell;
    [SerializeField] private GameObject fireSpell;

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
        if (this.eq == null || this.eq.Count == 0)
            return;
        
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
            ObjectSpawner.Spawn(this.rockSpell, this.spawnTransform);
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
        ObjectSpawner.Spawn(this.fireSpell, this.spawnTransform);
        return;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
