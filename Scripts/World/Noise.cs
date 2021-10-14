using System.Collections;
using UnityEngine;

public static class Noise
{
    public static float [,] GenerateNoiseMap (int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacuranity, Vector2 offset) {
        float [,]		noiseMap = new float[mapWidth, mapHeight];
        float			sampleX = 0;
        float			sampleY = 0;
        float			perlinValue = 0;

        float			maxNoiseHeight = float.MinValue;
        float			minNoiseHeight = float.MaxValue;

        float			amplitude = 0;
        float			frequency = 0;
        float			noiseHeigth = 0;

        System.Random	prng = new System.Random (seed);
        Vector2[]		octaveOffsets = new Vector2 [octaves];
        float			offsetX = 0;
        float			offsetY = 0;

        float			halfWidth = mapWidth / 2f;
        float			halfHeight = mapHeight / 2f;

        for (int i = 0; i < octaves; i++)
		{
            offsetX = prng.Next(-100000, 100000) + offset.x;
            offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        }

        if (scale <= 0)
            scale = 0.0001f;

        for (int y = 0; y < mapHeight; y++)
		{
            for (int x = 0; x < mapWidth; x++)
			{
                amplitude = 1;
                frequency = 1;
                noiseHeigth = 0;

                for (int i = 0; i < octaves; i++)
				{
                    sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    noiseHeigth += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacuranity;
                }

                if (noiseHeigth > maxNoiseHeight)
                    maxNoiseHeight = noiseHeigth;
                else if (noiseHeigth < minNoiseHeight)
                    minNoiseHeight = noiseHeigth;
                noiseMap [x, y] = noiseHeigth;
            }
        }

        for (int y = 0; y < mapHeight; y++)
		{
            for (int x = 0; x < mapWidth; x++)
			{
                noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
            }
        }

        return noiseMap;
    }
}
