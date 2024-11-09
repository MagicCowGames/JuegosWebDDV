using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// NOTE : In the future, if we want different reactions to take place on NPCs and the player based on the type of spell, we could make it so that
// there is a parameter that specifies whether the effect is meant to be a "healing" or "damaging" effect, even if the result adds or remove health,
// because this will determine if neutral enemies will aggro, or what sounds they make when affected by a spell, etc...

// NOTE : This should maybe be renamed to SpellEffectController or just EffectController, idk... I mean, it could be used for swords and bows as well so it's ok for now.

[ExecuteInEditMode]
public class HealthModifierController : MonoBehaviour
{
    #region Enums

    // This enum controls the application type of this health modifier.
    public enum Type
    {
        Instant = 0,
        OverTime,
        Repeat
    }

    #endregion

    #region Structs

    [System.Serializable]
    public struct InputDamageElement
    {
        public Element element;
        public int amount;
    }

    [System.Serializable]
    public struct InputDamageArray
    {
        public InputDamageElement[] damageValues;

        public InputDamageArray(int defaultAmount = 0)
        {
            this.damageValues = new InputDamageElement[(int)Element.COUNT];
            for (int i = 0; i < this.damageValues.Length; ++i)
            {
                damageValues[i].element = (Element)i;
                damageValues[i].amount = defaultAmount;
            }
        }
    }

    #endregion

    #region Variables

    [Header("Owner Configuration")]
    [SerializeField] private GameObject owner; // The owner game object of this component. Refers to the GameObject that contains the health component which we do not want to be able to affect.
    // TODO : Add some kind of team handling system in the future? maybe? or just let everyone damage everyone and leave things as they are.
    // Also, maybe rather than an owner it should be an "ignore list" so that we can ignore multiple entities / gameobjects?

    [Header("Modifier Configuration")]
    [SerializeField] private Type type;
    [SerializeField] private bool modifierEnabled = true; // Global switch that controls if the health modifier is enabled or not. If disabled, then no damage, healing or status effects can be applied to any entity that is targetted by this health modifier controller.
    [SerializeField] private bool collisionEnabled = true; // Global switch that determines whether the collision based damage application is enabled or not.
    [SerializeField] private float repeatTime = 1.0f; // How often to repeat the health modification Apply() call when a GameObject with a health component stays within the trigger area. Every 1.0f seconds by default.

    [Header("Element Modifier Values")]
    [SerializeField] private bool useDefaultValues; // Determines if this is a prefabricated damage area and it uses the default values located within the inspector panel in the Unity Editor.
    [SerializeField] private bool resetValues;
    [SerializeField] private InputDamageArray inputDamageArray; // Can't use default constructor for serializable structs for some reason. Ok Unity, you win...

    private int[] elementCounts;
    private float accumulatedTime = 0.0f;

    public GameObject Owner { get { return this.owner; } set { this.owner = value; } }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        // Initialize the elements counts array
        this.elementCounts = new int[(int)Element.COUNT];
        for (int i = 0; i < (int)Element.COUNT; ++i)
            this.elementCounts[i] = 0;
        if (this.inputDamageArray.damageValues != null)
            for (int i = 0; i < this.inputDamageArray.damageValues.Length; ++i)
                this.elementCounts[(int)this.inputDamageArray.damageValues[i].element] = this.inputDamageArray.damageValues[i].amount;
    }

    void Start()
    {
        
    }

    void Update()
    {
        this.accumulatedTime += Time.deltaTime;
    }

    void OnValidate()
    {
        if (this.useDefaultValues)
        {
            if (this.inputDamageArray.damageValues.Length <= 0 || this.resetValues)
            {
                this.inputDamageArray = new InputDamageArray(0);
            }
        }
        else
        {
            this.inputDamageArray = default;
        }

        if (this.resetValues)
            this.resetValues = false;
    }

    #endregion

    #region PublicMethods

    public void SetValue(Element element, int amount)
    {
        this.elementCounts[(int)element] = amount;
    }

    public void SetValues(int[] amounts)
    {
        for (int i = 0; i < (int)Element.COUNT; ++i)
            this.elementCounts[i] = amounts[i];
    }

    // NOTE : If the selected game object has no health component, we just return, so the game won't explode and things will be just fine.
    public void Apply(GameObject obj, float delta = 1.0f)
    {
        // If it's disabled, then bail out and don't apply any effects to the target.
        if (!this.modifierEnabled)
            return;

        // If the input object is null, return. This should never happen, but if it does, we're safe! (famous last words, I know...)
        if (obj == null)
            return;

        // If the input object is our owner, return. That way, we prevent entities damaging themselves in situations where we do not want them to be able to do so.
        if (obj == this.owner)
            return;

        var protection = obj.GetComponent<ProtectionController>();
        var hp = obj.GetComponent<HealthController>();
        // TODO : Add get component for status effect component so that we can handle burning and stuff in the future.

        // TODO : Add protection logic here.

        if (hp == null)
            return;

        for(int i = 0; i < (int)Element.COUNT; ++i)
            hp.Health += delta * MagicManager.Instance.GetElementDamage((Element)i) * elementCounts[i];
    }

    #endregion

    #region PrivateMethods
    #endregion

    #region CollisionMethods

    void OnTriggerEnter(Collider other)
    {
        if (!this.collisionEnabled)
            return;
        if (this.type == Type.Instant)
            Apply(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (!this.collisionEnabled)
            return;
        switch (this.type)
        {
            case Type.OverTime:
                Apply(other.gameObject, Time.deltaTime);
                break;
            case Type.Repeat:
                // This aux var crap should not be needed but for some reason having a float variable exposed on Unity's inspector breaks <, <=, > and => comparisons,
                // they will all basically act the same as == which makes no fucking sense at all, not to mention that equality makes no sense on floats
                // in the first place... The temporary, crappy and patchy workaround is to make a temp copy of the value to be compared and store it in an aux var.
                // This behaviour is also found on builds, so this means that this crap is at Unity's compiler level. Why? who the fuck knows.
                // All I know is that I hate Unity more and more by the day.
                // Oh btw adding a debug log here also fixes the problem, God knows why, I don't. There ain't no race conditions in this code, so wtf? It's a fucking
                // simple single threaded if statement, why tho??
                // Can this PLEASE be the last fucking hack on this game?
                float aux = this.repeatTime;
                if (this.accumulatedTime >= aux)
                {
                    Apply(other.gameObject);
                    this.accumulatedTime = 0.0f;
                }
                break;
            default:
                break;
        }
    }

    // This one is an exact copy of OnTriggerEnter, it only exists to support spells with a collider that has a rigid body and is not a trigger.
    // Altough, it is adviced that spell damage areas / health modifiers be placed as components on a slightly bigger trigger area that wraps around
    // the physics collider for the rigid body. Still, this has been implemented to allow things to work, even if it is not the best approach.
    void OnCollisionEnter(Collision collision)
    {
        if (!this.collisionEnabled)
            return;
        if (this.type == Type.Instant)
            Apply(collision.gameObject);
    }

    #endregion
}
