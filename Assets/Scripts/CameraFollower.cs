using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(target.position.x, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        if(target != null)
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.position.x, target.position.y + 2, this.transform.position.z), 1);
    }
}
