using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/*
 * TrackTotalShots class. 
 * Handles the logic surrounding how many shots have been taken and if any more are allowed.
 * Also handles the display of this information within the Update() function, which updates the UI to communicate this
 * information to players.
 */

public class TrackTotalShots : MonoBehaviour
{
    //track shot count and limit
    private int totalShots;
    private int shotsLimit;

    //update UI elements
    private CameraVision cameraVision;
    public TextMeshProUGUI percentageText;

    //customizable gradient for the text changing color as player reaches limit
    public Gradient gradient;

    public void Start()
    {
        //set initial variables
        totalShots = 0;
        cameraVision = gameObject.GetComponent<CameraVision>();
        shotsLimit = cameraVision.totalScreenshotLimit;
    }

    public bool NextShotPermitted() //is another shot allowed to be taken?
    {
        if (totalShots < shotsLimit) //as long as there is at least one more shot available
            return true;
        
        else
            return false;
    }

    public void addShot() //called when a shot is taken
    {
        totalShots++;
    }

    public void Update() //display needed UI elements. Percentage, seconds remaining.
    {
        //set color of text
        float limitPercentage = (float)totalShots / shotsLimit;
        percentageText.color = gradient.Evaluate(limitPercentage);

        //convert to more readable number for display
        float correctedLimitPercentage = Mathf.Round(limitPercentage * 100);

        //get seconds remaining before the camera is full
        float secondsRemaining = (shotsLimit - totalShots) / cameraVision.framesPerSecond;

        //update display
        percentageText.text = correctedLimitPercentage.ToString() + "%" + "   |   " + secondsRemaining.ToString() + "s left";
    }

    public void resetShots() //called when the footage is uploaded to the terminal
    {
        //start fresh
        totalShots = 0;
    }
}
