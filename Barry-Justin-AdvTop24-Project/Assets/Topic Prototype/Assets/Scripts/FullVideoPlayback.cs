using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FullVideoPlayback : MonoBehaviour
{
    private List<Texture2D> replayScreenshots = new List<Texture2D>(); //list of all frames
    private bool isPlayingVideo = false;

    public GameObject videoScreen;
    public GameObject recorderCamera;
    [SerializeField] private TextMeshPro timeRemainingText;
    private MeshRenderer videoScreenMesh;
    private CameraVision cameraVisionScript;

    private int currentFrame;
    private float fpsTimer;

    public void Start()
    {
        //get mesh renderer
        videoScreenMesh = videoScreen.GetComponent<MeshRenderer>();
        cameraVisionScript = recorderCamera.GetComponent<CameraVision>();
    }

    public void InheritVideo(List<Texture2D> playerVideo)
    {
        //inherit the video that the player recorded
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

        //video replay
        if (isPlayingVideo && currentFrame < replayScreenshots.Count) //while the video is playing and hasn't reached the end
        {  
            if (fpsTimer > ((1000 / cameraVisionScript.framesPerSecond) * 0.001)) //same fps as the camera
            {
                //update material to next screenshot instance
                videoScreenMesh.material.mainTexture = replayScreenshots[currentFrame];
                fpsTimer = 0;
                currentFrame++;
            }
        }
        else if (isPlayingVideo && currentFrame >= replayScreenshots.Count) //video is still playing but end has been reached
        {
            //video is done playing
            isPlayingVideo = false;
        }
    }
}
