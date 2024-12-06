using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/*
 * FullVideoPlayback class. 
 * Handles playing back the video to the player through the terminal screen.
 * Inherits the video from the camera script and plays it back as-is. 
 */

public class FullVideoPlayback : MonoBehaviour
{
    private List<Texture2D> replayScreenshots = new List<Texture2D>(); //list of all frames
    private bool isPlayingVideo = false;

    //referneces to GameObjects necessary for displaying the footage
    public GameObject videoScreen; //screen to play on
    public GameObject recorderCamera; //camera in which it was recorded
    [SerializeField] private TextMeshPro timeRemainingText;
    private MeshRenderer videoScreenMesh;
    private CameraVision cameraVisionScript;

    //logic for displaying time remaining
    private int currentFrame;
    private float fpsTimer;

    public void Start()
    {
        //get mesh renderer ready for playback at start
        videoScreenMesh = videoScreen.GetComponent<MeshRenderer>();
        cameraVisionScript = recorderCamera.GetComponent<CameraVision>();
    }

    public void InheritVideo(List<Texture2D> playerVideo) //inherit the video produced by the camera
    {
        replayScreenshots = playerVideo;

        //set relevant variables for playback
        isPlayingVideo = true;
        currentFrame = 0;
    }

    public void Update()
    {
        fpsTimer += Time.deltaTime;

        //display time remaining text
        float timeRemainingValue = Mathf.Round((replayScreenshots.Count - currentFrame) / cameraVisionScript.framesPerSecond);
        timeRemainingText.text = "Time Remaining: " + timeRemainingValue + "s";

        //replay the video on the screen
        if (isPlayingVideo && currentFrame < replayScreenshots.Count) //while the video is playing and hasn't reached the end
        {
            if (fpsTimer > ((1000 / cameraVisionScript.framesPerSecond) * 0.001)) //if enough time has passed since the last frame
            {
                //update material to next screenshot instance
                videoScreenMesh.material.mainTexture = replayScreenshots[currentFrame];
                fpsTimer = 0;
                currentFrame++;
            }
        }
        else if (isPlayingVideo && currentFrame >= replayScreenshots.Count) //video is still playing but end of shots has been reached
        {
            //video is done playing
            isPlayingVideo = false;
        }
    }
}
