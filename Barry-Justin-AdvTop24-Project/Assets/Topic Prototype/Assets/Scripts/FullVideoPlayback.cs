using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FullVideoPlayback : MonoBehaviour
{
    private List<Texture2D> replayScreenshots = new List<Texture2D>(); //list of all frames
    private bool isPlayingVideo = false;

    public GameObject videoScreen;
    public GameObject recorderCamera;
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

        //print(screenshots.Count);
        
    }

    public void Update()
    {
        fpsTimer += Time.deltaTime;

        //while the video isn't done
        if (isPlayingVideo && currentFrame < replayScreenshots.Count)
        {  
            if (fpsTimer > ((1000 / cameraVisionScript.framesPerSecond) * 0.001)) //same fps as the camera
            {
                //update material to next screenshot instance
                videoScreenMesh.material.mainTexture = replayScreenshots[currentFrame];
                fpsTimer = 0;
                currentFrame++;
            }
                
        }
        else if (isPlayingVideo && currentFrame >= replayScreenshots.Count)
        {
            //video is done playing
            isPlayingVideo = false;
        }
    }
}
