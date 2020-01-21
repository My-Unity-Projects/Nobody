using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFunctionality : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> buttons = new List<GameObject>();

    public GameObject door;

    bool openDoor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Capacity; i++)
        {
            if (enemies[i] == null)
            {
                if (!buttons[i].transform.GetChild(0).GetComponent<Button>().activated)
                {
                    buttons[i].transform.GetChild(0).GetComponent<Button>().Activate();
                }              
            }
        }

        if(!openDoor)
        {
            for (int i = 0; i < buttons.Capacity; i++)
            {
                openDoor = true;

                if (!buttons[i].transform.GetChild(0).GetComponent<Button>().pressed)
                {
                    openDoor = false;
                    break;
                }
            }
        }

        if(openDoor)
        {
            for (int i = 0; i < buttons.Capacity; i++)
            {
                buttons[i].transform.GetChild(0).GetComponent<Button>().pressTimeCounter = 1;
            }

            door.GetComponent<Animator>().SetBool("open", true);
        }
    }
}
