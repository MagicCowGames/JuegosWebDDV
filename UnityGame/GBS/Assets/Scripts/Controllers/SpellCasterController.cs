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
    [SerializeField] private GameObject sprayPrefab;


    [SerializeField] private Transform[] wallTransforms;

    private ElementQueue eq;
    private bool isCasting;

    /*
    // NOTE : This idea may get scrapped, so we're leaving it here for now...
    private float accumulatedTime = 0.0f; // seconds that the cast button has been held down (used for projectile spells to increase strength)
    private float accumulatedTimeMax = 3.0f; // 3 seconds max
    private float forcePerSecond = 1.5f; // force value added to the projectile based on the accumulatedTime value. The resulting force is forcePerSecond * accumulatedTime.
    */

    private GameObject activeSpell;

    private float autoStopCastingTime;

    [SerializeField] private Form form = Form.Projectile;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.form = Form.Projectile; // Default form is projectile.
        this.activeSpell = null;
        this.isCasting = false;
        this.autoStopCastingTime = 0.0f;
        this.eq = new ElementQueue(5);
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateAutoStopCasting(delta);
    }

    #endregion

    #region PublicMethods - Getters

    public ElementQueue GetElementQueue()
    {
        return this.eq;
    }

    public bool GetIsCasting()
    {
        return this.isCasting;
    }

    public Form GetForm()
    {
        return this.form;
    }

    #endregion

    #region PublicMethods - Spell Casting

    public void Cast()
    {
        // Can't cast if the element queue is null or if it has no elements queued up, so we bail out with an early return.
        if (this.eq == null || this.eq.Count <= 0)
            return;

        // Update casting status
        this.isCasting = true;

        // Select the type of spell to cast based on the selected form
        switch (this.form)
        {
            default:
                DebugManager.Instance?.Log("The form should never be an invalid value!!!");
                break;
            case Form.Shield:
                {
                    foreach (var transform in this.wallTransforms)
                    {
                        // bool wallSpawned = false;

                        // TODO : Remove some of the early return cases so that we can mix different elements.

                        // Spawn walls if earth or ice are involved
                        if (this.eq.GetElementCount(Element.Earth) > 0 || this.eq.GetElementCount(Element.Ice) > 0)
                        {
                            var obj = ObjectSpawner.Spawn(this.shieldPrefab, transform);
                            var wall = obj.GetComponent<SpellShieldController>();
                            wall.SetSpellData(this.eq);
                            // wallSpawned = true;
                        }

                        // Spawn mines if death or heal elements are involved
                        /*
                        if (this.eq.GetElementCount(Element.Death) > 0 || this.eq.GetElementCount(Element.Heal) > 0)
                        {
                            // var obj = ObjectSpawner.Spawn(shieldPrefab, transform);
                            // var wall = obj.GetComponent<SpellShieldController>();
                            continue;
                        }
                        */

                        // Spawn elemental barrier if any other element is involved
                        int otherElements = this.eq.Count - (this.eq.GetElementCount(Element.Earth) + this.eq.GetElementCount(Element.Ice));
                        if (otherElements > 0)
                        {
                            var obj = ObjectSpawner.Spawn(this.sprayPrefab, transform);
                            var wall = obj.GetComponent<SpellSprayController>();
                            wall.SetSpellData(this.eq);
                            continue;
                        }

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
                    }

                    // Stop casting since walls don't require constant casting.
                    SetCastTime(0.5f);
                }
                break;
            case Form.Projectile:
                {
                    ObjectSpawner.Spawn(this.projectilePrefab, this.spawnTransform);

                    // Stop casting since projectiles don't require constant casting.
                    SetCastTime(0.5f);
                }
                break;
            case Form.Beam:
                {
                    var obj = ObjectSpawner.Spawn(this.beamPrefab, this.spawnTransform);
                    obj.transform.parent = this.spawnTransform;
                    this.activeSpell = obj;

                    var beam = obj.GetComponent<SpellBeamController>();
                    beam.SetSpellData(this.eq);

                    // Auto stop casting after 5 seconds of sustained beam firing.
                    // The user can manually stop casting on their own if they release the cast button, but if they keep holding it, to prevent them from being too OP,
                    // we force them to stop casting after a set amount of time.
                    SetCastTime(5.0f);
                }
                break;
        }

        // After finishing casting, clear the element queue
        this.eq.Clear();
    }

    public void StopCast()
    {
        this.isCasting = false;
        if (this.activeSpell != null)
        {
            Destroy(this.activeSpell.gameObject);
            this.activeSpell = null;
        }
    }

    #endregion

    #region PublicMethods - Element Queue

    public void AddElement(Element element)
    {
        this.eq.Add(element);
    }

    public void SetForm(Form form)
    {
        this.form = form;
    }

    #endregion

    #region PrivateMethods

    private void UpdateAutoStopCasting(float delta)
    {
        if (autoStopCastingTime <= 0.0f)
        {
            StopCast();
        }
        else
        {
            autoStopCastingTime -= delta;
        }
    }

    private void SetCastTime(float time)
    {
        this.autoStopCastingTime = time;
    }

    #endregion
}
