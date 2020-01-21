using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 10);

        // Bodies
        // Physics2D.IgnoreLayerCollision(12, 9);
        Physics2D.IgnoreLayerCollision(12, 10);
        Physics2D.IgnoreLayerCollision(12, 11);
        Physics2D.IgnoreLayerCollision(12, 13);

        // Enemy Bullets
        Physics2D.IgnoreLayerCollision(11, 13);
        Physics2D.IgnoreLayerCollision(10, 13);

        // Cursor
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
