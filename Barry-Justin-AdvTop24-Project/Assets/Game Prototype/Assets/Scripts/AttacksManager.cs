using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    private BulletData CurrentAttack;
    public BossManager bossManager;
    public bulletBehaviour bulletBehaviour;

    private float timer;
    private float currentRepeats;
    private bool attackHappening  = false;
    public GameObject[] bulletArray;


    //performs the current attack
    private void Start()
    {
        bossManager = gameObject.GetComponent<BossManager>();
        //bulletBehaviour = GetComponent<bulletBehaviour>();
    }

    public void commitAttack(BulletData CustomAttack)
    {
        CurrentAttack = CustomAttack;
        attackHappening = true;

        timer = 0;
        currentRepeats = 0;
    }

    private void FixedUpdate()
    {
        while (attackHappening == true) //attacks should commence
        {
            switch(CurrentAttack.attackType) 
            {
                case BulletData.AttackType.Radial:

                    timer += Time.deltaTime;

                    if(timer > CurrentAttack.timeBetweenRepeats && currentRepeats < CurrentAttack.repeats)
                    {
                        for (int i = 0; i < CurrentAttack.count; i++) //perform a radial attack
                        {
                            print("reached");
                            float currentAngle = (CurrentAttack.spacing * i) * Mathf.Deg2Rad;
                            CurrentAttack.direction.x = (Mathf.Cos(currentAngle));
                            CurrentAttack.direction.y = (Mathf.Sin(currentAngle));
                            bulletBehaviour.Initialize(CurrentAttack);
                        }
                        currentRepeats++;
                    }

                break;

                case BulletData.AttackType.Targeted:
                    print("not a feature yet!");

                    break;
            }
        }
    }
}
