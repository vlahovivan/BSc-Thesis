using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositioner : MonoBehaviour
{
    public GeneratableObjectParams[] objects;
    public int objectsPerFrame = 100;

    private float[,] terrainNoiseMap;
    private float meshHeightMultiplier;
    private float[,] objectNoiseMap;
    private bool mapInitiated = false;

    private GameObject generatedObjects;

    private int currentX = 2;
    private int currentY = 2;
    private int mapWidth;
    private int mapHeight;
    private HashSet<string> objectsToGenerate = new HashSet<string>();
    

    private void Update() {        
        for(int rep=0; rep < objectsPerFrame; rep++) {
            if(currentX < mapWidth - 2 && currentY < mapHeight - 2) {
                GenerateOne(currentX, currentY);

                currentX += 2;
                if(currentX >= mapWidth - 2) {
                    currentX = 2;
                    currentY += 2;
                }
            }
        }
    }
    public void GenerateAll(ref float[,] noiseMap, float heightMultiplier) {

        terrainNoiseMap = noiseMap;
        meshHeightMultiplier = heightMultiplier;
        mapWidth = terrainNoiseMap.GetLength(0);
        mapHeight = terrainNoiseMap.GetLength(1);

        if(!mapInitiated) {
            objectNoiseMap = new float[mapWidth, mapHeight];
            mapInitiated = true;
        }

        Destroy(generatedObjects);
        generatedObjects = new GameObject("Generated Objects");

        Noise.GenerateNoiseMap(ref objectNoiseMap, mapWidth, mapHeight, UnityEngine.Random.Range(0, 10000), 80.0f, 1, 1.0f, 1.0f, Vector2.zero, false);
        
        objectsToGenerate.Clear();
        
        foreach(GeneratableObjectParams obj in objects) {
            if(!obj.active) continue;
                
            GameObject baseObject = new GameObject(obj.generator.GetType().Name + " objects");
            baseObject.transform.parent = generatedObjects.transform;
            baseObject.tag = obj.generator.GetType().Name;

            objectsToGenerate.Add(baseObject.tag);
        }

        currentX = 2;
        currentY = 2;

    }

    public void GenerateOne(int x, int y) {
        float topLeftX = (mapWidth - 1) / -2f;
        float topLeftZ = (mapHeight - 1) / 2f;

        foreach(GeneratableObjectParams obj in objects) {
            if(!objectsToGenerate.Contains(obj.generator.GetType().Name)) continue;

            if(terrainNoiseMap[x, y] < obj.lowerHeightThreshold || terrainNoiseMap[x, y] > obj.upperHeightThreshold) continue;
            
            GameObject baseObject = GameObject.FindWithTag(obj.generator.GetType().Name);

            if(UnityEngine.Random.Range(0.0f, 1.0f) < obj.probabilityMultiplier * objectNoiseMap[x, y]) {
                float xOff = UnityEngine.Random.Range(-1/(4*(float)mapWidth), 1/(4*(float)mapWidth));
                float yOff = UnityEngine.Random.Range(-1/(4*(float)mapHeight), 1/(4*(float)mapHeight));
                GameObject instance = obj.generator.Generate(new Vector3(topLeftX + x + xOff, terrainNoiseMap[x, y] * meshHeightMultiplier, topLeftZ - y + yOff));
                instance.transform.parent = baseObject.transform;

                return;
            }
            
        }
    }
}

[System.Serializable]
public struct GeneratableObjectParams
{
    public bool active;
    public GeneratorBase generator;
    public float lowerHeightThreshold;
    public float upperHeightThreshold;
    public float probabilityMultiplier;

    public float noiseScale;
    public int seed;
}
