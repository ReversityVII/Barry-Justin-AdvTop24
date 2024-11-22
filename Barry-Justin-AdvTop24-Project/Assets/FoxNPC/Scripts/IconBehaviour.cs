using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class IconBehaviour : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //look in the direction of the camera. Couldn't quite get it to look exactly at the camera, but this will do
        Vector3 targetPostition = new Vector3(transform.position.x,cam.transform.position.y, transform.position.z);
        transform.LookAt(targetPostition);
    }
}
