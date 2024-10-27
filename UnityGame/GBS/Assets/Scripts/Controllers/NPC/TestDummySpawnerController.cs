using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trashy script, should be replaced with some generic entity spawner...
// like an ObjectSpawnerController that works for using UnityEvents to trigger the internal static ObjectSpawner calls.
public class TestDummySpawnerController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] private Transform spawnTransform;

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

    public void SpawnDummy()
    {
        var obj = ObjectSpawner.Spawn(this.dummyPrefab, this.spawnTransform);
        var dummy = obj.GetComponent<TestDummyController>();
        dummy.CanDie = true;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
