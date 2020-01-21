using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public float speed;
    public float fireRate;
    public int direction;

    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            if(direction != 0)
                transform.position += transform.right * speed * direction * Time.deltaTime;
        }

        else
            Debug.Log("No speed!");
    }

    // Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(hitEffect.transform.GetChild(1), this.transform.position, Quaternion.identity);
        GameObject.FindObjectOfType<AudioManager>().Play("Bullet Impact");
        Destroy(this.gameObject);
    }
}
