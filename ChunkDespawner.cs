using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDespawner : MonoBehaviour
{
    [SerializeField]
    playerController player;
    [SerializeField]
    PerlinNoiseGenerator PNGenerator;
    [SerializeField]
    TerrainGenerator terrainGenerator;

    Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.GetPosition();
        for (int i = 0; i < terrainGenerator.chunkList.Count; i++)
        {
            if (inDespawnRange((int)playerPos.x + PNGenerator.chunkSize / 2, (int)playerPos.z + PNGenerator.chunkSize / 2, (int)terrainGenerator.chunkList[i].x, (int)terrainGenerator.chunkList[i].y))
            {
                //instance_destroy();
            }

        }
    }

    bool inDespawnRange(int playerX, int playerZ, int chunkX, int chunkZ)
    {
        float distance = Mathf.Sqrt(((playerX - chunkX) * (playerX - chunkX)) + (playerZ - chunkZ) * (playerZ - chunkZ));

        return distance > (PNGenerator.chunkSize * 5);
    }
}
