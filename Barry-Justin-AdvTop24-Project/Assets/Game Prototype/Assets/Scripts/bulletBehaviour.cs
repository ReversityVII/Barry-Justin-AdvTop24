using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    //private float speed;
    //private float acceleration;
    //private Vector3 direction;
    //private float maxLifetime;
    //private float scale; 
    private BulletData bulletData; //personal data stuff

    private float timeAlive;

    public void Initialize(BulletData readyBullet)
    {
        bulletData = readyBullet;
        transform.localScale += new Vector3(bulletData.scale, bulletData.scale, 1);
    }


    void FixedUpdate()
    {
        timeAlive += Time.deltaTime;

        if (bulletData.speed > 0) //dont start going backwards in case bullets are set to slow down
            bulletData.speed += bulletData.acceleration * Time.deltaTime;

        transform.position += (bulletData.direction.normalized * bulletData.speed) * Time.deltaTime;

        if (timeAlive > bulletData.maxLifetime) //when bullets dissapear
        {
            GameObject.Destroy(gameObject);
        }
    }
}
