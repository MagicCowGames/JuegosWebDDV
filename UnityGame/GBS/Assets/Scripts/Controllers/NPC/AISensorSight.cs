using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensorSight : AISensorBase
{
    protected override void Sense(GameObject obj)
    {
        // Only process the sight hit if the player is within the sight bounding area.
        var player = obj.GetComponent<PlayerController>();
        if (player == null)
            return;

        // Try to see if there's a direct line of sight to the player. If the hit is true and the hit target is not the player, then we quit.
        RaycastHit hit;
        bool hasHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit);
        if (hasHit && hit.collider.gameObject.GetComponent<PlayerController>() != null)
            this.OnSense(obj);
    }
}
