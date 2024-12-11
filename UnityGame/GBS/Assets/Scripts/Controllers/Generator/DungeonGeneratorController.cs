using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class DungeonGeneratorController : MonoBehaviour
{
    #region Structs

    [System.Serializable]
    public struct RoomData
    {
        public GameObject[] tiles;
        public int maxRoomSizeX;
        public int maxRoomSizeY;

        public RoomData(GameObject[] tiles, int sizeX = 1, int sizeY = 1)
        {
            this.tiles = tiles;
            this.maxRoomSizeX = sizeX;
            this.maxRoomSizeY = sizeY;
        }
    }

    public struct PointInt
    {
        int x;
        int y;
        public PointInt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    #endregion

    #region Variables

    [Header("Components")]
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private Transform spawnTransform;

    [Header("Room Types")]
    [SerializeField] private RoomData[] roomData;

    [Header("World Data")]
    [SerializeField] private int worldSizeX = 10;
    [SerializeField] private int worldSizeY = 10;

    [Header("Spawn Settings")]
    [SerializeField] private int roomsToSpawn = 3;

    private readonly float tileSize = 30;
    private int[] tiles;
    private List<PointInt> roomCoordinates;
    // private List<GameObject> rooms; // We could store the rooms we spawned and use that data later on for something, I guess.

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {
        int totalTiles = this.worldSizeX * this.worldSizeY;
        this.tiles = new int[totalTiles];
        for (int i = 0; i < tiles.Length; ++i)
            tiles[i] = -1;
        // this.rooms = new GameObject[totalRooms];
        this.roomCoordinates = new List<PointInt>();
        GenerateDungeon();
        GenerateNavMesh();
    }

    private void GenerateDungeon()
    {
        for (int i = 0; i < this.roomsToSpawn; ++i)
        {
            int type = Random.Range(0, this.roomData.Length);
            int x = Random.Range(0, this.worldSizeX);
            int y = Random.Range(0, this.worldSizeY);
            SpawnRoom(0, x, y);
        }
        // SpawnRoom(0, 0, 0);
        // SpawnRoom(0, 10, 0);
        InstantiateRooms();
    }

    #endregion

    #region PrivateMethods - Tiles

    private GameObject GetRandomTile(GameObject[] tiles)
    {
        return tiles[Random.Range(0, tiles.Length)];
    }

    private GameObject SpawnTile(GameObject[] tiles, Vector3 spawnPosition)
    {
        var obj = ObjectSpawner.Spawn(GetRandomTile(tiles), this.spawnTransform.position + spawnPosition);
        obj.gameObject.transform.parent = this.spawnTransform; // attach the spawned room to the spawn transform.
        return obj;
    }

    private RoomController SpawnTile(int x, int y, int roomType)
    {
        RoomController ans;
        var obj = SpawnTile(this.roomData[roomType].tiles, new Vector3(x * this.tileSize, 0, y * this.tileSize));
        ans = obj.GetComponent<RoomController>();
        return ans;
    }

    private int GetTileIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= this.worldSizeX || y >= this.worldSizeY)
            return -1;
        int globalIndex = y + x * this.worldSizeY;
        return globalIndex;
    }

    private int GetAdjacentTileIndex(int x, int y, Direction direction)
    {
        switch (direction)
        {
            default:
                return GetTileIndex(x, y);
            case Direction.Up:
                return GetTileIndex(x, y + 1);
            case Direction.Right:
                return GetTileIndex(x + 1, y);
            case Direction.Down:
                return GetTileIndex(x, y - 1);
            case Direction.Left:
                return GetTileIndex(x - 1, y);
        }
    }

    private void SetTile(int x, int y, int type)
    {
        int idx = GetTileIndex(x, y);
        if (idx < 0)
            return;
        this.tiles[idx] = type;
    }
    private int GetTile(int x, int y)
    {
        int idx = GetTileIndex(x, y);
        if (idx < 0)
            return -1;
        return this.tiles[idx];
    }
    private bool IsTileSet(int x, int y)
    {
        int idx = GetTileIndex(x, y);
        if(idx < 0)
            return false;
        return this.tiles[idx] >= 0;
    }
    private bool IsAdjacentTileSet(int x, int y, Direction direction)
    {
        int idx = GetAdjacentTileIndex(x, y, direction);
        if (idx < 0)
            return false;
        return this.tiles[idx] >= 0;
    }

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

    /*
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
                int globalIndex = GetGlobalIndex(i, j);

                int uidx = GetAdjacentRoomIndex(i, j, Direction.Up);
                int ridx = GetAdjacentRoomIndex(i, j, Direction.Right);
                int didx = GetAdjacentRoomIndex(i, j, Direction.Down);
                int lidx = GetAdjacentRoomIndex(i, j, Direction.Left);

                bool u = uidx < 0 ? false : this.spawnRooms[uidx];
                bool r = ridx < 0 ? false : this.spawnRooms[ridx];
                bool d = didx < 0 ? false : this.spawnRooms[didx];
                bool l = lidx < 0 ? false : this.spawnRooms[lidx];

                bool check = !u && !r && !d && !l;

                if (this.spawnRooms[globalIndex] && check)
                    this.spawnRooms[globalIndex] = false;
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
                        if (idx >= 0 && spawnRooms[idx])
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
    }
    */

    // A room is formed by a group of tiles
    private void SpawnRoom(int roomType, int startX, int startY)
    {
        var data = this.roomData[roomType];
        SpawnRoom(roomType, startX, startY, data.maxRoomSizeX, data.maxRoomSizeY);
    }

    private void SpawnRoom(int roomType, int startX, int startY, int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; ++i)
        {
            for (int j = 0; j < sizeY; ++j)
            {
                SetTile(startX + i, startY + j, roomType);
            }
        }

        int middlePointX = Mathf.Clamp(startX + sizeX / 2, 0, this.worldSizeX - 1);
        int middlePointY = Mathf.Clamp(startY + sizeY / 2, 0, this.worldSizeY - 1);

        this.roomCoordinates.Add(new PointInt(middlePointX, middlePointY));
    }

    // path type literally just indicates the room type index to be used when generating the path.
    private void ConnectRooms(int pathType, int roomA, int roomB)
    {

    }

    private void InstantiateRooms()
    {
        for (int i = 0; i < this.worldSizeX; ++i)
        {
            for (int j = 0; j < this.worldSizeY; ++j)
            {
                if (IsTileSet(i, j))
                {
                    var room = SpawnTile(i, j, GetTile(i, j));

                    Direction[] directions = {
                        Direction.Up,
                        Direction.Right,
                        Direction.Down,
                        Direction.Left
                    };

                    foreach (var direction in directions)
                    {
                        if (IsAdjacentTileSet(i, j, direction))
                        {
                            room.RemoveWall(direction);
                        }
                    }
                }
            }
        }
    }


    #endregion

    #region PrivateMethods - Nav Mesh

    private void GenerateNavMesh()
    {
        this.navMesh?.BuildNavMesh();
    }

    #endregion
}
