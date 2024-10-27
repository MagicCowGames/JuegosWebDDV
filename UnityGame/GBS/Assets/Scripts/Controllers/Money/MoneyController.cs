using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// And the dumbest class award goes to... Basically this is just a controller script that acts as a "pouch", so that we can use get component to add money to
// basically any entity at all that can pick up money, including the player.
public class MoneyController : MonoBehaviour
{
    #region Variables

    [SerializeField] private int money;

    public int Money { get { return this.money; } set{ this.money = value; } }

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
    #endregion

    #region PrivateMethods
    #endregion
}
