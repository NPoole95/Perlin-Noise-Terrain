using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject chunkPrefab;
    [SerializeField]
    //GameObject allChunks;
    PerlinNoiseGenerator noise;
    [SerializeField]
    playerController player;
    Vector3 playerPos; // players position in world space
    int playerChunkPosX; // the chunk the player is currently in
    int playerChunkPosY; // the chunk the player is currently in  

    const int mapWidth = 64;
    const int mapHeight = 32;
    const int mapDepth = 64;

    int minX;// = -mapWidth / 2;
    int maxX;// =  mapWidth / 2;
    int minY;// = -mapHeight / 2;
    int maxY;// =  mapHeight / 2;
    int minZ;// = -mapDepth / 2;
    int maxZ;// =  mapDepth / 2;

    int spawnDistance;
    int despawnDistance;

    int numSideBlocks;
    int[,] chunkGrid;
    const int StartingWorldSize = 1; // in chunks

    public struct IntVector2
    {
        public int x;
        public int y;
        public IntVector2(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    public List<IntVector2> chunkList; // a list that stores the coordinates of every chunk

    void Awake()
    {
        noise = new PerlinNoiseGenerator(UnityEngine.Random.Range(1000000, 10000000));
        chunkList = new List<IntVector2>();
        numSideBlocks = noise.chunkSize / noise.blockSize;
        chunkGrid = new int[numSideBlocks, numSideBlocks];

        minX = -noise.chunkSize / noise.blockSize / 2;
        maxX = noise.chunkSize / noise.blockSize / 2;
        minY = -noise.chunkSize / noise.blockSize / 4;
        maxY = noise.chunkSize / noise.blockSize / 4;
        minZ = -noise.chunkSize / noise.blockSize / 2;
        maxZ = noise.chunkSize / noise.blockSize / 2;

        spawnDistance = noise.chunkSize * 2;
        despawnDistance = noise.chunkSize * 3;

        //CreateChunks();
    }

    private void Update()
    {
        //handle the despawning of chunks that are out of range
        playerPos = player.GetPosition();
        playerChunkPosX = (int)Math.Floor(playerPos.x / noise.chunkSize);
        playerChunkPosY = (int)Math.Floor(playerPos.z / noise.chunkSize);

        for (int i = 0; i < chunkList.Count; i++)
        {
            int tempx = (int)(chunkList[i].x * noise.chunkSize) - (noise.chunkSize / 2);
            int tempy = (int)(chunkList[i].y * noise.chunkSize) - (noise.chunkSize / 2);
            if (inDespawnRange(tempx, tempy))
            {               
                DestroyChunk(chunkList[i]);
            }
        }

        LoadChunk(playerPos);

    }

    void Regenerate(int chunkX, int chunkY) // the physical chunk spawning method
    {
        float width = 1;// dirtPrefab.transform.lossyScale.x;
        float height = 1;// dirtPrefab.transform.lossyScale.y;
        float depth = 1;// dirtPrefab.transform.lossyScale.z;

        GameObject chunk = chunkPrefab;
        var chunkParent = Instantiate(chunk, new Vector3(chunkX, 0, chunkY), Quaternion.identity);
        chunkParent.name = "chunk " + chunkX + chunkY;

        for (int x = minX + (chunkX * noise.chunkSize); x < maxX + (chunkX * noise.chunkSize); x++)
        {
            for (int z = minZ + (chunkY * noise.chunkSize); z < maxZ + (chunkY * noise.chunkSize); z++)
            {
                int columnHeight = 2 + noise.GetNoise(x - minX, z - minZ, maxY - minY - 2);
                for (int y = minY; y < minY + columnHeight; y++)
                {
                    GameObject block = (y == minY + columnHeight - 1) ? grassPrefab : dirtPrefab;
                    var newBlock = Instantiate(block, new Vector3(x * width, y * height, z * depth), Quaternion.identity, chunkParent.transform);
                }
            }
        }
    }

    // Degeneration 
    bool inDespawnRange(int chunkX, int chunkZ)
    {
        float distance = Mathf.Sqrt(((playerPos.x - chunkX) * (playerPos.x - chunkX)) + (playerPos.z - chunkZ) * (playerPos.z - chunkZ));
        //if(chunkX == 0 && chunkZ == 144)
        //{ 
        //   Debug.Log("destroy X: " + chunkX + " Y: " + chunkZ + " Distance: " + distance);
        //}
        //if (distance > despawnDistance)
        //{

        //    Debug.Log("destroy X: " + chunkX + " Y: " + chunkZ + " Distance: " + distance);
        //}

        return distance > despawnDistance;
    }

    bool inSpawnRange(int chunkX, int chunkZ)
    {
        float distance = Mathf.Sqrt(((playerPos.x - chunkX) * (playerPos.x - chunkX)) + (playerPos.z - chunkZ) * (playerPos.z - chunkZ));

        return distance < spawnDistance;
    }

    void DestroyChunk(IntVector2 chunkPosition)
    {
        string chunkName = "chunk " + chunkPosition.x + chunkPosition.y;
        GameObject chunk = GameObject.Find(chunkName);
        // no longer destroy chunks, insteadd just disable them.
        //Destroy(chunk);
        //chunkList.Remove(chunkPosition);
        Debug.Log(chunkName);
        foreach (Transform child in chunk.transform)
        {
            child.gameObject.SetActive(false);
        }
        //chunk.SetActive(false);
    }

    // loading more chunks
    void LoadChunk(Vector3 playerPos)
    {
        var chunkRadius = Math.Ceiling(spawnDistance / (double)noise.chunkSize);
        for (var x = playerChunkPosX - chunkRadius; x <= playerChunkPosX + chunkRadius; x++)
        {
            for (var y = playerChunkPosY - chunkRadius; y <= playerChunkPosY + chunkRadius; y++)
            {
                int tempx = (int)(x * noise.chunkSize) - (noise.chunkSize / 2);
                int tempy = (int)(y * noise.chunkSize) - (noise.chunkSize / 2);
                if (!chunkList.Contains(new IntVector2((int)x, (int)y)) && inSpawnRange(tempx, tempy))
                {
                    Regenerate((int)x, (int)y);
                    chunkList.Add(new IntVector2((int)x, (int)y));
                }
                else if(chunkList.Contains(new IntVector2((int)x, (int)y)) && inSpawnRange(tempx, tempy)) // this is a problem, constantly enabling every block even if its already enabled
                {
                    string chunkName = "chunk " + (int)x + (int)y;
                    GameObject chunk = GameObject.Find(chunkName);
                    foreach (Transform child in chunk.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    //chunk.SetActive(true);
                }
            }
        }


    }
}

