using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventInfo
{
    public Sprite inventory;
    public int price;
    public int weight;
}

public class PlayerInventory : PlayerBase
{
    public List<InventInfo> inventInfo;

    [SerializeField] private GameObject inventory;
    private Transform inventoryTrans;
    private int invenValue;

    private void Start()
    {
        inventoryTrans = inventory.transform;
    }

    public void AddInventory(ItemBase item)
    {
        //switch (item.grabType)
        //{
        //    case ItemGrabType.Big:
        //        theImage.sprite = inventInfo[invenValue % ].inventory;
        //        break;
        //    case ItemGrabType.Small:
        //        theImage.sprite = inventInfo[invenValue].inventory;
        //        break;
        //}

        inventoryTrans.GetChild(invenValue).GetComponent<Image>().sprite = inventInfo[0].inventory;

        invenValue++;
    }

    public void RemoveInventory()
    {
        invenValue--;

        inventoryTrans.GetChild(invenValue).GetComponent<Image>().sprite = null;
    }
}
