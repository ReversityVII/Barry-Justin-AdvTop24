using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TrackTotalShots : MonoBehaviour
{
    private int totalShots;
    private int shotsLimit;

    private CameraVision cameraVision;
    public TextMeshProUGUI percentageText;

    public Gradient gradient;

    public void Start()
    {
        totalShots = 0;
        cameraVision = gameObject.GetComponent<CameraVision>();
        shotsLimit = cameraVision.totalScreenshotLimit;
    }

    public bool NextShotPermitted() //is another shot allowed to be taken?
    {
        if (totalShots < shotsLimit) 
        {
            return true; 
        }
        else
        return false;
    }

    public void addShot() //called when a shot is taken
    {
        totalShots++;
    }

    public void Update()
    {
        //set color of text
        float limitPercentage = (float) totalShots/shotsLimit;
        percentageText.color = gradient.Evaluate(limitPercentage);

        //convert to more readable number for display
        float correctedLimitPercentage = Mathf.Round(limitPercentage * 100);

        //get seconds remaining before the camera is full
        float secondsRemaining = (shotsLimit - totalShots) / cameraVision.framesPerSecond;

        //update display
        percentageText.text = correctedLimitPercentage.ToString() + "%" + "   |   " + secondsRemaining.ToString() + "s";
        

    }

    public void resetShots() //called when the footage is uploaded to the terminal
    {
        totalShots = 0; 
    }
}
