using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// NOTE : For now, only the player can trigger these. We will make this configurable in the future by adding entity controller scripts which will be gotten on trigger
// and used to determine what type of entity has collided with the trigger.
public class TriggerController : MonoBehaviour
{
    #region Variables

    [SerializeField] private UnityEvent OnTrigger;

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

    #region Collisions

    void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;
        var player = obj.GetComponent<PlayerController>();
        if (player == null)
            return;
        OnTrigger?.Invoke();
    }

    #endregion
}
