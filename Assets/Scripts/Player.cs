using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //floats
    public float speed = 3f;
    public float jumpForce = 10f;
    public float powerUpTimer;

    //Booleans
    public bool isDead;
    public bool levelFinished;
    public bool canControl;
    public bool hitGround;
    public bool onGround;
    public bool onSlope;
    public bool onVerticalLift;
    public bool onHorizontalLift;
    public bool powerUpActivated;

    //Physics materials
    public PhysicsMaterial2D slippery;
    public PhysicsMaterial2D sticky;

    //Components
    [HideInInspector] public Rigidbody2D rb;
    private SpriteRenderer sRend;
    [HideInInspector] public Animator anim;
    public ParticleSystem powerUpParticles;

    //Other gameobjects
    public Transform groundCheck;
    public LayerMask groundMask;
    public Animator switchAnim;
    public GameObject obstacle;
    public static List<GameObject> collectedPickUps = new List<GameObject>();

    // Audio
    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip gameOversound;
    public AudioClip pickUpSound;
    public AudioClip powerUpSound;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        powerUpParticles = GetComponentInChildren<ParticleSystem>();
        groundMask = LayerMask.GetMask("Ground");
        collectedPickUps.Clear(); // Clear the previous pickups


        canControl = true;
        isDead = false;
        levelFinished = false;
        onSlope = false;
        onVerticalLift = false;
        onHorizontalLift = false;
        powerUpActivated = false;
        powerUpTimer = 0f;
        rb.sharedMaterial = sticky;

        // Find the switch´s animator component
        // Avoid the null reference error if the scene doesn´t have the switch
        if (GameObject.FindGameObjectWithTag("Switch"))
        {
            switchAnim = GameObject.FindGameObjectWithTag("Switch").GetComponent<Animator>();
        }

        if (GameObject.Find("Obstacle"))
        {
            obstacle = GameObject.Find("Obstacle");
        }
    }

    void FixedUpdate()
    {
        // PowerUp(); // Make sure that the function is updated
        
        // Cast a line to check if the player is on the ground
        hitGround = Physics2D.Linecast(transform.position, groundCheck.position, groundMask); // Check for hits on ground collider
        Debug.DrawLine(transform.position, groundCheck.position, Color.red); //Visualizes the line created above in Scene view

        // Cast a ray to check the angle between the player and the ground
        RaycastHit2D slopeHit = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, groundMask);

        if (slopeHit)
        {
            float slopeAngle = Vector2.Angle(slopeHit.normal, Vector2.right);
            //print(slopeAngle);

        // If angle is smaller than 75 deg or greater than 115 deg, then ...
        if (slopeAngle < 75f || slopeAngle > 115f)
            {
                onSlope = true;
            }

            // If we are on flat or too steep ground
            else if (slopeAngle == 0f || (slopeAngle > 75f && slopeAngle < 115f))
            {
                onSlope = false;
            }



        }

    }

           // Update is called once per frame
    void Update()
    {
        PowerUp(); // Make sure that the function is updated

        float ySpeed = rb.velocity.y; 

       // If Player is on ground, change the physiscs material to high friction
        if (hitGround)
        {
            rb.sharedMaterial = sticky;
        }

        // If not, change the material to no friction
        else
        {
            rb.sharedMaterial = slippery;
            onGround = false;
        }

        // Eliminate slow falling after slope
        if (onGround && !onSlope && !onVerticalLift && rb.velocity.y < 0f)
        {
            onGround = false; 
        }



        if (canControl) 
        { 

            //Movement
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, ySpeed);
                sRend.flipX = true;           
                anim.SetBool("Moving", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, ySpeed);
                sRend.flipX = false;
                anim.SetBool("Moving", true);
            }
            //If landed & not moving
            else if (!Input.GetKey(KeyCode.A)  && !Input.GetKey(KeyCode.D) && onGround)
            {               
                anim.SetBool("Moving", false);

                // If not on an elevator
                if (!onVerticalLift && !onHorizontalLift)
                {
                    rb.velocity = Vector2.zero;
                }
                // If on a vertical elevator
                else if (onVerticalLift)
                {
                    rb.velocity = new Vector2(0f, ySpeed);
                }
            }
            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && hitGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("Jumping", true);
                onGround = false;
                onSlope = false;
                playerAudio.PlayOneShot(jumpSound, 0.5f);
            }
            // Checking if falling
            if(rb.velocity.y < 0f && !hitGround)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
            else
            {
                anim.SetBool("Falling", false);
            }
        }

        if (onGround && levelFinished)
        {
            anim.SetBool("Falling", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            // If player hits the enemy
            case "Enemy":
                if (!isDead && !powerUpActivated)
                {
                    canControl = false;
                    anim.SetBool("Death", true);
                    isDead = true;
                    AddBonusScore();
                    playerAudio.PlayOneShot(gameOversound, 0.7f);
                }
                break;

            case "Boundary":
                canControl = false;
                anim.SetBool("Death", true);
                isDead = true;
                AddBonusScore();
                rb.simulated = false; // Stops the player from falling endlessly
                playerAudio.PlayOneShot(gameOversound, 0.7f);
                break;

            // If player reaches the goal
            case "Finish":
                canControl = false;
                anim.SetBool("Moving", false);
                levelFinished = true;
                AddBonusScore();
                break;
        
            // If player touches the Switch
            case "Switch":
                switchAnim.SetTrigger("Pressed");
                obstacle.GetComponent<ObstacleMover>().switchActivated = true;
                break;

            // If player hits the elevator
            case "Elevator":
                Elevator elevator = other.gameObject.GetComponent<Elevator>(); // Get the script
                if (elevator.isHorizontal)
                {
                    onHorizontalLift = true;
                }
                else
                {
                    onVerticalLift = true;
                }
                break;

            // If player hits the pickUp 

            case "PickUp":
                UIController.score += other.gameObject.GetComponent<PickUpHandler>().points; // Rewarding the player
                collectedPickUps.Add(other.gameObject); // Adding the collected pickup on the list
                playerAudio.PlayOneShot(pickUpSound, 1f);
                Destroy(other.gameObject);
                break;


                // If player hits the powerup
            case "PowerUp":
                powerUpActivated = true;
                UIController.score += other.gameObject.GetComponent<PickUpHandler>().points; // Rewarding the player
                collectedPickUps.Add(other.gameObject); // Adding the collected pickup on the list
                playerAudio.PlayOneShot(powerUpSound, 1f);
                Destroy(other.gameObject);
                break;
        }             
    }

    void OnTriggerExit2D (Collider2D other)
    {
        // If player exits the elevator
        if (other.gameObject.tag == "Elevator")
        {
            onHorizontalLift = false;
            onVerticalLift = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // If the collider and linecast both touch the ground
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && hitGround)
        {
            onGround = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // If player hits the projectile
        if (other.gameObject.tag == "Projectile"  && !powerUpActivated  && !isDead)
        {
            canControl = false;
            anim.SetBool("Death", true);
            isDead = true;
            AddBonusScore();
            playerAudio.PlayOneShot(gameOversound, 0.7f);
        }
    }

    void AddBonusScore()
    {
        // print("Size of the list: " + collectedPickUps.Count);

        // This loop will repeat the multiplication of the score as many times as is the amount of collected pickups
        for (int i = 0; i< collectedPickUps.Count; i++)
        {
            UIController.score += Mathf.RoundToInt (UIController.score * 0.05f); // rounding the float to int
        }
    }

    // function ror powerup
    void PowerUp()
    {
        if (powerUpActivated)
        {
            powerUpTimer += Time.deltaTime;
            var emission = powerUpParticles.emission;
            emission.enabled = true; // Enable the particle system´s emission

            if (powerUpTimer > 10f)
            {
                powerUpActivated = false;
                emission.enabled = false; // Disable the emission
                powerUpTimer = 0f;
            }
        }
    }

}
