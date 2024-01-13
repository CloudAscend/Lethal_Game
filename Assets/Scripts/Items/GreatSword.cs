using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : ItemBase 
{
    
    
    protected override void Start()
    {
        base.Start();
        interactable = new ItemAttack();
        interactable.Init(
            new ItemAttackInfo
            {
                chargeRot = -50,
                chargeSpeed = 150,

                attackRot = 100,
                attackSpeed = 700,

                defaultRot = 0,
                defaultSpeed = 300,
            }
        );
    }

    protected override void Update()
    {
        base.Update();
    }
}
