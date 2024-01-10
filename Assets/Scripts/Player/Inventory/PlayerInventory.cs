using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : PlayerBase
{
    public Sprite inventoryImage;
    public int price;
    public int weight;

    [SerializeField] private ItemBase[] inventory;
    [SerializeField] private Image[] inventoryTrans;
    private int invenValue;

    //private void Start()
    //{
    //    inventoryTrans = inventory.transform;
    //}

    private void Awake()
    {
        inventory = new ItemBase[4];
    }

    private void Update()
    {
        ChooseInventory();
    }

    private void ChooseInventory()
    {
        if      (Input.GetKey(KeyCode.Alpha1)) invenValue = 0;
        else if (Input.GetKey(KeyCode.Alpha2)) invenValue = 1;
        else if (Input.GetKey(KeyCode.Alpha3)) invenValue = 2;
        else if (Input.GetKey(KeyCode.Alpha4)) invenValue = 3;
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

        //if (invenValue)
        if (inventory[invenValue] == null)
            inventoryTrans[invenValue].sprite = inventoryImage;
        inventory[invenValue] = item;
    }

    public void RemoveInventory()
    {
        if (inventory[invenValue] != null)
            inventoryTrans[invenValue].sprite = null;
        inventory[invenValue] = null;
    }
}
