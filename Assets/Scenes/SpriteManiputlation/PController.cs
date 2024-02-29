using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    public Collider2D Ground = null;
    private float horizontalMovement;
    private Vector2 worldMousePos = Vector3.zero;
    private Camera mainCamera;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Ground != null)
        {
            if(Input.GetButtonDown("Jump")) Jump();
            if(Input.GetKeyDown(KeyCode.F)) SpawnWall();
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
        SpriteRenderer spriteRenderer = Ground.GetComponent<SpriteRenderer>();
        Vector3 closestPoint = Ground.ClosestPoint(worldMousePos);

        Vector2 localHitPoint = Ground.transform.InverseTransformPoint(closestPoint);
        Vector2 pixelUV = new Vector2((localHitPoint.x + spriteRenderer.sprite.bounds.extents.x) / spriteRenderer.sprite.bounds.size.x,
                                       (localHitPoint.y + spriteRenderer.sprite.bounds.extents.y) / spriteRenderer.sprite.bounds.size.y);
        pixelUV.x *= spriteRenderer.sprite.texture.width;
        pixelUV.y *= spriteRenderer.sprite.texture.height;

        Texture2D texture = spriteRenderer.sprite.texture;

        // Convert the texture to a compatible format
        Texture2D readableTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        readableTexture.SetPixels(texture.GetPixels());
        readableTexture.Apply();

        int startX = Mathf.FloorToInt(pixelUV.x - 10); // Example: Remove a 20x20 pixel area
        int startY = Mathf.FloorToInt(pixelUV.y - 10);
        int endX = Mathf.FloorToInt(pixelUV.x + 10);
        int endY = Mathf.FloorToInt(pixelUV.y + 10);

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                if (x >= 0 && x < readableTexture.width && y >= 0 && y < readableTexture.height)
                {
                    readableTexture.SetPixel(x, y, Color.black);
                }
            }
        }
        readableTexture.Apply();

        // Apply the modified texture back to the sprite renderer
        spriteRenderer.sprite = Sprite.Create(readableTexture, new Rect(0, 0, readableTexture.width, readableTexture.height), Vector2.one * 0.5f);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Ground = collision.collider;
        }
    }
}
