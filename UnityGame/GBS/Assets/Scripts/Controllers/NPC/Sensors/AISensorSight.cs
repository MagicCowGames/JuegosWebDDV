using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensorSight : AISensorBase
{
    protected override void Sense(GameObject obj, float delta, float distance)
    {
        // Debug color for line trace
        Color debugColor;

        // Only process the sight hit if the player is within the sight bounding area.
        var player = obj.GetComponent<PlayerController>();
        if (player == null)
            return;

        // Try to see if there's a direct line of sight to the player. If the hit is true and the hit target is not the player, then we quit.
        RaycastHit hit;
        Vector3 target = obj.transform.position + new Vector3(0.0f, 3.0f, 0.0f); // This is a fucking hack that exists to overcome the fact that the character's origin is on its feet... which would make the fucking raycast hit the ground and never detect the entity...
        Vector3 origin = this.originTransform.position;
        Vector3 direction = (target - origin).normalized;

        bool hasHit = Physics.Raycast(origin, direction, out hit);
        if (!hasHit) // If for whatever reason the hit fails (which should never happen if we have detected the player...), then we bail.
            return;

        if (Vector3.Distance(hit.point, target) < 1.0f) // If the distance between the impact point and the target's location is small enough, we can see them.
        {
            // Case : Entity Detected
            this.OnSense(obj, delta * (1.0f / distance) * this.detectionAmount);
            debugColor = Color.green;
        }
        else
        {
            // Case : No Entity Detected
            debugColor = Color.red;
        }

        DebugManager.Instance?.DrawLine(origin, target, debugColor); // Debug sight line trace.
        DebugManager.Instance?.DrawSphere(hit.point, 2.0f, debugColor);
    }
}
