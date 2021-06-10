using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MapGenerator mapGenerator;

    public GameObject canvas;

    public Slider octaves;
    public Slider persistence;
    public InputField lacunarity;
    public InputField seed;
    public Toggle trees;
    public Toggle seaweed;
    public Slider objectsPerFrame;

    private void Start() {
        octaves.value = mapGenerator.octaves;
        persistence.value = mapGenerator.persistence;
        lacunarity.SetTextWithoutNotify(mapGenerator.lacunarity.ToString());
        seed.SetTextWithoutNotify(mapGenerator.seed.ToString());
        trees.isOn = mapGenerator.objectPositioner.objects[0].active;
        seaweed.isOn = mapGenerator.objectPositioner.objects[1].active;

        Debug.Log(mapGenerator.objectPositioner.objectsPerFrame);
        objectsPerFrame.value = mapGenerator.objectPositioner.objectsPerFrame;
        canvas.SetActive(false);
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.Space)) {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    public void OctavesChanged() {
        // mapGenerator.octaves = (int)octaves.value;
    }

    public void PersistenceChanged() {
        // mapGenerator.persistence = persistence.value;

    }

    public void LacunarityChanged() {
        // mapGenerator.lacunarity = float.Parse(lacunarity.text);
    }

    public void SeedChanged() {
        // mapGenerator.seed = int.Parse(seed.text);
    }

    public void RandomSeed() {
        seed.SetTextWithoutNotify(Random.Range(0, 100000).ToString());
        SeedChanged();
    }

    public void TreesChanged() {
        // mapGenerator.objectPositioner.objects[0].active = trees.isOn;
    }

    public void SeaweedChanged() {
        // mapGenerator.objectPositioner.objects[1].active = seaweed.isOn;
    }

    public void GenerateMap() {
        mapGenerator.octaves = (int)octaves.value;
        mapGenerator.persistence = persistence.value;
        mapGenerator.lacunarity = float.Parse(lacunarity.text);
        mapGenerator.seed = int.Parse(seed.text);
        mapGenerator.objectPositioner.objects[0].active = trees.isOn;
        mapGenerator.objectPositioner.objects[1].active = seaweed.isOn;
        mapGenerator.objectPositioner.objectsPerFrame = (int)objectsPerFrame.value;

        mapGenerator.GenerateMap();
        mapGenerator.GenerateObjects();
    }
}
