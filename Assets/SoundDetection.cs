using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    [Header("Health Settings")]
    public int health;

    [Header("Movement Settings")]
    public float speed;
    public int direction;

    [Header("Components Settings")]
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer colorSpriteRenderer;
    private Animator animator;

    [Header("Ground Enemy Stater")]
    public GroundEnemyState state = GroundEnemyState.MOVING;

    public enum GroundEnemyState
    {
        MOVING, 
        WAITING,
        ATTACKING
    }

    [Header("Listening Settings")]
    public float detectionRadius;
    private Collider2D detectorResult;

    [Header("Seeing Settings")]
    public Transform visionLimitOne;
    public Transform visionLimitTwo;

    [Header("Layer Mask")]
    public LayerMask layerMask;

    [Header("Pattern Points Settings")]
    public Transform pointOne;
    public Transform pointTwo;
    private Transform nextPoint;

    [Header("Particle Settings")]
    public ParticleSystem explosionPS;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            colorSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }

        nextPoint = pointOne; // Set point on the right as the first point for the pattern movement
    }

    // Update is called once per frame
    void Update()
    {
        Listening();
        Seeing();

        StateManagement();
    }

    /*STATE MANAGEMENT METHOD*/
    public void StateManagement()
    {
        switch (state)
        {
            case GroundEnemyState.MOVING:
                Moving();
                break;

            case GroundEnemyState.WAITING:
                Waiting();
                break;

            case GroundEnemyState.ATTACKING:
                Attacking();
                break;
        }
    }

    /*LISTENING METHOD*/
    public void Listening()
    {
        detectorResult = Physics2D.OverlapCircle(transform.position, detectionRadius, layerMask); // If the player is inside of the 5 radius circle he will be detected by the enemy

        // Gizmos.DrawSphere(transform.position, detectionRadius); // Debug listening area

        if (detectorResult != null && state != GroundEnemyState.ATTACKING) // The Enemy heared something and is not attacking
            state = GroundEnemyState.WAITING; // Is waiting for the sound source to approach

        else if (state == GroundEnemyState.WAITING)
            state = GroundEnemyState.MOVING;

    }

    /*SEEING METHOD*/
    public void Seeing()
    {
        RaycastHit2D hit = DrawRaycast();

        if (hit.collider != null) // The enemy saw something in the player layer (Must be the player)
            state = GroundEnemyState.ATTACKING; // Starts attacking

        else if(state == GroundEnemyState.ATTACKING)
            state = GroundEnemyState.WAITING;
    }

    public RaycastHit2D DrawRaycast()
    {
        RaycastHit2D hit;

        // When the enemy is moving right the raycast is drawed towards the right
        if (direction == 1)
        {
            float distance = Mathf.Abs(visionLimitOne.position.x - this.transform.position.x) - 0.5f; // Distance from Enemy to visionLimitOne

            hit = Physics2D.Raycast(this.transform.position, Vector2.right, distance, layerMask); // Create raycast

            Debug.DrawRay(this.transform.position, Vector2.right * distance, Color.red); // Debug raycast in scene view as a red line

        }
        // When the enemy is moving left the raycast is drawed towards the left
        else
        {
            float distance = Mathf.Abs(visionLimitTwo.position.x - this.transform.position.x) - 0.5f; // Distance from Enemy to visionLimitTwo

            hit = Physics2D.Raycast(this.transform.position, Vector2.left, distance, layerMask); // Create raycast

            Debug.DrawRay(this.transform.position, Vector2.left * distance, Color.red); // Debug raycast in scene view as a red line
        }

        return hit; // Return raycast created
    }

    /*%%%%%%%% STATE METHODS %%%%%%%%%*/
    /*MOVING METHOD*/
    public void Moving()
    {
        colorSpriteRenderer.color = Color.yellow;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(nextPoint.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime); // The enemy moves towards the nextPoint position

        if (transform.position.x == nextPoint.position.x) // When the enemy gets to the nextPoint position
        {
            if (nextPoint == pointOne) // If next point is already equal to pointOne now it must be pointTwo
                ChangePatternPoint(pointTwo);
            else
                ChangePatternPoint(pointOne); // If next point is already equal to pointTwo now it must be pointOne
        }
    }

    /*WAITING METHOD*/
    public void Waiting()
    {
        colorSpriteRenderer.color = Color.blue;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(nextPoint.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime); // The enemy moves towards the nextPoint position

        Transform player = GameObject.FindGameObjectWithTag("Player").transform; // Getting the player transform to know his position

        float playerToEnemyX = player.position.x - transform.position.x;

        //float playerToPointOne = (player.position - pointOne.position).magnitude; // Distance between player and pointOne
        //float playerToPointTwo = (player.position - pointTwo.position).magnitude; // Distance between player and pointTwo

        if (playerToEnemyX > 0) // If the player is closer to the pointOne the enemy will move to the pointOne
            ChangePatternPoint(pointOne);

        else // If the player is closer to the pointTwo the enemy will move to the pointTwo
            ChangePatternPoint(pointTwo);

        if (transform.position.x == nextPoint.position.x) // When the enemy gets to the nextPoint position
        {
            // Set idle animation
        }
    }

    /*ATTACKING METHOD*/
    public void Attacking()
    {
        colorSpriteRenderer.color = Color.red;
    }

    /*%%%%%%%%% CHANGE PATTERN POINT MEHTOD %%%%%%%%%%*/
    public void ChangePatternPoint(Transform newPoint)
    {
        nextPoint = newPoint; // Equal nextPoint to the parameter point

        if(newPoint == pointOne) // If the newPoint is equal pointOne the enemy is gonna move to the right
        {
            spriteRenderer.flipX = false;
            colorSpriteRenderer.flipX = false;
            direction = 1;
        }

        else // If the newPoint is different from pointOne (is equal to pointTwo) the enemy is gonna move to the left
        {
            spriteRenderer.flipX = true;
            colorSpriteRenderer.flipX = true;
            direction = -1;
        }
    }

    /*%%%%%%%% COLLISIONS %%%%%%%%%*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Bullet")
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
            health--;

            if (health == 0)
            {
                CameraShaker.Instance.ShakeOnce(10f, 10f, .3f, .3f);
                var main = explosionPS.main;
                main.startColor = colorSpriteRenderer.color;
                Instantiate(explosionPS, this.transform.position, Quaternion.identity);
                GameObject.FindObjectOfType<AudioManager>().Play("Ground Enemy Dead");
                Destroy(this.transform.parent.gameObject);
            }

            Destroy(collision.gameObject);
        }
    }

    /*%%%%%%%% DEBUG MEHTOD %%%%%%%*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

}
