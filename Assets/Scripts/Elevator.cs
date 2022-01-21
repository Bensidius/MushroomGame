using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 2f;
    public bool isHorizontal; // Enable ir disable on Inspector
    public bool hitTrigger;
    public bool isMovingUp;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If this elevator moves vertically
        if (!isHorizontal)
        {
            // Moving up
            if (isMovingUp && !hitTrigger)
            {
                rb.velocity = Vector2.up * speed;
            }

            // Moving down
            if (!isMovingUp && !hitTrigger)
            {
                rb.velocity = Vector2.down * speed;
            }
        }

        // If this elevator moves horizontally
        if (isHorizontal)
        {
            // Moving up
            if (isMovingUp && !hitTrigger)
            {
                rb.velocity = Vector2.right  * speed;
            }

            // Moving down
            if (!isMovingUp && !hitTrigger)
            {
                rb.velocity = Vector2.left * speed;
            }
        }
    }

    // Change the direction
    void Turn()
    {
        isMovingUp = !isMovingUp;
        hitTrigger = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ElevatorTrigger")
        {
            hitTrigger = true; // When the elevator hits the trigger area
            rb.velocity = Vector2.zero;
            Invoke("Turn", 5); // Call the Turn function after x seconds
        }
    }
}
