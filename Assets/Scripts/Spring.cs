using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float springForce = 15f;

    private Animator anim;
    private AudioSource springAudio;
    public AudioClip springSound;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        springAudio = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") // If player hits the spring
        {
            player.rb.velocity = new Vector2(0f, springForce); // Bounce the  player
            player.anim.SetBool("Jumping", true); // Change the player´s animations
            anim.SetTrigger("PlayerTouch"); // Change the spring`s animations
            springAudio.PlayOneShot(springSound, 0.5f);
        }
    }

}
