using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Mesh coinMesh;
    [SerializeField] private CoinType coinType;
    [SerializeField] private ValueType valueType;
    [SerializeField] private int valueBase;
    [SerializeField] private int valueTotal;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Debug.Log("DOES THIS WORK!");
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

        // TODO : Change visual mesh and material for the selected coint type, etc...
        /*
        switch (this.coinType)
        {

        }
        */
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
