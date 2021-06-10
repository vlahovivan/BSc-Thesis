using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static void GenerateNoiseMap(ref float[,] noiseMap, int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistence, float lacunarity, Vector2 offset, bool useFalloff) {
        // float[,] noiseMap = new float[mapWidth,mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY); 
        }

        if(noiseScale <= 0) {
            noiseScale = 0.001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;
                        
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }
                if(noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                float falloff = 0.0f;

                if(useFalloff) falloff = Mathf.Max(Noise.valley((x-mapWidth/2)/(float)(mapWidth/2)), Noise.valley((y-mapHeight/2)/(float)(mapHeight/2)));

                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloff);
            }
        }

        // return noiseMap;
    }

    public static float valley(float t) {
        float a = 2.0f;
        float b = 2.0f;
        t = Mathf.Abs(t);
        return Mathf.Pow(t, a) / (Mathf.Pow(t, a) + Mathf.Pow((b - b*t), a));
    }
}
