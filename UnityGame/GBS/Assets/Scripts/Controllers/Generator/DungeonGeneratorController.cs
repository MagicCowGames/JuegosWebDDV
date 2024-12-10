using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject[] smallRooms;
    [SerializeField] private GameObject[] mediumRooms;
    [SerializeField] private GameObject[] largeRooms;

    [SerializeField] private Transform spawnTransform;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        SpawnRandomRoom(this.smallRooms, this.spawnTransform);
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private GameObject GetRandomRoom(GameObject[] rooms)
    {
        return rooms[Random.Range(0, rooms.Length - 1)];
    }

    private void SpawnRandomRoom(GameObject[] rooms, Transform spawnTransform)
    {
        ObjectSpawner.Spawn(GetRandomRoom(rooms), spawnTransform);
    }

    #endregion
}
