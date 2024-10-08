using System.Collections.Generic;
using UnityEngine;

public class CameraVision : MonoBehaviour
{
    public Camera thisCam; //main camera
    public GameObject camFeed; //video feed

    private List<Texture2D> screenshots = new List<Texture2D>(); //list of all frames
    
    //recording vars
    private int totalShots;
    public int framesPerSecond;
    private float fpsTimer;
    private float consecutivePhotos;

    private MeshRenderer feedMesh;

    [SerializeField]
    private RenderTexture renderTexture;

    //physical appearance vars
    public Texture2D static1; //static for cuts
    public Texture2D static2;

    private CameraLightIndication camLight;


    private void Start()
    {
        //safeguard div by 0
        if(framesPerSecond < 1)
            framesPerSecond = 1;

        consecutivePhotos = 0;
        totalShots = 0;
        feedMesh = camFeed.GetComponent<MeshRenderer>();
        camLight = FindObjectOfType<CameraLightIndication>();
    }

    private void Update()
    {
        fpsTimer += Time.deltaTime;

        if (Input.GetMouseButton(1) && fpsTimer > ((1000 / framesPerSecond) * 0.001)) //scales with fps
        {
            camLight.UpdateState(1);
            consecutivePhotos++;
            takeScreenshot();
            fpsTimer = 0;
        }

        if (!Input.GetMouseButton(1))  //m2 released
        {
            consecutivePhotos = 0;
            camLight.UpdateState(-1);
        }
            

        if (Input.GetAxis("Mouse ScrollWheel") != 0) //if scrolling has happened
            AdjustZoom(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetMouseButtonDown(2)) //middle mouse pressed
            ResetZoom();

    }

    void takeScreenshot()
    {
        //set up screenshot properties
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        //ensure camera is rendered
        thisCam.Render();

        //read the pixels from the same spot and apply
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();    

        if (consecutivePhotos == 1) //add static for first 2 frames, enable light
        {
            screenshots.Add(static1);
            feedMesh.material.mainTexture = screenshots[totalShots];

            totalShots++; //account for the extra shots
        }
        else if (consecutivePhotos == 2) //second static frame
        {
            screenshots.Add(static2);
            feedMesh.material.mainTexture = screenshots[totalShots];
            totalShots++;
        }
        else //take a normal screenshot - no static is required
        {
            screenshots.Add(screenshot);
            feedMesh.material.mainTexture = screenshots[totalShots];

            totalShots++;
        }
    }

    void AdjustZoom(float axis)
    {
        if (axis < 0 && thisCam.fieldOfView < 120) //player scrolls down
            thisCam.fieldOfView += 2;
        
        else if (axis > 0 && thisCam.fieldOfView > 30) //player scrolls up
            thisCam.fieldOfView -= 2;
    }

    void ResetZoom()
    {
        //default FOV
        thisCam.fieldOfView = 70;
    }
}

