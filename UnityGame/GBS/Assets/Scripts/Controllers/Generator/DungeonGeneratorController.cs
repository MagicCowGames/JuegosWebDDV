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

    [SerializeField] private int roomsMaxX = 10;
    [SerializeField] private int roomsMaxY = 10;

    private bool[] spawnRooms;
    private GameObject[] rooms;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        int totalRooms = this.roomsMaxX * this.roomsMaxY;
        this.spawnRooms = new bool[totalRooms];
        this.rooms = new GameObject[totalRooms];

        for (int i = 0; i < this.roomsMaxX; ++i)
        {
            for (int j = 0; j < this.roomsMaxY; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;
                this.spawnRooms[globalIndex] = Random.Range(0, 2) == 1;
            }
        }

        for (int i = 0; i < this.roomsMaxX; ++i)
        {
            for (int j = 0; j < this.roomsMaxY; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;
                if (this.spawnRooms[globalIndex])
                    SpawnRandomRoom(this.smallRooms, new Vector3(i * 10, 0, j * 10));
            }
        }

        for(int i = 0; i < totalRooms; ++i)
            Debug.Log($"room[{i}] = {spawnRooms[i]}");
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

    private GameObject SpawnRandomRoom(GameObject[] rooms, Vector3 spawnPosition)
    {
        var obj = ObjectSpawner.Spawn(GetRandomRoom(rooms), this.spawnTransform.position + spawnPosition);
        return obj;
    }

    #endregion
}
