using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This should have been named MoneyPickupController or some shit tbh.
// NOTE : In the future, this should maybe inherit from a PickUp class, and the "pouch" class should inherit from a "Picker" class or some shit like that.
[ExecuteInEditMode]
public class CoinController : MonoBehaviour
{
    #region StaticData

    public static int[] CoinTypeValues = {
        1,
        5,
        10,
        20
    };

    #endregion

    #region Enums

    public enum CoinType
    {
        Copper = 0,
        Silver,
        Gold,
        Magic
    }

    public enum ValueType
    {
        Automatic = 0,
        Manual
    }

    #endregion

    #region Variables

    [Header("Coin Components")]
    [SerializeField] private SphereCollider coinCollider;
    [SerializeField] private Mesh coinMesh; // TODO : Fix this shit with mesh filter and all that jazz...
    [SerializeField] private AudioClip coinPickUpClip; // TODO : This would be a static, but static variables cannot be edited from the inspector, so in the future, make a fucking SoundManager or AudioClipManager or whatever the fuck and store all clips there.

    [Header("Coin Configuration")]
    [SerializeField] private CoinType coinType;
    [SerializeField] private ValueType valueType;
    [SerializeField] private int valueBase;
    [SerializeField] private int valueTotal;

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
        switch (this.valueType)
        {
            case ValueType.Automatic:
                this.valueTotal = this.valueBase * CoinTypeValues[(int)this.coinType];
                break;
            case ValueType.Manual:
                // Do nothing lol.
                break;
        }

        // TODO : Change visual mesh and material for the selected coint type, etc... for now it only changes the visual material and calls it a day. Need to pick
        // random money pile mesh, etc...
        // TODO : Change this shit to a simple set translating the fucking coin type to an integer and picking the value from an array
        switch (this.coinType)
        {
            default:
            case CoinType.Copper:
                break;
            case CoinType.Silver:
                break;
            case CoinType.Gold:
                break;
        }
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    // TODO : Make this into a function in that one PickUp parent class you said you'd implement in the future, ok?
    // Also maybe make an EnablePickup() method and that way we can just not despawn the pickups and we can reuse them? idk,
    // whatever, that's a problem for future me to deal with.
    private void DisablePickup()
    {
        Destroy(this.gameObject); // Maybe do some pooling or some shit in the future?
    }

    #endregion

    #region Collisions

    // PickUp logic goes here... should probably encapsulate this and make this controller inherit from some base PickUp controller of sorts.
    // It would either check for whatever specific "container" component it requires for its item type (could be a PickUp<T> thing maybe)
    // or it would just check for a "Picker" controller, or whatever, or maybe have a Picker/ItemContainer base controller and the Money and Potions ones
    // derive from it and we just call the base one and add the value and let each one write its own impl... etc etc... for now, this will suffice.
    void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;
        var money = other.GetComponent<MoneyController>();
        if (money == null)
            return;
        money.Money += this.valueTotal;

        SoundManager.Instance?.PlaySoundSFX("PickCoins");
        
        DisablePickup();
    }

    #endregion
}
