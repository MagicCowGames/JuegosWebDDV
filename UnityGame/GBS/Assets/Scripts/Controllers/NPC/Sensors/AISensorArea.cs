using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensorArea : AISensorBase
{
    protected override void Sense(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        if (player == null)
            return;
        this.OnSense(obj);
    }
}
