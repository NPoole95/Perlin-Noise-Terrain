using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGenerator PNGenerator;
    [SerializeField]
    ChunkDespawner despawner;

    int numSideBlocks;
    int[,] chunkGrid;
    const int MaxWorldSize = 1; // in chunks

    // Start is called before the first frame update
    void Awake()
    {
        numSideBlocks = PNGenerator.chunkSize / PNGenerator.blockSize;
        chunkGrid = new int[numSideBlocks,numSideBlocks];

    }
    private void Update()
    {
        
    }

    // Update is called once per frame
    void CreateChunks()
    {
        for(int i = 0; i < MaxWorldSize; i += PNGenerator.chunkSize)
        {
            for (int j = 0; j < MaxWorldSize; j += PNGenerator.chunkSize)
            {

            }
        }
    }

}
