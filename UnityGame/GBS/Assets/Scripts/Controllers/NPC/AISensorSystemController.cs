using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Possibly replace this with a regular trigger when the Entity system is implemented?
// Or keep it like this since the detection radius controller could be refactored into a more generic detection areas controller with a detection radius
// and a capsule for sight, and maybe some hearing abilities, etc...
public class AISensorController : MonoBehaviour
{
    #region Variables

    [Header("AI Controller")]
    [SerializeField] private TestDummyController aiController;

    [Header("Sensors")]
    [SerializeField] private SphereCollider radiusCollider;
    [SerializeField] private CapsuleCollider sightCollider;

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

    private void DetectEntity(GameObject obj)
    {
        // TODO : Modify system to detect any kind of entity with a team system in the future. For now, just detect the player since all NPCs will be enemies for now.
        // var entity = obj.GetComponent<Entity>();

        var player = obj.GetComponent<PlayerController>();

        if (player == null) // If the GO is not a player, then we bail and do nothing.
            return;

        this.aiController.Target = obj;
    }

    #endregion

    #region Collisions

    void OnTriggerEnter(Collider other)
    {
        DetectEntity(other.gameObject);
    }

    #endregion
}
