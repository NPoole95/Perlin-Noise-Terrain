using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public long seed;
    public int blockSize = 1; // size of a single block within a chunk
    public int chunkSize = 32; // size of a chunk in blocks

    public PerlinNoiseGenerator(long seed)
    {
        this.seed = seed;
    }

    int random(long x, int range)
    {
        return (int)(((x + seed) ^ 5) % range);
    }

    int random(long x, long y, int range)
    {
        return random( x + y * 65535, range);
    }

    public int GetNoise(int x, int y, int range)
    {
        int sampleSize = 16;
        float noise = 0;
        range /= 2;

        while(sampleSize > 0)
        {
            int sampleIndexX = x / sampleSize;
            int sampleIndexY = y / sampleSize;
            
            float progX = (Math.Abs(x) % sampleSize) / (sampleSize * 1.0f); // the progression along the x axis
            float progY = (Math.Abs(y) % sampleSize) / (sampleSize * 1.0f); // the progression along the y axis

            //code to fix perlin being weird in negative numbers
            if(x < 0)
            {
                progX = 1 - progX; 
                sampleIndexX -= 1;
            }
            if (y < 0)
            {
                progY = 1 - progY;
                sampleIndexY -= 1;
            }
            //

            float topLeftRandom     = random(sampleIndexX, sampleIndexY, range);
            float bottomLeftRandom  = random(sampleIndexX, sampleIndexY + 1, range);
            float topRightRandom    = random(sampleIndexX + 1, sampleIndexY, range);
            float bottomRightRandom = random(sampleIndexX + 1, sampleIndexY + 1, range);

            float leftRandom = lerp(topLeftRandom, bottomLeftRandom, progY);
            float rightRandom = lerp(topRightRandom, bottomRightRandom, progY);

            noise += lerp(leftRandom, rightRandom, progX); // uses linear interpolation to create the noise value

            sampleSize /= 2;
            range /= 2;

            range = Mathf.Max(1, range);
        }
        return (int)Mathf.Round(noise);
    }

    float lerp (float left, float right, float prog)
    {
        return left * (1 - prog) + right * prog;
    }
}
