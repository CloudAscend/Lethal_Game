using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInventory : PlayerBase
{
    public Sprite inventoryImage;
    public int price;
    public int weight;

    public static ItemBase[] inventory;
    [SerializeField] private Image[] inventoryTrans;
    public static int invenValue;

    private float size;

    //private void Start()
    //{
    //    inventoryTrans = inventory.transform;
    //}

    private void Awake()
    {
        inventory = new ItemBase[4];
        size = 1;
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

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        invenValue %= 4;

        if (wheelInput > 0)
        {
            invenValue++;
        }
        else if (wheelInput < 0)
        {
            invenValue--;
        }
        else
        {
            invenValue %= 4;
        }

        invenValue %= 4;
        
        for (int i = 0; i < 4; i++)
        {
            Transform t = inventoryTrans[i].transform.parent;
            if (i == invenValue)
            {
                t.DOScale(1.25f, 0.1f);
            }
            else
            {
                t.DOScale(1f, 0.1f);
            }    
        }
        ChangeInventory(invenValue);
    }

    public void AddInventory(ItemBase item)
    {
        if (GameManager.instance.HasHoldItem())
        {
            return;
        }

        if (inventory[invenValue] == null)
        {
            inventoryTrans[invenValue].gameObject.SetActive(true);
            inventoryTrans[invenValue].sprite = inventoryImage;
        }
        inventory[invenValue] = item;

        for (int index = 0; index < inventoryTrans.Length; index++)
        {
            if (index != invenValue && inventory[index] != null)
                inventory[index].gameObject.SetActive(false);
        }

        GameManager.instance.player.GetComponent<PlayerMovement>().AddInventoryWeight(item.weight);
    }

    public void RemoveInventory()
    {

        GameManager.instance.player.GetComponent<PlayerMovement>().RemoveInventoryWeight(inventory[invenValue].weight);
        if (inventory[invenValue] != null)
        {
            inventoryTrans[invenValue].gameObject.SetActive(false);
            inventoryTrans[invenValue].sprite = null;
        }
        inventory[invenValue] = null;
    }

    public bool IsInventoryFull()
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null) return false;
        }
        return true;    
    }

    private void ChangeInventory(int inventoryValue)
    {
        GameManager.curInventory = inventoryValue;

        for (int index = 0; index < inventoryTrans.Length; index++)
        {
            bool objectActive = index == inventoryValue == true;
            if (inventory[index] != null)
                inventory[index].gameObject.SetActive(objectActive);
        }
    }

    private bool CheckInventory(int value)
    {
        return inventory[value] == null == true;
    }
}
