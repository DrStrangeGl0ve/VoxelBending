using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BendRocks : MonoBehaviour
{
    // The number of subdivisions
    public int subdivisions = 4;
    public GameObject squarePrefab;

void Update () {
    if (Input.GetMouseButton(1)) // Check if right mouse button is clicked
    {
        // Calculate the size of each square
        float squareSize = transform.localScale.x / subdivisions;

        // Loop through each subdivision
        for (int i = 0; i < subdivisions; i++)
        {
            for (int j = 0; j < subdivisions; j++)
            {
                // Calculate the position of the current square
                Vector3 position = transform.position + new Vector3(i * squareSize, j * squareSize, 0);

                // Create a new square object
                GameObject newSquare = Instantiate(squarePrefab, position, Quaternion.identity);

                // Adjust the scale of the new square
                newSquare.transform.localPosition = new Vector3(i * squareSize, j * squareSize, 0);
                newSquare.transform.localScale = new Vector3(squareSize, squareSize, 1f);

                // Pull the new square towards the mouse pointer
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - newSquare.transform.position;
                newSquare.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 10f, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
void OnMouseDown()
    
    {
        // Calculate the size of each square
        float squareSize = transform.localScale.x / subdivisions;

        // Loop through each subdivision
        for (int i = 0; i < subdivisions; i++)
        {
            for (int j = 0; j < subdivisions; j++)
            {
                // Calculate the position of the current square
                Vector3 position = transform.position + new Vector3(i * squareSize, j * squareSize, 0);

                // Create a new square object
                GameObject newSquare = Instantiate(squarePrefab, position, Quaternion.identity);
                //newSquare.AddComponent<Rigidbody2D>();
                //newSquare.AddComponent<BoxCollider2D>();

                // Adjust the scale of the new square
                newSquare.transform.localPosition= new Vector3(i * squareSize, j * squareSize, 0);
                newSquare.transform.localScale = new Vector3(squareSize, squareSize, 1f);

        }

     Destroy(gameObject);
        }

    }
}
    


