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

    // NOTE : This idea may get scrapped, so we're leaving it here for now...
    private float accumulatedTime = 0.0f; // seconds that the cast button has been held down (used for projectile spells to increase strength)
    private float accumulatedTimeMax = 3.0f; // 3 seconds max
    private float forcePerSecond = 1.5f; // force value added to the projectile based on the accumulatedTime value. The resulting force is forcePerSecond * accumulatedTime.

    private BasicSpellController spellController;

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
        SpawnSpraySpell();
        return;
    }

    #endregion

    #region PrivateMethods

    private BasicSpellController SpawnSpell(GameObject prefab)
    {
        var obj = ObjectSpawner.Spawn(prefab, this.spawnTransform);
        var controller = obj.GetComponent<BasicSpellController>();
        return controller;
    }

    private void SpawnSpraySpell()
    {
        var spell = SpawnSpell(this.fireSpell);
        spell.owner = null;
        spell.isProjectile = false;
        spell.transform.SetParent(this.spawnTransform);
        this.spellController = spell;
    }

    #endregion
}
