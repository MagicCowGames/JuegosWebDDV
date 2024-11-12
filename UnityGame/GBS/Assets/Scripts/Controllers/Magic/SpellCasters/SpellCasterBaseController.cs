using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : At some point, the idea was to handle spell casting through inheritance of a base spell caster controller component.
// Since forms are enum based and are extensible by adding new elements to the enum, we came back to the original idea of just having a "monolithic" class with
// a switch on form type to determine the behaviour of the spell caster and the spawned spells.
public class SpellCasterBaseController : MonoBehaviour, ISpellCaster
{
    #region Variables

    // NOTE : Maybe the magic manager should be the one to have a field with all of the prefabs stored so that we do not have multiple copies needlessly stored in memory?

    [Header("Spell Data - Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private float projectileMaxChargeTime;
    [SerializeField] private float projectileForcePerSecond;
    private GameObject activeProjectile;

    [Header("Spell Data - Beam")]
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private Transform beamTransform;
    private GameObject activeBeam;

    [Header("Spell Data - Wall")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject elementalWallPrefab;
    [SerializeField] private Transform[] wallTransforms;
    [SerializeField] private int maxWalls;
    private GameObject[] activeWalls;
    private GameObject[] activeElementalWalls;

    [Header("Spell Data - Spray")]
    [SerializeField] private GameObject sprayPrefab;
    [SerializeField] private Transform sprayTransform;

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
    }

    #endregion

    #region ProtectedMethods
    #endregion

    // TODO : Implement
    #region ISpellCaster

    public void StartCasting()
    {
        // Can't cast if the element queue is null or if it has no elements queued up, so we bail out with an early return.
        if (this.elementQueue == null || this.elementQueue.Count <= 0)
            return;

        // Update isCasting status.
        this.isCasting = true;

        HandleStartCasting();

        // After casting the spell, clear the element queue. The element queue is cleared "as soon as the casting starts" (kinda), not after it is finished, so during
        // sustained casting the player will already see the queue as empty.
        this.elementQueue.Clear();
    }
    public void StopCasting()
    {
        this.isCasting = false;
        this.castTimeAccumulator = 0.0f;
        HandleStopCasting(); // Here, specific impls for sustained spells such as beam spells will handle cleaning up their own spawned spells when they are no longer needed.
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

    #region PrivateMethods - Handling

    private void HandleStartCasting_Projectile()
    { }

    private void HandleStartCasting_Beam()
    { }

    private void HandleStartCasting_Shield()
    { }

    private void HandleStopCasting_Projectile()
    { }

    private void HandleStopCasting_Beam()
    { }

    private void HandleStopCasting_Shield()
    { }

    #endregion
}
