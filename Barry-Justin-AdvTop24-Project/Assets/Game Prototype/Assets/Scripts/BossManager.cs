using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<BulletData> CustomAttack = new List<BulletData>();
    public AttacksManager AttacksManager;

    private int currentAttack; //increases when an attack is complete
    private bool attackPlaying; //if an attack is happening or not

    private void Start()
    {
        attackPlaying = false;
        currentAttack = 0;
        AttacksManager = GameObject.FindObjectOfType<AttacksManager>();
    }

    // Update is called once per frame
    void Update()
    {
        print(CustomAttack.Count);

        if(!attackPlaying && currentAttack < CustomAttack.Count) //play attacks in order
        {
            attackPlaying = true;
            print(currentAttack);

            AttacksManager.commitAttack(CustomAttack[currentAttack]);

            currentAttack += 1;
        }
    }
}
