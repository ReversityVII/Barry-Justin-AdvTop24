using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private float direction = -1;
    private float timer = 0; 

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        gameObject.transform.position = new Vector3(transform.position.x + (0.3f * direction), transform.position.y, transform.position.z);

        if(timer > 0.5)
        {
            direction = direction * -1;
            timer = 0;
        }

        
    }
}
