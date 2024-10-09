using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectSpawner
{
    public static GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
    {
        GameObject spawn = null;
        spawn = GameObject.Instantiate(prefab, position, rotation);
        return spawn;
    }

    public static GameObject Spawn(GameObject prefab, Transform transform)
    {
        return Spawn(prefab, transform.position, transform.rotation);
    }
}
