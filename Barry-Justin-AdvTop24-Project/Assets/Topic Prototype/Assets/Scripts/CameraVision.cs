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

    private List<Texture2D> screenshots = new List<Texture2D>();
    private int totalShots;
    public int FramesPerSecond;
    private float fpsTimer;

    private void Start()
    {
        totalShots = 0;
    }

    private void Update()
    {
        fpsTimer += Time.deltaTime;

        if (Input.GetMouseButton(1) && fpsTimer > ((1000 / FramesPerSecond) * 0.001)) //scales with fps
        {
            takeScreenshot();
            fpsTimer = 0;
        }
    }

    void takeScreenshot()
    {
        //yield return new WaitForEndOfFrame();
        //wait til everything has been done before proceeding
        print(totalShots);
        
        //set up screenshot properties
        Texture2D snap = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGB24, false);

        //ensure camera is rendered
        cam.Render();

        //read the pixels from the same spot and apply
        snap.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
        snap.Apply();

        screenshots.Add(snap);

        //overwrite quad texture
        feed.GetComponent<MeshRenderer>().material.mainTexture = screenshots[totalShots];
        
        totalShots++;

        //yield return null;
    }


}

