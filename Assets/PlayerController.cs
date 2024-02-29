using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f; // Changed variable name to follow C# naming conventions
    public float MaxRockThrowingDistance = 5;
    private Rigidbody2D rb;
    public Collider2D Ground = null;
    private float horizontalMovement;
    private Vector2 worldMousePos = Vector3.zero;

    [Header("Earth Ability's")]
    [Indent(1)]public GameObject EarthWall;
    [Indent(1)]public float EarthSpeed = 5f;

    [Indent(1)]public GameObject EarthDisk;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Ground != null)
        {
            if(Input.GetButtonDown("Jump")) Jump();
            if(Input.GetKeyDown(KeyCode.F)) SpawnWall();
            if(Input.GetKeyDown(KeyCode.E)) ThrowDisk();
        }
    }
    private void FixedUpdate()
    {
        float targetVelocityX = moveSpeed * horizontalMovement;
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetVelocityX, 0.1f), rb.velocity.y);
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Ground = null;
    }
    private void SpawnWall()
    {
        if (Ground == null) return;

        Vector3 closestPoint = Ground.ClosestPoint(worldMousePos);

        GameObject newWall = Instantiate(EarthWall, closestPoint - new Vector3(0,EarthWall.transform.localScale.y/2), Quaternion.identity);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // Rotate the object to align with the surface normal
        newWall.transform.rotation = Ground.transform.rotation;
        // Check if the closest point is under the player's feet
        if (closestPoint.y <= transform.position.y && closestPoint.x >= sr.bounds.min.x && closestPoint.x <= sr.bounds.max.x)
        {
            // Apply a vertical force to launch the player up
            Vector2 launchDirection = (transform.position - newWall.transform.position).normalized;
            rb.AddForce(launchDirection * jumpForce * 2, ForceMode2D.Impulse);
            Ground = null;
        }
        StartCoroutine(RaiseEarth(newWall.GetComponent<Rigidbody2D>(), closestPoint));
    }
    private void ThrowDisk()
    {
        if (Ground == null) return;

        Vector3 closestPoint = Ground.ClosestPoint(worldMousePos);
        closestPoint.x = Mathf.Clamp(closestPoint.x,transform.position.x - MaxRockThrowingDistance, transform.position.x + MaxRockThrowingDistance);
        GameObject newDisk = Instantiate(EarthDisk, closestPoint, Quaternion.identity);
        Vector2 dir = new Vector2(closestPoint.x - worldMousePos.x, closestPoint.y - worldMousePos.y);
        newDisk.transform.right = dir;

        newDisk.GetComponent<Rigidbody2D>().velocity = -newDisk.transform.right * EarthSpeed;
    }
    public IEnumerator RaiseEarth(Rigidbody2D earth, Vector3 point)
    {
        float endpointy = point.y + earth.transform.localScale.y/2;
        earth.gameObject.layer = 6;
        earth.velocity = new Vector3(0, EarthSpeed, 0);
        yield return new WaitUntil(() => earth.transform.position.y >= endpointy);
        earth.transform.position = new Vector3(earth.transform.position.x,endpointy);
        earth.velocity = Vector3.zero;
        earth.gameObject.layer = 0;
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Ground = collision.collider;
        }
    }
}
