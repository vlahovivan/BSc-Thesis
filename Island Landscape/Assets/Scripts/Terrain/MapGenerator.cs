using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;

    [Range(0.0f, 10.0f)]
    public float persistence; 
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public float meshHeightMultiplier;
    public bool autoUpdate;
    public bool generateAllObjects = false;

    public TerrainType[] regions;
    public ObjectPositioner objectPositioner;

    private float[,] noiseMap; 
    private bool mapInitiated = false; 

    private void Start() {
        noiseMap = new float[mapWidth, mapHeight];
        mapInitiated = true;

        GenerateMap();

        if(generateAllObjects) GenerateObjects();
    }

    public void GenerateMap() {
        if(!mapInitiated) {
            noiseMap = new float[mapWidth, mapHeight];
            mapInitiated = true;
        }
        Noise.GenerateNoiseMap(ref noiseMap, mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset, true);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x, y];

                for(int i = 0; i < regions.Length; i++) {
                    if(currentHeight <= regions[i].height) {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
    }

    public void GenerateObjects() {
        objectPositioner.GenerateAll(ref noiseMap, meshHeightMultiplier);
    }

    // private void OnValidate() {
    //     if(mapWidth < 1) {
    //         mapWidth = 1;
    //     }
    //     if(mapHeight < 1) {
    //         mapHeight = 1;
    //     }

    //     if(lacunarity <= 0.01f) {
    //         lacunarity = 0.01f;
    //     }

    //     if(octaves < 0) {
    //         octaves = 0;
    //     }
    // }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}
