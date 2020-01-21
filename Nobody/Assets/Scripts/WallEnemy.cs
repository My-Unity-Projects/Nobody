using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class WallEnemy : MonoBehaviour
{

    SpriteRenderer sr;
    Rigidbody2D rb;

    bool atack;

    public ParticleSystem explosionPS;


    // Start is called before the first frame update
    void Start()
    {
        sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        atack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Trigger
    public void Trigger()
    {
        rb.gravityScale = 5;
        sr.color = Color.red;

        atack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Player")
        {
            Trigger();
        }
    }

    // Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(atack)
        {
            CameraShaker.Instance.ShakeOnce(20, 20, .2f, .2f);
            var main = explosionPS.main;
            main.startColor = sr.color;
            this.GetComponent<CircleCollider2D>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(explosionPS, this.transform.position, Quaternion.identity);
            GameObject.FindObjectOfType<AudioManager>().Play("Wall Enemy Dead");
            Destroy(this.transform.parent.gameObject, 0.1f);
        }
    }

}
