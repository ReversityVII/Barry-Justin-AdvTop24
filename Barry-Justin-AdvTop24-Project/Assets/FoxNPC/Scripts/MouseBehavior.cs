using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{
    public bool hasBeenCollected;

    void Start()
    {
        //all mice prefabs need to ignore layer 6 for collision so they fall under the snow
        Physics.IgnoreLayerCollision(8, 0, true);  
    }
}
