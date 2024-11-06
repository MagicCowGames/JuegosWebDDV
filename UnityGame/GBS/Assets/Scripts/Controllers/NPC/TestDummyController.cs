using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO : Move some of this logic to a base NPC class or something like that so that we can more easily implement NPCs. Either that or make an NPC component.
// Or maybe just have a basic set of "Entity" related things on an Entity component (like the health bar and handling all of the life stuff and canDie stuff, wet / burning status, etc...)
// And also an "NPCController" or "AIController" component to handle all of the combat and path finding related stuff.
// For now, just bundle it all together in the same place.
public class TestDummyController : MonoBehaviour
{
    #region Variables

    [Header("TestDummy Components")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private NavMeshAgent agent;

    [Header("TestDummy Config")]
    [SerializeField] private bool hasAi = false;
    [SerializeField] private bool canDie = false;
    
    public bool CanDie { get { return this.canDie; } set { this.canDie = value; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.healthController.OnDeath += HandleDeath;
    }

    void Update()
    {
        agent.destination = PlayerDataManager.Instance.GetPlayer().transform.position;
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void HandleDeath()
    {
        if (this.canDie)
            Destroy(this.gameObject);
    }

    #endregion
}
