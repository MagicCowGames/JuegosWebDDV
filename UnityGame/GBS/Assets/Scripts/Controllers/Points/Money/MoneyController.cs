using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This class would make more sense in multiplayer since each player would hold their own score. Currently it goes unused since we just hook into the
// GameManager's money and score variables directly and fuck right off...

// And the dumbest class award goes to... Basically this is just a controller script that acts as a "pouch", so that we can use get component to add money to
// basically any entity at all that can pick up money, including the player.
public class MoneyController : MonoBehaviour
{
    #region Variables

    [SerializeField] private int money;

    public int Money { get { return this.money; } set{ this.money = value; } }

    #endregion
}
