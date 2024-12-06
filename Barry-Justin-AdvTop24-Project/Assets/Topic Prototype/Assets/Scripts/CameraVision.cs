using System.Collections.Generic;
using UnityEngine;

/*
 * CameraVision class.
 * This script handles letting the player record video with their camera.
 * They can record, and zoom in/out.
 * Also prompts the player to extract their video if they are close enough to the video terminal. 
 * NOTE: This one is a little comment heavy, consider slimming down in the future
 */
public class CameraVision : MonoBehaviour
{
    //gameobject references 
    public Camera thisCam; //video camera 
    public GameObject camFeed; //video feed quad attached to the video camera, for referencing the mesh from
    private MeshRenderer feedMesh; //mesh for camFeed object, this is what gets updated to display what is going on
    [SerializeField] private RenderTexture renderTexture; //reference to dimensions that the texture is supposed to be
    public GameObject extractPrompt; //the prompt given to extract video 

    [SerializeField]
    private List<Texture2D> screenshots = new List<Texture2D>(); //list of all frames

    //variables necessary to record at the right intervals and add static
    private int totalShots;
    public int framesPerSecond;
    public int totalScreenshotLimit; //1000 frames is around a gigabyte of memory. Adjust accordingly. 
    private float fpsTimer;
    private float consecutivePhotos;


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
        //track time since last frame
        fpsTimer += Time.deltaTime;
        double timeSinceLastFrame = ((1000 / framesPerSecond) * 0.001);

        //RECORD VIDEO
        if (Input.GetMouseButton(1) && fpsTimer > timeSinceLastFrame && trackTotalShots.NextShotPermitted()) //player pressed record button, enough time has passed since last frame, and the next shot is permitted
        {
            camLight.UpdateState(true);

            //NOTE: consider blending into one
            consecutivePhotos++; //locally, in script
            trackTotalShots.addShot(); //for UI

            //take the screenshot, reset the timer.
            TakeScreenshot();
            fpsTimer = 0;
        }

        //only record if the player holds down m1
        if (!Input.GetMouseButton(1))  //m1 released
        {
            //disable signifiers of recording in progress
            consecutivePhotos = 0;
            camLight.UpdateState(false);
        }


        //adjust zoom
        if (Input.GetAxis("Mouse ScrollWheel") != 0) //if scrolling has happened
            AdjustZoom(Input.GetAxis("Mouse ScrollWheel"));

        //reset zoom
        if (Input.GetMouseButtonDown(2)) //middle mouse pressed
            ResetZoom();

        //handle video extraction prompt and process
        if (Vector3.Distance(transform.position, fullVideoPlayback.gameObject.transform.position) < 6 && !Input.GetMouseButton(1)) //if player is close enough and is not recording
        {
            //prompt for extraction
            extractPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Q)) //if player presses extract footage button
            {
                //extract footage 
                fullVideoPlayback.InheritVideo(screenshots);

                //reset necessary logic
                feedMesh.material.mainTexture = null;
                ResetVideo();
            }
        }
        else //disallow extraction
            extractPrompt.SetActive(false);
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

        //add static for first 2 frames, enable light
        if (consecutivePhotos == 1) //first static frame
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
        else //no static is required
        {
            //take a normal screenshot
            screenshots.Add(screenshot);
            feedMesh.material.mainTexture = screenshots[totalShots];

            totalShots++;
        }
    }

    //zoom is adjusted
    void AdjustZoom(float axis)
    {
        if (axis < 0 && thisCam.fieldOfView < 120) //player scrolls down
            thisCam.fieldOfView += 2;

        else if (axis > 0 && thisCam.fieldOfView > 10) //player scrolls up
            thisCam.fieldOfView -= 2;
    }

    //zoom is rest
    void ResetZoom()
    {
        //default FOV
        thisCam.fieldOfView = 70;
    }

    //video is reset, to record more footage from scratch
    private void ResetVideo()
    {
        //reset logic variables
        trackTotalShots.resetShots();
        consecutivePhotos = 0;
        totalShots = 0;
        screenshots = new List<Texture2D>();
    }
}

