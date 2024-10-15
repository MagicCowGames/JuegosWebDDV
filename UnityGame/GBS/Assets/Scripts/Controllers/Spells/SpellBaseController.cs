using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Implement
public class SpellBaseController : MonoBehaviour
{
    #region Variables

    [SerializeField] private int[] ElementsCounts; // Array that holds the count of each element type.

    public Color SpellColor { get; private set; }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        CalculateSpellColor();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void CalculateSpellColor()
    {
        this.SpellColor = Color.white;
        // TODO : Implement color calculation, maybe with something like average color of the mix of all the elements used for the spell?
    }

    #endregion
}
