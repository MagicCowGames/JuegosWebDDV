using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trashy script, should be replaced with some generic entity spawner...
// like an ObjectSpawnerController that works for using UnityEvents to trigger the internal static ObjectSpawner calls.
public class NPCSpawnerController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] private Transform[] spawnTransforms;

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

    public void SpawnDummy(Transform spawnTransform)
    {
        var obj = ObjectSpawner.Spawn(this.dummyPrefab, spawnTransform);
        var dummy = obj.GetComponent<NPCController>();
        dummy.CanDie = true;
    }

    public void SpawnDummies()
    {
        foreach (var transform in this.spawnTransforms)
            SpawnDummy(transform);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
