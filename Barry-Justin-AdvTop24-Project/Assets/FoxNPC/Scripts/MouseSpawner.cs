using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawner : MonoBehaviour
{

    public float mouseCooldown;
    private float timer = 0;

    public GameObject mousePrefab;

    void Update()
    {
        timer += Time.deltaTime;

            if(Input.GetKey(KeyCode.E) && timer > mouseCooldown) 
            {
                //spawn mouse prefab
                Instantiate(mousePrefab, gameObject.transform.position, gameObject.transform.rotation);
                timer = 0;

            } 
    }
}
