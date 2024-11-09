using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FullVideoPlayback : MonoBehaviour
{
    private List<Texture2D> screenshots = new List<Texture2D>(); //list of all frames
    private bool isPlayingVideo = false;

    public GameObject videoScreen;
    private MeshRenderer videoScreenMesh;

    public void Start()
    {
        videoScreenMesh = videoScreen.GetComponent<MeshRenderer>();
    }

    public void InheritVideo(List<Texture2D> playerVideo)
    {
        isPlayingVideo = true;
        screenshots = playerVideo;
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitForEndOfFrame();
        
        for(int i = 0; i < screenshots.Count; i++)
        {
            videoScreenMesh.material.mainTexture = screenshots[i];
        }
    }
}
