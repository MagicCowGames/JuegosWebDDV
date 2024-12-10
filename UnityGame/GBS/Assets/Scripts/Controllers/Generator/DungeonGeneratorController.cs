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
        SpawnDungeonAlgo1();
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
        return rooms[Random.Range(0, rooms.Length)];
    }

    private GameObject SpawnRandomRoom(GameObject[] rooms, Vector3 spawnPosition)
    {
        var obj = ObjectSpawner.Spawn(GetRandomRoom(rooms), this.spawnTransform.position + spawnPosition);
        return obj;
    }

    private RoomController SpawnRoom(RoomType type, int x, int y)
    {
        RoomController ans;
        switch (type)
        {
            default:
            case RoomType.Small:
                {
                    var obj = SpawnRandomRoom(smallRooms, new Vector3(x * 30, 0, y * 30));
                    ans = obj.GetComponent<RoomController>();
                }
                break;
            case RoomType.Medium:
                {
                    var obj = SpawnRandomRoom(mediumRooms, new Vector3(x * 30, 0, y * 30));
                    ans = obj.GetComponent<RoomController>();
                }
                break;
            case RoomType.Large:
                {
                    var obj = SpawnRandomRoom(largeRooms, new Vector3(x * 30, 0, y * 30));
                    ans = obj.GetComponent<RoomController>();
                }
                break;
        }
        return ans;
    }

    private int GetGlobalIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= this.roomsMaxX || y >= this.roomsMaxY)
            return -1;

        int globalIndex = y + x * this.roomsMaxY;
        return globalIndex;
    }

    private int GetAdjacentRoomIndex(int x, int y, Direction direction)
    {
        switch (direction)
        {
            default:
                return GetGlobalIndex(x, y);
            case Direction.Up:
                return GetGlobalIndex(x, y + 1);
            case Direction.Right:
                return GetGlobalIndex(x + 1, y);
            case Direction.Down:
                return GetGlobalIndex(x, y - 1);
            case Direction.Left:
                return GetGlobalIndex(x - 1, y);
        }
    }

    /*
    private Room RandomRoom()
    {
        int x = Random.Range(0, this.roomsMaxX);
        int y = Random.Range(0, this.roomsMaxY);
        Room room = new Room(x, y);
        return room;
    }
    */

    #endregion

    #region PrivateMethods - Spawn Algorithms

    /*
    private void SpawnDungeonAlgo2()
    {
        int totalRooms = this.roomsMaxX * this.roomsMaxY;
        this.spawnRooms = new bool[totalRooms];
        this.rooms = new List<Room>();

        // Mark the start and end rooms
        this.rooms.Add(RandomRoom());
        this.rooms.Add(RandomRoom());

        // Mark 5 "important" rooms
        this.rooms.Add(RandomRoom());
        this.rooms.Add(RandomRoom());
        this.rooms.Add(RandomRoom());
        this.rooms.Add(RandomRoom());
        this.rooms.Add(RandomRoom());

        // Draw a path from start room to room1, then room2, then..., then the final room


        // Spawn all rooms

        for (int i = 0; i < this.roomsMaxX; ++i)
        {
            for (int j = 0; j < this.roomsMaxY; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;
                this.spawnRooms[globalIndex] = Random.Range(0, 2) == 1;
            }
        }

        for (int i = 1; i < this.roomsMaxX - 1; ++i)
        {
            for (int j = 1; j < this.roomsMaxY - 1; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;

                int u = this.spawnRooms[GetGlobalIndex(i, j + 1)] ? 1 : 0;
                int r = this.spawnRooms[GetGlobalIndex(i + 1, j)] ? 1 : 0;
                int d = this.spawnRooms[GetGlobalIndex(i, j - 1)] ? 1 : 0;
                int l = this.spawnRooms[GetGlobalIndex(i - 1, j)] ? 1 : 0;

                int count = u + r + d + l;

                if (!this.spawnRooms[globalIndex] && count < 3)
                    this.spawnRooms[globalIndex] = true;
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

        for (int i = 0; i < totalRooms; ++i)
            Debug.Log($"room[{i}] = {spawnRooms[i]}");
    }
    */

    private void SpawnDungeonAlgo1()
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

        for (int i = 1; i < this.roomsMaxX - 1; ++i)
        {
            for (int j = 1; j < this.roomsMaxY - 1; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;

                bool u = this.spawnRooms[GetGlobalIndex(i, j + 1)];
                bool r = this.spawnRooms[GetGlobalIndex(i + 1, j)];
                bool d = this.spawnRooms[GetGlobalIndex(i, j - 1)];
                bool l = this.spawnRooms[GetGlobalIndex(i - 1, j)];

                // int count = u + r + d + l;
                bool check = u && r && d && l;

                if (!this.spawnRooms[globalIndex] && !check)
                    this.spawnRooms[globalIndex] = true;
            }
        }

        for (int i = 0; i < this.roomsMaxX; ++i)
        {
            for (int j = 0; j < this.roomsMaxY; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;
                if (this.spawnRooms[globalIndex])
                {
                    var room = SpawnRoom(RoomType.Small, i, j);

                    room.gameObject.transform.parent = this.spawnTransform; // attach the spawned room to the spawn transform.
                    
                    Direction[] directions = {
                        Direction.Up,
                        Direction.Right,
                        Direction.Down,
                        Direction.Left
                    };

                    foreach (var direction in directions)
                    {
                        int idx = GetAdjacentRoomIndex(i, j, direction);
                        if (idx > 0 && spawnRooms[idx])
                        {
                            room.RemoveWall(direction);
                        }
                    }
                }
            }
        }
    }

    private void SpawnDungeonAlgo3()
    {
        int totalRooms = this.roomsMaxX * this.roomsMaxY;
        this.spawnRooms = new bool[totalRooms];
        this.rooms = new GameObject[totalRooms];

        int randomAmountOfRooms = Random.Range(Mathf.Min(this.roomsMaxX, this.roomsMaxY), (int)Mathf.Sqrt(totalRooms));

        for (int i = 0; i < randomAmountOfRooms; ++i)
        {
            int x = Random.Range(0, this.roomsMaxX);
            int y = Random.Range(0, this.roomsMaxY);
            SpawnRoom(RoomType.Small, x, y);
            Debug.Log("fsafsafsa");
        }

        /*
        for (int i = 0; i < this.roomsMaxX; ++i)
        {
            for (int j = 0; j < this.roomsMaxY; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;
                this.spawnRooms[globalIndex] = Random.Range(0, 2) == 1;
            }
        }

        for (int i = 1; i < this.roomsMaxX - 1; ++i)
        {
            for (int j = 1; j < this.roomsMaxY - 1; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;

                bool u = this.spawnRooms[GetGlobalIndex(i, j + 1)];
                bool r = this.spawnRooms[GetGlobalIndex(i + 1, j)];
                bool d = this.spawnRooms[GetGlobalIndex(i, j - 1)];
                bool l = this.spawnRooms[GetGlobalIndex(i - 1, j)];

                // int count = u + r + d + l;
                bool check = u && r && d && l;

                if (!this.spawnRooms[globalIndex] && !check)
                    this.spawnRooms[globalIndex] = true;
            }
        }

        for (int i = 0; i < this.roomsMaxX; ++i)
        {
            for (int j = 0; j < this.roomsMaxY; ++j)
            {
                int globalIndex = j + i * this.roomsMaxY;
                if (this.spawnRooms[globalIndex])
                {
                    var obj = SpawnRandomRoom(this.smallRooms, new Vector3(i * 30, 0, j * 30));
                    var room = obj.GetComponent<RoomController>();

                    Direction[] directions = {
                        Direction.Up,
                        Direction.Right,
                        Direction.Down,
                        Direction.Left
                    };

                    foreach (var direction in directions)
                    {
                        int idx = GetAdjacentRoomIndex(i, j, direction);
                        if (idx > 0 && spawnRooms[idx])
                        {
                            room.RemoveWall(direction);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < totalRooms; ++i)
            Debug.Log($"room[{i}] = {spawnRooms[i]}");
        */
    }

    #endregion
}
