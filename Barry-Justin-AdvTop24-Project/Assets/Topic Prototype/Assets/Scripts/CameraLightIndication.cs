using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CameraLightIndication class.
 * Simple script that turns light on/off based on whether or not the player is recording. 
 */
public class CameraLightIndication : MonoBehaviour
{
    //material reference for light being on or off
    public Material lightOn;
    public Material lightOff;

    //reference the light itself
    public GameObject areaLight;

    //update the state of the light depending on whether or not the player is recording
    public void UpdateState(bool state)
    {
        if (state == true) //player is recording
        {
            areaLight.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().material = lightOn;
        }
        else //player is not recording
        {
            areaLight.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().material = lightOff;
        }
    }
}
