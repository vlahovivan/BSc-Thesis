using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : GeneratorBase
{
    public GameObject treeBase;
    public GameObject treeBranch;

    public float xCoordinateFactor = 6.0f;
    public float yCoordinateFactor = 1.3f;
    public int numberOfBases = 10;
    public int numberOfBranches = 6;
    public float treeScale = 0.2f;
    public bool destroyOldTrees = true;
    private GameObject currentTree;


    public override GameObject Generate(Vector3 position) {
        GameObject tree = new GameObject("Tree");
        
        float c = Random.Range(0.0f, 1.0f);

        // Tree base generation
        GameObject baseObject = new GameObject("Base");
        baseObject.transform.parent = tree.transform;

        for(int i=0; i<numberOfBases; i++) {
            float t = i / (float) numberOfBases + 1.0f / (2 * numberOfBases);
            GameObject baseInstance = Instantiate(treeBase, Vector3.up * yCoordinateFactor * i + Vector3.right * xCoordinateFactor * GetPosition(c, t), Quaternion.Euler(-90.0f + GetAngle(c, t), 90.0f, 0.0f));
            baseInstance.transform.parent = baseObject.transform;
        }

        // Tree branches generation
        GameObject branchesObject = new GameObject("Branches");
        branchesObject.transform.parent = tree.transform;

        for(int i=0; i < numberOfBranches; i++) {
            float yAngle = i * 2.0f * 360.0f / numberOfBranches + Random.Range(0.0f, 360.0f / numberOfBranches);
            float zAngle;
            float scale;

            if(i < numberOfBranches / 2) {
                zAngle = Random.Range(0.0f, 7.5f);
                scale = Random.Range(75.0f, 100.0f);
            } else {
                zAngle = Random.Range(15.0f, 30.0f);
                scale = Random.Range(45.0f, 60.0f);
            }

            GameObject branchInstance = Instantiate(treeBranch, Vector3.zero, Quaternion.Euler(0.0f, yAngle, zAngle));
            branchInstance.transform.localScale = scale * Vector3.one;
            branchInstance.transform.parent = branchesObject.transform;
        }

        float yOffset = 0.2f;
        branchesObject.transform.localPosition = new Vector3(xCoordinateFactor * GetPosition(c, 1.0f), numberOfBases * yCoordinateFactor + yOffset, 0.0f);
        branchesObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, -1.0f * GetAngle(c, 1.0f));

        float treeAngle = Random.Range(0.0f, 360.0f);
        tree.transform.eulerAngles = new Vector3(0.0f, treeAngle, 0.0f);
        tree.transform.position = position; 
        tree.transform.localScale = treeScale * Vector3.one;

        return tree;
    }

    private float GetPosition(float c, float t) {
        return c*t*t/4.0f;
    }

    private float GetAngle(float c, float t) {
        return Mathf.Rad2Deg * Mathf.Atan(c*t/2.0f);
    }

    
}
