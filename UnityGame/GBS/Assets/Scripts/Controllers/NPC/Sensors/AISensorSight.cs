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
        Vector3 origin = this.transform.position;
        Vector3 direction = obj.transform.position - this.transform.position;
        bool hasHit = Physics.Raycast(origin, direction, out hit);
        if (hasHit && obj != hit.collider.gameObject) // This check is kinda crap actually since it's always going to hit something... or it should at least!
            return;
        
        this.OnSense(obj);
        DebugManager.Instance?.DrawLine(this.transform.position, hit.point, Color.red); // Debug sight line trace.
    }
}
