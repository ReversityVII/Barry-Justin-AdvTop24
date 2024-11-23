using System.Collections.Generic;
using UnityEngine;

public class CameraVision : MonoBehaviour
{
    public Camera thisCam; //main camera
    public GameObject camFeed; //video feed
    public GameObject extractPrompt; //the prompt given to extract video 

    [SerializeField]
    private List<Texture2D> screenshots = new List<Texture2D>(); //list of all frames

    //recording vars
    private int totalShots;
    public int framesPerSecond;
    public int totalScreenshotLimit; //1000 frames is around a gigabyte of memory. Adjust accordingly. 
    private float fpsTimer;
    private float consecutivePhotos;

    private MeshRenderer feedMesh;

    [SerializeField]
    private RenderTexture renderTexture;

    //physical appearance vars
    public Texture2D static1; //static for cuts
    public Texture2D static2;

    [SerializeField] private CameraLightIndication camLight;
    [SerializeField] private TrackTotalShots trackTotalShots;

    private FullVideoPlayback fullVideoPlayback;
    


    private void Start()
    {
        //safeguard div by 0
        if (framesPerSecond < 1)
            framesPerSecond = 1;

        //prepare video for first time use
        ResetVideo();

        //find necessary objects in scene
        feedMesh = camFeed.GetComponent<MeshRenderer>();
        fullVideoPlayback = FindObjectOfType<FullVideoPlayback>();
    }

    private void Update()
    {
        fpsTimer += Time.deltaTime;

        //RECORD VIDEO
        if (Input.GetMouseButton(1) && fpsTimer > ((1000 / framesPerSecond) * 0.001) && trackTotalShots.NextShotPermitted()) //scales with fps
        {
            camLight.UpdateState(1);

            //consider blending into one
            consecutivePhotos++; //locally, in script
            trackTotalShots.addShot(); //for UI

            TakeScreenshot();
            fpsTimer = 0;
        }

        if (!Input.GetMouseButton(1))  //m2 released
        {
            consecutivePhotos = 0;
            camLight.UpdateState(-1);
        }


        //ADJUST ZOOM
        if (Input.GetAxis("Mouse ScrollWheel") != 0) //if scrolling has happened
            AdjustZoom(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetMouseButtonDown(2)) //middle mouse pressed
            ResetZoom();



        //EXTRACT FOOTAGE
        extractPrompt.SetActive(false);

        if (Vector3.Distance(transform.position, fullVideoPlayback.gameObject.transform.position) < 6 && !Input.GetMouseButton(1)) //if player is close enough and is not recording
        {
            extractPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Q)) //if player presses extract footage button
            {
                feedMesh.material.mainTexture = null;
                fullVideoPlayback.InheritVideo(screenshots);
                ResetVideo();

            }
        }
    }

    void TakeScreenshot()
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

        else if (axis > 0 && thisCam.fieldOfView > 10) //player scrolls up
            thisCam.fieldOfView -= 2;
    }

    void ResetZoom()
    {
        //default FOV
        thisCam.fieldOfView = 70;
    }

    private void ResetVideo()
    {
        //reset logic variables
        trackTotalShots.resetShots();
        consecutivePhotos = 0;
        totalShots = 0;
        screenshots = new List<Texture2D>();
    }
}

