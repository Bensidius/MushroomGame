using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnail : MonoBehaviour
{
    public int points; // The amount of points rewarded to the player
    public float speed = -0.5f;
    private bool isDead;

    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D hitCollider;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hitCollider = GetComponentInChildren<BoxCollider2D>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If alive, keep moving
        if (!isDead)
        {
            rb.velocity = new Vector2(speed, 0f);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "EnemyTrigger" || other.gameObject.tag == "Player") && !isDead) //If enemy hits the trigger or player´s corpse
        {
            speed = -speed; // Change the moving direction
            transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f); //Flip the whole gameobject
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !player.isDead && !isDead) // If player and snail are alive and they collide
        {
            UIController.score += points; // Rewarding the player
            isDead = true;
            hitCollider.enabled = false;
            rb.freezeRotation = false; // Enable rotations so that player can push and rotate the shell
            anim.SetTrigger("Death");
        }
    }
}
