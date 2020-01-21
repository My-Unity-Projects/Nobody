using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotChild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This script is place in the ground enemy's father gameobject
    // I need it becuase I want the ground enemy to shot in a certain frame of his father animation

    public void SpawnVFXChild()
    {
        this.transform.GetChild(0).GetComponent<GroundEnemy>().SpawnVFX(); // Execute SpawnVFX method of the ground enemy
    }
}
