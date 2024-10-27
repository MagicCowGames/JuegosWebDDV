using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Move some of this logic to a base NPC class or something like that so that we can more easily implement NPCs. Either that or make an NPC component.
public class TestDummyController : MonoBehaviour
{
    #region Variables

    [Header("TestDummy Components")]
    [SerializeField] private HealthController healthController;

    [Header("TestDummy Config")]
    [SerializeField] private bool canDie;
    
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
