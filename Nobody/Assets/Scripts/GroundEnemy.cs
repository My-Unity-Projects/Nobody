using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public enum GroundEnemyState
    {
        MOVING, 
        WAITING,
        ATTACKING
    }

    [Header("State Settings")]
    public GroundEnemyState state = GroundEnemyState.MOVING;

    // Components
    [Header("Component Settings")]
    SpriteRenderer sr;
    SpriteRenderer childSr;
    Animator ani;
    Rigidbody2D rb;

    // Variables
    [Header("Movement Settings")]
    public int health;
    float speed;
    public int direction;
    
    // Particles
    [Header("Particle Settings")]
    public ParticleSystem explosionPS;

    // Pattern points
    [Header("Pattern points Settings")]
    public Transform pointOne;
    public Transform pointTwo;
    Transform nextPoint;

    // Raycast 
    [Header("Raycast Settings")]
    public Transform limitOne;
    public Transform limitTwo;
    int layerMask;

    public Color srColor;

    // Shoot
    [Header("Shoot Settings")]
    public Transform firePoint;

    public List<GameObject> vfx = new List<GameObject>();
    public GameObject effectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        childSr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speed = 4;

        nextPoint = pointOne;

        layerMask = LayerMask.GetMask("Player");

        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Trigger();
    }

    // Movement
    public void Movement()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(nextPoint.position.x, this.transform.position.y, this.transform.position.z), speed * Time.deltaTime);

        if(this.transform.position.x == nextPoint.position.x)
        {

            if (nextPoint == pointOne)
            {
                ChangePoint(pointTwo);
            }

            else
                ChangePoint(pointOne);
        }
    }

    public void ChangePoint(Transform newPoint)
    {
        sr.flipX = !sr.flipX;
        childSr.flipX = !childSr.flipX;

        direction = -direction;

        nextPoint = newPoint;
    }

    // Triggered state
    public void Trigger()
    {
        RaycastHit2D hit;

        // Moving right
        if (direction == 1)
        {
            float distance = Mathf.Abs(limitOne.position.x - this.transform.position.x) - 0.5f;

            hit = Physics2D.Raycast(this.transform.position, Vector2.right, distance, layerMask);

            Debug.DrawRay(this.transform.position, Vector2.right * distance, Color.red);
 
        }
        // Moving left
        else
        {
            float distance = Mathf.Abs(limitTwo.position.x - this.transform.position.x) - 0.5f;

            hit = Physics2D.Raycast(this.transform.position, Vector2.left, distance, layerMask);

            Debug.DrawRay(this.transform.position, Vector2.left * distance, Color.red);
        }

        // Triggered
        if (hit.collider != null)
        {
            string tag = hit.collider.gameObject.tag;

            if (tag == "Player")
            {
                speed = 0;
                childSr.color = Color.red;
                this.transform.parent.GetComponent<Animator>().SetBool("shot", true);
                this.GetComponent<Animator>().SetBool("move", false);
            }
        }

        else
        {
            speed = 3;
            childSr.color = srColor;
            this.transform.parent.GetComponent<Animator>().SetBool("shot", false);
            this.GetComponent<Animator>().SetBool("move", true);
        }          
    }

    // Shoot 
    public void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.position, Quaternion.identity);
            vfx.GetComponent<ProjectileMove>().direction = this.direction;
            GameObject.FindObjectOfType<AudioManager>().Play("Ground Enemy Shot");
        }

        else
        {
            Debug.Log("No fire point!");
        }
    }

    // Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Bullet")
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
            health--;

            if(health == 0)
            {
                CameraShaker.Instance.ShakeOnce(10f, 10f, .3f, .3f);
                var main = explosionPS.main;
                main.startColor = childSr.color;
                Instantiate(explosionPS, this.transform.position, Quaternion.identity);
                GameObject.FindObjectOfType<AudioManager>().Play("Ground Enemy Dead");
                Destroy(this.transform.parent.gameObject);
            }

            Destroy(collision.gameObject);
        }
    }
}
