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
        OverTime
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

    [Header("Modifier Configuration")]
    [SerializeField] private Type type;
    [SerializeField] private bool collisionEnabled;

    [Header("Element Modifier Values")]
    [SerializeField] private bool useDefaultValues; // determines if this is a prefabricated damage area and it uses the default values located within the inspector panel in the Unity Editor.
    [SerializeField] private bool resetValues;
    [SerializeField] private InputDamageArray inputDamageArray; // Can't use default constructor for serializable structs for some reason. Ok Unity, you win...

    int[] elementCounts;

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
        if (this.type == Type.OverTime)
            Apply(other.gameObject, Time.deltaTime);
    }

    #endregion
}
