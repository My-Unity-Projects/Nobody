using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Animator ani;
    SpriteRenderer sr;
    public bool pressed, activated;

    public float pressTimeCounter;
    public float pressTime;

    // Start is called before the first frame update
    void Start()
    {
        ani = this.GetComponent<Animator>();
        sr = this.transform.GetComponent<SpriteRenderer>();

        sr.color = Color.red;

        pressed = false;
        activated = false;

        pressTime = 1f;
        pressTimeCounter = pressTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && !pressed && ani.GetBool("press"))
        {
            pressTimeCounter -= Time.deltaTime;

            if (pressTimeCounter < 0)
            {
                sr.color = Color.blue;
                ani.SetBool("press", false);

                GameObject.FindObjectOfType<AudioManager>().Play("Unpress Button");
            }
        }

        else
            pressTimeCounter = pressTime;
    }

    // Activate
    public void Activate()
    {
        sr.color = Color.blue;
        activated = true;
        GameObject.FindObjectOfType<AudioManager>().Play("Unlock Button");
    }

    // Press
    public void Press()
    {
        if (!ani.GetBool("press"))
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");

        pressed = true;
        sr.color = Color.green;
        ani.SetBool("press", true);
    }

    // Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player" || tag == "Body")
        {
            if (activated)
            {
                Press();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player" || tag == "Body")
        {
            if (activated)
            {
                pressed = false;
            }
        }
    }
}
