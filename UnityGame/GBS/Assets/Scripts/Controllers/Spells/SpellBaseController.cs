using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Implement
public class SpellBaseController : MonoBehaviour, ISpell
{
    #region Variables

    [Header("Basic Spell Settings")]
    [SerializeField] protected int[] elementsCounts; // Array that holds the count of each element type.
    [SerializeField] protected Color spellColor;
    [SerializeField] protected HealthModifierController healthModifierController;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion

    #region ISpell

    public virtual void UpdateSpellColor()
    {

    }

    public Color GetSpellColor()
    {
        return this.spellColor;
    }

    public void SetSpellColor(Color color)
    {
        this.spellColor = color;
        UpdateSpellColor();
    }

    // This is the one that should be used by the public interface when spawning spells from code.
    public void SetSpellData(ElementQueue queue)
    {
        Color color = Color.white;
        float r = 0.0f;
        float g = 0.0f;
        float b = 0.0f;

        this.elementsCounts = new int[(int)Element.COUNT];
        for(int i = 0; i < queue.ElementsCounts.Length; ++i)
        {
            this.elementsCounts[i] = queue.ElementsCounts[i];
            Element currentElement = (Element)i;
            Color currentColor = MagicManager.Instance.GetElementColor(currentElement);
            var cr = (currentColor.r / queue.Count) * queue.ElementsCounts[i];
            var cg = (currentColor.g / queue.Count) * queue.ElementsCounts[i];
            var cb = (currentColor.b / queue.Count) * queue.ElementsCounts[i];
            r += cr;
            g += cg;
            b += cb;
        }
        this.healthModifierController?.SetValues(this.elementsCounts);

        Color colorAns = new Color(r, g, b, 1.0f);
        SetSpellColor(colorAns);
    }

    #endregion
}
