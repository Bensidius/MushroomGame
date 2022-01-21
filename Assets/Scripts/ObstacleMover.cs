using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private float timer;
    [HideInInspector] public bool switchActivated;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        switchActivated = false;
        timer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If player touches the switch
        if (switchActivated)
        {
            timer += Time.deltaTime; // Same as timer = timer + Time.deltatime

            // If less than x seconds have passed, move the obstacle
            if (timer < 12f)
            {
                rb.velocity = new Vector2(0f, -0.7f);
            }

            // If the obstacle has moved out of sight
            else
            {
                switchActivated = false;
                gameObject.SetActive(false);
            }
        }
    }
}
