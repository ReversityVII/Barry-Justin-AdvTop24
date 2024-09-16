using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraVision : MonoBehaviour
{

    /*
    https://stackoverflow.com/questions/32996534/rendering-complete-camera-view169-onto-a-texture-in-unity3d
    https://stackoverflow.com/questions/46595055/readpixels-was-called-to-read-pixels-from-system-frame-buffer-while-not-inside
    */

    public Camera cam;
    public GameObject feed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(1))
        {
            

            //RenderTexture rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
            //cam.targetTexture = rt;
            print("reached");

            Texture2D screenshot = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGB24, false);

            cam.Render();

            screenshot.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
            
        }
    }
}

