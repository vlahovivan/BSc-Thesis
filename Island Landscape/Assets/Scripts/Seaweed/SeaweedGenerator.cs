using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaweedGenerator : GeneratorBase
{
    public GameObject seaweedPrefab;

    public float squareSize = 2.5f;

    public float minScale = 0.75f;
    public float maxScale = 1.33f;

    public int minNumberOfSeaweeds = 30;
    public int maxNumberOfSeaweeds = 50;

    public override GameObject Generate(Vector3 position) {
        GameObject seaweedCluster = new GameObject("Seaweed");

        int numOfSeaweeds = Random.Range(minNumberOfSeaweeds, maxNumberOfSeaweeds);

        for(int rep=0; rep < numOfSeaweeds; rep++) {
            Vector3 seaweedPosition = new Vector3(Random.Range(-squareSize/2.0f, squareSize/2.0f), 0.0f, Random.Range(-squareSize/2.0f, squareSize/2.0f));

            GameObject seaweed = Instantiate(seaweedPrefab, seaweedPosition, Quaternion.identity);

            Mesh mesh = seaweed.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;

            float xAmplitude = Random.Range(0.0f, 0.002f);
            float xFrequency = Random.Range(0.5f, 2.0f);
            
            float zAmplitude = Random.Range(0.0f, 0.002f);
            float zFrequency = Random.Range(0.5f, 2.0f);

            float magicNumber = 1000.0f / 28.8f;

            for (int i = 0; i < vertices.Length; i++) {
                vertices[i].x += xAmplitude * Mathf.Sin(6.28f * xFrequency * vertices[i].y * magicNumber);
                vertices[i].z += zAmplitude * Mathf.Sin(6.28f * zFrequency * vertices[i].y * magicNumber);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();

            float scale = Random.Range(minScale, maxScale);
            seaweed.transform.localScale *= scale;

            seaweed.transform.parent = seaweedCluster.transform;

        }

        seaweedCluster.transform.localPosition = position;


        return seaweedCluster;
    }
}
