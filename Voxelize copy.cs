using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class Voxelize : MonoBehaviour
{

    //Determine here which object to use and how many subdivisions to use.
    public int subdivisions = 9;
    public GameObject cubePrefab;
   
   
    public int numberOfCubes = 0;

        //roll a random number; if even or divisible by 3, destroy it

void Update() {
    Debug.Log(FindObjectsOfType<Voxelize>(cubePrefab).Length);
    
    
   
}
void ReduceVoxels() {
            int rollForSurvival = Random.Range(1, 2);
            if (numberOfCubes % 2 == 0) {
                Destroy(gameObject);
            }
            else if (numberOfCubes % 3 == 0) {
                Destroy(gameObject);
            }
    }

//eliminates a portion of the cubes on collision to slow the growth rate of objects.
//can probably replace this later by making the cubes into a single mesh after they settle.
void OnCollisionEnter(Collision collision) {
    if (numberOfCubes > 1 && collision.gameObject.tag != "Ground") {
        ReduceVoxels();
        Disintegrate();
        }
 }
void OnMouseDown()
    
    {
        Disintegrate();
    }

void Disintegrate() {
        // Calculate the size of each square
        float cubeSizeX = transform.localScale.x / subdivisions;
        float cubeSizeY = transform.localScale.y / subdivisions;
        float cubeSizeZ = transform.localScale.z / subdivisions;

        // Loop through each subdivision
        for (int i = 0; i < subdivisions; i++)
        {
            for (int j = 0; j < subdivisions; j++)
            {
                for (int k = 0; k < subdivisions; k++)
                {
                
                //Generate a new cube at the correct position based on the amount of subdivisions.
                GameObject newCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
                //Changes the variable to determine if cube should survive.
                numberOfCubes += 1;
                //Places and resizes the cubes.
                newCube.transform.localPosition= new Vector3((i * cubeSizeX) + transform.position.x, (j * cubeSizeY) + transform.position.y, (k * cubeSizeZ) + transform.position.z);
                newCube.transform.localScale = new Vector3(cubeSizeX, cubeSizeY, cubeSizeZ);
                }
                
            }
            Destroy(gameObject);
        }
    }
}

