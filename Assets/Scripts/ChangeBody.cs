using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBody : MonoBehaviour
{
    bool canChangeBody = false;
    public GameObject newBody;

    public GameObject detectingArea;
    SpriteRenderer srDa;

    // Control variable to avoid problems when changing body
    bool changingBody = false;

    // Start is called before the first frame update
    void Start()
    {
        srDa = detectingArea.GetComponent<SpriteRenderer>();
        srDa.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        if(canChangeBody && newBody != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                changingBody = true;
                changeBody();
            }
        }
    }

    /*CHANGE TO NEW BODY METHOD*/
    private void changeBody()
    {
        if(newBody != null)
        {
            CameraFollower cf = GameObject.FindObjectOfType<CameraFollower>();
            cf.target = newBody.transform;

            // Disable this body
            this.gameObject.tag = "Body";
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            this.gameObject.GetComponent<Animator>().Play("Desactivate");
            this.gameObject.GetComponent<ChangeBody>().enabled = false;
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.gameObject.layer = 12;

            // Enable new body
            Debug.Log(newBody);
            newBody.tag = "Player";
            newBody.GetComponent<PlayerController>().enabled = true;
            newBody.GetComponent<Animator>().Play("Activate");
            newBody.GetComponent<ChangeBody>().enabled = true;
            newBody.transform.GetChild(2).gameObject.SetActive(true);
            newBody.gameObject.layer = 9;

            // When the changing body process is done the changingBody variable is set to false
            changingBody = false;
        }
    }

    /*DETECT NEW BODY IN THE SCENE*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Body")
        {
            canChangeBody = true;
            srDa.color = Color.green;
            newBody = collision.gameObject;
            GameObject.FindObjectOfType<AudioManager>().Play("New Body Detected");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Body" && !changingBody)
        {
            canChangeBody = false;
            srDa.color = Color.gray;
            newBody = null;
        }
    }
}
