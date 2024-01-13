using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemAttackInfo
{
    public float chargeRot;
    public float chargeSpeed;

    public float attackRot;
    public float attackSpeed;

    public float defaultRot;
    public float defaultSpeed;
}

public class ItemAttack : IItemInteractable
{
    private float chargeRot = -30f;
    private float chargeSpeed = 200f;

    private float attackRot = 60f;
    private float attackSpeed = 600f;

    private float defaultRot = 0f;
    private float defaultSpeed = 300f;

    private bool isCharge = false;
    private bool isAttack = false;

    private bool isInteract = false;

    private ItemBase item;

    public void Interact(ItemBase item)
    {
        if(this.item == null || this.item != item)
        {
            this.item = item;
        }
        State();
        InputKey();
        
    }

    private void State()
    {
        if (isCharge)
        {
            item.customRot.x -= Time.deltaTime * chargeSpeed;
            if (item.customRot.x < chargeRot) item.customRot.x = chargeRot;
        }
        else if (isAttack)
        {
            item.customRot.x += Time.deltaTime * attackSpeed;
            if (item.customRot.x > attackRot)
            {
                isAttack = false;
                item.customRot.x = attackRot;
            }
        }
        else
        {
            if (item.customRot.x > defaultRot)
            {
                item.customRot.x = Mathf.Clamp(item.customRot.x - (Time.deltaTime * defaultSpeed), defaultRot, attackRot);
            }
            else if (item.customRot.x < defaultRot)
            {
                item.customRot.x = Mathf.Clamp(item.customRot.x + (Time.deltaTime * defaultSpeed), chargeRot, defaultRot);

            }
            if(item.customRot.x == defaultRot)
            {
                isInteract = false;
            }
        }
    }

    private void InputKey()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isCharge && !isInteract)
            {
                isCharge = true;
                isInteract = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isCharge)
            {
                return;
            }
            isCharge = false;
            isAttack = true;
        }
    }

    public void Init(object param = null)
    {
        if (param == null) return;
        if(param.GetType().Equals(typeof(ItemAttackInfo)))
        {
            var attackInfo = (ItemAttackInfo)param;
            chargeRot = attackInfo.chargeRot;
            chargeSpeed = attackInfo.chargeSpeed;

            attackRot = attackInfo.attackRot;
            attackSpeed = attackInfo.attackSpeed;

            defaultRot = attackInfo.defaultRot;
            defaultSpeed = attackInfo.defaultSpeed;
        }
    }
}
