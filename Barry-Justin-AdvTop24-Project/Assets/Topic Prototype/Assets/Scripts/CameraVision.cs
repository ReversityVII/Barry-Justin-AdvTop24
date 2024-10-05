using System.Collections.Generic;
using UnityEngine;

public class CameraVision : MonoBehaviour
{

    /*
    https://stackoverflow.com/questions/32996534/rendering-complete-camera-view169-onto-a-texture-in-unity3d
    https://stackoverflow.com/questions/46595055/readpixels-was-called-to-read-pixels-from-system-frame-buffer-while-not-inside
    */

    public Camera cam;
    public GameObject feed;

    public List<Texture2D> screenshots = new List<Texture2D>();
    
    private int totalShots;
    public int framesPerSecond;
    private float fpsTimer;
    private float consecutivePhotos = 0;

    public Texture2D static1;
    public Texture2D static2;
    private MeshRenderer feedMesh;

    [SerializeField]
    private RenderTexture renderTexture;


    private void Start()
    {
        totalShots = 0;
        feedMesh = feed.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        fpsTimer += Time.deltaTime;

        if (Input.GetMouseButton(1) && fpsTimer > ((1000 / framesPerSecond) * 0.001)) //scales with fps
        {
            consecutivePhotos++;
            takeScreenshot();
            fpsTimer = 0;
        }

        if (!Input.GetMouseButton(1))  //m2 released
            consecutivePhotos = 0;

        if (Input.GetAxis("Mouse ScrollWheel") != 0) //if scrolling has happened
            AdjustZoom(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetMouseButtonDown(2)) //middle mouse pressed
            ResetZoom();

    }

    void takeScreenshot()
    {

        //set up screenshot properties
        Texture2D snap = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        //ensure camera is rendered
        cam.Render();

        //read the pixels from the same spot and apply
        RenderTexture.active = renderTexture;
        snap.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        snap.Apply();    

        if (consecutivePhotos == 1) //add static
        {
            screenshots.Add(static1);
            screenshots.Add(static2);
            feedMesh.material.mainTexture = screenshots[totalShots];

            totalShots += 2; //account for the extra shot
        }
        else //just take a screenshot
        {
            screenshots.Add(snap);
            feedMesh.material.mainTexture = screenshots[totalShots];

            totalShots++;
        }
    }

    void AdjustZoom(float axis)
    {
        if (axis < 0)
            cam.fieldOfView += 2;
        
        else
            cam.fieldOfView -= 2;
        
    }

    void ResetZoom()
    {
        cam.fieldOfView = 70;
    }
}

