using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortalController : MonoBehaviour
{
    #region Variables

    private bool hasBeenActivated = false;

    #endregion

    #region Collision

    void OnTriggerEnter(Collider other)
    {
        if (this.hasBeenActivated)
            return;

        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        this.hasBeenActivated = true;
        GameManager.Instance?.FinishGame();
    }

    #endregion
}
