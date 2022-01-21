using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AudioSource projectileAudio;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        projectileAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            // Ignore other projectiles
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }

        else
        {
            projectileAudio.PlayOneShot(hitSound, 0.3f); 
            Destroy(gameObject, 0.1f);
        }
    }

}
