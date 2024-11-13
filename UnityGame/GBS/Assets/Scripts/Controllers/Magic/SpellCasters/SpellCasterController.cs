using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : At some point, the idea was to handle spell casting through inheritance of a base spell caster controller component.
// Since forms are enum based and are extensible by adding new elements to the enum, we came back to the original idea of just having a "monolithic" class with
// a switch on form type to determine the behaviour of the spell caster and the spawned spells.
public class SpellCasterController : MonoBehaviour, ISpellCaster
{
    #region Variables

    // NOTE : Maybe the magic manager should be the one to have a field with all of the prefabs stored so that we do not have multiple copies needlessly stored in memory?

    [Header("Spell Data - Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private float projectileDuration = 15.0f; // max allowed projectile charge time
    [SerializeField] private float projectileForceGain = 20.0f; // force gained per second holding down the cast button.
    private GameObject activeProjectile; // NOTE : This goes unused for now...

    [Header("Spell Data - Beam")]
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private Transform beamTransform;
    [SerializeField] private float beamDuration = 5.0f; // max allowed beam cast time
    private GameObject activeBeam;

    [Header("Spell Data - Wall")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject elementalWallPrefab;
    [SerializeField] private Transform[] wallTransforms;
    [SerializeField] private int maxWalls;
    private GameObject[] activeWalls;
    private GameObject[] activeElementalWalls;

    /*
    [Header("Spell Data - Spray")]
    [SerializeField] private GameObject sprayPrefab;
    [SerializeField] private Transform sprayTransform;
    */

    private Form form;
    private ElementQueue elementQueue;
    private bool isCasting;

    private float castDuration;
    private float castTimeAccumulator;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }

    void Update()
    {
        float delta = Time.deltaTime;
        HandleAutoStopCasting(delta);
    }

    #endregion

    #region PublicMethods

    // Could maybe be renamed to "UpdateAutoStopCasting"? Also, need to notify the owner of this spell caster that the casting has been terminated manually.
    // Either that or they are manually polling / listening to the casting status.
    private void HandleAutoStopCasting(float delta)
    {
        if (!this.isCasting)
            return;
        
        this.castTimeAccumulator += delta;

        if (this.castTimeAccumulator >= castDuration)
            StopCasting();
    }

    #endregion

    #region PrivateMethods

    private void Init()
    {
        // Initialize variables to default values.
        this.elementQueue = null;
        this.isCasting = false;
        this.castDuration = 0.0f;
        this.form = Form.Projectile; // Projectile form by default.

        // Initialize "activeSpell" game objects (which serve as pointers to the currently spawned spell)
        this.activeProjectile = null;
        this.activeBeam = null;
        this.activeWalls = new GameObject[this.maxWalls];
        this.activeElementalWalls = new GameObject[this.maxWalls];

        // Initialize element queue class
        this.elementQueue = new ElementQueue(5); // Gets initialized with 5 slots for 5 elements per combination.
        
    }

    #endregion

    #region ProtectedMethods
    #endregion

    #region ISpellCaster

    public void StartCasting()
    {
        // Can't cast if the element queue is null or if it has no elements queued up, so we bail out with an early return.
        if (this.elementQueue == null || this.elementQueue.Count <= 0)
            return;

        // Update isCasting status.
        this.isCasting = true;


        HandleStartCasting();
    }
    public void StopCasting()
    {
        // Can't call the stop casting function if we are not casting. Otherwise, projectile spells would shoot even with an empty element queue.
        if (!this.isCasting)
            return;

        // NOTE : We handle the stop casting function first so that we can access the cast time accumulator for projectile spells so that we can get the accumulated charge time.
        this.isCasting = false;
        HandleStopCasting(); // Here, specific impls for sustained spells such as beam spells will handle cleaning up their own spawned spells when they are no longer needed.
        this.castTimeAccumulator = 0.0f;
    }
    public bool GetIsCasting()
    {
        return this.isCasting;
    }

    public void SetElementQueue(ElementQueue queue)
    {
        this.elementQueue = queue;
    }
    public ElementQueue GetElementQueue()
    {
        return this.elementQueue;
    }

    public void AddElements(Element[] elements)
    {
        this.elementQueue.Add(elements); // Add the elements within the array one by one to the element queue to make sure that combinations are handled properly.
    }
    public Element[] GetElements()
    {
        return this.elementQueue.Elements;
    }
    public void AddElement(Element element)
    {
        this.elementQueue.Add(element);
    }

    public void SetForm(Form form)
    {
        this.form = form;
    }
    public Form GetForm()
    {
        return this.form;
    }

    public void SetCastDuration(float time)
    {
        this.castDuration = time;
    }
    public float GetCastDuration()
    {
        return this.castDuration;
    }

    public void HandleStartCasting()
    {
        DebugManager.Instance?.Log($"HandleStartCasting() with form \"{this.form}\"");
        switch (this.form)
        {
            default:
            case Form.Projectile:
                HandleStartCasting_Projectile();
                break;
            case Form.Beam:
                HandleStartCasting_Beam();
                break;
            case Form.Shield:
                HandleStartCasting_Shield();
                break;
        }
    }
    public void HandleStopCasting()
    {
        DebugManager.Instance?.Log($"HandleStopCasting() with form \"{this.form}\"");
        switch (this.form)
        {
            default:
            case Form.Projectile:
                HandleStopCasting_Projectile();
                break;
            case Form.Beam:
                HandleStopCasting_Beam();
                break;
            case Form.Shield:
                HandleStopCasting_Shield();
                break;
        }
    }

    #endregion

    #region PrivateMethods - Handling - Start Casting

    private void HandleStartCasting_Projectile()
    {
        // Projectiles don't require constant casting. What this does is charge up the projectile speed
        // Auto stop casting projectiles after N seconds of charging.
        SetCastDuration(this.projectileDuration);
    }

    private void HandleStartCasting_Beam()
    {
        SpawnBeam();

        // Auto stop casting after N seconds of sustained beam firing.
        // The user can manually stop casting on their own if they release the cast button, but if they keep holding it, to prevent them from being too OP,
        // we force them to stop casting after a set amount of time.
        SetCastDuration(this.beamDuration);
    }

    private void HandleStartCasting_Shield()
    {
        SpawnShield();

        // Stop casting since walls don't require constant casting.
        // TODO : Modify logic to add wall creation cooldown? maybe through player animations?
        SetCastDuration(0.5f);
    }

    #endregion

    #region PrivateMethods - Handling - Stop Casting

    // Projectiles spawn here because they spawn on RMB release.
    private void HandleStopCasting_Projectile()
    {
        SpawnProjectile();
    }

    private void HandleStopCasting_Beam()
    {
        // Despawn the beam
        // TODO : Modify logic when object pooling is implemented for beam spawning.
        if (this.activeBeam != null)
        {
            Destroy(this.activeBeam.gameObject);
            this.activeBeam = null;
        }
    }

    private void HandleStopCasting_Shield()
    {
        // Walls don't need any extra handling, they despawn on their own (maybe this behaviour will change in the future when object pooling is implemented).
    }

    #endregion

    #region Cast Spell Methods

    // These are the methods that handle the logic for spawning each spell type based on form / prefab.
    // NOTE : After casting the spell, we must clear the element queue.
    // The element queue is cleared "as soon as the casting starts" (kinda), not after it is finished, so during
    // sustained casting the player will already see the queue as empty.
    // In the case of projectile spells, since we spawn those after we release the click, we must store the value in a temporary.

    private void SpawnProjectile()
    {
        // Spawn a projectile
        var obj = ObjectSpawner.Spawn(this.projectilePrefab, this.projectileTransform);

        // Set spell data
        var proj = obj.GetComponent<SpellProjectileController>();
        proj.SetSpellData(this.elementQueueTemp);

        // Set the projectile's force based on the current cast time accumulator.
        // NOTE : We have a base of 1.0f for the cast time multiplider so that the projectile will always have a small amount of forwards force, even when
        // we just press the RMB click without holding down to charge the spell's force.
        proj.Force = (1.0f + this.castTimeAccumulator) * this.projectileForceGain;
    }

    private void SpawnBeam()
    {
        // Spawn a beam
        var obj = ObjectSpawner.Spawn(this.beamPrefab, this.beamTransform);
        obj.transform.parent = this.beamTransform; // Attach the beam so that it follows the player's rotation
        this.activeBeam = obj;

        // Set spell data
        var beam = obj.GetComponent<SpellBeamController>();
        beam.SetSpellData(this.elementQueue);
    }

    private void SpawnShield()
    {
        foreach (var transform in this.wallTransforms)
        {
            // Spawn walls if earth or ice are involved
            if (this.elementQueue.GetElementCount(Element.Earth) > 0 || this.elementQueue.GetElementCount(Element.Ice) > 0)
            {
                var obj = ObjectSpawner.Spawn(this.wallPrefab, transform);
                var wall = obj.GetComponent<SpellShieldController>();
                wall.SetSpellData(this.elementQueue);
                // TODO : Add the logic to store the spawned walls and despawn old walls when going over the max walls cap.
            }

            #region DisabledCode

            // Spawn mines if death or heal elements are involved
            /*
            if (this.eq.GetElementCount(Element.Death) > 0 || this.eq.GetElementCount(Element.Heal) > 0)
            {
                // var obj = ObjectSpawner.Spawn(shieldPrefab, transform);
                // var wall = obj.GetComponent<SpellShieldController>();
                continue;
            }
            */

            #endregion

            // Spawn elemental barrier if any other element is involved
            int otherElements = this.elementQueue.Count - (this.elementQueue.GetElementCount(Element.Earth) + this.elementQueue.GetElementCount(Element.Ice));
            if (otherElements > 0)
            {
                var obj = ObjectSpawner.Spawn(this.elementalWallPrefab, transform);
                var wall = obj.GetComponent<SpellSprayController>();
                wall.SetSpellData(this.elementQueue);
                continue;
            }

            #region DisabledCode

            // Spawn regular shield if no other type of wall was spawned.
            /*
            if (!wallSpawned)
            {
                var obj = ObjectSpawner.Spawn(shieldPrefab, transform);
                var shield = obj.GetComponent<SpellShieldController>();
            }
            */



            // NOTE : Currently mines and regular shield are disabled cause this is a Magicka thing and
            // I'm thinking that I don't want to fully copy it even tho it's a cool feature.
            // TODO : Figure out whether I want this in the final game or not.

            #endregion
        }
    }

    #endregion
}
