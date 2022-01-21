using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public int points; // The amount of points rewarded to the player
    private float randomizer;
    private bool animHasStarted;
    private float timer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f;
        randomizer = Random.Range(0f, 0.7f); // Get a random value
        animHasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the animation is not playing already, start the timer
        if (!animHasStarted)
        {
            timer += Time.deltaTime;
        }

        // when timer is equal or greater than randomizer and animation hasn´t been played yet, start the animation
        // This will let the pickups float in the air at different pace
        if (timer >= randomizer && !animHasStarted)
        {
            anim.speed = 1f;
            animHasStarted = true;
        }
    }
}
