using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLightIndication : MonoBehaviour
{
    public Material lightOn;
    public Material lightOff;

    public GameObject areaLight;

    public void UpdateState(int state)
    {
        if (state > 0)
        {
            areaLight.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().material = lightOn;
        }
        else
        {
            areaLight.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().material = lightOff;
        }
    }
}
