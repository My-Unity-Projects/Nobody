using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoxCollider2D bc;

    private Rigidbody2D rb; // Player rigidbody component

    private Animator ani; // Player animator component

    private SpriteRenderer sr; // Player sprite renderer component
    private SpriteRenderer src; // Player color sprite renderer
    private SpriteRenderer srw; // Player weapon sprite renderer

    private float moveInput; // Direction the character is looking to 1 = right | -1 = left

    [Header("Shoot Settings")]
    public float shotTime;
    private float shotTimeCounter;
    public Transform firePoint;

    public List<GameObject> vfx = new List<GameObject>();
    public GameObject effectToSpawn;

    [Header("Movement Settings")]
    public int direction;
    public float speed;

    private bool idle, walk, shoot, isJumping, isGrounded;

    [Header("Jump Settings")]
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;
    private float jumpTimeCounter;
    public float jumpTime;

    public Transform headPos;
    public float checkRadius2;
    public LayerMask whatIsGround2;
    bool isCeiling;

    [Header("Particle Effects Settings")]
    public ParticleSystem explosionPS;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            bc = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
            ani = this.GetComponent<Animator>();
            sr = this.GetComponent<SpriteRenderer>();
            src = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
            srw = this.transform.GetChild(1).GetComponent<SpriteRenderer>();

        }

        catch(MissingComponentException e)
        {
            Debug.Log(e.Message);
        }

        direction = 1;

        // Animation variables
        idle = true;
        walk = false;
        shoot = false;
        isJumping = false;
        isGrounded = true;

        SetAnimation();

        // Bullet variables
        shotTimeCounter = shotTime;
        effectToSpawn = vfx[0];

        if (this.gameObject.layer == 12)
        {
            ani.Play("Desactivate");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    // FixedUpdate is called once per frame execute in here the physics related methods
    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        Movement();
        Jump();

    }

    /*MOVEMENT METHODS*/
    public void Movement()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false; // Player sprite renderer looking to the right
            src.flipX = false; // Player color sprite renderer looking to the right
            srw.flipX = false; // Player weapon sprite renderer looking to the right
            // bc.offset = new Vector2(-0.5f, 0); // Set box collider at the center of the player
            // bulletSpawn.localPosition = new Vector3(1, 0, 0); // Set bullet spawn at the end of the weapon
            direction = 1;

            if (isGrounded)
            {
                idle = false;
                walk = true;

                SetAnimation();
            }
        }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = true; // Player sprite renderer looking to the left
            src.flipX = true; // Player color sprite renderer looking to the left
            srw.flipX = true; // Player weapon sprite renderer lookint to the left
            // bc.offset = new Vector2(0.5f, 0); // Set box collider at the center of the player
            // bulletSpawn.localPosition = new Vector3(-1, 0, 0); // Set bullet spawn at the end of the weapon

            direction = -1;

            if (isGrounded)
            {
                idle = false;
                walk = true;

                SetAnimation();
            }
        }

        else
        {
            idle = true;
            walk = false;

            SetAnimation();
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    /*SHOOT METHOD*/
    public void Shoot()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return))
        {
            if(isGrounded && !walk)
            {
                idle = false;
                shoot = true;
                SetAnimation();
            }

            shotTimeCounter -= Time.deltaTime;

            if(shotTimeCounter < 0)
            {
                shotTimeCounter = shotTime;

                SpawnVFX();
            }
        }

        else
        {
            if(isGrounded && !walk)
            {
                idle = true;
                shoot = false;
                SetAnimation();
            }
        } 
    }

    public void SpawnVFX()
    {
        GameObject vfx;

        if(firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.position, Quaternion.identity);
            vfx.GetComponent<ProjectileMove>().direction = this.direction;
            GameObject.FindObjectOfType<AudioManager>().Play("Player Shot");
        }

        else
        {
            Debug.Log("No fire point!");
        }
    }

    /*JUMP METHOD*/
    public void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isCeiling = Physics2D.OverlapCircle(headPos.position, checkRadius2, whatIsGround2);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(moveInput * speed, Vector2.up.y * jumpForce);
        }

        if(Input.GetKey(KeyCode.Space) && isJumping && !isCeiling)
        {
            if(jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(moveInput * speed, Vector2.up.y * jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

            SetAnimation();
        }

        else if(isCeiling)
        {
            isJumping = false;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    /*ANIMATION MANAGER*/
    public void SetAnimation()
    {
        if(idle && isGrounded)
        {
            ani.SetBool("idle", true);
            ani.SetBool("walk", false);
            ani.SetBool("shoot", false);
            ani.SetBool("jump", false);
        }

        else if(walk && isGrounded)
        {
            ani.SetBool("idle", false);
            ani.SetBool("walk", true);
            ani.SetBool("shoot", false);
            ani.SetBool("jump", false);
        }

        else if(shoot && isGrounded)
        {
            ani.SetBool("idle", false);
            ani.SetBool("walk", false);
            ani.SetBool("shoot", true);
            ani.SetBool("jump", false);
        }

        else if(isJumping)
        {
            ani.SetBool("idle", false);
            ani.SetBool("walk", false);
            ani.SetBool("shoot", false);
            ani.SetBool("jump", true);
        }
    }


    /*DEAD METHOD*/
    public void Dead()
    {
        var main = explosionPS.main;
        main.startColor = new Color(0, 1, .688f);

        Instantiate(explosionPS, this.transform.position, Quaternion.identity);
        CameraShaker.Instance.ShakeOnce(20, 20, .2f, .2f);

        GameObject.FindObjectOfType<SceneTransitions>().LoadNewScene(SceneManager.GetActiveScene().name);
        GameObject.FindObjectOfType<AudioManager>().Play("Player Dead");

        Destroy(this.gameObject);

    }

    /*COLLISIONS*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Enemy" || tag == "Bullet")
        {
            Dead();
        }
    }

    /*SOUNDS*/
    public void PlayStep()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Player Walk");
    }

}
