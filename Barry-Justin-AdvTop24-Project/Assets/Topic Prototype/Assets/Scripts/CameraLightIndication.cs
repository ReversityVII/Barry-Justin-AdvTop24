using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLightIndication : MonoBehaviour
{
    public Material lightOn;
    public Material lightOff;

    public void UpdateState(int state)
    {
        if (state > 0)
        {
            gameObject.GetComponent<MeshRenderer>().material = lightOn;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = lightOff;
        }
    }
}
