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
        public float damage;
    }

    [System.Serializable]
    public struct InputDamageArray
    {
        public InputDamageElement[] damageValues;

        public InputDamageArray(float defaultDamage = 1.0f)
        {
            this.damageValues = new InputDamageElement[(int)Element.COUNT];
            for (int i = 0; i < this.damageValues.Length; ++i)
            {
                damageValues[i].element = (Element)i;
                damageValues[i].damage = defaultDamage;
            }
        }
    }

    #endregion

    #region Variables

    [Header("Modifier Configuration")]
    [SerializeField] private Type type;
    [SerializeField] private bool collisionEnabled;

    [Header("Element Modifier Values")]
    [SerializeField] private bool resetValues;
    [SerializeField] private InputDamageArray inputDamageArray = new InputDamageArray(5.0f);

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        if (this.inputDamageArray.damageValues.Length <= 0 || this.resetValues)
        {
            this.inputDamageArray = new InputDamageArray(1.0f);
            this.resetValues = false;
        }
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
