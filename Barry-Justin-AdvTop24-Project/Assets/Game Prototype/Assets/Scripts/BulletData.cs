using System;
using System.Collections.Generic;
using UnityEngine;

//Data class used for propagating the details of a Sim when creating them in the simulation.
[Serializable]

public class BulletData
{
    public float speed;

    public float acceleration;

    public Vector3 direction;

    public float maxLifetime;

    public float scale;

    public enum AttackType
    {
        Radial, Targeted
    }
    
    public AttackType attackType;

    public float count;
    public float spacing; 

    public float repeats; //how many times, for example, a radial attack will happen
    
    public float timeBetweenRepeats; //time between these repeats



    //later: type, color
}
