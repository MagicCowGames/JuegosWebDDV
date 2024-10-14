using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform spawnTransform;
    
    // TODO : Implement this system in a somewhat clean way...?
    // [SerializeField] private GameObject parentTarget; // The target entity that owns this spell caster. This is the entity to which the self-cast spells must be applied to.

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

    public void Cast(ElementQueue queue)
    {
        // If the queue is null or the queue has no elements queued up, then we return because there is nothing else to be done.
        if (queue == null || queue.Count == 0)
            return;
        
        if (queue.GetElementCount(Element.Shield) > 0)
        {
            DebugManager.Instance?.Log("Shield-like spell");
            
            if (queue.GetElementCount(Element.Earth) > 0)
            {
                DebugManager.Instance?.Log("Shield-like spell with rock wall");
            }

            if (queue.GetElementCount(Element.Ice) > 0)
            {
                DebugManager.Instance?.Log("Shield-like spell with ice wall");
            }

            return;
        }

        if (queue.GetElementCount(Element.Earth) > 0)
        {
            DebugManager.Instance?.Log("Projectile-like spell");
            return;
        }

        if (queue.GetElementCount(Element.Ice) > 0)
        {
            DebugManager.Instance?.Log("Shard-like spell");
            return;
        }

        if (queue.GetElementCount(Element.Heal) > 0 || queue.GetElementCount(Element.Death) > 0)
        {
            DebugManager.Instance?.Log("Beam-like spell");
            return;
        }

        if (queue.GetElementCount(Element.Electricity) > 0)
        {
            DebugManager.Instance?.Log("Electric-like spell");
            return;
        }

        DebugManager.Instance?.Log("Spray-like spell");
        return;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
