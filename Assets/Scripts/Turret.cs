using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float fireFrequency = 3f; // Interval for shooting in seconds
    private float fireTimer; // Timer that calculates between shots
    public float projectileSpeed = 5f;
    public bool facingLeft;

    public GameObject projectile; // For the prefab we will instantiate
    public Transform projectileSpawn; // Coordinates where we want to instantiate the projectile to

    // Start is called before the first frame update
    void Start()
    {
        // Automating the flipping of the gameobject when facing left, so that we don´t need to do it manually on inspector
       if (facingLeft)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f); //Flips the whole gameobject and it`s children, not only the sprite
        } 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fireTimer += Time.deltaTime; // Update the timer
        GameObject projectileClone; // Store the instantiated projectile here

        if (fireTimer > fireFrequency)
        {
            fireTimer = 0f; // Reset the timer
            projectileClone = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation); // Creates the projectile and stores it

            // Give the projectile speed according to turret´s rotation (left or right)
            if (facingLeft)
            {
                projectileClone.transform.localScale = new Vector3(-1f, 1f, 1f); // Flip the projectile to correct direction
                projectileClone.GetComponent<Rigidbody2D>().velocity = transform.right * -projectileSpeed; // Move left, negative X axis
            }

            else
            {
                projectileClone.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed; // Move right, positive X axis
            }
        }
    }
}
