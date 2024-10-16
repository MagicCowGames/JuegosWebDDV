using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellCasterController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform spawnTransform;

    // TODO : Implement this system in a somewhat clean way...?
    // [SerializeField] private GameObject parentTarget; // The target entity that owns this spell caster. This is the entity to which the self-cast spells must be applied to.

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject shieldPrefab;


    [SerializeField] private Transform[] wallTransforms;

    private ElementQueue eq;

    // NOTE : This idea may get scrapped, so we're leaving it here for now...
    private float accumulatedTime = 0.0f; // seconds that the cast button has been held down (used for projectile spells to increase strength)
    private float accumulatedTimeMax = 3.0f; // 3 seconds max
    private float forcePerSecond = 1.5f; // force value added to the projectile based on the accumulatedTime value. The resulting force is forcePerSecond * accumulatedTime.

    GameObject activeSpell;

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
        if (this.eq == null || this.eq.Count <= 0)
            return;

        if (this.eq.GetElementCount(Element.Shield) > 0)
        {
            foreach (var transform in this.wallTransforms)
            {
                // TODO : Remove some of the early return cases so that we can mix different elements.

                // Spawn walls if earth or ice are involved
                if (this.eq.GetElementCount(Element.Earth) > 0 || this.eq.GetElementCount(Element.Ice) > 0)
                {
                    var obj = ObjectSpawner.Spawn(shieldPrefab, transform);
                    var wall = obj.GetComponent<SpellShieldController>();
                    return;
                }

                // Spawn mines if death or heal elements are involved
                if (this.eq.GetElementCount(Element.Earth) > 0 || this.eq.GetElementCount(Element.Ice) > 0)
                {
                    // var obj = ObjectSpawner.Spawn(shieldPrefab, transform);
                    // var wall = obj.GetComponent<SpellShieldController>();
                    return;
                }

                // Spawn elemental barrier if any other element is involved
                if (this.eq.GetElementCount(Element.Earth) > 0 || this.eq.GetElementCount(Element.Ice) > 0)
                {
                    // var obj = ObjectSpawner.Spawn(shieldPrefab, transform);
                    // var wall = obj.GetComponent<SpellShieldController>();
                    return;
                }

                // Spawn regular shield otherwise
                var shieldObj = ObjectSpawner.Spawn(shieldPrefab, transform);
                var shield = shieldObj.GetComponent<SpellShieldController>();
            }
        }
        else
        if (this.eq.GetElementCount(Element.Projectile) > 0)
        {
            ObjectSpawner.Spawn(projectilePrefab, this.spawnTransform);
        }
        else
        if(this.eq.GetElementCount(Element.Beam) > 0)
        {
            var obj = ObjectSpawner.Spawn(beamPrefab, this.spawnTransform);
            obj.transform.parent = this.spawnTransform;
            this.activeSpell = obj;

            // TODO : This code is shit, please fix
            Color color = Color.white;
            float r = 0.0f;
            float g = 0.0f;
            float b = 0.0f;
            foreach (var element in this.eq.Elements)
                if (element != Element.Beam)
                {
                    var c = MagicManager.Instance.GetColor(element);
                    r += c.r;
                    g += c.g;
                    b += c.b;
                }

            r /= this.eq.Count;
            g /= this.eq.Count;
            b /= this.eq.Count;

            color = new Color(r, g, b, 255);

            var beam = obj.GetComponent<SpellBeamController>();
            beam.SetSpellColor(color);
        }
    }

    public void StopCast()
    {
        DebugManager.Instance?.Log("stop!");
        if (this.activeSpell != null)
            Destroy(this.activeSpell.gameObject);
    }

    /*
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
    */

    #endregion

    #region PrivateMethods

    /*
    private BasicSpellController SpawnSpell(GameObject prefab)
    {
        var obj = ObjectSpawner.Spawn(prefab, this.spawnTransform);
        var controller = obj.GetComponent<BasicSpellController>();
        return controller;
    }

    private void SpawnSpraySpell()
    {
        var spell = SpawnSpell(this.fireSpell);
        spell.SetOwner(null);
        spell.SetSpellType(SpellType.Spray);
        spell.transform.SetParent(this.spawnTransform);
        this.spellController = spell;
    }
    */

    #endregion
}
